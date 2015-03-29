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

uniform vec4 u_color;
uniform bool enable_v_color;
uniform float u_point_size;
uniform mat4 u_mvp_matrix;
uniform mat4 u_tex_matrix;

varying lowp vec4 v_texCoord;
varying lowp vec4 v_color;

void main()
{
	v_texCoord = u_tex_matrix * a_texCoord;
    if (enable_v_color)
        v_color = a_color;
    else
        v_color = u_color;
    
    gl_PointSize = u_point_size;
    gl_Position = u_mvp_matrix * a_position;
}