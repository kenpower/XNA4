float4x4 world_view_proj_matrix;
float4 Light_Ambient;
float4 Light1_Position;
float4 Light1_Color;
float3 view_position;
float4x4 inv_world_matrix;
float4x4 world_matrix;


struct VS_OUTPUT_PER_PIXEL
{
   float4 Pos:    	POSITION;
   float3 normal:	TEXCOORD1;
   float3 light:	TEXCOORD2;
   float3 halfvect:	TEXCOORD3;
   vector color:    COLOR;
};


vector lighting( float3 normal, float3 light_dir, float3 half_vect){
  // Output the lit color

   //ambient

   // Specular
   float SpecularAttn =  max(0,pow(  dot(normal, half_vect),100));

   // Diffuse
   float DiffuseAttn = max(0, dot(normal, light_dir) );
 
   // Compute final lighting
   vector color= Light_Ambient+  DiffuseAttn*Light1_Color+SpecularAttn*(1,1,1,1);
      
   return color;
}

VS_OUTPUT_PER_PIXEL vs_main_per_pixel(float4 inPos: POSITION, float3 inNormal: NORMAL)
{
   VS_OUTPUT_PER_PIXEL Out=(VS_OUTPUT_PER_PIXEL)0;

   // Compute the projected position and send out the texture coordinates
   Out.Pos = mul(inPos,world_view_proj_matrix );
   
   
   inNormal=normalize(inNormal);


   // Determine the eye vector
   
   vector obj_eye=mul(view_position,inv_world_matrix);
   
   vector EyeDir = normalize(obj_eye-inPos);

   vector obj_light=mul(Light1_Position,inv_world_matrix);
   vector LightDir = normalize(obj_light - inPos);


   // Compute half vector
   vector HalfVect = normalize((LightDir+EyeDir)/2);

	Out.normal=	inNormal;
	Out.light=	LightDir;
	Out.halfvect=HalfVect;

   return Out;
}

VS_OUTPUT_PER_PIXEL vs_main_gouraud(float4 inPos: POSITION, float3 inNormal: NORMAL)
{
   VS_OUTPUT_PER_PIXEL Out=(VS_OUTPUT_PER_PIXEL)0;

   // Compute the projected position and send out the texture coordinates
   Out.Pos = mul(inPos,world_view_proj_matrix );
   
   
   inNormal=normalize(inNormal);
   // Output the ambient color

   // Determine the eye vector
   
   vector obj_eye=mul(view_position,inv_world_matrix);
   
   vector EyeDir = normalize(obj_eye-inPos);

   vector obj_light=mul(Light1_Position,inv_world_matrix);
   vector LightDir = normalize(obj_light - inPos);


   // Compute half vector
     vector HalfVect = normalize((LightDir+EyeDir)/2);

   // Output Final Color
    Out.color=lighting(inNormal,LightDir,HalfVect);
   
	Out.normal=	inNormal;
	Out.light=	LightDir;
	Out.halfvect=HalfVect;

   return Out;
}

float4 ps_main_per_pixel(
	float3 inNormal:	TEXCOORD1,
	float3 LightDir:	TEXCOORD2,
	float3 HalfVect:	TEXCOORD3
) : COLOR 
{
   
   
   return lighting(inNormal,LightDir,HalfVect);
   
}

float4 ps_main_gouraud(
	float4 inColor: COLOR0
) : COLOR 
{
   
   //do nothing	
   return inColor;
   
}


technique TransformPerPixel
{
    pass P0
    {
        vertexShader = compile vs_2_0 vs_main_per_pixel();
        pixelShader = compile ps_2_0 ps_main_per_pixel();
    }
}

technique Gouraud
{
    pass P0
    {
        vertexShader = compile vs_2_0 vs_main_gouraud();
        pixelShader = compile ps_2_0  ps_main_gouraud();
    }
}