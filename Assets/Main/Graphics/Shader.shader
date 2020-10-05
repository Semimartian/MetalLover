// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,1)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.09
		_Colour("Colour", Color) = (1,0,0,0)
		_MetalMap("MetalMap", 2D) = "white" {}
		_SmoothMap("SmoothMap", 2D) = "white" {}
		_MetalMultiplier("MetalMultiplier", Float) = 0
		_SmoothMultiplier("SmoothMultiplier", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		
		struct Input {
			half filler;
		};
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform half4 _Colour;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetalMap);
		uniform float4 _MetalMap_ST;
		SamplerState sampler_MetalMap;
		uniform float _MetalMultiplier;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_SmoothMap);
		uniform float4 _SmoothMap_ST;
		SamplerState sampler_SmoothMap;
		uniform float _SmoothMultiplier;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Colour.rgb;
			float2 uv_MetalMap = i.uv_texcoord * _MetalMap_ST.xy + _MetalMap_ST.zw;
			o.Metallic = ( SAMPLE_TEXTURE2D( _MetalMap, sampler_MetalMap, uv_MetalMap ) * _MetalMultiplier ).r;
			float2 uv_SmoothMap = i.uv_texcoord * _SmoothMap_ST.xy + _SmoothMap_ST.zw;
			o.Smoothness = ( SAMPLE_TEXTURE2D( _SmoothMap, sampler_SmoothMap, uv_SmoothMap ) * _SmoothMultiplier ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
0;73.6;1536;710;702.5377;-88.2941;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;9;-231.1105,674.799;Inherit;False;Property;_SmoothMultiplier;SmoothMultiplier;7;0;Create;True;0;0;False;0;False;0;0.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-624.4134,676.1587;Inherit;True;Property;_SmoothMap;SmoothMap;5;0;Create;True;0;0;False;0;False;-1;a51d5c4e4ec56454bb80dba3a9bf8909;a51d5c4e4ec56454bb80dba3a9bf8909;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-688.3638,318.5219;Inherit;True;Property;_MetalMap;MetalMap;4;0;Create;True;0;0;False;0;False;-1;a51d5c4e4ec56454bb80dba3a9bf8909;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-379.5099,478.7426;Inherit;False;Property;_MetalMultiplier;MetalMultiplier;6;0;Create;True;0;0;False;0;False;0;1.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-449.7021,39.36061;Half;False;Property;_Colour;Colour;0;0;Create;True;0;0;False;0;False;1,0,0,0;1,0.3723443,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-26.21053,502.399;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-359.2098,355.4426;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-867.3375,106.0365;Inherit;False;Property;_Smooth;Smooth;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-877.4214,-7.194285;Inherit;False;Property;_Metal;Metal;1;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-889.278,-114.7207;Inherit;False;Property;_ColourFloat;ColourFloat;3;0;Create;True;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;11;-243.4024,-89.80826;Inherit;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;343.2466,40.63498;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.09;0,0,0,1;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;10;0
WireConnection;8;1;9;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;0;0;1;0
WireConnection;0;3;6;0
WireConnection;0;4;8;0
ASEEND*/
//CHKSM=F145F7F3473BFF157E62AC07203E0DE2831D660B