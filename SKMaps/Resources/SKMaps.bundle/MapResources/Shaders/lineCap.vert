//
//  lineCap.vert
//  @brief Vertex shader used to render line caps, start and end cap, based on the line radius(half of the line width).
//
//  @note Created to work with a GL_TRIANGLES structure.
//  @note Works in mercator coordinates and transforms to pixels coordinates using u_scale.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//
attribute highp vec4    a_position;
attribute mediump vec2  a_tangents;     //cap tangent
attribute lowp float    a_lineCoord;    //encoding of coordinates of the cap points

uniform mediump vec3    u_uniforms;     //line width radius, start and end cap type
uniform mediump float   u_antialias;    //line antialias radius
uniform lowp vec4       u_color;
uniform highp mat4      u_mvpMatrix;
uniform highp float     u_scale;

varying mediump vec2    v_lineCoord;
varying mediump float   v_lineCap; //flat

void main()
{
    float w = ceil(1.25*u_antialias)/2.0+u_uniforms.x;
    float wMerc = w / u_scale;
    vec4 position = a_position;
    
    lowp int pointEncode = int(abs(a_lineCoord));
    float u = 0.0;
    float v = a_lineCoord / float(pointEncode);
    //cap end points
    if(pointEncode == 2 || pointEncode == 4)
        u = 1.0;
    //start cap
    if(pointEncode == 1 || pointEncode == 2)
        v_lineCap = (u_uniforms.y);
    else
        v_lineCap = (u_uniforms.z);
    
    vec2 o = vec2( +a_tangents.y, -a_tangents.x);
    position.xy += v * wMerc * o;
    position.xy += u * wMerc * a_tangents;
    
    v_lineCoord = vec2(w*u, w*v);
    gl_Position = u_mvpMatrix * position;
}
