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
//  @note Works in mercator coordinates and transforms to pixels coordinates using u_scale.
//
precision mediump float;

const float PI = 3.14159265358979323846264;
const float THETA = 15.0 * PI/180.0;

uniform mediump float  u_antialias;    //line antialias radius
uniform mediump vec4   u_uniforms;     //line width and line join
uniform highp mat4     u_mvpMatrix;
uniform highp float    u_scale;

attribute highp vec2    a_position;
attribute mediump vec4  a_tangents;
attribute highp vec2    a_segment;
attribute mediump vec2  a_angles;
attribute lowp vec2     a_lineCoord;

varying vec2  v_segment;    //can use flat​ qualifier
varying vec2  v_miter;
varying vec2  v_lineCoord;
varying float v_length;     //can use flat​ qualifier
#ifdef USE_FOLDING
varying vec2  v_angles;     //can use flat​ qualifier
#endif

// Cross product of v1 and v2
float cross(in vec2 v1, in vec2 v2);
// Returns distance of v3 to line v1-v2
float signed_distance(in vec2 v1, in vec2 v2, in vec2 v3);
// Rotate v around origin
void rotate( in vec2 v, in float alpha, out vec2 result );

void main()
{
    // This is the actual half width of the line
    float w = ceil(1.25 * u_antialias)/2.0 + u_uniforms.x;
    float wMerc = w / u_scale;
    
    vec2 position = a_position;
    vec2 t1 = normalize(a_tangents.xy);
    vec2 t2 = normalize(a_tangents.zw);
    //start and end points are encoded, must normalize
    float u = a_lineCoord.x / abs(a_lineCoord.x);
    float v = a_lineCoord.y;
    
    // adjust to pixel coordinates
    v_segment = a_segment * u_scale;
#ifdef USE_FOLDING
    v_angles = a_angles;
#endif
    
    // This is a join
    if( abs(a_lineCoord.x) < 2.5) {
        float angle  = atan (t1.x*t2.y-t1.y*t2.x, t1.x*t2.x+t1.y*t2.y);
        vec2 t  = normalize(t1+t2);
        vec2 o  = vec2( + t.y, - t.x);
        
#ifdef USE_FOLDING
        // Broken angle
        if( (abs(angle) > THETA) )
        {
            position += v * wMerc * o / cos(angle/2.0);
            float s = sign(angle);
            if( angle < 0.0 ) {
                if( u == +1.0 ) {
                    u = v_segment.y + v * w * tan(angle/2.0);
                    if( v == 1.0 ) {
                        position -= 2.0 * wMerc * t1 / sin(angle);
                        u -= 2.0 * w / sin(angle);
                    }
                } else {
                    u = v_segment.x - v * w * tan(angle/2.0);
                    if( v == 1.0 ) {
                        position += 2.0 * wMerc * t2 / sin(angle);
                        u += 2.0*w / sin(angle);
                    }
                }
            } else {
                if( u == +1.0 ) {
                    u = v_segment.y + v * w * tan(angle/2.0);
                    if( v == -1.0 ) {
                        position += 2.0 * wMerc * t1 / sin(angle);
                        u += 2.0 * w / sin(angle);
                    }
                } else {
                    u = v_segment.x - v * w * tan(angle/2.0);
                    if( v == -1.0 ) {
                        position -= 2.0 * wMerc * t2 / sin(angle);
                        u -= 2.0*w / sin(angle);
                    }
                }
            }
        // Continuous angle
        } else {
            position += v * wMerc * o / cos(angle/2.0);
            if( u == +1.0 )
                u = v_segment.y;
            else
                u = v_segment.x;
        }
#else
        position.xy += v * wMerc * o / cos(angle/2.0);
        if( angle < 0.0 ) {
            if( u == +1.0 )
                u = v_segment.y + v * w * tan(angle/2.0);
            else
                u = v_segment.x - v * w * tan(angle/2.0);
        } else {
            if( u == +1.0 )
                u = v_segment.y + v * w * tan(angle/2.0);
            else
                u = v_segment.x - v * w * tan(angle/2.0);
        }
  #endif
        
        // This is a line start or end
    } else {
        vec2 o1 = vec2( +t1.y, -t1.x);
        position += v * wMerc * o1;
        if( u == -1.0 ) {
            u = v_segment.x - w;
            position -=  wMerc * t1;
        } else {
            u = v_segment.y + w;
            position +=  wMerc * t2;
        }
    }
    
    // Miter distance
    vec2 t;
    vec2 curr = a_position * u_scale;
    vec2 positionPix = position * u_scale;
    if( a_lineCoord.x < 0.0 ) {
        vec2 next = curr + t2*(v_segment.y-v_segment.x);
        
        rotate( t1, +a_angles.x/2.0, t);
        v_miter.x = signed_distance(curr, curr+t, positionPix);
        
        rotate( t2, +a_angles.y/2.0, t);
        v_miter.y = signed_distance(next, next+t, positionPix);
    } else {
        vec2 prev = curr - t1*(v_segment.y-v_segment.x);
        
        rotate( t1, -a_angles.x/2.0,t);
        v_miter.x = signed_distance(prev, prev+t, positionPix);
        
        rotate( t2, -a_angles.y/2.0,t);
        v_miter.y = signed_distance(curr, curr+t, positionPix);
    }
    
    //adjust previous and next limits for start and end points
    lowp int pointEncode = int(abs(a_lineCoord.x));
    if (v_segment.x <= 0.0)
        v_miter.x = 1e10;
    if (pointEncode == 2 || pointEncode == 4) {
        v_miter.y = 1e10;
        v_length = v_segment.y;
    }
    else
        v_length = -1.0;
    v_lineCoord = vec2( u, v*w );
    
    gl_Position = u_mvpMatrix*vec4(position,0.0,1.0);
}


// Cross product of v1 and v2
float cross(in vec2 v1, in vec2 v2) {
    return v1.x*v2.y - v1.y*v2.x;
}

// Returns distance of v3 to line v1-v2
float signed_distance(in vec2 v1, in vec2 v2, in vec2 v3) {
    return cross(v2-v1,v1-v3) / length(v2-v1);
}

// Rotate v around origin
void rotate( in vec2 v, in float alpha, out vec2 result ) {
    float c = cos(alpha);
    float s = sin(alpha);
    result = vec2( c*v.x - s*v.y, s*v.x + c*v.y );
}
