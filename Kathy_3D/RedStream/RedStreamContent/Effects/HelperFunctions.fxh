float4 _texCUBE(samplerCUBE samp, float3 vec)
{
	vec.z*=-1.0f;
	return texCUBE(samp, vec);	
}
float2 projCoords(float4 vec)
{
	float2 TexCoords = 0.5 * vec.xy / 
                            vec.w + float2( 0.5, 0.5 );
    TexCoords.y = 1.0f - TexCoords.y;
	return TexCoords;
}
float4 _tex2DProj(sampler2D samp, float4 vec)
{
	return tex2D(samp, projCoords(vec));
}
float4 _tex2DProjLoop(sampler2D samp, float4 vec)
{
	return tex2Dlod(samp, float4(projCoords(vec),0,0));
}
float4 _texCUBELoop(samplerCUBE samp, float3 vec)
{
       vec.z*=-1.0f;
       return texCUBElod(samp, float4(vec, 0));

}