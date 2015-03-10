sampler s;
float2 Resolution;
float2 texelSize;
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
	float4 color=tex2D(s, uv)+tex2D(s, uv+float2(texelSize.x,0))+tex2D(s, uv+float2(0,texelSize.y))+tex2D(s, uv+texelSize);
	color*=.25f;
    return color;
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 VS();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
