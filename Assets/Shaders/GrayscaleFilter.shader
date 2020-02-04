Shader "Hidden/GrayscaleFilter"
{
  Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Gray1("Gray - Forumal1", int) = 0
		_Gray2("Gray - Forumal2", int) = 0
		_Gray3("Gray - Forumal3", int) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			int _Gray1;
			int _Gray2;
			int _Gray3;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 color = fixed4(0,0,0,1);
				float value;
				if(_Gray1 == 1){
					value = (max(col.r,max(col.g, col.b)) + min(col.r,max(col.g,col.b))) / 2;
					color = fixed4(value,value,value,1);
				}else if(_Gray2 == 1){
					value = (col.r + col.g + col.b) / 3.;
					color = fixed4(value,value,value,1);
				}else if(_Gray3 == 1){
					value = 0.21 * col.r + 0.72 * col.g + 0.07 * col.b;
					color = fixed4(value,value,value,1);
				}else{
					return col;
				}
				return color;
			}
			ENDCG
		}
	}
	// Reference https://gist.github.com/CptAsgard/edfffdcd73f2979bb09a 
}