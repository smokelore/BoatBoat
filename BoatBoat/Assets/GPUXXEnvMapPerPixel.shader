// Aaron Lanterman, June 21, 2014
// Cobbled together from numerous sources, particularly The Cg Turtorial
Shader "Reflective/Mesh Water" {
	Properties {
		_MainTex("Main Texture (RGBA)", 2D) = ""
		_Cube ("Reflection Cubemap", CUBE) = "white" {}
		//_Alpha("Alpha", Range(0,1)) = 1
    	_etaRatio ("Eta Ratio", Range(0.01,3)) = 1
    	_crossfade ("Crossfade", Range(0,1)) = 0
    	_crossfade2 ("Crossfade2", Range(0,1)) = 0
	}
		
	SubShader {
			Pass {
				CGPROGRAM
	      			#include "UnityCG.cginc"
	      			// includes #define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw) 
		 
					#pragma vertex vert_envmappervertex
					#pragma fragment frag_envmappervertex
				 
				 	uniform sampler2D _MainTex;
				 	uniform float4 _MainTex_ST;
				 	uniform float _Alpha;
				 	uniform samplerCUBE _Cube;	
				 	uniform float _etaRatio;
				 	uniform float _crossfade;
				 	uniform float _crossfade2;
				 						 	
				 	struct a2v {						// application to vertex
				 		float4 v: POSITION;
				 		float3 n: NORMAL;	
				 		float2 tc: TEXCOORD0;
				 	};
				 	
				 	
					struct v2f {				// vertex to fragment
				 		float4 sv: SV_POSITION;	 
				 		float3 nWorld: TEXCOORD0;
				 		float3 vWorldPos: TEXCOORD1; 
				 		float2 tc: TEXCOORD2;

				 	};

					v2f vert_envmappervertex(a2v input) {
						v2f output;										 
						output.sv = mul(UNITY_MATRIX_MVP, input.v);
						 // To transform normals, we want to use the inverse transpose of upper left 3x3
						// Putting input.n in first argument is like doing trans((float3x3)_World2Object) * input.n;
						output.vWorldPos = mul(_Object2World, input.v).xyz;
						output.nWorld = normalize(mul(input.n, (float3x3) _World2Object));
						output.tc = TRANSFORM_TEX(input.tc, _MainTex);
						return output;
					}

		 			float4 frag_envmappervertex(v2f input) : COLOR {
    	 				// incident is opposite the direction of eyeDir in our other programs
    	 				float3 incidentWorld = normalize(input.vWorldPos - _WorldSpaceCameraPos.xyz);
    	 				float3 reflectWorld = reflect(incidentWorld,input.nWorld);
    					float3 refractWorld = refract(incidentWorld,input.nWorld, _etaRatio);
    							
    					float4 reflectColor = texCUBE(_Cube, reflectWorld);
    	 				float4 refractColor = texCUBE(_Cube, refractWorld);
    					//float reflectFactor = saturate(_fresnelBias +
    					//			 _fresnelScale * pow(1 + dot(incidentWorld, input.nWorld), _fresnelPower));
    						
    					// Diagnostic: uncomment next lines to code refraction as yellow & reflection as blue
    					// refractColor = float4(1,1,0,1); 
    					// reflectColor = float4(0,0,1,1);
    				
    					// Comment out one of the following two lines and leave the other as desired	
    					float4 one = lerp(reflectColor, refractColor, _crossfade);

	    				float4 two = tex2D(_MainTex, input.tc);
	    				float4 both = lerp(one, two, _crossfade2);

	    				return(both);
	  				}
				ENDCG
			}
	}
}
