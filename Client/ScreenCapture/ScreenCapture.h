#define WIN32_LEAN_AND_MEAN
#pragma comment(lib, "D3D11.lib")
#include <atlbase.h>
#include <dxgi1_6.h>
#include <d3d11.h>

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
CComPtr<IDXGIDevice> iDxgiDevice;
CComPtr<IDXGIAdapter> iDxgiAdapter;
CComPtr<IDXGIOutput> iDxgiOutput;
CComPtr<IDXGIOutput6> iDxgiOutput6;
CComPtr<IDXGIOutputDuplication> iDxgiOutputDuplication;
CComPtr<ID3D11Device> iD3DDevice;
CComPtr<ID3D11DeviceContext> iD3DDeviceContext;

//Screenshot Variables
CComPtr<IDXGIResource> iDxgiResource;
CComPtr<ID3D11Texture2D> iD3DScreenCaptureTextureOriginal;
CComPtr<ID3D11Texture2D> iD3DScreenCaptureTextureOutput;
CComPtr<ID3D11Texture2D> iD3DScreenCaptureTextureResize;
CComPtr<ID3D11ShaderResourceView> iD3D11ShaderResourceView;

//Result Variables
HRESULT hResult = E_FAIL;