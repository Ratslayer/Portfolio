float DiffuseFactor
<
 string UIName = "Diffuse Factor";
> = 0;
float3 EtaRatio
<
 string UIName = "Refraction Coefficient";
> = {0.5, 0.6, 0.7};
float FresnelPower
<
 string UIName = "Fresnel Power";
> = 10;
float FresnelScale
<
 string UIName = "Fresnel Scale";
> = 0.5;
float FresnelBias
<
 string UIName = "Fresnel Bias";
> = 0.01;
textureCUBE EnvTexture
<
    string ResourceName = "default_reflection.dds";
    string UIName = "Environment";
    string ResourceType = "Cube";
>;
samplerCUBE EnvSampler = sampler_state
{
    Texture = <EnvTexture>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};