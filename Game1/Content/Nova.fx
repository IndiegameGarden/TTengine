// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

// parameters used from DrawComp TODO check if used
float4 DrawColor = float4(1,1,1,1);

// parameters defined for this effect
float Time = 0;
float Zoom = 50;
float3 ColorScale = float3(4,5,6);

// internal stuff
sampler TextureSampler : register(s0);

float4 Nova_PixelShader(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	// sample existing pixel into p
	float4 p = tex2D(TextureSampler,texCoord); 
	float4 pout = p;
	//pout.a = (p.r + p.g + p.b) / 3;

	// render the resulting pixel with alpha
	return pout;
}

technique Nova_Technique
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 Nova_PixelShader();
    }
}
