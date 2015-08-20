// -----------------------------------------------------------------------------
// Copyright (C) 2013 Nicolas P. Rougier. All rights reserved.
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
//  dashLine.frag
//  @brief Fragment shader used to render antialiased segements of a dash line with customizable join and caps.
//  Used for customizable dash caps, joins.
//
//  @note the dash textures should be of float type with at least half float precision.
//

//#define DEBUG

#ifndef OVERLAP_ANGLE_THRESHOLD
#define OVERLAP_ANGLE_THRESHOLD 15.0
#endif

const float PI = 3.14159265358979323846264;
const float THETA = OVERLAP_ANGLE_THRESHOLD * PI/180.0;

#ifdef UNIFORM_DASH_LENGTH

#ifndef ARRAY_INDEXING
#define ARRAY_INDEXING(array, index) array[index];
#endif

uniform float u_dash_period;
uniform vec4  u_dash_pattern[UNIFORM_DASH_LENGTH];
#else
uniform sampler2D s_dash_atlas;
uniform sampler2D s_dash_period;  //1D texture
#endif

uniform lowp vec4      u_color;
uniform mediump float  u_miter_limit;
uniform mediump float  u_antialias;     //line antialias radius
uniform mediump vec4   u_uniforms;      //line width and line join
uniform mediump vec4   u_dash_settings; //dash phase, index(not used for dash uniform), cap start and cap end
#ifdef USE_OUTLINE
uniform mediump float  u_outline_width;
uniform lowp vec4      u_outline_color;
#endif
uniform mediump float  u_znear;        //lowest z value, in the [0,1] range, after which to apply aliasing

varying highp vec2    v_segment;  //can use flat qualifier
varying highp float   v_length;   //can use flat qualifier, line length
varying mediump float v_start;    //can use flat qualifier, line start
varying highp float   v_dx;
varying mediump float v_dy;
varying mediump vec2  v_angles;
varying mediump vec2  v_miter;

void main()
{
    //aliasing based on depth
    float antialias = aliasingDepth(gl_FragCoord.z, u_znear, u_antialias);
    float t = u_uniforms.x - antialias;
    float d = 0.0;
    
    float dash_center, dash_type, dash_start, dash_stop;
    float u, _start, _stop;
    lowp ivec2 dash_caps = ivec2(roundi(u_dash_settings.z), roundi(u_dash_settings.w));
    //get dash pattern
    {
#ifdef USE_OUTLINE
        float width = 2.0 * u_uniforms.x + 2.0 * u_outline_width;
#else
        float width = 2.0 * u_uniforms.x;
#endif
        
#ifdef UNIFORM_DASH_LENGTH
        float freq = u_dash_period * width;
        u = mod(v_dx + u_dash_settings.x * width, freq); //add dash phase
        vec4 tex = ARRAY_INDEXING(u_dash_pattern, int(u / freq * float(UNIFORM_DASH_LENGTH)));
#else
        float freq = texture2D(s_dash_period, vec2(0.0, u_dash_settings.y)).x * width;
        u = mod(v_dx + u_dash_settings.x * width, freq); //add dash phase
        vec4 tex = texture2D(s_dash_atlas, vec2(u/freq,  u_dash_settings.y));
#endif
        
        dash_center= tex.x * width;
        dash_type  = tex.y;
        _start = tex.z * width;
        _stop  = tex.a * width;
        dash_start = v_dx - u + _start;
        dash_stop  = v_dx - u + _stop;
        
        // Compute extents of the first dash (the one relative to v_segment.x)
        // Note: this could be computed in the vertex shader
        if(dash_stop <= v_segment.x)
        {
            float u = mod(v_segment.x + u_dash_settings.x * width, freq);
#ifdef UNIFORM_DASH_LENGTH
            vec4 tex = ARRAY_INDEXING(u_dash_pattern, int(u / freq * float(UNIFORM_DASH_LENGTH)));
#else
            vec4 tex = texture2D(s_dash_atlas, vec2(u/freq,  u_dash_settings.y));
#endif
            dash_center= tex.x * width;
            float _start = tex.z * width;
            float _stop  = tex.a * width;
            dash_start = v_segment.x - u + _start;
            dash_stop = v_segment.x - u + _stop;
        }
        // Compute extents of the last dash (the one relatives to v_segment.y)
        // Note: This could be computed in the vertex shader
        else if(dash_start >= v_segment.y)
        {
            float u = mod(v_segment.y + u_dash_settings.x * width, freq);
#ifdef UNIFORM_DASH_LENGTH
            vec4 tex = ARRAY_INDEXING(u_dash_pattern, int(u / freq * float(UNIFORM_DASH_LENGTH)));
#else
            vec4 tex = texture2D(s_dash_atlas, vec2(u/freq,  u_dash_settings.y));
#endif
            dash_center= tex.x * width;
            float _start = tex.z * width;
            float _stop  = tex.a * width;
            dash_start = v_segment.y - u + _start;
            dash_stop  = v_segment.y - u + _stop;
        }
    }
    
    // Check is dash stop is before line start or dash start is beyond line stop
    if( dash_stop <= v_start || (dash_start >= v_length) )
    {
#ifdef DEBUG
        gl_FragColor = vec4(0.0,1.0,0.0,0.5);
        return;
#endif
        discard;
    }
    
    // This test if the we are dealing with a discontinuous angle
    {
        float segment_center = (v_segment.x+v_segment.y) * 0.5;
        bool discontinuous = ((v_dx <  segment_center) && abs(v_angles.x) > THETA) ||
        ((v_dx >= segment_center) && abs(v_angles.y) > THETA);
        // Check if current dash start is beyond segment stop
        if( discontinuous )
        {
            // Dash start is beyond segment or dash stop is before segment
            if( (dash_start > v_segment.y) || (dash_stop < v_segment.x) ) {
#ifdef DEBUG
                gl_FragColor = vec4(1.0,0.0,0.0,0.5);
                return;
#endif
                discard;
            }
            
            // Special case for round caps  (nicer with this)
            if( dash_caps.y == 1 && (u < _start) && (dash_start < v_segment.x )  && (abs(v_angles.x) < PI/2.0) ||
               dash_caps.x == 1 && (u > _stop) && (dash_stop > v_segment.y )  && (abs(v_angles.y) < PI/2.0) )
            {
#ifdef DEBUG
                gl_FragColor = vec4(1.0,1.0,0.0,0.5);
                return;
#endif
                discard;
            }
            
#ifdef TRIANGLES_CAPS_CASE
            // Special case for triangle caps (in & out) and square
            // We make sure the cap stop at crossing frontier
            if( (dash_caps.x != 1) && (dash_caps.x != 5) && (dash_start < v_segment.x )  && (abs(v_angles.x) < PI/2.0) )
            {
                float a = v_angles.x/2.0;
                float x = (v_segment.x-v_dx)*cos(a) - v_dy*sin(a);
                if( x > 0.0 )
                    discard;
                // We transform the cap into square to avoid holes
                dash_caps.x = 4;
            }
            if( (dash_caps.y != 1) && (dash_caps.y != 5) && (dash_stop > v_segment.y )  && (abs(v_angles.y) < PI/2.0) )
            {
                float a = v_angles.y/2.0;
                float x = (v_dx-v_segment.y)*cos(a) - v_dy*sin(a);
                if( x > 0.0 )
                    discard;
                // We transform the caps into square to avoid holes
                dash_caps.y = 4;
            }
#endif
        }
    }
    //Calculate distance
    {
        bool inner = (v_dx > v_start) && (v_dx < v_length);
        float d_join = join( roundi(u_uniforms.y), v_dy,
                            v_segment, v_dx, v_dy, v_miter, u_miter_limit, u_uniforms.x );

        // Line cap at start
        if( (v_dx <= v_start) && (dash_start <= v_start) && (dash_stop > v_start) ) {
            d = cap(dash_caps.x, v_dx, v_dy, t);
        }
        // Line cap at stop
        else if( (v_dx >= v_length) && (dash_stop > v_length) && (dash_start < v_length)  ) {
            d = cap(dash_caps.y, v_dx-v_length, v_dy, t);
        }
        // Dash cap left
        else if( dash_type < 0.0 )  {
            d = cap(dash_caps.y, u-dash_center, v_dy, t);
            if( inner )
                d = max(d,d_join);
        }
        // Dash cap right
        else if( dash_type > 0.0 ) {
            d = cap(dash_caps.x, dash_center-u, v_dy, t);
            if( inner )
                d = max(d,d_join);
        }
        // Dash body (plain)
        else if( dash_type == 0.0 )
            d = abs(v_dy);

        // Line join
        if( inner )
        {
            if( (v_dx <= v_segment.x) && (dash_start <= v_segment.x) && (dash_stop >= v_segment.x) )
            {
                d = d_join;
                // Antialias at outer border
                float angle = PI/2.+v_angles.x;
                float f = abs( (v_segment.x - v_dx)*cos(angle) - v_dy*sin(angle));
                d = max(f,d);
            }
            else if( (v_dx > v_segment.y) && (dash_start <= v_segment.y)
                    && (dash_stop >= v_segment.y) )
            {
                d = d_join;
                // Antialias at outer border
                float angle = PI/2.+v_angles.y;
                float f = abs((v_dx - v_segment.y)*cos(angle) - v_dy*sin(angle));
                d = max(f,d);
            }
            else if( v_dx < (v_segment.x - u_uniforms.x) || v_dx > (v_segment.y + u_uniforms.x) )
            {
#ifdef DEBUG
                gl_FragColor = vec4(0.0,0.0,1.0,0.5);
                return;
#endif
                discard;
            }
        }
    }
    
   
#ifdef USE_OUTLINE
   gl_FragColor = lineColor(u_color, u_outline_color, d, t, antialias, u_outline_width);
#else
    // Distance to border
    d -= t;
    lowp vec4 color = u_color;
    if( d >= 0.0 )
    {
        d /= antialias;
        color = vec4(u_color.rgb, exp(-d*d)*u_color.a);
    }
    gl_FragColor = color;
#endif
}
