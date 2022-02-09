#pragma comment(lib, "d3d11.lib")
#pragma comment(lib, "d3dcompiler.lib")
#include <dxgi1_6.h>
#include <d3d11_4.h>
#include <d3dcompiler.h>
#include <atlbase.h>

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
CComPtr<IDXGIDevice4> iDxgiDevice4;
CComPtr<IDXGIAdapter4> iDxgiAdapter4;
CComPtr<IDXGIOutput> iDxgiOutput0;
CComPtr<IDXGIOutput6> iDxgiOutput6;
CComPtr<IDXGIOutputDuplication> iDxgiOutputDuplication0;
CComPtr<ID3D11Device> iD3DDevice0;
CComPtr<ID3D11Device5> iD3DDevice5;
CComPtr<ID3D11DeviceContext> iD3DDeviceContext0;
CComPtr<ID3D11DeviceContext4> iD3DDeviceContext4;

//Screenshot Variables
CComPtr<IDXGIResource> iDxgiResource0;
CComPtr<ID3D11Texture2D1> iD3D11Texture2D1Original;
CComPtr<ID3D11Texture2D1> iD3D11Texture2D1Resize;
CComPtr<ID3D11Texture2D1> iD3D11Texture2D1Cpu;
CComPtr<ID3DBlob> iD3DBlob0;
CComPtr<ID3D11PixelShader> iD3D11PixelShader0;
CComPtr<ID3D11ShaderResourceView1> iD3D11ShaderResourceView1;

//Bitmap Variables
UINT BitmapWidthPixels;
UINT BitmapHeightPixels;
UINT BitmapWidthRows;
UINT BitmapByteSize;
UINT BitmapMipLevel;
BOOL BitmapHDR;

//Result Variables
HRESULT hResult = E_FAIL;