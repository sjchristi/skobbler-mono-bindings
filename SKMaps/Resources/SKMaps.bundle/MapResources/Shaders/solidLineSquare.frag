//
//  solidLineSquare.frag
//  @brief Fragment shader used to render antialiased segements of a solid line.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//
precision mediump float;

uniform mediump vec2    u_uniforms;  //line width radius(pixels) and joint type(not used)
uniform mediump float   u_antialias; //line antialias radius
uniform lowp vec4       u_color;

varying mediump float  v_lineCoord;

void main() 
{
    float t = u_uniforms.x-u_antialias;
    float d = abs(v_lineCoord) - t;
    // Distance to border
    if( d >= 0.0 )
    {
        d /= u_antialias;
        gl_FragColor = vec4(u_color.rgb, exp(-d*d)*u_color.a);
    }
    else
        gl_FragColor = u_color;
}