Texture2D _texture2D : register(t0);
SamplerState _samplerState : register(s0);
cbuffer _shaderVariables : register(b0)
{
	bool HDREnabled;
	float SDRWhiteLevel;
};

struct PS_INPUT
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD;
};

float4 AdjustHDRWhiteLevel(float4 color)
{
	return color / (SDRWhiteLevel / 75.0F);
}

float4 main(PS_INPUT input) : SV_TARGET
{
	float4 color = _texture2D.Sample(_samplerState, input.TexCoord);
	if (HDREnabled)
	{
		color = AdjustHDRWhiteLevel(color);
	}
	return color;
}