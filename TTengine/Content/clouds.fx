// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

// the relative position for the texture coordinates, inducing a slight parallax
float2 Position;

// modify the sampler state on the zero texture sampler, used by SpriteBatch
sampler TextureSampler : register(s0) =
sampler_state
{
	AddressU = Wrap;
	AddressV = Wrap;
};

float4 PixelShaderFunction(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	// sample from the cloud texture for the blue component
	texCoord.x += Position.x * 0.0002f; //0.00025f;
	texCoord.y += Position.y * 0.0002f; //0.00025f;
	texCoord *= 0.5f;
	float4 results = float4(0,0,1,0.25) * tex2D(TextureSampler, texCoord);

	// sample from the cloud texture for the green component
	texCoord.x += Position.x * 0.00025f + 0.25f;
	texCoord.y += Position.y * 0.00025f - 0.15;
	texCoord *= 0.4f;
	results += float4(0,1,0,0.15) * tex2D(TextureSampler, texCoord);

	return results;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}