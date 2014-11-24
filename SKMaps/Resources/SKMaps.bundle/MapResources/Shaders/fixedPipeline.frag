//
//  fixedPipeline.frag
//  @brief Fragment shader used to provide backwards compatibility to ES1(i.e. the fixed pipeline).
//
//  Created by Alin Loghin on 10/02/13.
//  Copyright (c) 2013 Skobbler. All rights reserved.
//
varying lowp vec4 v_texCoord;
varying lowp vec4 v_color; 

uniform lowp sampler2D s_texture; 
uniform bool enable_s_texture; 

void main()  { 
    if (enable_s_texture) 
        gl_FragColor = v_color * texture2D(s_texture, v_texCoord.st); 
    else 
        gl_FragColor = v_color; 
}