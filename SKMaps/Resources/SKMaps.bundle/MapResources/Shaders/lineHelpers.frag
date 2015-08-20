//
//  lineHelpers.frag
//  @brief Fragment shader functions used to render customizable and antialiased line caps, joins, blending and other.
//

#ifdef BLEND_MODE
#extension GL_EXT_shader_framebuffer_fetch : require
#else
#define  BLEND_MODE -1 //no blending
#endif

#if (BLEND_MODE == 0) //overlay
#define BlendOverlay(a, b) ( (b<0.5) ? (2.0*b*a) : (1.0-2.0*(1.0-a)*(1.0-b)) )

lowp vec4 blend(in lowp vec4 sourceColor)
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
lowp vec4 blend(in lowp vec4 sourceColor)
{
    lowp vec4 destColor = gl_LastFragData[0];
    lowp vec4 color;
    
    color = abs( destColor - sourceColor );
    
    return color;
}
#elif (BLEND_MODE == 2) // mask
lowp vec4 blend(in lowp vec4 sourceColor)
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
#define blend(sourceColor) sourceColor
#endif

float cap(in lowp int type, in float dx, in float dy, in float t)
{
    float d;
    dx = abs(dx);
    dy = abs(dy);
    
    // Round
    if (type == 1)
        d = sqrt(dx*dx+dy*dy);
    // Triangle in
    else if (type == 3)
        d = (dx+dy);
    // Triangle out
    else if (type == 2)
        d = max(dy,(t+dx-dy));
    // Square
    else if (type == 4)
        d = max(dx,dy);
    // Butt
    else if (type == 5)
        d = max(dx+t,dy);
    // None
    else
        d = 0.0;
    
    return d;
}

float join(in lowp int type, in float d, in vec2 segment, in float dx, in float dy, in vec2 miter,
           in float miter_limit, in float width_radius)
{
    d = abs(d);
    // Round join
    if( type == 1 )
    {
        if (dx < segment.x)
            d = max(d,length(vec2(dx, dy) - vec2(segment.x,0.0)));
        else if (dx > segment.y)
            d = max(d,length(vec2(dx, dy) - vec2(segment.y,0.0)));
    }
    // Bevel join
    else if ( type == 2 )
    {
        if( (dx < segment.x) ||  (dx > segment.y) )
            d = max(d, min(abs(miter.x),abs(miter.y)));
    }
    
    // Miter limit
    if( (dx < segment.x) ||  (dx > segment.y) )
        d = max(d, min(abs(miter.x),abs(miter.y)) - miter_limit*width_radius );
    
    return d;
}

int roundi(in float x)
{
    return int( floor(x + 0.5) );
}

float viewNormalizedDepth(in float z, in float zlimit)
{
    //avoid branching
    return step(zlimit, gl_FragCoord.z) *  step(0.05, zlimit) * (gl_FragCoord.z - zlimit) / (1.0 - zlimit);
}

//get anti-aliasing value based on depth
float aliasingDepth(in float z, in float zlimit, in float antialias)
{
    float normZ = viewNormalizedDepth(z, zlimit);
    return mix(antialias, 4.0 * antialias, normZ*normZ); // quadratic in function
}

lowp vec4 lineColor(in lowp vec4 fill_color, in lowp vec4 outline_color, in float d, in float t, in float antialias, in float outline_width)
{
    lowp vec4 color = blend(fill_color);
    
    t -= step(0.0, outline_width) * outline_width;
    // Distance to border
    d -= t;
    //fill
    if( d >= 0.0 )
    {
        lowp vec4 borderColor = fill_color;
        lowp float outline_antialias  = 0.;
        if(outline_width > 0.)
        {
            borderColor = outline_color;
            outline_antialias = 1.5 * antialias;
        }
        
        //outline inner border
        if ( d - outline_antialias < 0.0 )
        {
            d /= antialias;
            //blend foreground(fill_color) and background(outline_color)
            lowp vec3 blendColor = fill_color.rgb * fill_color.a + outline_color.rgb * (1.0 - fill_color.a);
            lowp float blendVal = exp(-d*d);
            blendColor = mix(outline_color.rgb, blendColor, blendVal);
            color = vec4(blendColor.rgb, fill_color.a * blendVal + outline_color.a);
        }
        //outline
        else if( d  - outline_width < 0.0 )
        {
            color = blend(outline_color);
        }
        //border
        else
        {
            d -= outline_width;
            d /= antialias;
            color = vec4(borderColor.rgb, exp(-d*d)*borderColor.a);
        }
    }

    return color;
}
