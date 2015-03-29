/*============================================================================
 
 
 NVIDIA FXAA III.8 by TIMOTHY LOTTES
 
 
 ------------------------------------------------------------------------------
 COPYRIGHT (C) 2010, 2011 NVIDIA CORPORATION. ALL RIGHTS RESERVED.
 ------------------------------------------------------------------------------
 TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, THIS SOFTWARE IS PROVIDED
 *AS IS* AND NVIDIA AND ITS SUPPLIERS DISCLAIM ALL WARRANTIES, EITHER EXPRESS
 OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF
 MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL NVIDIA
 OR ITS SUPPLIERS BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT, OR
 CONSEQUENTIAL DAMAGES WHATSOEVER (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR
 LOSS OF BUSINESS PROFITS, BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION,
 OR ANY OTHER PECUNIARY LOSS) ARISING OUT OF THE USE OF OR INABILITY TO USE
 THIS SOFTWARE, EVEN IF NVIDIA HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH
 DAMAGES.
 ============================================================================*/
//
//  fxaa.frag
//  @brief Fragment shader used to apply FXAA to a given image.
//
precision mediump float;

// 1 = Use discard on pixels which don't need AA.
//     For APIs which enable concurrent TEX+ROP from same surface.
// 0 = Return unchanged color on pixels which don't need AA.
#ifndef FXAA_DISCARD
#define FXAA_DISCARD 0
#endif

// Controls algorithm's early exit path.
// Turning this off on console will result in a more blurry image.
// So this defaults to on.
//
// 1 = On.
// 0 = Off.
#ifndef FXAA_EARLY_EXIT
#define FXAA_EARLY_EXIT 1
#endif

// Consoles the sharpness of edges.
//
// Due to the PS3 being ALU bound,
// there are only two safe values here: 4 and 8.
// These options use the shaders ability to a free *|/ by 4|8.
//
// 8.0 is sharper
// 4.0 is softer
#ifndef FXAA_CONSOLE_EDGE_SHARPNESS
#define FXAA_CONSOLE_EDGE_SHARPNESS 8.0
#endif

// The minimum amount of local contrast required to apply algorithm.
// The console setting has a different mapping than the quality setting.
//
// This only applies when FXAA_EARLY_EXIT is 1.
//
// Due to the PS3 being ALU bound,
// there are only two safe values here: 0.25 and 0.125.
// These options use the shaders ability to a free *|/ by 4|8.
//
// 0.125 leaves less aliasing, but is softer
// 0.25 leaves more aliasing, and is sharper
#ifndef FXAA_CONSOLE_EDGE_THRESHOLD
#define FXAA_CONSOLE_EDGE_THRESHOLD 0.125
#endif

// Trims the algorithm from processing darks.
// The console setting has a different mapping than the quality setting.
//
// This only applies when FXAA_EARLY_EXIT is 1.
#ifndef FXAA_CONSOLE_EDGE_THRESHOLD_MIN
#define FXAA_CONSOLE_EDGE_THRESHOLD_MIN 0.05
#endif

uniform lowp sampler2D s_texture;

varying lowp vec4 v_texCoord;
varying mediump vec2 v_pos;
varying mediump vec4 v_posPos;
varying mediump vec4 v_rcpFrameOpt;

vec4 FxaaPixelShader(
                     // {xy} = center of pixel
                     vec2 pos,
                     // {xy__} = upper left of pixel
                     // {__zw} = lower right of pixel
                     vec4 posPos,
                     // {rgb_} = color in linear or perceptual color space
                     // {___a} = alpha output is junk value
                     sampler2D tex,
                     // This must be from a constant/uniform.
                     // {x___} = 2.0/screenWidthInPixels
                     // {_y__} = 2.0/screenHeightInPixels
                     // {__z_} = 0.5/screenWidthInPixels
                     // {___w} = 0.5/screenHeightInPixels
                     vec4 rcpFrameOpt
                     );
void main()
{
    gl_FragColor = FxaaPixelShader(v_pos, v_posPos, s_texture, v_rcpFrameOpt);
}

vec4 FxaaPixelShader(vec2 pos, vec4 posPos, sampler2D tex, vec4 rcpFrameOpt)
{
    /*--------------------------------------------------------------------------*/
    vec4 dir;
    dir.y = 0.0;
    vec4 lumaNe = texture2D(tex, posPos.zy);
    lumaNe.w += float(1.0/384.0);
    dir.x = -lumaNe.w;
    dir.z = -lumaNe.w;
    /*--------------------------------------------------------------------------*/
    vec4 lumaSw = texture2D(tex, posPos.xw);
    dir.x += lumaSw.w;
    dir.z += lumaSw.w;
    /*--------------------------------------------------------------------------*/
    vec4 lumaNw = texture2D(tex, posPos.xy);
    dir.x -= lumaNw.w;
    dir.z += lumaNw.w;
    /*--------------------------------------------------------------------------*/
    vec4 lumaSe = texture2D(tex, posPos.zw);
    dir.x += lumaSe.w;
    dir.z -= lumaSe.w;
    /*==========================================================================*/
#if (FXAA_EARLY_EXIT == 1)
    vec4 rgbyM = texture2D(tex, pos.xy);
    /*--------------------------------------------------------------------------*/
    float lumaMin = min(min(lumaNw.w, lumaSw.w), min(lumaNe.w, lumaSe.w));
    float lumaMax = max(max(lumaNw.w, lumaSw.w), max(lumaNe.w, lumaSe.w));
    /*--------------------------------------------------------------------------*/
    float lumaMinM = min(lumaMin, rgbyM.w);
    float lumaMaxM = max(lumaMax, rgbyM.w);
    /*--------------------------------------------------------------------------*/
    if((lumaMaxM - lumaMinM) < max(FXAA_CONSOLE_EDGE_THRESHOLD_MIN, lumaMax * FXAA_CONSOLE_EDGE_THRESHOLD))
#if (FXAA_DISCARD == 1)
        discard;
#else
    return rgbyM;
#endif
    
#endif
    /*==========================================================================*/
    vec4 dir1_pos;
    dir1_pos.xy = normalize(dir.xyz).xz;
    float dirAbsMinTimesC = min(abs(dir1_pos.x), abs(dir1_pos.y)) * FXAA_CONSOLE_EDGE_SHARPNESS;
    /*--------------------------------------------------------------------------*/
    vec4 dir2_pos;
    dir2_pos.xy = clamp(dir1_pos.xy / dirAbsMinTimesC, -2.0, 2.0);
    dir1_pos.zw = pos.xy;
    dir2_pos.zw = pos.xy;
    vec4 temp1N;
    temp1N.xy = dir1_pos.zw - dir1_pos.xy * rcpFrameOpt.zw;
    /*--------------------------------------------------------------------------*/
    temp1N = texture2D(tex, temp1N.xy);
    vec4 rgby1;
    rgby1.xy = dir1_pos.zw + dir1_pos.xy * rcpFrameOpt.zw;
    /*--------------------------------------------------------------------------*/
    rgby1 = texture2D(tex, rgby1.xy);
    rgby1 = (temp1N + rgby1) * 0.5;
    /*--------------------------------------------------------------------------*/
    vec4 temp2N;
    temp2N.xy = dir2_pos.zw - dir2_pos.xy * rcpFrameOpt.xy;
    temp2N = texture2D(tex, temp2N.xy);
    /*--------------------------------------------------------------------------*/
    vec4 rgby2;
    rgby2.xy = dir2_pos.zw + dir2_pos.xy * rcpFrameOpt.xy;
    rgby2 = texture2D(tex, rgby2.xy);
    rgby2 = (temp2N + rgby2) * 0.5;
    /*--------------------------------------------------------------------------*/
#if (FXAA_EARLY_EXIT == 0)
    float lumaMin = min(min(lumaNw.w, lumaSw.w), min(lumaNe.w, lumaSe.w));
    float lumaMax = max(max(lumaNw.w, lumaSw.w), max(lumaNe.w, lumaSe.w));
#endif
    rgby2 = (rgby2 + rgby1) * 0.5;
    /*--------------------------------------------------------------------------*/
    bool twoTapLt = rgby2.w < lumaMin;
    bool twoTapGt = rgby2.w > lumaMax;
    /*--------------------------------------------------------------------------*/
    if(twoTapLt || twoTapGt) rgby2 = rgby1;
    /*--------------------------------------------------------------------------*/
    return rgby2;
}
