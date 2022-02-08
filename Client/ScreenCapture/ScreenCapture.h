#pragma comment(lib, "D3D11.lib")
#include <dxgi1_6.h>
#include <d3d11_4.h>

#include <wrl.h>
using Microsoft::WRL::ComPtr;

//D3D feature levels
D3D_FEATURE_LEVEL ArrayD3DFeatureLevels[] =
{
	D3D_FEATURE_LEVEL_12_1,
	D3D_FEATURE_LEVEL_11_1,
	D3D_FEATURE_LEVEL_10_1,
	D3D_FEATURE_LEVEL_9_3
};
UINT NumD3DFeatureLevels = ARRAYSIZE(ArrayD3DFeatureLevels);

//Capture Variables
ComPtr<IDXGIDevice4> iDxgiDevice4;
ComPtr<IDXGIAdapter4> iDxgiAdapter4;
ComPtr<IDXGIOutput> iDxgiOutput0;
ComPtr<IDXGIOutput6> iDxgiOutput6;
ComPtr<IDXGIOutputDuplication> iDxgiOutputDuplication0;
ComPtr<ID3D11Device> iD3DDevice0;
ComPtr<ID3D11DeviceContext> iD3DDeviceContext0;

//Screenshot Variables
ComPtr<IDXGIResource> iDxgiResource0;
ComPtr<ID3D11Texture2D> iD3DScreenCaptureTextureOriginal0;
ComPtr<ID3D11Texture2D> iD3DScreenCaptureTextureOutput0;
ComPtr<ID3D11Texture2D> iD3DScreenCaptureTextureResize0;
ComPtr<ID3D11ShaderResourceView> iD3D11ShaderResourceView0;

//Shader Variables
ComPtr<ID3D11PixelShader> iD3D11PixelShader0;

//Bitmap Variables
UINT BitmapWidthPixels;
UINT BitmapHeightPixels;
UINT BitmapWidthRows;
UINT BitmapByteSize;
UINT BitmapMipLevel;

//Result Variables
HRESULT hResult = E_FAIL;