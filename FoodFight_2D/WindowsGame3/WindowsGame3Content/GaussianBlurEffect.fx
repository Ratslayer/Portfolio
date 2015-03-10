sampler s;
float2 dir;
float2 Resolution;
#define NUMWT 9
float Gauss[NUMWT]={0.93f,.8f,.7f,.6f,.5f,.4f,.3f,.2f,.1f};
#define WT_NORMALIZE 1.0f/(1.0f+2.0f*(0.93f+.8f+.7f+.6f+.5f+.4f+.3f+.2f+.1f))
float4 Blur(float2 uv)
{
	float4 color=tex2D(s,uv)*WT_NORMALIZE;
	float2 step=dir;
	for(int i=0;i<NUMWT;i++)
	{
		float gaussFactor=Gauss[i]*WT_NORMALIZE;
		color+=tex2D(s,uv+step)*gaussFactor;
		color+=tex2D(s,uv-step)*gaussFactor;
		step+=dir;
	}
	return color;
}
struct input
{
	float4 Pos : POSITION;
	float2 UV : TEXCOORD;
	float4 Color : COLOR;
};
struct vout
{
	float4 Pos : POSITION;
	float2 UV : TEXCOORD0;
	float2 Pos2D : TEXCOORD1;
	float4 Color : COLOR;
};
vout VS(input IN)
{
	vout OUT;
	OUT.Pos=IN.Pos;
	//OUT.Pos.xy-=float2(.5,.5);
	OUT.Pos.xy/=Resolution;
	OUT.Pos.xy*=2;
	OUT.Pos.xy-=float2(1,1);
	OUT.Pos.y*=-1.0f;

	OUT.Pos2D=OUT.Pos.xy;
	OUT.UV=IN.UV;
	OUT.Color=IN.Color;
	return OUT;
}
float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
    return Blur(uv);
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 VS();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
