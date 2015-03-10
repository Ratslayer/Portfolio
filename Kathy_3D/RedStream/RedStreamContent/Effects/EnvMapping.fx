#include "Matrices.fxh"
#include "Material.fxh"
#include "EnvMapping.fxh"
#include "HelperFunctions.fxh"

struct appdata
{
	float4 Position : POSITION;
	float2 UV		: TEXCOORD;
	float3 Normal	: NORMAL;
	float3 Tangent	: TANGENT;
	float3 Binormal	: BINORMAL;
};
struct vout
{
	float4 Position	: POSITION;
	float2 UV		: TEXCOORD0;
	float3 Normal	: TEXCOORD1;
	float3 Tangent	: TEXCOORD2;
	float3 Binormal	: TEXCOORD3;
	float3 WorldPos	: TEXCOORD4;
};
vout bumpVS(appdata IN)
{
	vout OUT;
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	
	OUT.Position=mul(IN.Position, WorldViewProj);
	OUT.UV=IN.UV;
	OUT.Normal=mul(IN.Normal, (float3x3)World);
	OUT.Tangent=mul(IN.Tangent, (float3x3)World);
	OUT.Binormal=mul(IN.Binormal, (float3x3)World);
	OUT.WorldPos=mul(IN.Position, World).xyz;
	return OUT;
};

float3 _GetNorm(vout IN)
{
	return GetNormal(IN.UV, IN.Normal, IN.Tangent, IN.Binormal);
}

float4 reflectionBumpPS(vout IN) : COLOR
{
	float3 Nn=_GetNorm(IN);
	float3 Ray=normalize(IN.WorldPos-EyePos);
	float3 UVW=reflect(Ray, Nn);
	float3 diffuseColor = tex2D(DiffuseSampler,IN.UV).rgb;
	float3 envColor=_texCUBE(EnvSampler, UVW).xyz;
    float3 result = lerp(envColor, diffuseColor, DiffuseFactor);
    // return as float4
    return float4(result,1);
}
float4 fresnelBumpPS(vout IN) : COLOR
{
	float3 Nn=_GetNorm(IN);
	float3 Ray=normalize(IN.WorldPos-EyePos);
	//refraction
	float3 CRefract;
	CRefract.r=_texCUBE(EnvSampler, refract(Ray, Nn, EtaRatio.r)).r;
	CRefract.g=_texCUBE(EnvSampler, refract(Ray, Nn, EtaRatio.g)).g;
	CRefract.b=_texCUBE(EnvSampler, refract(Ray, Nn, EtaRatio.b)).b;
	//reflection
	float3 CReflect=_texCUBE(EnvSampler, reflect(Ray, Nn));
	//fresnel factor
#pragma warning( push )
#pragma warning( disable : 3571 )
	float ReflectionFactor=saturate(FresnelBias+FresnelScale*abs(pow(1+dot(Ray,Nn),FresnelPower)));
#pragma warning( pop )
	//fresnel color
	float3 CFresnel=lerp(CRefract, CReflect, ReflectionFactor);
	//diffuse color
	float3 CDiffuse = tex2D(DiffuseSampler, IN.UV).rgb;
	//final color
	float3 FinalColor = lerp(CFresnel, CDiffuse, DiffuseFactor);
	return float4(FinalColor * MaterialColor, 1);
}
float4 directPS(vout IN) : COLOR
{
	float3 Ray=IN.WorldPos;
	float3 Color=_texCUBE(EnvSampler, Ray);
	return float4(Color, 1);
}
technique T_Debug
{
	pass p0
	{
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
		VertexShader=compile vs_3_0 bumpVS();
		PixelShader=compile ps_3_0 fresnelBumpPS();
	}
}
technique T_Reflection
{
	pass p0
	{
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
		VertexShader=compile vs_3_0 bumpVS();
		PixelShader=compile ps_3_0 reflectionBumpPS();
	}
}
technique T_Fresnel
{
	pass p0
	{
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
		VertexShader=compile vs_3_0 bumpVS();
		PixelShader=compile ps_3_0 fresnelBumpPS();
	}
}
technique T_Direct
{
	pass p0
	{
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
		VertexShader=compile vs_3_0 bumpVS();
		PixelShader=compile ps_3_0 directPS();
	}
}

