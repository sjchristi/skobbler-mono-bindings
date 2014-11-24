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
precision mediump float;

const float PI = 3.14159265358979323846264;
const float THETA = 15.0 * PI/180.0;

uniform lowp sampler2D s_dash_atlas;
uniform lowp sampler2D s_dash_period;  //1D texture

uniform lowp vec4      u_color;
uniform mediump float  u_miter_limit;
uniform mediump float  u_antialias;    //line antialias radius
uniform mediump vec4   u_uniforms;     //line width and line join
uniform mediump vec4   u_dashUniforms; //dash phase, index, cap stard and cap end
#ifdef USE_OUTLINE
uniform mediump float  u_outlineWidth;
uniform lowp vec4      u_outlineColor;
#endif

varying vec2  v_segment;
varying vec2  v_angles;
varying vec2  v_lineCoord;
varying vec2  v_miter;
varying float v_length;

// Compute distance to cap
float cap( lowp int type, float dx, float dy, float t );
// Compute distance to join
float join( lowp int type,  float d, vec2 segment, vec2 texcoord, vec2 miter,
           float miter_limit, float linewidth );
int roundi( float x );

void main()
{
    lowp vec4 color = u_color;
    float dx = v_lineCoord.x;
    float dy = v_lineCoord.y;
    float t = u_uniforms.x-u_antialias;
    float width = 2.0 * u_uniforms.x;
    float d = 0.0;
    float line_start = 0.0;
    float line_stop  = v_length;
    //check if set, only set for end points so adjust such that dx < line_stop && dash_start < line_stop && dash_stop < line_stop
    if (line_stop < 0.0)
        line_stop = dx + 1e5;
    
    ivec2 dash_caps = ivec2( roundi(u_dashUniforms.z), roundi(u_dashUniforms.w) );
    float segment_start = v_segment.x;
    float segment_stop  = v_segment.y;
    float segment_center= (segment_start+segment_stop)/2.0;
    
    float freq          = texture2D(s_dash_period, vec2(0.0, u_dashUniforms.y)).x * width;
    float u = mod( dx + u_dashUniforms.x * width,freq );
    vec4 tex = texture2D(s_dash_atlas, vec2(u/freq,  u_dashUniforms.y));
    
    float dash_center= tex.x * width;
    float dash_type  = tex.y;
    float _start = tex.z * width;
    float _stop  = tex.a * width;
    float dash_start = dx - u + _start;
    float dash_stop  = dx - u + _stop;
    
    // Compute extents of the first dash (the one relative to v_segment.x)
    // Note: this could be computed in the vertex shader
    if( (dash_stop < segment_start) && (dash_caps.x != 5) )
    {
        float u = mod(segment_start + u_dashUniforms.x * width, freq);
        vec4 tex = texture2D(s_dash_atlas, vec2(u/freq,  u_dashUniforms.y));
        dash_center= tex.x * width;
        //dash_type  = tex.y;
        float _start = tex.z * width;
        float _stop  = tex.a * width;
        dash_start = segment_start - u + _start;
        dash_stop = segment_start - u + _stop;
    }
    // Compute extents of the last dash (the one relatives to v_segment.y)
    // Note: This could be computed in the vertex shader
    else if( (dash_start > segment_stop)  && (dash_caps.y != 5) )
    {
        float u = mod(segment_stop + u_dashUniforms.x * width, freq);
        vec4 tex = texture2D(s_dash_atlas, vec2(u/freq,  u_dashUniforms.y));
        dash_center= tex.x * width;
        //dash_type  = tex.y;
        float _start = tex.z * width;
        float _stop  = tex.a * width;
        dash_start = segment_stop - u + _start;
        dash_stop  = segment_stop - u + _stop;
    }

    // Check is dash stop is before line start or dash start is beyond line stop
    if( dash_stop <= line_start || (dash_start >= line_stop) ) {
        discard;
    }
    
    // This test if the we are dealing with a discontinuous angle
    bool discontinuous = ((dx <  segment_center) && abs(v_angles.x) > THETA) ||
                         ((dx >= segment_center) && abs(v_angles.y) > THETA);
    // Check if current dash start is beyond segment stop
    if( discontinuous )
    {
        // Dash start is beyond segment or dash stop is before segment
        if( (dash_start > segment_stop) || (dash_stop < segment_start) ) {
            discard;
        }
        
        // Special case for round caps  (nicer with this)
        if( dash_caps.y == 1 && (u < _start) && (dash_start < segment_start )  && (abs(v_angles.x) < PI/2.0) ||
            dash_caps.x == 1 && (u > _stop) && (dash_stop > segment_stop )  && (abs(v_angles.y) < PI/2.0) )
                discard;
        
        // Special case for triangle caps (in & out) and square
        // We make sure the cap stop at crossing frontier
        if( (dash_caps.x != 1) && (dash_caps.x != 5) && (dash_start < segment_start )  && (abs(v_angles.x) < PI/2.0) )
        {
            float a = v_angles.x/2.0;
            float x = (segment_start-dx)*cos(a) - dy*sin(a);
            float y = (segment_start-dx)*sin(a) + dy*cos(a);
            if( x > 0.0 ) discard;
            // We transform the cap into square to avoid holes
            dash_caps.x = 4;
        }
        if( (dash_caps.y != 1) && (dash_caps.y != 5) && (dash_stop > segment_stop )  && (abs(v_angles.y) < PI/2.0) )
        {
            float a = v_angles.y/2.0;
            float x = (dx-segment_stop)*cos(a) - dy*sin(a);
            float y = (dx-segment_stop)*sin(a) + dy*cos(a);
            if( x > 0.0 ) discard;
            // We transform the caps into square to avoid holes
            dash_caps.y = 4;
        }
    }
    bool inner = (dx > line_start) && (dx < line_stop);
    float d_join = join( roundi(u_uniforms.y), dy,
                        v_segment, v_lineCoord, v_miter, u_miter_limit, u_uniforms.x );
    // Line cap at start
    if( (dx < line_start) && (dash_start < line_start) && (dash_stop > line_start) ) {
        d = cap( dash_caps.x, dx-line_start, dy, t);
    }
    // Line cap at stop
    else if( (dx > line_stop) && (dash_stop > line_stop) && (dash_start < line_stop)  ) {
        d = cap( dash_caps.y, dx-line_stop, dy, t);
    }
    // Dash cap left
    else if( dash_type < 0.0 )  {
        d = cap( dash_caps.y, u-dash_center, dy, t);
        if( inner )
            d = max(d,d_join);
    }
    // Dash cap right
    else if( dash_type > 0.0 ) {
        d = cap( dash_caps.x, dash_center-u, dy, t);
        if( inner )
            d = max(d,d_join);
    }
    // Dash body (plain)
    else if( dash_type == 0.0 )
        d = abs(dy);
    
    // Line join
    if( inner )
    {
        if( (dx <= segment_start) && (dash_start <= segment_start) && (dash_stop >= segment_start) )
        {
            d = d_join;
            // Antialias at outer border
            float angle = PI/2.+v_angles.x;
            float f = abs( (segment_start - dx)*cos(angle) - dy*sin(angle));
            d = max(f,d);
        }
        else if( (dx > segment_stop) && (dash_start <= segment_stop)
                && (dash_stop >= segment_stop) )
        {
            d = d_join;
            // Antialias at outer border
            float angle = PI/2.+v_angles.y;
            float f = abs((dx - segment_stop)*cos(angle) - dy*sin(angle));
            d = max(f,d);
        }
        else if( dx < (segment_start - u_uniforms.x) || dx > (segment_stop + u_uniforms.x) )
            discard;
    }
    
#ifdef USE_OUTLINE
    lowp vec4 borderColor = u_color;
    lowp float outlineAntialias  = 0.;
    if(u_outlineWidth > 0.) {
        borderColor = u_outlineColor;
        outlineAntialias = u_antialias;
        t -= u_outlineWidth;
    }
    // Distance to border
    d -= t;
    //fill
    if( d < 0. )
        gl_FragColor = u_color;
    //outline inner border
    else if ( d - outlineAntialias < 0. ) {
        d /= u_antialias;
        float blend = 1. - clamp(exp(-d*d), 0., 1.);
        vec3 colour = mix( u_outlineColor.rgb, u_color.rgb, min(u_color.a, 1. - blend) );
        gl_FragColor = vec4(colour, max(u_color.a, u_outlineColor.a * blend) );
    }
    //outline
    else if( d  - u_outlineWidth < 0. ) {
        gl_FragColor = u_outlineColor;
    }
    //border
    else {
        d -= u_outlineWidth;
        d /= u_antialias;
        gl_FragColor = vec4(borderColor.rgb, exp(-d*d)*borderColor.a);
    }
#else
    // Distance to border
    d -= t;
    if( d < 0.0 )
        gl_FragColor = color;
    else {
        d /= u_antialias;
        gl_FragColor = vec4(color.xyz, exp(-d*d)*color.a);
    }
#endif
}

float cap( lowp int type, float dx, float dy, float t )
{
    float d = 0.0;
    dx = abs(dx);
    dy = abs(dy);
    
    // None
    if      (type == 0)  discard;
    // Round
    else if (type == 1)  d = sqrt(dx*dx+dy*dy);
    // Triangle in
    else if (type == 3)  d = (dx+abs(dy));
    // Triangle out
    else if (type == 2)  d = max(abs(dy),(t+dx-abs(dy)));
    // Square
    else if (type == 4)  d = max(dx,dy);
    // Butt
    else if (type == 5)  d = max(dx+t,dy);
    
    return d;
}

float join( lowp int type,  float d, vec2 segment, vec2 texcoord, vec2 miter,
           float miter_limit, float lineWidthRadius )
{
    float dx = texcoord.x;
    d = abs(d);
    // Round join
    if( type == 1 )
    {
        if (dx < segment.x)
            d = max(d,length( texcoord - vec2(segment.x,0.0)));
        else if (dx > segment.y)
            d = max(d,length( texcoord - vec2(segment.y,0.0)));
    }
    // Bevel join
    else if ( type == 2 )
    {
        if( (dx < segment.x) ||  (dx > segment.y) )
            d = max(d, min(abs(miter.x),abs(miter.y)));
    }
    
    // Miter limit
    if( (dx < segment.x) ||  (dx > segment.y) )
        d = max(d, min(abs(miter.x),abs(miter.y)) - miter_limit*lineWidthRadius );
    
    return d;
}

int roundi( float x )
{
    return int( floor(x + 0.5) );
}
