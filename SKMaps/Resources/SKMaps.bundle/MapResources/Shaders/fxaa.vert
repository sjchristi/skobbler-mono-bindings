//
//  fxaa.vert
//  @brief Vertex shader used to apply AA on an image, FXAA.
//
//  @note The input image must be clamped
//
//  @see:
//  https://docs.google.com/leaf?id=0B2manFVVrzQAOGE3ZjgzMDUtYWZmNC00ZWVlLWFhMGEtZDdjMzMxMjM4M2Y4&authkey=CPWYuLIJ&sort=name&layout=list&num=50
//

#define FXAA_SUBPIX_SHIFT 1.0/4.0

uniform mat4 u_proj_matrix;
uniform vec2 u_size;

attribute highp vec4 a_position;
attribute lowp vec4  a_texCoord;

varying lowp vec4    v_texCoord;
varying mediump vec2 v_pos;
varying mediump vec4 v_posPos;
varying mediump vec4 v_rcpFrameOpt;

void main(void)
{
    vec2 rcpFrame = vec2(1.0/u_size.x, 1.0/u_size.y);
    v_rcpFrameOpt = vec4(2.0/u_size.x, 2.0/u_size.y, 0.5/u_size.x, 0.5/u_size.y);
    //center pixel
    v_pos.xy = a_texCoord.xy;
    //upper left pixel
    v_posPos.xy = v_pos.xy + (rcpFrame * (0.5 + FXAA_SUBPIX_SHIFT));
    //lower right pixel
    v_posPos.zw = v_pos.xy - (rcpFrame * (0.5 + FXAA_SUBPIX_SHIFT));
    v_texCoord = a_texCoord;
    gl_Position = u_proj_matrix * a_position;
}