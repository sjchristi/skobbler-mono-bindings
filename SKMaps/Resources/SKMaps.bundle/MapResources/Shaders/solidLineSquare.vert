//
//  solidLineSquare.vert
//  @brief Vertex shader used to modify the geometry of line segments(ie. joints) based on the line radius(half of the line width).
//  Creates only square joints.
//
//  @note Created to work with a GL_TRIANGLES_STRIP structure.
//  @note Works in world coordinates and transforms to pixels coordinates using u_scale.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//
attribute highp vec4    a_position;
attribute mediump float a_tangents;     //joint tangent, polar form
attribute mediump float a_lineCoord;    //parametric coordinates of the polyline
#ifdef USE_TEXTURE
attribute mediump float a_segment;      //nromalized line lengths per segment
#endif

uniform mediump vec2    u_uniforms;     //line width radius(pixels) and join type(not used)
uniform mediump float   u_line_offset;  //line offset(pixels)
uniform mediump float   u_antialias;    //line antialias radius
uniform lowp vec4       u_color;
uniform highp mat4      u_mvp_matrix;
uniform highp float     u_scale;

#ifdef USE_TEXTURE
uniform mediump vec2    u_tiling;       //u and v tilling factor

varying mediump vec2    v_texCoord;
#else
varying mediump float   v_dy;
#endif

void main()
{
#ifdef USE_TEXTURE
    mediump float w = u_uniforms.x;
#else
    mediump float w = ceil(1.25*u_antialias)/2.0+u_uniforms.x;
#endif
    highp float wWorld = w / u_scale;
    highp vec4 position = a_position;
    //normalize as it's the amplitude of the tangent
    mediump float v = a_lineCoord / abs(a_lineCoord);
    mediump vec2 tangent = vec2(cos(a_tangents), sin(a_tangents));
    position.xy += (u_line_offset / u_scale) * tangent;
    position.xy += a_lineCoord * wWorld * tangent;
    
#ifdef USE_TEXTURE
    if(v < 0.0)
        v_texCoord.t = 0.0;
	else
		v_texCoord.t = 1.0 * u_tiling.t;
	v_texCoord.s = a_segment * u_tiling.s;
#else
    v_dy = w*v;
#endif
    
    gl_Position = u_mvp_matrix * position;
}
