#define WIN32_LEAN_AND_MEAN
#pragma comment(lib, "D3D11.lib")
#include "ScreenCapture.h"
#include <atlbase.h>
#include <dxgi1_2.h>
#include <d3d11.h>
#include <memory>
#include <algorithm>

extern "C"
{
	__declspec(dllexport) bool CaptureInitialize(UINT CaptureMonitor)
	{
		try
		{
			//Disable debug reporting
			_CrtSetReportMode(_CRT_ASSERT, 0);

			//Reset initialize variables
			hResult = E_FAIL;
			iDxgiDevice.Release();
			iDxgiAdapter.Release();
			iDxgiOutput.Release();
			iDxgiOutput1.Release();
			iDxgiOutputDuplication.Release();
			iD3DDevice.Release();
			iD3DDeviceContext.Release();
			iD3DDestinationTexture.Release();

			//Create D3D Device
			D3D_FEATURE_LEVEL iD3DFeatureLevel;
			for (UINT FeatureIndex = 0; FeatureIndex < NumD3DFeatureLevels; FeatureIndex++)
			{
				hResult = D3D11CreateDevice(nullptr, D3D_DRIVER_TYPE_HARDWARE, nullptr, 0, &ArrayD3DFeatureLevels[FeatureIndex], 1, D3D11_SDK_VERSION, &iD3DDevice, &iD3DFeatureLevel, &iD3DDeviceContext);
				if (SUCCEEDED(hResult)) { break; }
				else
				{
					iD3DDevice.Release();
					iD3DDeviceContext.Release();
				}
			}
			if (!SUCCEEDED(hResult)) { return false; }

			//Get DXGI Device
			hResult = iD3DDevice->QueryInterface(IID_PPV_ARGS(&iDxgiDevice));
			if (!SUCCEEDED(hResult)) { return false; }

			//Get DXGI Adapter
			hResult = iDxgiDevice->GetParent(IID_PPV_ARGS(&iDxgiAdapter));
			iDxgiDevice.Release();
			if (!SUCCEEDED(hResult)) { return false; }

			//Get DXGI Output
			hResult = iDxgiAdapter->EnumOutputs(CaptureMonitor, &iDxgiOutput);
			iDxgiAdapter.Release();
			if (!SUCCEEDED(hResult)) { return false; }

			//Query DXGI Output1
			hResult = iDxgiOutput->QueryInterface(IID_PPV_ARGS(&iDxgiOutput1));
			//iDxgiOutput.Release();
			if (!SUCCEEDED(hResult)) { return false; }

			//Create desktop duplicate
			hResult = iDxgiOutput1->DuplicateOutput(iD3DDevice, &iDxgiOutputDuplication);
			//iDxgiOutput1.Release();
			if (!SUCCEEDED(hResult)) { return false; }

			//Get duplicate description
			iDxgiOutputDuplication->GetDesc(&iDxgiOutputDuplicationDescription);

			//Create CPU staging texture
			D3D11_TEXTURE2D_DESC D3DTextureDescription;
			D3DTextureDescription.Width = iDxgiOutputDuplicationDescription.ModeDesc.Width;
			D3DTextureDescription.Height = iDxgiOutputDuplicationDescription.ModeDesc.Height;
			D3DTextureDescription.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
			D3DTextureDescription.ArraySize = 1;
			D3DTextureDescription.BindFlags = 0;
			D3DTextureDescription.MiscFlags = 0;
			D3DTextureDescription.SampleDesc.Count = 1;
			D3DTextureDescription.SampleDesc.Quality = 0;
			D3DTextureDescription.MipLevels = 1;
			D3DTextureDescription.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
			D3DTextureDescription.Usage = D3D11_USAGE_STAGING;

			hResult = iD3DDevice->CreateTexture2D(&D3DTextureDescription, NULL, &iD3DDestinationTexture);
			iD3DDevice.Release();
			if (!SUCCEEDED(hResult)) { return false; }

			//Map CPU texture to bitmap buffer
			hResult = iD3DDeviceContext->Map(iD3DDestinationTexture, 0, D3D11_MAP_READ, 0, &iD3DMappedSubResource);
			if (!SUCCEEDED(hResult)) { return false; }

			//Calculate and set bitmap information
			BitmapByteSize = iDxgiOutputDuplicationDescription.ModeDesc.Width * iDxgiOutputDuplicationDescription.ModeDesc.Height * 4;
			BitmapWidthRows = iDxgiOutputDuplicationDescription.ModeDesc.Width * 4;
			BitmapPitchRows = std::min<UINT>(BitmapWidthRows, iD3DMappedSubResource.RowPitch);
			return true;
		}
		catch (bool) { return false; }
	}

	__declspec(dllexport) bool CaptureReset()
	{
		try
		{
			//Disable debug reporting
			_CrtSetReportMode(_CRT_ASSERT, 0);

			//Reset initialize variables
			hResult = E_FAIL;
			iDxgiDevice.Release();
			iDxgiAdapter.Release();
			iDxgiOutput.Release();
			iDxgiOutput1.Release();
			iDxgiOutputDuplication.Release();
			iD3DDevice.Release();
			iD3DDeviceContext.Release();
			iD3DDestinationTexture.Release();
			return true;
		}
		catch (bool) { return false; }
	}

	__declspec(dllexport) bool CaptureFreeMemory(void* FreeMemory)
	{
		try
		{
			delete[] FreeMemory;
			return true;
		}
		catch (bool) { return false; }
	}

	__declspec(dllexport) BYTE* CaptureScreenshot(unsigned int* OutputWidth, unsigned int* OutputHeight, unsigned int* OutputSize)
	{
		try
		{
			//Wait for vertical blank
			iDxgiOutput->WaitForVBlank();
			iDxgiOutput1->WaitForVBlank();

			//Get output duplication frame
			CComPtr<IDXGIResource> iDxgiResource;
			DXGI_OUTDUPL_FRAME_INFO DxgiOutputDuplicationFrameInfo;
			hResult = iDxgiOutputDuplication->AcquireNextFrame(60000, &DxgiOutputDuplicationFrameInfo, &iDxgiResource);
			if (!SUCCEEDED(hResult)) { return NULL; }

			//Query for d3d screen texture
			CComPtr<ID3D11Texture2D> iD3DScreenTexture;
			hResult = iDxgiResource->QueryInterface(IID_PPV_ARGS(&iD3DScreenTexture));
			iDxgiResource.Release();
			if (!SUCCEEDED(hResult)) { return NULL; }

			//Copy image into CPU texture
			iD3DDeviceContext->CopyResource(iD3DDestinationTexture, iD3DScreenTexture);
			iD3DDeviceContext->Unmap(iD3DDestinationTexture, 0);
			iD3DDeviceContext->Unmap(iD3DScreenTexture, 0);
			iD3DScreenTexture.Release();

			//Release output duplication frame
			iDxgiOutputDuplication->ReleaseFrame();

			//Collect information for image
			BYTE* BitmapBuffer(new BYTE[BitmapByteSize]);
			BYTE* SourcePointer = (BYTE*)iD3DMappedSubResource.pData;
			BYTE* DestinationPointer = BitmapBuffer + BitmapByteSize - BitmapWidthRows;
			for (UINT h = 0; h < iDxgiOutputDuplicationDescription.ModeDesc.Height; h++)
			{
				memcpy_s(DestinationPointer, BitmapWidthRows, SourcePointer, BitmapPitchRows);
				SourcePointer += iD3DMappedSubResource.RowPitch;
				DestinationPointer -= BitmapWidthRows;
			}

			*OutputWidth = iDxgiOutputDuplicationDescription.ModeDesc.Width;
			*OutputHeight = iDxgiOutputDuplicationDescription.ModeDesc.Height;
			*OutputSize = BitmapByteSize;
			return BitmapBuffer;
		}
		catch (BYTE*) { return NULL; }
	}
}