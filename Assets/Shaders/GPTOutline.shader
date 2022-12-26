Shader "Custom/GPTOutline"
{
    Properties
    {
        _Color ("Outline Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            ZTest Always
            Cull front
            ZWrite Off

            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG

        }
    }
}