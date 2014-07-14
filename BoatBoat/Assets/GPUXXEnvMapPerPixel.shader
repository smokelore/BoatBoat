// Aaron Lanterman, June 21, 2014
// Cobbled together from numerous sources, particularly The Cg Turtorial
Shader "GPUXX/EnvMapPerPixel" {
	Properties {
		_Cube ("Reflection Cubemap", CUBE) = "white" {}
    	_etaRatio ("Eta Ratio", Range(0.01,3)) = 1.5
    	_crossfade ("Crossfade", Range(0,1)) = 0
    	_fresnelBias ("Fresnel Bias", Range(0,1)) = 0.5
    	_fresnelScale ("Fresnel Scale", Range(0,1)) = 0.5
    	_fresnelPower ("Fresnel Power", Range(0,10)) = 0.5
	}
		
	SubShader {
			Pass {
				CGPROGRAM
	      			#include "UnityCG.cginc"
	      			// includes #define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw) 
		 
					#pragma vertex vert_envmapperpixel
					#pragma fragment frag_envmapperpixel
				 
				 	uniform samplerCUBE _Cube;		
				 	uniform float _etaRatio;
				 	uniform float _crossfade;
				 	uniform float _fresnelBias;
				 	uniform float _fresnelScale;
				 	uniform float _fresnelPower;
				 						 	
				 	struct a2v {						// application to vertex
				 		float4 v: POSITION;
				 		float3 n: NORMAL;	
				 	};
				 	
				 	
					struct v2f {				// vertex to fragment
				 		float4 sv: SV_POSITION;	 
				 		float3 nWorld: TEXCOORD0;
				 		float3 vWorldPos: TEXCOORD1; 

				 	};

					v2f vert_envmapperpixel(a2v input) {
						v2f output;										 
						output.sv = mul(UNITY_MATRIX_MVP, input.v);
						 // To transform normals, we want to use the inverse transpose of upper left 3x3
						// Putting input.n in first argument is like doing trans((float3x3)_World2Object) * input.n;
						output.vWorldPos = mul(_Object2World, input.v).xyz;
						output.nWorld = normalize(mul(input.n, (float3x3) _World2Object));
						return output;
					}

		 			float4 frag_envmapperpixel(v2f input) : COLOR {
    	 				// incident is opposite the direction of eyeDir in our other programs
    	 				float3 incidentWorld = normalize(input.vWorldPos - _WorldSpaceCameraPos.xyz);
    	 				float3 reflectWorld = reflect(incidentWorld,input.nWorld);
    					float3 refractWorld = refract(incidentWorld,input.nWorld,_etaRatio);
    							
    					float4 reflectColor = texCUBE(_Cube, reflectWorld);
    	 				float4 refractColor = texCUBE(_Cube, refractWorld);
    					float reflectFactor = saturate(_fresnelBias +
    								 _fresnelScale * pow(1 + dot(incidentWorld, input.nWorld), _fresnelPower));
    						
    					// Diagnostic: uncomment next lines to code refraction as yellow & reflection as blue
    					// refractColor = float4(1,1,0,1); 
    					// reflectColor = float4(0,0,1,1);
    				
    					// Comment out one of the following two lines and leave the other as desired	
    					// return(lerp(reflectColor, refractColor, _crossfade));
	    				return(lerp(refractColor, reflectColor, reflectFactor));
	  				}
				ENDCG
			}
	}
}
