//================================================================================================================================
//
//  Copyright (c) 2015-2021 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

Shader "EasyAR/CameraImage_BGR"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4x4 _projection;

            v2f vert(appdata i)
            {
                v2f o;
                o.vertex = mul(_projection, i.vertex);
                if (_ProjectionParams.x < 0) // check if the render target is a texture rather than the screen, https://docs.unity3d.com/Manual/SL-PlatformDifferences.html
                {
                    o.vertex.y = -o.vertex.y;
                }
                o.texcoord = float2(i.texcoord.x, 1.0 - i.texcoord.y); // Texture2D.LoadRawTextureData follows OpenGL convention and will invert Y-axis for uncompressed data on all platforms
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.texcoord).bgra;
#ifndef UNITY_COLORSPACE_GAMMA
                col.xyz = GammaToLinearSpace(col.xyz);
#endif
                return col;
            }
        ENDCG
    }
    }
}
