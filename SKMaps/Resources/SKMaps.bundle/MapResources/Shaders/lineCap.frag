//
//  lineCap.frag
//  @brief Fragment shader used to render customizable and antialiased line caps.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//

uniform mediump vec3    u_uniforms;     //line width radius, start and end cap type
uniform mediump float   u_antialias;    //line antialias radius
uniform lowp vec4       u_color;
uniform mediump float   u_znear;        //lowest z value, in the [0,1] range, after which to apply aliasing

varying mediump vec2    v_lineCoord;
varying mediump float   v_capType;

void main() 
{
    //aliasing based on depth
    mediump float antialias = aliasingDepth(gl_FragCoord.z, u_znear, u_antialias);
    mediump float t = u_uniforms.x - antialias;
    mediump float d = cap( roundi(v_capType), v_lineCoord.x, v_lineCoord.y, t );
    d -= t;
    // Distance to border
    if( d >= 0.0 )
    {
        d /= antialias;
        gl_FragColor = vec4(u_color.rgb, exp(-d*d)*u_color.a);
    }
    else
        gl_FragColor = u_color;
}
