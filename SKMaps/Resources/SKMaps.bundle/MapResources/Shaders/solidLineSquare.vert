//
//  solidLineSquare.vert
//  @brief Vertex shader used to modify the geometry of line segments(ie. joints) based on the line radius(half of the line width).
//  Creates only square joints.
//
//  @note Created to work with a GL_TRIANGLES_STRIP structure.
//  @note Works in mercator coordinates and transforms to pixels coordinates using u_scale.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//
attribute highp vec4    a_position;
attribute mediump vec2  a_tangents;     //joint tangent
attribute lowp float    a_lineCoord;    //parametric coordinates of the polyline

uniform mediump vec2    u_uniforms;  //line width radius(pixels) and joint type(not used)
uniform mediump float   u_antialias;    //line antialias radius
uniform lowp vec4       u_color;
uniform highp mat4      u_mvpMatrix;
uniform highp float     u_scale;

varying mediump float   v_lineCoord;

void main()
{
    float w = ceil(1.25*u_antialias)/2.0+u_uniforms.x;
    float wMerc = w / u_scale;
    vec4 position = a_position;
    
    mediump float v = a_lineCoord;
    position.xy += v * wMerc * a_tangents;
    
    v_lineCoord = w*v;
    gl_Position = u_mvpMatrix * position;
}
