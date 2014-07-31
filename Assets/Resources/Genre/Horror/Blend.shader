     Shader "CrossFade"
    {
      Properties
      {
        _Blend ( "Blend", Range ( 0, 1 ) ) = 0.5
        _Color ( "Main Color", Color ) = ( 1, 1, 1, 1 )
        _MainTex("Main Texture", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump"{}
        _EmitMap("Emission", 2D) = ""{}
        _BumpIntensity("Normal Intensity", Range(0,3)) = 1
        _EmitStrength ("Emission Strength", Range(0.0,2.0)) = 0
        _Texture1 ( "Texture 1", 2D ) = "" {}
        _Texture2 ( "Texture 2", 2D ) = "" {}
      }

      SubShader
      {
      	Lighting Off
        ZWrite On
       	Alphatest Greater 0.5
       	Cull Off
       	Blend SrcAlpha OneMinusSrcAlpha
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}//{ Queue=Transparent }
        LOD 300
        Pass
        {
        	SetTexture[_MainTex]
          	SetTexture[_Texture1]
          	{
          		Combine previous, texture
          	}
          	SetTexture[_Texture2]
          {
            ConstantColor ( 0, 0, 0, [_Blend] )
           
            Combine texture Lerp( constant ) previous
          }    
          SetTexture [_MainTex]
          {
          	combine previous * primary DOUBLE 
          }
        }
      
        CGPROGRAM
        #pragma surface surf Lambert decal:blend
        
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _Texture1;
        sampler2D _Texture2;
        sampler2D _EmitMap;
        fixed _EmitStrength;
        float _BumpIntensity;
        fixed4 _Color;
        float _Blend;
        
        struct Input
        {
        	float2 uv_MainTex;
        	float2 uv_BumpMap;
        	float2 uv_EmitMap;
        	float2 uv_Texture1;
          	float2 uv_Texture2;
        };
        
        void surf ( Input IN, inout SurfaceOutput o )
        {
        	
        	fixed4 pt = tex2D(_MainTex, IN.uv_MainTex);//*_Color;
        	float3 normalMap = tex2D(_BumpMap, IN.uv_BumpMap);
        	//float4 Emit = tex2D(_EmitMap, IN.uv_EmitMap)*_EmitStrength;
          	fixed4 t1  = tex2D( _Texture1, IN.uv_Texture1 )* _Color ;//* _EmitStrength ;
          	fixed4 t2  = tex2D ( _Texture2, IN.uv_Texture2 ) ;
          	fixed4 change = lerp(t1, t2, _Blend);
          	
          	normalMap = float3(normalMap.x * _BumpIntensity, normalMap.y * _BumpIntensity, normalMap.z);
          	
          	o.Albedo  =  pt.rgb + change.rgb;
          	o.Alpha  = change.a;
          	o.Normal = normalMap.rgb;
        }
        ENDCG
      }
      FallBack "Diffuse"
    }