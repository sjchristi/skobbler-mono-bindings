//
//  lineCap.frag
//  @brief Fragment shader used to render customizable and antialiased line caps.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//
precision mediump float;

uniform mediump vec3    u_uniforms;     //line width radius, start and end cap type
uniform mediump float   u_antialias;    //line antialias radius
uniform lowp vec4       u_color;

varying mediump vec2    v_lineCoord;
varying mediump float   v_lineCap; //flat

float cap( lowp int type, float dx, float dy, float t );

void main() 
{
    float t = u_uniforms.x-u_antialias;
    float d = cap( int(floor(v_lineCap+0.5)), v_lineCoord.x, v_lineCoord.y, t );
    d = d -t;
    // Distance to border
    if( d >= 0.0 )
    {
        d /= u_antialias;
        gl_FragColor = vec4(u_color.rgb, exp(-d*d)*u_color.a);
    }
    else
        gl_FragColor = u_color;
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
    else if (type == 3)  d = (dx+dy);
    // Triangle out
    else if (type == 2)  d = max(abs(dy),(t+dx-dy));
    // Square
    else if (type == 4)  d = max(dx,dy);
    // Butt
    else if (type == 5)  d = max(dx+t,dy);
    
    return d;
}