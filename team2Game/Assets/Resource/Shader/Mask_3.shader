Shader "Unlit/Mask_3"
{
	Properties{
		_Texture("Texture",2D) = "white"{}
		_Float("float",float) = 1
		_Color("Color",Color) = (1,1,1,1)
		_Range("Range",Range(-10,10)) = 0.5
		_Vector("ector",Vector) = (0,0,0,0)
	}
		SubShader
		{
			
			Tags{
				"RenderType" = "Transparent"
				"Queue" = "Geometry"
			}
			Pass
			{
			Stencil{
		Ref 2
		// ステンシルは常に成功
		Comp Always
			// ステンシルに成功したら2に置き換える
			Pass Replace
			
	}
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _Texture;
				float4 _Texture_ST;

				fixed _Float;
				fixed4 _Color;
				fixed _Range;
				fixed4 _Vector;
				struct appdata
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					float4 vec = v.vertex;
					o.vertex = UnityObjectToClipPos(vec);
					o.uv = TRANSFORM_TEX(v.texcoord, _Texture);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{

					fixed4 tex = tex2D(_Texture,i.uv)*_Color;
					return tex;
				}

					
				ENDCG
			}
		}
}