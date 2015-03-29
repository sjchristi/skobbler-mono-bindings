//
//  fixedPipeline.frag
//  @brief Fragment shader used to provide backwards compatibility to ES1(i.e. the fixed pipeline).
//
//  Created by Alin Loghin on 10/02/13.
//  Copyright (c) 2013 Skobbler. All rights reserved.
//

uniform lowp sampler2D s_texture;
uniform bool enable_s_texture;

varying lowp vec4 v_texCoord;
varying lowp vec4 v_color; 

void main()
{
#ifdef USE_GL_ALPHA_SUPPORT
    //Needed because the specification of GL ES says:
    //GL_ALPHA Each element is a single alpha component.
    //The GL converts it to floating point and assembles it into an RGBA element by attaching 0 for red, green, and blue.
    //Thus ignore the RGB values from the texture
    gl_FragColor = vec4( v_color.rgb, v_color.a * texture2D(s_texture, v_texCoord.st).a );
#else
    if (enable_s_texture)
        gl_FragColor = v_color * texture2D(s_texture, v_texCoord.st); 
    else 
        gl_FragColor = v_color;
#endif
}