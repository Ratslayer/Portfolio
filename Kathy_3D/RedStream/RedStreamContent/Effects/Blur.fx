#include "PostProcess.fxh"
float4 downsamplePS(vout IN) : COLOR
{
	float2 ts=TexelSize*2.0f;
	return (tex2D(Sampler1, IN.UV)
		   +tex2D(Sampler1, IN.UV+float2(ts.x,0))
		   +tex2D(Sampler1, IN.UV+float2(0,ts.y))
		   +tex2D(Sampler1, IN.UV+ts))*0.25;
}
float4 lightPassPS(vout IN) : COLOR
{
	float4 color=tex2D(Sampler1, IN.UV);
	return color.r<LightCutoff
			&&color.g<LightCutoff
			&&color.b<LightCutoff
		? float4(0,0,0,1)
		: color;
}
float4 get1DGaussBlur(float2 uv)
{
	float4 color=tex2D(Sampler1,uv)*WT_NORMALIZE;
	float2 dir = BlurDir;
	float2 step = dir;
	for(int i=0;i<NUMWT;i++)
	{
		float gaussFactor=Gauss[i]*WT_NORMALIZE;
		color+=tex2D(Sampler1,uv+step)*gaussFactor;
		color+=tex2D(Sampler1,uv-step)*gaussFactor;
		step+=dir;
	}
	return color;
}
float4 gaussianBlurPS(vout IN) : COLOR
{
	return get1DGaussBlur(IN.UV);
}
technique T_Debug
{
	pass p0
	{
		VertexShader = compile vs_3_0 debugVS();
		PixelShader = compile ps_3_0 downsamplePS();
	}
}
technique T_LightPass
{
	pass p0
	{
		VertexShader = compile vs_3_0 postProcessVS();
		PixelShader = compile ps_3_0 lightPassPS();
	}
}
technique T_Downsample
{
	pass p0
	{
		VertexShader = compile vs_3_0 postProcessVS();
		PixelShader = compile ps_3_0 downsamplePS();
	}
}
technique T_GaussianBlur
{
	pass p0
	{
		VertexShader = compile vs_3_0 blurPostProcessVS();
		PixelShader = compile ps_3_0 gaussianBlurPS();
	}
}