// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StandardColorInstanced"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _PrimaryTex ("Albedo (RGB)", 2D) = "white" {}
        _SecondaryTex ("Albedo (RGB)", 2D) = "white" {}
        _TertiaryTex ("Albedo (RGB)", 2D) = "white" {}
        _OverlayTex ("Albedo (RGB)", 2D) = "white" {}
        _Accent1Tex ("Albedo (RGB)", 2D) = "white" {}
        _Accent2Tex ("Albedo (RGB)", 2D) = "white" {}
        _Accent3Tex ("Albedo (RGB)", 2D) = "white" {}
        _Accent4Tex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
		Lighting Off
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }
        LOD 200
		
        CGPROGRAM
			#pragma surface surf NoLighting noambient vertex:vert alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _PrimaryTex;
		sampler2D _SecondaryTex;
		sampler2D _TertiaryTex;
		sampler2D _OverlayTex;
		sampler2D _Accent1Tex;
		sampler2D _Accent2Tex;
		sampler2D _Accent3Tex;
		sampler2D _Accent4Tex;

        struct Input
        {
            float2 uv_MainTex;
			float3 vertexColor;
        };
		
		void vert (inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertexColor = v.color;
		}

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _MainColor)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Primary)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Secondary)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Tertiary)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Overlay)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Accent1)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Accent2)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Accent3)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Accent4)
           UNITY_DEFINE_INSTANCED_PROP(float, _Emission)
        UNITY_INSTANCING_BUFFER_END(Props)
		
		fixed4 LightingNoLighting(SurfaceOutput s, fixed lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 m = UNITY_ACCESS_INSTANCED_PROP(Props, _MainColor);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			c.rgb = m.rgb;
			m.a = (c.a * m.a);
			
			// TODO :: Account for the Overlay Texture.
			
			fixed4 p = tex2D(_PrimaryTex, IN.uv_MainTex);
			fixed4 s = tex2D(_SecondaryTex, IN.uv_MainTex);
			fixed4 t = tex2D(_TertiaryTex, IN.uv_MainTex);
			fixed4 a1 = tex2D(_Accent1Tex, IN.uv_MainTex);
			fixed4 a2 = tex2D(_Accent2Tex, IN.uv_MainTex);
			fixed4 a3 = tex2D(_Accent3Tex, IN.uv_MainTex);
			fixed4 a4 = tex2D(_Accent4Tex, IN.uv_MainTex);
			
			float pa = p.a * UNITY_ACCESS_INSTANCED_PROP(Props, _Primary).a;
			float sa = s.a * UNITY_ACCESS_INSTANCED_PROP(Props, _Secondary).a;
			float ta = t.a * UNITY_ACCESS_INSTANCED_PROP(Props, _Tertiary).a;
			float a1a = a1.a * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent1).a;
			float a2a = a2.a * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent2).a;
			float a3a = a3.a * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent3).a;
			float a4a = a4.a * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent4).a;
			
			float aTotal = pa + sa + ta + a1a + a2a + a3a + a4a;
			if (aTotal > 0)
			{
				c = (pa / aTotal) * UNITY_ACCESS_INSTANCED_PROP(Props, _Primary) + 
					(sa / aTotal) * UNITY_ACCESS_INSTANCED_PROP(Props, _Secondary) + 
					(ta / aTotal) * UNITY_ACCESS_INSTANCED_PROP(Props, _Tertiary) + 
					(a1a / aTotal) * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent1) + 
					(a2a / aTotal) * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent2) + 
					(a3a / aTotal) * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent3) + 
					(a4a / aTotal) * UNITY_ACCESS_INSTANCED_PROP(Props, _Accent4);
			}
			
			float ma = m.a;
			if (aTotal > 0 && aTotal < 1)
			{
				float aAllTotal = aTotal + ma;
				if (aAllTotal > 1)
				{
					ma = ma - (aAllTotal - 1);
				}
				
				c = aTotal * c + (1 - aTotal) * m;
			}
			
			o.Albedo = c.rgb;
			if (aTotal > ma)
			{
				o.Alpha = aTotal;
			}
			else
			{
				o.Alpha = ma;
			}
			
			// TODO :: Remove Emission setting if Emission won't be needed.
			//o.Emission = UNITY_ACCESS_INSTANCED_PROP(Props, _Emission);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
