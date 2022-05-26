Shader "Custom/TestOutline"
{
    Properties
    {
        _MainTex("Main Texture(RGB)", 2D) = "white"{}
        _Color("Color", Color) = (1,1,1,1)

        _OutlineTex("Outline Texture", 2D) = "white"{}
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth("Outline Width", Range(1.0,10.0)) = 1.1

        
    }
    SubShader
    {
         Pass
        {
            Name "OBJECT"

            //Cull back

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            //includes

            #include "UnityCG.cginc"

            //structures

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

            };

            struct v2f{
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //imports

            float4 _Color;
            sampler2D _MainTex;

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, IN.uv);
                return texColor * _Color;
            }

            ENDCG        
        }



        Pass
        {

            Name "OUTLINE"

            Tags
            {
                "Queue"="Transparent"
                "RenerType"="Opaque"
                //"LightMode"="Outline"

             }

            ZWrite Off
            //Cull front

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            //includes

            #include "UnityCG.cginc"

            //structures

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

            };

            struct v2f{
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //imports

            float _OutlineWidth;
            float4 _OutlineColor;
            sampler2D _OutlineTex;


            v2f vert(appdata IN)
            {
                v2f OUT;

                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                IN.vertex.xyz *= _OutlineWidth;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float4 texColor = tex2D(_OutlineTex, IN.uv);
                return texColor * _OutlineColor;
            }

            ENDCG
    
        }
    }

}
