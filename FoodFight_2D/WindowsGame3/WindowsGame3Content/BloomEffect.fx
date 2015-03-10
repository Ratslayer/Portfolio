sampler s;
texture bloomTexture;
sampler bloom=sampler_state
{
	Texture=<bloomTexture>;
};
float bloomFactor;
float2 Resolution;
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
    float4 c=tex2D(s, uv);
	float4 b=tex2D(bloom,uv);
    return c+b*bloomFactor;
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 VS();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
