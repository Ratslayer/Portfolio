#define NUMWT 9
float Gauss[NUMWT]={0.93f,.8f,.7f,.6f,.5f,.4f,.3f,.2f,.1f};
#define WT_NORMALIZE 1.0f/(1.0f+2.0f*(0.93f+.8f+.7f+.6f+.5f+.4f+.3f+.2f+.1f))
float LightCutoff = 0.7f;
float2 BlendFactor
<
	string UIName="Blend Factor";
> = {1,1};
float2 TexelSize
<
	string UIName="Texel Size";
> = {1,1};
float2 BlurDir
<
	string UIName="Blur Direction";
> = {1,1};
float2 Scale;
texture Texture1
<
    string ResourceName = "Default_color.dds";
    string UIName =  "Texture 1";
    string ResourceType = "2D";
>;
texture Texture2
<
    string ResourceName = "Default_bump_normal.dds";
    string UIName =  "Texture 2";
    string ResourceType = "2D";
>;
texture Texture3
<
    string ResourceName = "Default_bump_normal.dds";
    string UIName =  "Texture 3";
    string ResourceType = "2D";
>;

sampler Sampler1 = sampler_state
{
    Texture = <Texture1>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};
sampler Sampler2 = sampler_state
{
    Texture = <Texture2>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};
sampler Sampler3 = sampler_state
{
    Texture = <Texture3>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct appdata
{
	float4 Pos : POSITION;
	float2 UV  : TEXCOORD0;
};
struct vout
{
	float4 Pos : POSITION;
	float2 UV  : TEXCOORD0;
};
vout debugVS(appdata IN)
{
	vout OUT;
	OUT.Pos=IN.Pos;
	OUT.UV=IN.UV;
	return OUT;
}
vout blurPostProcessVS(appdata IN)
{
        vout OUT;

	OUT.Pos=IN.Pos;
	//OUT.Pos.xy+=float2(.5,.5);
	OUT.Pos.xy/=Scale;
	OUT.Pos.xy*=2;
	OUT.Pos.xy-=float2(1,1);
	OUT.Pos.y*=-1.0f;

	OUT.UV=IN.UV;
        return OUT;
}
vout postProcessVS(appdata IN)
{
	vout OUT;

	OUT.Pos=IN.Pos;
	OUT.Pos.xy-=float2(.5,.5);
	OUT.Pos.xy/=Scale;
	OUT.Pos.xy*=2;
	OUT.Pos.xy-=float2(1,1);
	OUT.Pos.y*=-1.0f;
	
	OUT.UV=IN.UV;
	
	return OUT;
}
