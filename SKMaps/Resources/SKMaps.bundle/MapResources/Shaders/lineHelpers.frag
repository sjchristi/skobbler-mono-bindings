//
//  lineHelpers.frag
//  @brief Fragment shader functions used to render customizable and antialiased line caps and joins.
//
float cap( lowp int type, highp float dx, float dy, float t )
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

float join( lowp int type,  float d, vec2 segment, highp float dx, float dy, vec2 miter,
           float miter_limit, float lineWidthRadius )
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
        d = max(d, min(abs(miter.x),abs(miter.y)) - miter_limit*lineWidthRadius );
    
    return d;
}

int roundi( float x )
{
    return int( floor(x + 0.5) );
}

//
float viewNormalizedDepth(float z, float zlimit)
{
    //avoid branching
    return step(zlimit, gl_FragCoord.z) *  step(0.05, zlimit) * (gl_FragCoord.z - zlimit) / (1.0 - zlimit);
}

//get aliasing value based on depth
float aliasingDepth(float z, float zlimit, float aliasRadius)
{
    float normZ = viewNormalizedDepth(z, zlimit);
    return mix(aliasRadius, 4.0 * aliasRadius, normZ*normZ); // quadratic in function
}
