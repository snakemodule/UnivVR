Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Front

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 lpos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float4x4 _TextureRotation;

			float3 norm(float3 v)
			{
				return v / sqrt(dot(v, v));
			}

			v2f vert (appdata vert)
			{
				v2f output;
				output.vertex = UnityObjectToClipPos(vert.vertex);

				float4 rotatedVert = mul(_TextureRotation, vert.vertex);
				output.lpos = rotatedVert;

				float3 normVert = norm(rotatedVert.xyz);

				float phi = acos(normVert.y);
				float theta = atan2(-normVert.x, -normVert.z);

				float r = phi / (3.1415);

				float u = r * cos(theta) + 0.5;
				float v = r * sin(theta) + 0.5;

				//output.uv = TRANSFORM_TEX(v.uv, _MainTex);
				output.uv = TRANSFORM_TEX(float2(u, (1-v)), _MainTex);
				
				UNITY_TRANSFER_FOG(output, output.vertex);
				return output;
			}
			
			fixed4 frag (v2f input) : SV_Target
			{
				if (input.lpos.y < 0)
					discard;

				// sample the texture
				fixed4 col = tex2D(_MainTex, input.uv);
				// apply fog
				//UNITY_APPLY_FOG(input.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
