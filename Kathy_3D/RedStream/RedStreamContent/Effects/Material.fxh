#include "NormalMapping.fxh"
float Specular
<
 string UIName =  "Specular";
> = 1;
float Shininess
<
 string UIName =  "Shininess";
> = 100;
float Emissive
<
 string UIName = "Emissive";
> = 0;
float4 MaterialColor
<
 string UIName =  "Color";
> = {0,0,0,1};

texture DiffuseTexture
<
    string ResourceName = "Default_color.dds";
    string UIName =  "Diffuse-Map Texture";
    string ResourceType = "2D";
>;

texture SpecularTexture
<
    string ResourceName = "default_bump_normal.dds";
    string UIName =  "Specular-Map Texture";
    string ResourceType = "2D";
>;
sampler2D DiffuseSampler = sampler_state {
    Texture = <DiffuseTexture>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

sampler2D SpecularSampler = sampler_state {
    Texture = <SpecularTexture>;
    //Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture DisplacementTexture;
sampler2D DisplacementSampler  =  sampler_state
{
    Texture = <DisplacementTexture>;
    MipFilter = Point;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};