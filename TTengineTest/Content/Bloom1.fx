// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
// Pixel shader extracts the brighter areas of an image.
// This is the first step in applying a bloom postprocess.
// from: http://community.monogame.net/t/how-to-achieve-a-bloom-effect-on-2d-textures/2614/7
//
sampler TextureSampler : register(s0);

float BloomThreshold = 0.5;

float4 PixelShaderFunction(float4 position : SV_Position, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	// Look up the original image color.
	float4 c = tex2D(TextureSampler, coords);

	// Adjust it to keep only values brighter than the specified threshold.
	return saturate((c - BloomThreshold) / (1 - BloomThreshold));
	//return c;
	//return float4(coords.x, coords.y, 0, 1);
}


technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}
