//
//  fixedPipeline_alpha.frag
//  @brief Fragment shader used to provide backwards compatibility to ES1(i.e. the fixed pipeline).
//          Needed because the specification of GL ES says:
//          GL_ALPHA Each element is a single alpha component.
//          The GL converts it to floating point and assembles it into an RGBA element by attaching 0 for red, green, and blue.
//
//  @note To be used only for textures in GL_ALPHA format.
//
//  Created by Alin Loghin on 10/02/13.
//  Copyright (c) 2013 Skobbler. All rights reserved.
//
varying lowp vec4 v_texCoord; 
varying lowp vec4 v_color; 

uniform lowp sampler2D s_texture;  

void main()  { 
    //ignore the RGB values from the texture as they will be 0,0,0
    gl_FragColor = vec4( v_color.rgb, v_color.a * texture2D(s_texture, v_texCoord.st).a ); 
}