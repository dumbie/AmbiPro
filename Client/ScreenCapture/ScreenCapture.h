#define WIN32_LEAN_AND_MEAN
#pragma comment(lib, "D3D11.lib")
#include <atlbase.h>
#include <dxgi1_2.h>
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

//DXGI Variables
CComPtr<IDXGIDevice> iDxgiDevice;
CComPtr<IDXGIAdapter> iDxgiAdapter;
CComPtr<IDXGIOutput> iDxgiOutput;
CComPtr<IDXGIOutput1> iDxgiOutput1;
CComPtr<IDXGIOutputDuplication> iDxgiOutputDuplication;
DXGI_OUTDUPL_DESC iDxgiOutputDuplicationDescription;

//D3D Variables
CComPtr<ID3D11Device> iD3DDevice;
CComPtr<ID3D11DeviceContext> iD3DDeviceContext;
CComPtr<ID3D11Texture2D> iD3DDestinationTexture;
D3D11_MAPPED_SUBRESOURCE iD3DMappedSubResource;

//Bitmap Variables
UINT BitmapByteSize;
UINT BitmapWidthPixels;
UINT BitmapHeightPixels;
UINT BitmapWidthRows;
UINT BitmapPitchRows;

//Other Variables
HRESULT hResult = E_FAIL;