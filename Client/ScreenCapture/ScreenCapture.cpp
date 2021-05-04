#define WIN32_LEAN_AND_MEAN
#include "ScreenCapture.h"

bool CaptureResetVariables()
{
	try
	{
		//Disable debug reporting
		_CrtSetReportMode(_CRT_ASSERT, 0);

		//DXGI Variables
		iDxgiDevice.Release();
		iDxgiAdapter.Release();
		iDxgiOutput.Release();
		iDxgiOutput1.Release();
		iDxgiResource.Release();
		iDxgiOutputDuplication.Release();

		//D3D Variables
		iD3DDevice.Release();
		iD3DDeviceContext.Release();
		iD3DDestinationTexture.Release();
		iD3DScreenCaptureTexture.Release();

		//Other Variables
		hResult = E_FAIL;

		return true;
	}
	catch (bool) { return false; }
}

BYTE* CaptureScreenshotByte()
{
	try
	{
		if (iDxgiOutputDuplication == NULL) { return NULL; }

		//Wait for vertical blank
		iDxgiOutput->WaitForVBlank();
		iDxgiOutput1->WaitForVBlank();

		//Get output duplication frame
		hResult = iDxgiOutputDuplication->AcquireNextFrame(600000, &DxgiOutputDuplicationFrameInfo, &iDxgiResource);
		if (!SUCCEEDED(hResult)) { return NULL; }

		//Query for D3D screen texture
		hResult = iDxgiResource->QueryInterface(&iD3DScreenCaptureTexture);
		iDxgiResource.Release();
		if (!SUCCEEDED(hResult)) { return NULL; }

		//Copy image into CPU texture
		iD3DDeviceContext->CopyResource(iD3DDestinationTexture, iD3DScreenCaptureTexture);
		iD3DDeviceContext->Unmap(iD3DDestinationTexture, 0);
		iD3DDeviceContext->Unmap(iD3DScreenCaptureTexture, 0);
		iD3DScreenCaptureTexture.Release();

		//Release output duplication frame
		iDxgiOutputDuplication->ReleaseFrame();
		return (BYTE*)iD3DMappedSubResource.pData;
	}
	catch (BYTE*) { return NULL; }
}

extern "C"
{
	__declspec(dllexport) bool CaptureInitialize(int CaptureMonitor)
	{
		try
		{
			//Reset initialize variables
			CaptureResetVariables();

			//Create D3D Device
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
			hResult = iD3DDevice->QueryInterface(&iDxgiDevice);
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
			hResult = iDxgiOutput->QueryInterface(&iDxgiOutput1);
			if (!SUCCEEDED(hResult)) { return false; }

			//Create desktop duplicate
			hResult = iDxgiOutput1->DuplicateOutput(iD3DDevice, &iDxgiOutputDuplication);
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
			BitmapWidthPixels = iDxgiOutputDuplicationDescription.ModeDesc.Width;
			BitmapHeightPixels = iDxgiOutputDuplicationDescription.ModeDesc.Height;
			BitmapWidthRows = iDxgiOutputDuplicationDescription.ModeDesc.Width * 4;
			BitmapPitchRows = iD3DMappedSubResource.RowPitch;
			return true;
		}
		catch (bool) { return false; }
	}

	__declspec(dllexport) bool CaptureReset()
	{
		try
		{
			//Reset initialize variables
			return CaptureResetVariables();
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
			//Return image byte array
			*OutputWidth = BitmapWidthPixels;
			*OutputHeight = BitmapHeightPixels;
			*OutputSize = BitmapByteSize;
			return CaptureScreenshotByte();
		}
		catch (BYTE*) { return NULL; }
	}

	__declspec(dllexport) BYTE* CaptureScreenshotResize(int ResizeWidth, int ResizeHeight, unsigned int* OutputSize)
	{
		try
		{
			BYTE* BitmapBuffer = CaptureScreenshotByte();
			if (BitmapBuffer == NULL) { return NULL; }

			int PixelSize = 4;
			double ScaleWidth = (double)ResizeWidth / (double)BitmapWidthPixels;
			double ScaleHeight = (double)ResizeHeight / (double)BitmapHeightPixels;

			UINT ResizeByteSize = ResizeWidth * ResizeHeight * PixelSize;
			BYTE* ResizeBuffer = new BYTE[ResizeByteSize];

			for (UINT xHeight = 0; xHeight < ResizeHeight; xHeight++)
			{
				for (UINT xWidth = 0; xWidth < ResizeWidth; xWidth++)
				{
					int pixel = PixelSize * (xHeight * ResizeWidth + xWidth);
					int nearest = PixelSize * ((int)(xHeight / ScaleHeight) * BitmapWidthPixels + (int)(xWidth / ScaleWidth));
					ResizeBuffer[pixel++] = BitmapBuffer[nearest++];
					ResizeBuffer[pixel++] = BitmapBuffer[nearest++];
					ResizeBuffer[pixel++] = BitmapBuffer[nearest++];
					ResizeBuffer[pixel] = BitmapBuffer[nearest];
				}
			}

			//Return image byte array
			*OutputSize = ResizeByteSize;
			return ResizeBuffer;
		}
		catch (BYTE*) { return NULL; }
	}
}