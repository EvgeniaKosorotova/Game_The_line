// unlit, vertex colour, alpha blended
// cull off

Shader "tk2d/BlendVertexColorGrayscaleColorize" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_GrayscaleFactor ( "GrayscaleFactor", Range (0,1.0)) = 0
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off Lighting Off Cull Off Fog { Mode Off } Blend SrcAlpha OneMinusSrcAlpha
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_vct
			#pragma fragment frag_mult 
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float _GrayscaleFactor;

			struct vin_vct 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_vct
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			v2f_vct vert_vct(vin_vct v)
			{
				v2f_vct o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag_mult(v2f_vct i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);

				float totalBrightness = col.r + col.g + col.b;


				col = float4((totalBrightness / 3) * _GrayscaleFactor + col.r * (1 - _GrayscaleFactor), 
				(totalBrightness / 3) * _GrayscaleFactor + col.g * (1 - _GrayscaleFactor),
				(totalBrightness / 3) * _GrayscaleFactor + col.b * (1 - _GrayscaleFactor),
				col.a);

				col.rgb += i.color.rgb;
				col.a *= i.color.a;

				return col;
			}
			
			ENDCG
		} 
	}
}
