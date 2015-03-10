#include "Matrices.fxh"
#include "Light.fxh"
#include "Material.fxh"
#include "HelperFunctions.fxh"
/*float2 TexelSize
<
	string UIName="Texel Size";
> = {1,1};*/
void phong_shading(
		    float3 LColor,
		    float3 Nn,
		    float3 Ln,
		    float3 Vn,
			float4 Specular,
			float Shininess,
		    out float3 DiffuseContrib,
		    out float3 SpecularContrib)
{
    float3 Hn = normalize(Vn + Ln);
    float4 litV = lit(dot(Ln,Nn),dot(Hn,Nn),Shininess);
    DiffuseContrib = litV.y * LColor;
    SpecularContrib = litV.y * litV.z * Specular.xyz * LColor;
}
struct appdata
{
	float4 Position : POSITION;
	float2 UV		: TEXCOORD0;
	float3 Normal 	: NORMAL;
    float3 Tangent	: TANGENT;
    float3 Binormal	: BINORMAL;
};
struct vout
{
	float4 Position  : POSITION;
	float2 UV		 : TEXCOORD0;
	float3 WorldPos	 : TEXCOORD1;
	//float3 LightVec  : TEXCOORD2;
	float4 LProj 	 : TEXCOORD2;
	float3 Normal 	 : TEXCOORD3;
	float3 Tangent	 : TEXCOORD4;
    float3 Binormal	 : TEXCOORD5;
	
};
struct pout
{
	float4 DiffuseColor : COLOR0; //new total light map
	float4 SpecularColor : COLOR1; //new total specular map
};
vout mainVS(appdata IN)
{
 	vout OUT;
	//preshading
	//camera projection
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	//light projection
	float4x4 LightWorldView=mul(World, LightView);
	float4x4 LightWorldViewProj=mul(LightWorldView, LightProj);
	
	OUT.Position=mul(IN.Position, WorldViewProj);
	OUT.UV=IN.UV;
	OUT.Normal=mul(IN.Normal, (float3x3)World);
	OUT.Tangent=mul(IN.Tangent, (float3x3)World);
	OUT.Binormal=mul(IN.Binormal, (float3x3)World);
	
	OUT.WorldPos=mul(IN.Position, World).xyz;
	//OUT.ViewVec=EyePos-WorldPos;
	//OUT.LightVec=LightPos-WorldPos;
	OUT.LProj=mul(IN.Position, LightWorldViewProj);
	return OUT;
}
vout dispVS(appdata IN)
{
 	vout OUT;
	//preshading
	//camera projection
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	//light projection
	float4x4 LightWorldView=mul(World, LightView);
	float4x4 LightWorldViewProj=mul(LightWorldView, LightProj);
	float height = tex2Dlod(DisplacementSampler, float4(IN.UV.xy, 0, 0)).r;
	IN.Position.y-=height*displacementDepth;
	OUT.Position=mul(IN.Position, WorldViewProj);
	OUT.UV=IN.UV;
	OUT.Normal=mul(IN.Normal, (float3x3)World);
	OUT.Tangent=mul(IN.Tangent, (float3x3)World);
	OUT.Binormal=mul(IN.Binormal, (float3x3)World);
	
	OUT.WorldPos=mul(IN.Position, World).xyz;
	//OUT.ViewVec=EyePos-WorldPos;
	//OUT.LightVec=LightPos-WorldPos;
	OUT.LProj=mul(IN.Position, LightWorldViewProj);
	return OUT;
}
float GetDepth(float4 vLightProj)
{
	return 1-_tex2DProj(ShadowSampler, vLightProj).r;
}
float GetDepthLoop(float4 vLightProj)
{
	return 1-_tex2DProjLoop(ShadowSampler, vLightProj).r;
}
float GetDepthCube(float3 vLight)
{
	return _texCUBE(CubeShadowSampler, normalize(vLight)).r * LightRadius;
}
float GetDepthCubeLoop(float3 vLight)
{
	return _texCUBELoop(CubeShadowSampler, normalize(vLight)).r * LightRadius;
}
float TestShadow(float distance, float depth)
{
	return distance <= depth ? 1.0 : 0.0;
}
float GetBasicShadowSpot(float4 vLightProj)
{
	
	float LightDistance = vLightProj.z/vLightProj.w * Bias;
	float Shadow = vLightProj.w < 0 
		? 0.0 
		: TestShadow(LightDistance, GetDepth(vLightProj));
	return Shadow;
}
float GetBasicShadowCube(float3 vLight)
{
	float LightDistance = length(vLight) * CubeBias;
	float Shadow = LightDistance <= GetDepthCube(vLight)
		? 1.0
		: 0.0;
	return Shadow;
}
float GetPCFShadowSpot(float4 vLightProj)
{
	if(vLightProj.w<0)
		return 0;
	vLightProj/=vLightProj.w;
	float LightDistance = vLightProj.z * Bias;
	float KernelWidth = LightDistance * FilterWidth;
	float Step = KernelWidth / NumShadowSamples;
	float Shadow = 0;
	//filter through the kernel
	for(int x=-NumShadowSamples;x<=NumShadowSamples;++x)
		for(int y=-NumShadowSamples;y<=NumShadowSamples;++y)
		{
			float4 uvProj=vLightProj+float4(x,y,0,0)*Step;
			Shadow+=TestShadow(LightDistance, GetDepthLoop(uvProj));
		}
	float numSamples=(NumShadowSamples*2+1);
	return Shadow/=numSamples*numSamples;
}
float GetPCFShadowCube(float3 vLight)
{
	//if(GetBasicShadowCube(vLight)>0)
	//	return 1.0f;
	float LightDistance=length(vLight);
	float KernelWidth = LightDistance * FilterWidth;
	float Step = KernelWidth / NumShadowSamples;
	float3 v = vLight.x == 0.0 ? float3(1,0,0) : float3(0,1,0);
	float3 tang=normalize(cross(vLight, v));
	float3 bitang=normalize(cross(tang, vLight));
	float Shadow = 0;
	for(int x=-NumShadowSamples;x<=NumShadowSamples;++x)
		for(int y=-NumShadowSamples;y<=NumShadowSamples;++y)
		{
			float3 UV=vLight+(x*tang+y*bitang)*Step;
			Shadow+=TestShadow(length(UV)*CubeBias, GetDepthCubeLoop(UV));
		}
	float numSamples=(NumShadowSamples*2+1);
	return Shadow/=numSamples*numSamples;
}

float3 _GetNorm(vout IN)
{
	return GetNormal(IN.UV, IN.Normal, IN.Tangent, IN.Binormal);
}
float GetAttenuation(float3 distance)
{
	return saturate(1/(LightAttenuation.x
					+LightAttenuation.y*distance
					+LightAttenuation.z*distance*distance));
}
pout GetLightValues(vout IN, float Shadow)
{
	pout OUT;
	//temp vars to hold diffuse/specular values
	float3 DiffuseCont, SpecularCont;
	//get the per pixel values from the textures
	float LightDistance=length(LightPos-IN.WorldPos);
	float3 LightVec=normalize(LightPos-IN.WorldPos);//incident light vector
	float3 ViewVec=normalize(EyePos-IN.WorldPos);//view vector
	float3 Nn=_GetNorm(IN);
	//get the phong shading values
	phong_shading(LightColor, Nn, LightVec, ViewVec, Specular, Shininess, DiffuseCont, SpecularCont);
	//apply attenuation
	Shadow*=GetAttenuation(LightDistance);
	//apply shadows
	DiffuseCont*=Shadow;
	SpecularCont*=Shadow;
	//store and return
	OUT.DiffuseColor=float4(DiffuseCont,1);
	OUT.SpecularColor=float4(SpecularCont,1);
	return OUT;
}
pout GetDebugValues(vout IN, float Shadow)
{
	pout OUT;
	OUT.DiffuseColor=LightColor*Shadow;
	OUT.SpecularColor=float4(0,0,0,1);
	return OUT;
}
//shadow pixel shader
pout basicSpotPS_Bump(vout IN) 
{
	return GetLightValues(IN, GetBasicShadowSpot(IN.LProj));
}
pout basicPointPS_Bump(vout IN)
{
	return GetLightValues(IN, GetBasicShadowCube(IN.WorldPos-LightPos));
}
pout debugSpotPS_Bump(vout IN)
{
	return GetDebugValues(IN, GetBasicShadowSpot(IN.LProj));
}
pout debugPointPS_Bump(vout IN)
{
	return GetDebugValues(IN, GetBasicShadowCube(IN.WorldPos-LightPos));
}
pout PCFSpotPS_Bump(vout IN)
{
	return GetLightValues(IN, GetPCFShadowSpot(IN.LProj));
}
pout PCFPointPS_Bump(vout IN)
{
	return GetLightValues(IN, GetPCFShadowCube(IN.WorldPos-LightPos));
}
technique T_DebugPoint
{
	pass p0
	{
        VertexShader = compile vs_3_0 mainVS();
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
        PixelShader = compile ps_3_0 debugPointPS_Bump();
    }
}
technique T_DebugSpot
{
	pass p0
	{
        VertexShader = compile vs_3_0 mainVS();
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
        PixelShader = compile ps_3_0 debugSpotPS_Bump();
    }
}
technique T_BasicSpot
{
    pass p0
	{
        VertexShader = compile vs_3_0 mainVS();
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
        PixelShader = compile ps_3_0 basicSpotPS_Bump();
    }
}
technique T_BasicPoint
{
    pass p0
	{
        VertexShader = compile vs_3_0 mainVS();
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
        PixelShader = compile ps_3_0 basicPointPS_Bump();
    }
}
technique T_PCFSpot
{
    pass p0
	{
        VertexShader = compile vs_3_0 mainVS();
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
        PixelShader = compile ps_3_0 PCFSpotPS_Bump();
    }
}
technique T_PCFPoint
{
    pass p0
	{
        VertexShader = compile vs_3_0 mainVS();
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
        PixelShader = compile ps_3_0 PCFPointPS_Bump();
    }
}
technique T_PCFPointDisp
{
    pass p0
	{
        VertexShader = compile vs_3_0 dispVS();
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
        PixelShader = compile ps_3_0 PCFPointPS_Bump();
    }
}