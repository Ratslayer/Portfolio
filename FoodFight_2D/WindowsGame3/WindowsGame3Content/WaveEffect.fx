sampler s;
float Radius;
float Width;
float2 Resolution;
float2 Position;
float4 GetBubble(float2 UV : TEXCOORD0)
{
	float2 centerUV=UV-float2(.5f, .5f);
	float curPos=length(centerUV)-Radius;
	float posSign=sign(curPos);
	curPos=abs(curPos);
	if(curPos<Width)
	{
		float uvCoefficient=Radius-posSign*curPos*(log((Width-curPos)/Width));
		centerUV=normalize(centerUV)*uvCoefficient;
		return tex2D(s, centerUV+float2(.5f, .5f));
	}
	else return tex2D(s, UV);
}
float4 GetPinch(float2 UV : TEXCOORD0)
{
	float2 centerUV=UV-Position/Resolution;//float2(.5f, .5f);
	float2 coeff=Resolution.xy/Resolution.x;
	centerUV*=coeff;
	float curPos=length(centerUV)-Radius;
	float posSign=sign(curPos);
	curPos=abs(curPos);
	if(curPos<Width)
	{
		//float uvCoefficient=Radius+posSign*curPos*(exp((Width-curPos)/Width));
		float uvCoefficient=Radius-posSign*curPos*(log((Width-curPos)/Width));
		centerUV=normalize(centerUV)*uvCoefficient;
		return tex2D(s, centerUV+Position/Resolution);
	}
	else return tex2D(s, UV);
}
float4 PinchPS(float2 UV : TEXCOORD0, float4 color : COLOR) : COLOR0
{
	return GetPinch(UV);
}
float4 BubblePS(float2 UV : TEXCOORD0, float4 color : COLOR) : COLOR0
{
	return GetBubble(UV);
}
technique PinchTechnique
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PinchPS();
    }
}
technique BubbleTechnique
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 BubblePS();
    }
}

