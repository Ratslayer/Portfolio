#include "Matrices.fxh"
#include "Material.fxh"
struct VIN
{
    float4 Position : POSITION0;
	float4 UV 		: TEXCOORD0;
};
struct VOUT
{
	float4 Position : POSITION0;
	float2 UV 		: TEXCOORD0;
};
VOUT stdVS(VIN IN)
{
	VOUT OUT;
	//preshading
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	//vertex transformation
    OUT.Position = mul(IN.Position, WorldViewProj);
	OUT.UV=IN.UV.xy;
	return OUT;
}
VOUT dispVS(VIN IN)
{
	VOUT OUT;
	//preshading
	float4x4 WorldView=mul(World, View);
	float4x4 WorldViewProj=mul(WorldView, Proj);
	//vertex transformation
	float height = tex2Dlod(DisplacementSampler, float4(IN.UV.xy, 0, 0)).r;
	IN.Position.y-=displacementDepth*height;
    OUT.Position = mul(IN.Position, WorldViewProj);
	OUT.UV=IN.UV.xy;
	return OUT;
}
float4 ColorPS() : COLOR
{
	return MaterialColor;
}
float4 DiffusePS(VOUT IN) : COLOR
{
	return tex2D(DiffuseSampler, IN.UV.xy);
}
float4 DiffuseColorPS(VOUT IN) : COLOR
{
	return tex2D(DiffuseSampler, IN.UV.xy) * MaterialColor;
}
float4 DiffuseHighlightPS(VOUT IN) : COLOR
{
	float4 color=/*float4(tex2Dlod(DisplacementSampler, float4(IN.UV.xy, 0, 0)).r,0,0,1);*/tex2D(DiffuseSampler, IN.UV.xy);
	float4 invHigh=float4(1,1,1,1)-MaterialColor;
	float invLum=1-max(max(color.r, color.g), color.b);
	return color-invHigh*invLum;
}
technique T_Color
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 stdVS();
        PixelShader = compile ps_3_0 ColorPS();
		ZEnable=true;
    }
}
technique T_Diffuse
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 stdVS();
        PixelShader = compile ps_3_0 DiffusePS();
		ZEnable=true;
    }
}
technique T_DiffuseColor
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 stdVS();
        PixelShader = compile ps_3_0 DiffuseColorPS();
		ZEnable=true;
		AlphaBlendEnable=false;
    }
}
technique T_DiffuseHighlight
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 stdVS();
        PixelShader = compile ps_3_0 DiffuseHighlightPS();
		ZEnable=true;
		AlphaBlendEnable=false;
    }
}
technique T_DiffuseHighlightDisp
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 dispVS();
        PixelShader = compile ps_3_0 DiffuseHighlightPS();
		ZEnable=true;
		AlphaBlendEnable=false;
    }
}
technique T_Wireframe
{
	pass Pass1
    {
        VertexShader = compile vs_3_0 stdVS();
        PixelShader = compile ps_3_0 ColorPS();
		FillMode=WireFrame;
		ZEnable=true;
		AlphaBlendEnable=false;
		//CullMode=ccw;
    }
}
