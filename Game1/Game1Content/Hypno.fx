// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

// parameters used directly from Efflet class
float4 DrawColor = float4(1,1,1,1);

// parameters defined for interfacing with HypnoEfflet class
float Time = 0;
float Zoom = 50;
float3 ColorScale = float3(4,5,6);

// internal stuff
sampler TextureSampler : register(s0);

float4 Hypno_PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	// sample existing pixel into p
	float4 p = tex2D(TextureSampler,texCoord); 

	// compute a value for rendering
	float2 v = (texCoord - 0.5) * Zoom ; 
	float val;
	val = v.x * v.x + v.y * v.y + sin(v.x * v.y + Time + p.r + p.g );
	float4 hypnoColor = float4(sin(val * ColorScale.x), sin(val * ColorScale.y), sin(val * ColorScale.z), 1) ;

	// render the resulting pixel by mixing
	return DrawColor.a * p + (1-DrawColor.a) * hypnoColor;
}

technique Hypno_Technique
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 Hypno_PixelShader();
    }
}
