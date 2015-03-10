float4x4 World : World;
float4x4 View : View;
float4x4 Proj : Projection;
float4x4 LightView : View;
float4x4 LightProj : Projection;

float3 EyePos : POSITION
<
       string Object = "Camera";
       string UIName =  "Eye";
       string Space = "World";
> = {0,0,0};
float3 LightPos : Position
<
    string UIName =  "Lamp 0 Position";
    string Space = "World";
> = {0,0,30};
float displacementDepth = 50;