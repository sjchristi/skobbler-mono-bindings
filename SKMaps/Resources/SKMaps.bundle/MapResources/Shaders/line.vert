// -----------------------------------------------------------------------------
// Copyright (c) 2013 Nicolas P. Rougier. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY NICOLAS P. ROUGIER ''AS IS'' AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
// EVENT SHALL NICOLAS P. ROUGIER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// The views and conclusions contained in the software and documentation are
// those of the authors and should not be interpreted as representing official
// policies, either expressed or implied, of Nicolas P. Rougier.
// -----------------------------------------------------------------------------
//
//  line.vert
//  @brief Vertex shader used to modify the geometry of line segments(ie. joints) based on the line radius(half of the line width).
//  Used for customizable joins and caps.
//
//  @note Created to work with a GL_TRIANGLES structure.
//  @note Works in world coordinates and transforms to pixels coordinates using u_scale.
//
#ifdef OVERLAP_ANGLE_THRESHOLD
const float PI = 3.14159265358979323846264;
const float THETA = OVERLAP_ANGLE_THRESHOLD * PI/180.0;
#endif

attribute highp vec4    a_position;
attribute mediump vec2  a_tangents;    //two tangents in polar form, no magnitude
#ifdef USE_SEGMENT_ATTRIBUTE
attribute highp float   a_segment;     //current segment length/period phase in mercators
#endif
attribute mediump vec2  a_angles;      //current and next segment angles
attribute mediump vec2  a_lineCoord;   //line coordinates(the absolute value of x marks the start/end and of y is the magnitude of the current segment)

uniform mediump float  u_antialias;    //line antialias radius
uniform mediump vec4   u_uniforms;     //line width and line join
uniform mediump float  u_line_offset;  //line offset(pixels)
uniform highp mat4     u_mvp_matrix;
uniform highp float    u_scale;
#ifdef USE_OUTLINE
uniform mediump float  u_outlineWidth;
#endif

varying highp vec2      v_segment;  //can use flat qualifier
varying highp float     v_length;   //can use flat qualifier, marks line end
varying mediump float   v_start;    //can use flat qualifier, line start
varying highp float     v_dx;
varying mediump float   v_dy;
varying mediump vec2    v_miter;
#ifdef USE_ANGLE
varying mediump vec2    v_angles;   //can use flat qualifier
#endif

// Cross product of v1 and v2
float cross2(in vec2 v1, in vec2 v2);
// Returns distance of v3 to line v1-v2
float signed_distance(in vec2 v1, in vec2 v2, in vec2 v3);
// Rotate v around origin
void rotate(in vec2 v, in float alpha, out vec2 result);

void main()
{
    // this is the actual half width of the line
#ifdef USE_OUTLINE
    float w = ceil(1.25 * u_antialias) * 0.5 + u_uniforms.x + u_outlineWidth;
#else
    float w = ceil(1.25 * u_antialias) * 0.5 + u_uniforms.x;
#endif
    float wWorld = w / u_scale;
    
    vec2 position = a_position.xy;
    vec2 t1 = vec2(cos(a_tangents.x), sin(a_tangents.x));
    vec2 t2 = vec2(cos(a_tangents.y), sin(a_tangents.y));
    // get segments direction
    float u = sign(a_lineCoord.x);
    float v = sign(a_lineCoord.y);
    float coord = abs(a_lineCoord.x);
    
    // adjust to pixel coordinates
#ifdef USE_SEGMENT_ATTRIBUTE
    v_segment = vec2(a_segment, a_segment + abs(a_lineCoord.y)) * u_scale;
#else
    v_segment = vec2(0.0, abs(a_lineCoord.y) * u_scale);
#endif
#ifdef USE_ANGLE
    v_angles = a_angles;
#endif

    // marked as a join
    if(coord < 3.5)
    {
        //current angle
        float angle = mix(a_angles.x, a_angles.y, (1.0 + u) * 0.5);
        vec2 t  = normalize(t1+t2);
        vec2 o  = vec2(t.y, - t.x);
        position += (u_line_offset / u_scale) * o;
        
#ifdef OVERLAP_ANGLE_THRESHOLD
        // discontinous angles
        if(abs(angle) > THETA)
        {
            float invSinAngle = 1.0 / sin(angle);
            //get the half angle
            angle *= 0.5;
            position += v * wWorld * o / cos(angle);
            if( angle < 0.0 )
            {
                if( u > 0.0 ) {
                    u = v_segment.y + v * w * tan(angle);
                    if( v > 0.0 ) {
                        position -= 2.0 * wWorld * t1 * invSinAngle;
                        u -= 2.0 * w * invSinAngle;
                    }
                } else {
                    u = v_segment.x - v * w * tan(angle);
                    if( v > 0.0 ) {
                        position += 2.0 * wWorld * t2 * invSinAngle;
                        u += 2.0 * w * invSinAngle;
                    }
                }
            }
            else
            {
                if( u > 0.0 ) {
                    u = v_segment.y + v * w * tan(angle);
                    if( v < 0.0 ) {
                        position += 2.0 * wWorld * t1 * invSinAngle;
                        u += 2.0 * w * invSinAngle;
                    }
                } else {
                    u = v_segment.x - v * w * tan(angle);
                    if( v < 0.0 ) {
                        position -= 2.0 * wWorld * t2 * invSinAngle;
                        u -= 2.0 * w * invSinAngle;
                    }
                }
            }
        }
        // continuous angle
        else
        {
            position += v * wWorld * o / cos(angle*0.5);
            if( u > 0.0 )
                u = v_segment.y;
            else
                u = v_segment.x;
        }
#else
        //get the half angle
        angle *= 0.5;
        position += v * wWorld * o / cos(angle);
        if( angle < 0.0 ) {
            if( u > 0.0 )
                u = v_segment.y + v * w * tan(angle);
            else
                u = v_segment.x - v * w * tan(angle);
        } else {
            if( u > 0.0 )
                u = v_segment.y + v * w * tan(angle);
            else
                u = v_segment.x - v * w * tan(angle);
        }
#endif
    }
    // this is a line start or end
    else
    {
        vec2 o1 = vec2(t1.y, -t1.x);
        position += (u_line_offset / u_scale) * o1;
        position += v * wWorld * o1;
        if( u == -1.0 ) {
            u = v_segment.x - w;
            position -=  wWorld * t1;
            
        } else {
            u = v_segment.y + w;
            position +=  wWorld * t1;
        }
    }
    
    // miter distance
    {
        vec2 t;
        vec2 curr = a_position.xy * u_scale;
        vec2 positionPix = position * u_scale;
        if( a_lineCoord.x < 0.0 ) {
            vec2 next = curr + t2*(v_segment.y-v_segment.x);
            
            rotate(t1, +a_angles.x * 0.5, t);
            v_miter.x = signed_distance(curr, curr+t, positionPix);
            
            rotate(t2, +a_angles.y * 0.5, t);
            v_miter.y = signed_distance(next, next+t, positionPix);
        } else {
            vec2 prev = curr - t1*(v_segment.y-v_segment.x);
            
            rotate(t1, -a_angles.x * 0.5,t);
            v_miter.x = signed_distance(prev, prev+t, positionPix);
            
            rotate(t2, -a_angles.y * 0.5,t);
            v_miter.y = signed_distance(curr, curr+t, positionPix);
        }
    }

    //adjust previous and next limits for start and end points
    {
        float notStartSegment = mod(coord, 3.0);
        if(notStartSegment > 0.5)
            v_start =  -w * 100.0;
        else
        {
            v_start = 0.0;
            v_miter.x = 1e3;
        }
    }
    {
        float notEndSegment = mod(coord, 2.0);
        if(notEndSegment > 0.5)
            v_length = v_segment.y + w * 100.0;
        else
        {
            v_length = v_segment.y;
            v_miter.y = 1e3;
        }
    }
    v_dx = u;
    v_dy = v * w;
    
    gl_Position = u_mvp_matrix*vec4(position, a_position.zw);
}

float cross2(in vec2 v1, in vec2 v2)
{
    return v1.x*v2.y - v1.y*v2.x;
}

float signed_distance(in vec2 v1, in vec2 v2, in vec2 v3)
{
    return cross2(v2-v1,v1-v3) / length(v2-v1);
}

void rotate(in vec2 v, in float alpha, out vec2 result)
{
    float c = cos(alpha);
    float s = sin(alpha);
    result = vec2( c*v.x - s*v.y, s*v.x + c*v.y );
}
