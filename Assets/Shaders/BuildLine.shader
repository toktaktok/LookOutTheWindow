Shader "Custom/BuildLine"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OutlineTex("Outline Texture", 2D) = "white" {}
        _OutlineWidth("Outline Width", Float) = 0.1
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RednerType"="Transparent"
            "Lightmode" = "Outline"
        }

        LOD 200
        Cull front
        

        pass
        {
            Name "OUTLINE"

            ZWrite Off

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0


            //float _OutlineWidth;
            //float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _OutlineWidth;
            float4 _OutlineColor;
            sampler2D _OutlineTex;

            v2f vert(appdata IN)
            {
  	            IN.vertex.xyz *= _OutlineWidth;
				v2f OUT;

				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
			{
				float4 texColor = tex2D(_OutlineTex, IN.uv);

				return texColor * _OutlineColor;
			}


            ENDCG
        }
        

        Pass
		{
			Name "OBJECT"

			CGPROGRAM // Allows talk between two languages : shader lab and nvidia C for graphics

			#pragma vertex vert // Define for the building function
			#pragma fragment frag // Define for coloring function

			#include "UnityCG.cginc" // Built in shader functions

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

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
	}
}