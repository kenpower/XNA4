float4x4 view_proj_matrix;
float4 Light_Ambient;
float4 Light1_Position;
float4 Light1_Attenuation;
float4 Light1_Color;
float4 view_position;
float4x4 inv_world_matrix;
float4x4 world_matrix;

uniform extern texture Texture;

struct VS_OUTPUT 
{
   float4 Pos:    	POSITION;
   float2 TexCoord:	TEXCOORD0;
   float3 normal:	TEXCOORD1;
   float3 light:	TEXCOORD2;
   float3 halfvect:	     TEXCOORD3;
   float3 Color:		COLOR0;
};





VS_OUTPUT vs_main(float4 inPos: POSITION, float3 inNormal: NORMAL,float2 inTxr: TEXCOORD0)
{
   VS_OUTPUT Out=(VS_OUTPUT)0;

   // Compute the projected position and send out the texture coordinates
   Out.Pos = mul(inPos,view_proj_matrix );
   Out.TexCoord = inTxr;
   
   inNormal=normalize(inNormal);
   // Output the ambient color
   float4 Color =Light_Ambient;


   
   
   // Determine the eye vector
   
   vector obj_eye=mul(view_position,inv_world_matrix);
   
   vector EyeDir = normalize(obj_eye-inPos);

   vector obj_light=mul(Light1_Position,inv_world_matrix);
   vector LightDir = normalize(obj_light - inPos);


   // Compute half vector
   vector HalfVect = normalize(LightDir+EyeDir);

   

  
   

   // Output Final Color
   Out.Color=Color;
   
	Out.normal=	inNormal;
	Out.light=	LightDir;
	Out.halfvect=	HalfVect;

   return Out;
}

float4 ps_main(
	float4 inColor: COLOR0,
	float2 inTxr:	TEXCOORD0,   
	float3 inNormal:	TEXCOORD1,
	float3 LightDir:	TEXCOORD2,
	float3 HalfVect:	TEXCOORD3
) : COLOR 
{
   // Output the lit color
    // Specular
   float SpecularAttn =  max(0,pow(  dot(inNormal, HalfVect),32));
   //float SpecularAttn = 0;

   // Diffuse
   float AngleAttn = max(0, dot(inNormal, LightDir) );
   //float AngleAttn = 0;
 
   // Compute final lighting
   //return LightColor * DistAttn * (SpecularAttn+AngleAttn);
   inColor *=  (SpecularAttn+AngleAttn);
   // Compute light contribution
   
   return inColor;
   //return inColor*tex2D(textureSampler,inTxr);
   //return tex2D(Texture,inTxr);
}



technique TransformTechnique
{
    pass P0
    {
        vertexShader = compile vs_2_0 vs_main();
        pixelShader = compile ps_2_0 ps_main();
    }
}