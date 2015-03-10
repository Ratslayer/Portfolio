float4 LightColor
<
    string UIName =  "Light Color";
    string UIWidget = "Color";
> = {1.0f,1.0f,1.0f,1.0f};
float3 LightAttenuation = {1,1,1};
float Bias = .999;
float CubeBias = .99;
float LightRadius
<
    string UIName =  "Light Radius";
> = 1000.0f;
int NumShadowSamples = 2;
float FilterWidth = 0.01f;
//iterative textures
//those are updated after every pass
//total lighting texture
texture CurLightTexture;
//total specular texture
texture CurSpecularTexture;
//per pixel depth
texture ShadowTexture;
textureCUBE CubeShadowTexture;

sampler2D CurLightSampler = sampler_state
{
    Texture = <CurLightTexture>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};
sampler2D CurSpecularSampler = sampler_state
{
    Texture = <CurSpecularTexture>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};
sampler2D ShadowSampler = sampler_state
{
    Texture = <ShadowTexture>;
    MipFilter = Point;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};
samplerCUBE CubeShadowSampler = sampler_state
{
    Texture = <CubeShadowTexture>;
    MipFilter = Point;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};      