Shader "Custom/ToonShaderBasic" {
	Properties
	{
		// Toon color.
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.4,0.4,0.4,1.0)
		// Diffuse.
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
		// Toon ramp.
		_ToonRamp ("Toon Ramp (RGB)", 2D) = "gray" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		CGPROGRAM
		#pragma surface CalculateSurface ToonyColors
		#pragma target 2.0
		#pragma glsl
		// Variables.
		fixed4 _Color;
		sampler2D _MainTex;
		struct Input
		{
			half2 uv_MainTex;
		};
		// Lighting variables.
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _ToonRamp;
		// Custom SurfaceOutput.
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};
		// Lighting toon.
		inline half4 LightingToonyColors (SurfaceOutputCustom surfaceOut, half3 lightDir, half3 viewDir, half atten)
		{
			surfaceOut.Normal = normalize(surfaceOut.Normal);
			fixed ndl = max(0, dot(surfaceOut.Normal, lightDir)*0.5 + 0.5);
			
			fixed3 ramp = tex2D(_ToonRamp, fixed2(ndl,ndl));
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);
			ramp = lerp(_SColor.rgb,_HColor.rgb,ramp);
			fixed4 c;
			c.rgb = surfaceOut.Albedo * _LightColor0.rgb * ramp;
			c.a = surfaceOut.Alpha;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif
			return c;
		}
		// Surface.
		void CalculateSurface (Input IN, inout SurfaceOutputCustom outputCustom)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
			outputCustom.Albedo = mainTex.rgb * _Color.rgb;
			outputCustom.Alpha = mainTex.a * _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}