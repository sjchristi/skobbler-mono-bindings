//
//  solidLine.frag
//  @brief Fragment shader used to render antialiased segements of a solid line with customizable join and caps.
//
//  Created by Alin Loghin on 01/12/14.
//  Copyright (c) 2014 Skobbler. All rights reserved.
//
precision mediump float;

#ifdef BLEND_MODE
#extension GL_EXT_shader_framebuffer_fetch : require
#else
#define  BLEND_MODE -1 //no blending
#endif

#if (BLEND_MODE == 0) //overlay
#define BlendOverlay(a, b) ( (b<0.5) ? (2.0*b*a) : (1.0-2.0*(1.0-a)*(1.0-b)) )

lowp vec4 blend(lowp vec4 sourceColor)
{
    lowp vec4 destColor = gl_LastFragData[0];
    lowp vec4 color;

    color.r = BlendOverlay(sourceColor.r, destColor.r);
    color.g = BlendOverlay(sourceColor.g, destColor.g);
    color.b = BlendOverlay(sourceColor.b, destColor.b);
    color.a = sourceColor.a;
    
    return color;
}
#elif (BLEND_MODE == 1) // difference
lowp vec4 blend(lowp vec4 sourceColor)
{
    lowp vec4 destColor = gl_LastFragData[0];
    lowp vec4 color;
    
    color = abs( destColor - sourceColor );
    
    return color;
}
#elif (BLEND_MODE == 2) // mask
lowp vec4 blend(lowp vec4 sourceColor)
{
    lowp vec4 color = sourceColor;
    if(sourceColor.a > 0.1 && sourceColor.a < 0.9)
    {
        lowp vec3 destColor = gl_LastFragData[0].rgb;
        lowp float luminance = max(max(destColor.r, destColor.g), destColor.b);
        destColor = vec3(luminance);
        color.rgb = sourceColor.rgb * sourceColor.a + destColor.rgb * (1.0 - sourceColor.a);
        color.a = 1.0;
    }
    
    return color;
}
#else //none
lowp vec4 blend(lowp vec4 sourceColor)
{
    return sourceColor;
}
#endif

uniform lowp vec4      u_color;
uniform mediump float  u_miter_limit;
uniform mediump float  u_antialias;    //line antialias radius
uniform mediump vec4   u_uniforms;     //line width and line join
uniform mediump float  u_outline_width;
uniform lowp vec4      u_outline_color;
uniform mediump float  u_znear;        //lowest z value, in the [0,1] range, after which to apply aliasing

varying highp vec2    v_segment;
varying highp float   v_length;   //marks line end
varying mediump float v_start;    //line start
varying highp float   v_dx;
varying mediump float v_dy;
varying mediump vec2  v_miter;

// forward declarations
float cap( lowp int type, highp float dx, float dy, float t );
float join( lowp int type,  float d, vec2 segment, highp float dx, float dy, vec2 miter, float miter_limit, float linewidth );
int roundi( float x );
float aliasingDepth(float z, float zlimit, float aliasRadius);

void main() 
{
    //aliasing based on depth
    float antialias = aliasingDepth(gl_FragCoord.z, u_znear, u_antialias);
    float t = u_uniforms.x - antialias;
    float d;
    //line join
    if(v_dx - v_length < 0. && v_dx >= v_start)
        d = join( roundi(u_uniforms.y), v_dy, v_segment, v_dx, v_dy, v_miter, u_miter_limit, u_uniforms.x );
    //line start
    else if( v_dx < v_start)
        d = cap( roundi(u_uniforms.z), v_dx, v_dy, t );
    //line end
    else
        d = cap( roundi(u_uniforms.w), v_dx - v_length, v_dy, t );

    lowp vec4 borderColor = u_color;
    lowp float outlineAntialias  = 0.;
    if(u_outline_width > 0.)
    {
        borderColor = u_outline_color;
        outlineAntialias = 1.5*antialias;
        t -= u_outline_width;
    }
    
    // Distance to border
    d -= t;
    //fill
    if( d < 0. )
        gl_FragColor = blend(u_color);
    //outline inner border
    else if ( d - outlineAntialias < 0. )
    {
        d /= antialias;
        //blend foreground(u_color) and background(u_outline_color)
        lowp vec3 colour = u_color.rgb * u_color.a + u_outline_color.rgb * (1.0 - u_color.a);
        lowp float blendVal = exp(-d*d);
        colour = mix(u_outline_color.rgb, colour, blendVal);
        gl_FragColor = vec4(colour.rgb, u_color.a * blendVal + u_outline_color.a);
    }
    //outline
    else if( d  - u_outline_width < 0. )
    {
        gl_FragColor = blend(u_outline_color);
    }
    //border
    else
    {
        d -= u_outline_width;
        d /= antialias;
        gl_FragColor = vec4(borderColor.rgb, exp(-d*d)*borderColor.a);
    }
}
