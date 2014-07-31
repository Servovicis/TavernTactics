Shader "gargoyle" 
{
	Properties 
	{
		_Color("Color Tint", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Diffuse Texture gloss(A)", 2D) = "white" {}
		//_BumpMap ("Normal Texture", 2D) = "bump" {}
		_EmitMap ("Emission Texture", 2D) = "black" {}
		//_BumpDepth ("Bump Depth", Range(0.0,1.0)) = 1.0
		//_SpecColor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		//_Shininess ("Shininess", Float) = 10
		//_RimColor ("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		//_RimPower ("Rim Power", Range(0.1,10.0)) = 3.0
		_EmitStrength ("Emission Strength", Range(0.0,4.0)) = 0
		_Speed("Speed", float) = 5
		
	}
	SubShader 
	{

		
			Lighting Off
       		ZWrite On
       		Alphatest Greater 0.5
       		Cull Off
       		Blend SrcAlpha OneMinusSrcAlpha
       		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
			LOD 300
		
		
		
			
			CGPROGRAM
       		#pragma surface surf Lambert 
       		
       		uniform sampler2D _MainTex;
			//uniform half4 _MainTex_ST;
			//uniform sampler2D _BumpMap;
			//uniform half4 _BumpMap_ST; 
			uniform sampler2D _EmitMap;
			//uniform half4 _EmitMap_ST;
			uniform fixed4 _Color;
			//uniform fixed4 _SpecColor;
			//uniform fixed4 _RimColor;
			//uniform half _Shininess;
			//uniform half _RimPower;
			//uniform fixed _BumpDepth;
			uniform fixed _EmitStrength;
			float _Speed;
		
			struct Input
        	{
        		float2 uv_MainTex;
        		//float2 uv_BumpMap;
        		float2 uv_EmitMap;
        	
        	};
        	
        	void surf( Input IN, inout SurfaceOutput o)
        	{
        		
        		fixed4 Maintex = tex2D(_MainTex, IN.uv_MainTex);
        		fixed4 Emit = tex2D(_EmitMap, IN.uv_EmitMap)* _EmitStrength;
        		
        		
        		//float4 c = pow((_EmitMap + _Color), _EmitStrength);
        		
        		o.Albedo = Maintex.rgb + Emit;
        		o.Alpha = Maintex.a;
        		
        		
        		
        	}
        	ENDCG
        }
	  
     
}