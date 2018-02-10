// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

// parameters from DrawComp (TODO check if used)
float4 DrawColor = float4(1,1,1,1);

// parameters defined for this specific effect
float Time = 0;
float Zoom = 50;
float3 ColorScale = float3(4,5,6);

// internal stuff
sampler TextureSampler : register(s0);

float4 Hypno_PixelShader(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	// sample existing pixel into p
	float4 p = tex2D(TextureSampler,texCoord); 

	// compute a value for rendering
	float2 v = (texCoord - 0.5) * Zoom ; 
	float val;
	val = v.x * v.x + v.y * v.y + sin(v.x * v.y + Time + p.r + p.g );
	float4 hypnoColor = float4(sin(val * ColorScale.x), sin(val * ColorScale.y), sin(val * ColorScale.z), 1) ;

	// render the resulting pixel by mixing
	return hypnoColor; // DrawColor.a * p + (1 - DrawColor.a) * hypnoColor;
}

technique Hypno_Technique
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 Hypno_PixelShader();
    }
}
