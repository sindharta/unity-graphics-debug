Shader "Shin/GraphicsDebug/BuiltIn/Shadow" {
	Properties 
	{
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		pass {		
			Tags { "LightMode"="ForwardBase"}
			cull back

			CGPROGRAM

			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest           
            #pragma only_renderers d3d11 d3d9 opengl gles

			#pragma vertex ShadowVS
			#pragma fragment ShadowPS
			#pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            
            struct PS_IN {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD2;
            };

//---------------------------------------------------------------------------------------------------------------------

            PS_IN ShadowVS(appdata_base v) {
                PS_IN o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

//---------------------------------------------------------------------------------------------------------------------

            float4 ShadowPS(PS_IN input) : COLOR {
                float atten = 1.0;
#if defined (SHADOWS_SCREEN)
                atten = tex2D(_ShadowMapTexture, input.uv);
#endif
                return float4(atten,atten,atten, 1.0);
            }

			ENDCG
		}		

	}
}