//
//  solidLine.frag
//  @brief Fragment shader used to render antialiased segements of a solid line with customizable join and caps.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//

uniform lowp vec4      u_color;
uniform mediump float  u_miter_limit;
uniform mediump float  u_antialias;    //line antialias radius
uniform mediump vec4   u_uniforms;     //line width and line join
uniform mediump float  u_outline_width;
uniform lowp vec4      u_outline_color;
uniform mediump float  u_znear;        //lowest z value, in the [0,1] range, after which to apply aliasing

varying highp vec2    v_segment;
varying highp float   v_length;   //marks line end
varying mediump float v_start;    //line start
varying highp float   v_dx;
varying mediump float v_dy;
varying mediump vec2  v_miter;

void main() 
{
    //aliasing based on depth
    float antialias = aliasingDepth(gl_FragCoord.z, u_znear, u_antialias);
    float t = u_uniforms.x - antialias;
    float d;
    //line join
    if(v_dx - v_length < 0. && v_dx >= v_start)
        d = join(roundi(u_uniforms.y), v_dy, v_segment, v_dx, v_dy, v_miter, u_miter_limit, u_uniforms.x);
    //line start
    else if( v_dx < v_start)
        d = cap(roundi(u_uniforms.z), v_dx, v_dy, t);
    //line end
    else
        d = cap(roundi(u_uniforms.w), v_dx - v_length, v_dy, t);

    gl_FragColor = lineColor(u_color, u_outline_color, d, t, antialias, u_outline_width);
}
