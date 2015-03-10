#include "PostProcess.fxh"

float4 additivePS(vout IN) : COLOR
{
	float3 color1=tex2D(Sampler1, IN.UV).xyz;
	float3 color2=tex2D(Sampler2, IN.UV).xyz;
	return float4(color1*BlendFactor.x+color2*BlendFactor.y,1);
}
float4 multiplicativePS(vout IN) : COLOR
{
	float3 color1=tex2D(Sampler1, IN.UV).xyz;
	float3 color2=tex2D(Sampler2, IN.UV).xyz;
	return float4(color1*color2,1);
}
float4 lightBlendPS(vout IN) : COLOR
{
	float3 diffuse=tex2D(Sampler1, IN.UV).xyz;
	float3 light=tex2D(Sampler2, IN.UV).xyz;
	float3 specular=tex2D(Sampler3, IN.UV).xyz;
	return float4(diffuse*light+specular,1);
}
technique T_Basic_Debug
{
	pass p0 
	{	
		VertexShader = compile vs_3_0 debugVS();
		PixelShader = compile ps_3_0 lightBlendPS();
	}
}
technique T_Additive
{
	pass p0 
	{	
		VertexShader = compile vs_3_0 postProcessVS();
		PixelShader = compile ps_3_0 additivePS();
	}
}
technique T_Multiplicative
{
	pass p0 
	{	
		VertexShader = compile vs_3_0 postProcessVS();
		PixelShader = compile ps_3_0 multiplicativePS();
	}
}
technique T_LightBlend
{
	pass p0 
	{	
		VertexShader = compile vs_3_0 postProcessVS();
		PixelShader = compile ps_3_0 lightBlendPS();
	}
}
