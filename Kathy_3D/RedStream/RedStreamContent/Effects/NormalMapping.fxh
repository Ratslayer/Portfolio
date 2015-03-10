float Bump
<
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 3.0;
    float UIStep = 0.01;
    string UIName =  "Bumpiness";
> = 1.0;
texture NormalTexture
<
    string ResourceName = "Default_bump_normal.dds";
    string UIName =  "Normal-Map Texture";
    string ResourceType = "2D";
>;
sampler2D NormalSampler = sampler_state {
    Texture = <NormalTexture>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

float3 GetNormal(float2 UV, float3 normal, float3 tangent, float3 binormal)
{
	float3 Nn = normalize(normal);
	float3 Tn = normalize(tangent);
    float3 Bn = normalize(binormal);
	float3 bump = Bump * (tex2D(NormalSampler, UV).rgb - float3(0.5,0.5,0.5));
    Nn = Nn + bump.x*Tn + bump.y*Bn;
	normalize(Nn);
	return Nn;
}