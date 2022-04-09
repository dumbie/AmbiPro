#pragma once
#pragma comment(lib, "d3d11.lib")
#pragma comment(lib, "d3dcompiler.lib")
#pragma comment(lib, "gdiplus.lib")
#include "CaptureClasses.h"
#include <dxgi1_6.h>
#include <d3d11_4.h>
#include <directxmath.h>
#include <d3dcompiler.h>
#include <gdiplus.h>
#include <atlbase.h>

namespace
{
	//Results
	HRESULT hResult;

	//Capture
	UINT vCaptureWidth;
	UINT vCaptureHeight;
	UINT vCapturePixelByteSize;
	UINT vCaptureWidthByteSize;
	UINT vCaptureTotalByteSize;
	BOOL vCaptureHDREnabled;
	FLOAT vCaptureSDRWhiteLevel;

	//Devices
	CComPtr<IDXGIDevice4> iDxgiDevice4;
	CComPtr<IDXGIAdapter4> iDxgiAdapter4;
	CComPtr<IDXGIOutput> iDxgiOutput0;
	CComPtr<IDXGIOutput6> iDxgiOutput6;
	CComPtr<IDXGIOutputDuplication> iDxgiOutputDuplication0;
	CComPtr<ID3D11Device> iD3D11Device0;
	CComPtr<ID3D11Device5> iD3D11Device5;
	CComPtr<ID3D11DeviceContext> iD3D11DeviceContext0;
	CComPtr<ID3D11DeviceContext4> iD3D11DeviceContext4;

	//Views
	CComPtr<ID3D11InputLayout> iD3D11InputLayout0;
	CComPtr<ID3D11RenderTargetView> iD3D11RenderTargetView0;
	CComPtr<ID3D11ShaderResourceView> iD3D11ShaderResourceView0;

	//Shaders
	CComPtr<ID3D11Buffer> iD3D11Buffer0;
	CComPtr<ID3DBlob> iD3DBlob0VertexShader;
	CComPtr<ID3DBlob> iD3DBlob0PixelShader;
	CComPtr<ID3D11VertexShader> iD3D11VertexShader0;
	CComPtr<ID3D11PixelShader> iD3D11PixelShader0;

	//Textures
	CComPtr<IDXGIResource> iDxgiResource0;
	CComPtr<ID3D11Texture2D1> iD3D11Texture2D1CpuRead;
	CComPtr<ID3D11Texture2D1> iD3D11Texture2D1Capture;
	CComPtr<ID3D11Texture2D1> iD3D11Texture2D1RenderTargetView;

	//Arrays
	FLOAT ColorRgbaBlack[] = { 0.0f, 0.0f, 0.0f, 0.0f };

	D3D_FEATURE_LEVEL D3DFeatureLevelsArray[] =
	{
		D3D_FEATURE_LEVEL_12_0,
		D3D_FEATURE_LEVEL_11_0
	};
	UINT D3DFeatureLevelsCount = ARRAYSIZE(D3DFeatureLevelsArray);

	DXGI_FORMAT iDxgiFormatsArray[] =
	{
		DXGI_FORMAT_B8G8R8A8_UNORM,
		DXGI_FORMAT_R16G16B16A16_FLOAT
	};
	UINT iDxgiFormatsCount = ARRAYSIZE(iDxgiFormatsArray);

	D3D11_INPUT_ELEMENT_DESC InputElementsArray[] =
	{
		{"POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0},
		{"TEXCOORD", 0, DXGI_FORMAT_R32G32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0}
	};
	UINT InputElementsCount = ARRAYSIZE(InputElementsArray);

	VertexVertice VertexVerticesArray[] =
	{
		{DirectX::XMFLOAT3(-1.0f, -1.0f, 0), DirectX::XMFLOAT2(0.0f, 1.0f)},
		{DirectX::XMFLOAT3(-1.0f, 1.0f, 0), DirectX::XMFLOAT2(0.0f, 0.0f)},
		{DirectX::XMFLOAT3(1.0f, -1.0f, 0), DirectX::XMFLOAT2(1.0f, 1.0f)},
		{DirectX::XMFLOAT3(1.0f, 1.0f, 0), DirectX::XMFLOAT2(1.0f, 0.0f)},
	};
	UINT VertexVerticesCount = ARRAYSIZE(VertexVerticesArray);
};