//
//  solidLineSquare.frag
//  @brief Fragment shader used to render antialiased segements of a solid line.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//

uniform mediump vec2    u_uniforms;  //line width radius(pixels) and joint type(not used)
uniform mediump float   u_antialias; //line antialias radius
uniform lowp vec4       u_color;
uniform mediump float   u_znear;     //lowest z value, in the [0,1] range, after which to apply aliasing

#define USE_FADE_DEPTH

#ifdef USE_TEXTURE
uniform lowp sampler2D  s_texture;

varying mediump vec2    v_texCoord;
#else
varying mediump float   v_dy;
#endif

void main() 
{
#ifndef USE_TEXTURE
    //aliasing based on depth
    mediump float antialias = aliasingDepth(gl_FragCoord.z, u_znear, u_antialias);
    // Distance to border
    mediump float d = abs(v_dy) - (u_uniforms.x - antialias);
    
    lowp vec4 color = u_color;
    if( d >= 0.0 )
    {
        d /= antialias;
        color = vec4(u_color.rgb, exp(-d*d)*u_color.a);
    }
    gl_FragColor = color;
   
#ifdef USE_FADE_DEPTH
    //remove aliasing in perspective view
    if(u_znear>0.0)
    {
        float normZ = viewNormalizedDepth(gl_FragCoord.z, u_znear);
        float width = u_uniforms.x + antialias;
        float depthAntialias = normZ * width;
        float df = abs(v_dy) - (width - depthAntialias);
        if(df >= 0.0)
        {
            df /= depthAntialias;
            gl_FragColor.a *= 1.0 - df;
        }
    }
#endif

#else
    float normZ = viewNormalizedDepth(gl_FragCoord.z, u_znear);
    normZ = clamp(normZ, 0.0, 0.5);
    vec4 color = u_color * texture2D(s_texture, v_texCoord.st);
    gl_FragColor = vec4(color.rgb, color.a * (1.0 - normZ));
#endif
}