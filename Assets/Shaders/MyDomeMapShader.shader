// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/DomeMapShader"
{
	Properties
	{
		_VideoTex( "Video Texture", 2D ) = "white" {}
		_ScreenTex( "Screen Texture", 2D ) = "white" {}
		_LightIntensity( "Light Intensity", Color ) = ( 0, 0, 0, 1 )
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

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
					float2 uvVideo : TEXCOORD0;
					float2 uvScreen : TEXCOORD1;
					UNITY_FOG_COORDS( 1 )
					float4 vertex : SV_POSITION;
					float3 lpos : TEXCOORD2;
				};

				sampler2D _VideoTex;
				float4 _VideoTex_ST;
				float4x4 _TextureRotation;

				sampler2D _ScreenTex;
				float4 _ScreenTex_ST;

				float4 _LightIntensity;

				float3 norm( float3 v )
				{
					return v / sqrt( dot( v, v ) );
				}

				v2f vert( appdata vert )
				{
					v2f output;
					output.vertex = UnityObjectToClipPos( vert.vertex );

					float4 rotatedVert = mul( _TextureRotation, vert.vertex );
					output.lpos = rotatedVert;

					float3 normVert = norm( rotatedVert.xyz );

					float phi = acos( normVert.y );
					float theta = atan2( -normVert.x, -normVert.z );

					float r = phi / ( 3.1415 );

					float u = r * cos( theta ) + 0.5;
					float v = r * sin( theta ) + 0.5;

					output.uvVideo = float2( u, v );
					output.uvVideo = TRANSFORM_TEX( output.uvVideo, _VideoTex );
					output.uvScreen = float2( u, v );
					output.uvScreen = TRANSFORM_TEX( output.uvScreen, _ScreenTex );

					UNITY_TRANSFER_FOG( output, output.vertex );
					return output;
				}

				fixed4 frag( v2f input ) : SV_Target
				{
					if( input.lpos.y < 0 )
						discard;
				// sample the texture
				fixed4 colorVideo = tex2D( _VideoTex, input.uvVideo ) * ( float4( 1, 1, 1, 0 ) - _LightIntensity * 0.75 );
				fixed4 colorScreen = tex2D( _ScreenTex, input.uvScreen ) * ( _LightIntensity * 0.75 );

				fixed4 col = colorVideo + colorScreen;

				return col;
			}
			ENDCG
		}
		}
}
