//
//  fixedPipeline.vert
//  @brief Vertex shader used to provide backwards compatibility to ES1(i.e. the fixed pipeline).
//
//  Created by Alin Loghin on 10/02/13.
//  Copyright (c) 2013 Skobbler. All rights reserved.
//
attribute highp vec4 a_position;
attribute lowp vec4 a_texCoord;
attribute lowp vec4 a_color;

varying lowp vec4 v_texCoord;
varying lowp vec4 v_color;

uniform vec4 u_color;
uniform bool enable_v_color;
uniform float u_pointSize;
uniform mat4 u_mvpMatrix;
uniform mat4 u_texMatrix;

void main()  { 
    gl_Position = u_mvpMatrix * a_position;
	v_texCoord = u_texMatrix * a_texCoord;
    if (enable_v_color)
        v_color = a_color;
    else
        v_color = u_color;
    gl_PointSize = u_pointSize;
}