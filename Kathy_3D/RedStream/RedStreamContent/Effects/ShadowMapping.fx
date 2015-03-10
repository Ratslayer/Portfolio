#include "Matrices.fxh"
#include "Material.fxh"
#include "Light.fxh"
struct appdata
{
	float4 Pos : POSITION;
	float2 UV : TEXCOORD0;
};
struct vout
{
	float4 Pos : POSITION;
	float  Depth : TEXCOORD0;
};
struct voutCube
{
	float4 Pos : POSITION;
	float3 LightVec : TEXCOORD0;
};
vout mainVS(appdata IN)
{
	vout OUT;
	//preshading
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	//vertex transformation
	OUT.Pos=mul(IN.Pos, WorldViewProj);
	OUT.Depth=OUT.Pos.z/OUT.Pos.w;
	return OUT;
}
voutCube mainCubeVS(appdata IN)
{
	voutCube OUT;
	//preshading
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	//vertex transformation
	OUT.Pos=mul(IN.Pos, WorldViewProj);
	//float3 WorldPos=mul(IN.Pos, World);
	OUT.LightVec=mul(IN.Pos, WorldView).xyz;//WorldPos-LightPos;
	return OUT;
}
voutCube dispCubeVS(appdata IN)
{
	voutCube OUT;
	//preshading
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	//vertex transformation
	float height = tex2Dlod(DisplacementSampler, float4(IN.UV.xy, 0, 0)).r;
	IN.Pos.y-=height*displacementDepth;
	OUT.Pos=mul(IN.Pos, WorldViewProj);
	//float3 WorldPos=mul(IN.Pos, World);
	OUT.LightVec=mul(IN.Pos, WorldView).xyz;//WorldPos-LightPos;
	return OUT;
}
float4 mainPS(vout IN) : COLOR
{
	return float4(1-IN.Depth,0,0,1);
}
float4 mainCubePS(voutCube IN) : COLOR
{
	//preshader
	float invRadius=1.0/LightRadius;
	//distance
	float dist=length(IN.LightVec);
	return float4(dist*invRadius,0,0,1);
}
technique T_Spot 
{
	pass p0 
	{
		AlphaBlendEnable=false;	
		ZEnable=true;
		ZFunc=LessEqual;
		VertexShader = compile vs_3_0 mainVS();
		PixelShader = compile ps_3_0 mainPS();
	}
}
technique T_Cube
{
	pass p0 
	{
		AlphaBlendEnable=false;	
		ZEnable=true;
		ZFunc=LessEqual;
		VertexShader = compile vs_3_0 mainCubeVS();
		PixelShader = compile ps_3_0 mainCubePS();
	}
}
technique T_CubeHeight
{
	pass p0 
	{
		AlphaBlendEnable=false;	
		ZEnable=true;
		ZFunc=LessEqual;
		VertexShader = compile vs_3_0 dispCubeVS();
		PixelShader = compile ps_3_0 mainCubePS();
	}
}