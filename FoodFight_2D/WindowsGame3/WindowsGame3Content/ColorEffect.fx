sampler s;
float2 Resolution;
float4 GetHighlight(float4 color, float4 highlight)
{
	float4 invHigh=float4(1,1,1,1)-highlight;
	float4 invLum=1-max(max(color.x,color.y),color.z);
	float4 finalColor=color-invLum*invHigh;
	if(highlight.a<1)
		finalColor.a=length(color.xyz)*highlight.a;
	else finalColor.a=color.a;
	return finalColor;
}
float4 PixelShaderFunction(float2 uv : TEXCOORD0, float4 color : COLOR0) : COLOR0
{
    float4 sColor=tex2D(s,uv);
	return GetHighlight(sColor,color);
}

technique Technique1
{
    pass Pass1
    {
		ZEnable = true;
		ZFunc=LessEqual;
		ZWriteEnable=true;
		AlphaBlendEnable = True;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
