#pragma once
#include <directxmath.h>

namespace
{
	typedef struct ShaderVariables
	{
		bool HDREnabled;
		float SDRWhiteLevel;
		float Saturation;
		float Brightness;
		float Gamma;
	} ShaderVariables;

	typedef struct VertexVertice
	{
		DirectX::XMFLOAT3 Position;
		DirectX::XMFLOAT2 TexCoord;
	} VertexVertice;
};