Shader "Toon/outline_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Space(10)]
        _OutColor ("Outline Color", Color) = (1,1,1,1)
        _OutValue ("Outline Value", Range(0.0,0.2)) = 0.1
    }
    SubShader
    {
        // OUTLINE PASS
        Pass
        {
            // for scy fighting
            Tags{
                "Queue"="Transparent"
            }

            // to get transparency layer, enable
            Blend SrcAlpha OneMinusSrcAlpha
            // deactivate z buffer
            Zwrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // connection variable
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutColor;
            float _OutValue;

            float4 outline(float4 vertexPos, float outValue){
                // pass vertex from vertex input of 4 dimensions
                // outvalue 1 dimension to connect to _OutValue
                // matrix 4x4
                float4x4 scale = float4x4(
                    1+outValue,0,0,0,
                    0,1+outValue,0,0,
                    0,0,1+outValue,0,
                    0,0,0,1+outValue
                );

                return mul(scale,vertexPos);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 vertexPos = outline(v.vertex, _OutValue);

                o.vertex = UnityObjectToClipPos(vertexPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // to apreciate the change of outline use _OutColor instead of col
                // eturn _OutColor;
                return _OutColor; // float4(_OutColor.r, _OutColor.g, _OutColor.b, _OutColor.a);
            }
            ENDCG
        }

        // Texture Pass
        Pass
        {
            // for scy fighting: +1 for ensure the texture is render in the last
            Tags{
                "Queue"="Transparent + 1"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
