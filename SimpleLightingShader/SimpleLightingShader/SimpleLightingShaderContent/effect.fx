uniform extern float4x4 WorldViewProj : WORLDVIEWPROJECTION;
uniform extern texture Texture;


struct VS_OUTPUT
{
    float4 position : POSITION;
    float4 textcoord:TEXCOORD0;
    
};

sampler textureSampler = sampler_state
{
    Texture = <Texture>;
    mipfilter = LINEAR; 
};


VS_OUTPUT Transform(
    float4 Pos  : POSITION, 
    float4 Color : COLOR0, 
    float4 textcoord:TEXCOORD0,
    float4 norm:NORMAL
    )
{
    VS_OUTPUT Out = (VS_OUTPUT)0;

	Out.position = mul(Pos, WorldViewProj);
    
    //Out.color = Pos+float4(0.5,0.5,0.5,0.0);
    Out.textcoord=textcoord;

    return Out;
}

float4 PixelShader( float4 textureCoordinate : TEXCOORD0):COLOR
{
 return tex2D(textureSampler, textureCoordinate);

}

technique TransformTechnique
{
    pass P0
    {
        vertexShader = compile vs_2_0 Transform();
        pixelShader = compile ps_1_1 PixelShader();
    }
}
