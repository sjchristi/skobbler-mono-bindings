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
//  solidLine.frag
//  @brief Fragment shader used to render antialiased segements of a solid line with customizable join and caps.
//
precision mediump float;

uniform mediump float  u_miter_limit;
uniform mediump vec4   u_uniforms;  //line width radius(pixels), join type and line caps
uniform mediump float  u_antialias; //line antialias radius
uniform lowp vec4      u_color;
uniform mediump float  u_outlineWidth;
uniform lowp vec4      u_outlineColor;

varying vec2  v_segment;    //can use flat​ qualifier
varying vec2  v_miter;
varying vec2  v_lineCoord;
varying float v_length;     //can use flat​ qualifier

// Compute distance to cap
float cap( lowp int type, float dx, float dy, float t );
// Compute distance to join
float join( lowp int type,  float d, vec2 segment, vec2 texcoord, vec2 miter,
           float miter_limit, float lineWidthRadius );
int roundi( float x );

void main() 
{
    float dx = v_lineCoord.x;
    float dy = v_lineCoord.y;
    float t = u_uniforms.x - u_antialias;
    float d;
    
    float line_stop = v_length;
    //check if set, only set for end points so adjust such that dx < line_stop
    if (line_stop < 0.)
        line_stop = dx + 1e5;
    
    //line start
    if( dx < 0. ) {
        d = cap( roundi(u_uniforms.z), dx, dy, t );
    }
    else if( dx > line_stop ) {
        d = cap( roundi(u_uniforms.w), abs(dx)-line_stop, dy, t );
    }
    else {
        d = join( roundi(u_uniforms.y), dy, v_segment, v_lineCoord,
                 v_miter, u_miter_limit, u_uniforms.x );
    }
    
    lowp vec4 borderColor = u_color;
    lowp float outlineAntialias  = 0.;
    if(u_outlineWidth > 0.) {
        borderColor = u_outlineColor;
        outlineAntialias = u_antialias;
        t -= u_outlineWidth;
    }
    // Distance to border
    d -= t;
    //fill:)
    if( d < 0. )
        gl_FragColor = u_color;
    //outline inner border
    else if ( d - outlineAntialias < 0. ) {
        d /= u_antialias;
        float blend = 1. - clamp(exp(-d*d), 0., 1.);
        vec3 colour = mix( u_outlineColor.rgb, u_color.rgb, min(u_color.a, 1. - blend) );
        gl_FragColor = u_outlineColor.a *  vec4(colour, max(u_color.a, blend) );
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
