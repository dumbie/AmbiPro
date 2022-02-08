#include "ScreenCapture.h"

BOOL CaptureResetVariables()
{
	try
	{
		//Capture Variables
		iDxgiDevice4.Reset();
		iDxgiAdapter4.Reset();
		iDxgiOutput0.Reset();
		iDxgiOutput6.Reset();
		iDxgiOutputDuplication0.Reset();
		iD3DDevice0.Reset();
		iD3DDeviceContext0.Reset();

		//Result Variables
		hResult = E_FAIL;
		return true;
	}
	catch (BOOL)
	{
		return false;
	}
}

BOOL ScreenshotResetVariables()
{
	try
	{
		//Screenshot Variables
		iDxgiResource0.Reset();
		iD3DScreenCaptureTextureOriginal0.Reset();
		iD3DScreenCaptureTextureOutput0.Reset();
		iD3DScreenCaptureTextureResize0.Reset();
		iD3D11ShaderResourceView0.Reset();

		//Result Variables
		hResult = E_FAIL;
		return true;
	}
	catch (BOOL)
	{
		return false;
	}
}

extern "C"
{
	__declspec(dllexport) BOOL CaptureInitialize(UINT CaptureMonitor, UINT* OutputWidth, UINT* OutputHeight, UINT* OutputByteSize, UINT* OutputHDR, UINT ResizeMipLevel)
	{
		try
		{
			//Disable debug reporting
			_CrtSetReportMode(_CRT_ASSERT, 0);

			//Reset capture variables
			CaptureResetVariables();

			//Create D3D Device
			D3D_FEATURE_LEVEL iD3DFeatureLevel;
			for (UINT FeatureIndex = 0; FeatureIndex < NumD3DFeatureLevels; FeatureIndex++)
			{
				hResult = D3D11CreateDevice(nullptr, D3D_DRIVER_TYPE_HARDWARE, nullptr, 0, &ArrayD3DFeatureLevels[FeatureIndex], 1, D3D11_SDK_VERSION, &iD3DDevice0, &iD3DFeatureLevel, &iD3DDeviceContext0);
				if (SUCCEEDED(hResult)) { break; }
				else
				{
					CaptureResetVariables();
				}
			}
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get DXGI Device
			hResult = iD3DDevice0.As(&iDxgiDevice4);
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get DXGI Adapter
			hResult = iDxgiDevice4->GetParent(IID_PPV_ARGS(&iDxgiAdapter4));
			iDxgiDevice4.Reset();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get DXGI Output
			hResult = iDxgiAdapter4->EnumOutputs(CaptureMonitor, &iDxgiOutput0);
			iDxgiAdapter4.Reset();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Query DXGI Output
			hResult = iDxgiOutput0.As(&iDxgiOutput6);
			iDxgiOutput0.Reset();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get output description
			DXGI_OUTPUT_DESC1 iDxgiOutputDescription;
			hResult = iDxgiOutput6->GetDesc1(&iDxgiOutputDescription);
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get output duplicate
			hResult = iDxgiOutput6->DuplicateOutput(iD3DDevice0.Get(), &iDxgiOutputDuplication0);
			iDxgiOutput6.Reset();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get duplicate description
			DXGI_OUTDUPL_DESC iDxgiOutputDuplicationDescription;
			iDxgiOutputDuplication0->GetDesc(&iDxgiOutputDuplicationDescription);

			//Check duplicate resolution
			BitmapMipLevel = ResizeMipLevel;
			BitmapWidthPixels = iDxgiOutputDuplicationDescription.ModeDesc.Width >> BitmapMipLevel;
			BitmapHeightPixels = iDxgiOutputDuplicationDescription.ModeDesc.Height >> BitmapMipLevel;
			if (BitmapWidthPixels < 300 || BitmapHeightPixels < 300)
			{
				BitmapMipLevel = 0;
				BitmapWidthPixels = iDxgiOutputDuplicationDescription.ModeDesc.Width;
				BitmapHeightPixels = iDxgiOutputDuplicationDescription.ModeDesc.Height;
			}
			BitmapWidthRows = BitmapWidthPixels * 4;
			BitmapByteSize = BitmapWidthPixels * BitmapHeightPixels * 4;
			*OutputWidth = BitmapWidthPixels;
			*OutputHeight = BitmapHeightPixels;
			*OutputByteSize = BitmapByteSize;

			//Check if HDR is enabled
			*OutputHDR = iDxgiOutputDescription.ColorSpace == DXGI_COLOR_SPACE_RGB_FULL_G2084_NONE_P2020 || iDxgiOutputDescription.ColorSpace == DXGI_COLOR_SPACE_RGB_STUDIO_G2084_NONE_P2020;
			return true;
		}
		catch (BOOL)
		{
			CaptureResetVariables();
			return false;
		}
	}

	__declspec(dllexport) bool CaptureFreeMemory(void* FreeMemory)
	{
		try
		{
			delete[] FreeMemory;
			return true;
		}
		catch (bool)
		{
			return false;
		}
	}

	__declspec(dllexport) BOOL CaptureReset()
	{
		try
		{
			return CaptureResetVariables() && ScreenshotResetVariables();
		}
		catch (BOOL)
		{
			return false;
		}
	}

	__declspec(dllexport) BYTE* CaptureScreenshot()
	{
		try
		{
			//Get output duplication frame
			DXGI_OUTDUPL_FRAME_INFO iDxgiOutputDuplicationFrameInfo;
			hResult = iDxgiOutputDuplication0->AcquireNextFrame(INFINITE, &iDxgiOutputDuplicationFrameInfo, &iDxgiResource0);
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Get screen texture
			hResult = iDxgiResource0.As(&iD3DScreenCaptureTextureOriginal0);
			iDxgiResource0.Reset();
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Create output texture
			if (BitmapMipLevel > 0)
			{
				//Create resize description
				D3D11_TEXTURE2D_DESC iD3DTextureDescriptionResize;
				iD3DScreenCaptureTextureOriginal0->GetDesc(&iD3DTextureDescriptionResize);
				iD3DTextureDescriptionResize.MipLevels = BitmapMipLevel + 1;
				iD3DTextureDescriptionResize.MiscFlags = D3D11_RESOURCE_MISC_GENERATE_MIPS;

				//Create resize texture
				hResult = iD3DDevice0->CreateTexture2D(&iD3DTextureDescriptionResize, NULL, &iD3DScreenCaptureTextureResize0);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Create resize shader view
				hResult = iD3DDevice0->CreateShaderResourceView(iD3DScreenCaptureTextureResize0.Get(), NULL, &iD3D11ShaderResourceView0);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Create output description
				D3D11_TEXTURE2D_DESC iD3DTextureDescriptionOutput;
				iD3DTextureDescriptionOutput.Width = BitmapWidthPixels;
				iD3DTextureDescriptionOutput.Height = BitmapHeightPixels;
				iD3DTextureDescriptionOutput.MipLevels = 1;
				iD3DTextureDescriptionOutput.ArraySize = 1;
				iD3DTextureDescriptionOutput.Format = DXGI_FORMAT_B8G8R8A8_TYPELESS;
				iD3DTextureDescriptionOutput.SampleDesc.Count = 1;
				iD3DTextureDescriptionOutput.SampleDesc.Quality = 0;
				iD3DTextureDescriptionOutput.Usage = D3D11_USAGE_STAGING;
				iD3DTextureDescriptionOutput.BindFlags = 0;
				iD3DTextureDescriptionOutput.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
				iD3DTextureDescriptionOutput.MiscFlags = 0;

				//Create output texture
				hResult = iD3DDevice0->CreateTexture2D(&iD3DTextureDescriptionOutput, NULL, &iD3DScreenCaptureTextureOutput0);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Copy resize to output texture
				iD3DDeviceContext0->CopySubresourceRegion(iD3DScreenCaptureTextureResize0.Get(), 0, 0, 0, 0, iD3DScreenCaptureTextureOriginal0.Get(), 0, NULL);
				iD3DDeviceContext0->GenerateMips(iD3D11ShaderResourceView0.Get());
				iD3DDeviceContext0->CopySubresourceRegion(iD3DScreenCaptureTextureOutput0.Get(), 0, 0, 0, 0, iD3DScreenCaptureTextureResize0.Get(), BitmapMipLevel, NULL);
			}
			else
			{
				//Create output description
				D3D11_TEXTURE2D_DESC iD3DTextureDescriptionOutput;
				iD3DScreenCaptureTextureOriginal0->GetDesc(&iD3DTextureDescriptionOutput);
				iD3DTextureDescriptionOutput.Format = DXGI_FORMAT_B8G8R8A8_TYPELESS;
				iD3DTextureDescriptionOutput.Usage = D3D11_USAGE_STAGING;
				iD3DTextureDescriptionOutput.BindFlags = 0;
				iD3DTextureDescriptionOutput.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
				iD3DTextureDescriptionOutput.MiscFlags = 0;

				//Create output texture
				hResult = iD3DDevice0->CreateTexture2D(&iD3DTextureDescriptionOutput, NULL, &iD3DScreenCaptureTextureOutput0);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Copy original to output texture
				iD3DDeviceContext0->CopySubresourceRegion(iD3DScreenCaptureTextureOutput0.Get(), 0, 0, 0, 0, iD3DScreenCaptureTextureOriginal0.Get(), 0, NULL);
			}

			//Release output duplication frame
			hResult = iDxgiOutputDuplication0->ReleaseFrame();
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Map texture to subresource
			D3D11_MAPPED_SUBRESOURCE iD3DMappedSubResource;
			hResult = iD3DDeviceContext0->Map(iD3DScreenCaptureTextureOutput0.Get(), 0, D3D11_MAP_READ, 0, &iD3DMappedSubResource);
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Reset screenshot variables
			ScreenshotResetVariables();

			//Return image byte array
			BYTE* BitmapBuffer = new BYTE[BitmapByteSize];
			BYTE* BitmapBufferReturn = BitmapBuffer;
			BYTE* SourceBuffer = (BYTE*)iD3DMappedSubResource.pData;
			for (int i = 0; i < BitmapHeightPixels; i++)
			{
				memcpy(BitmapBuffer, SourceBuffer, BitmapWidthRows);
				SourceBuffer += iD3DMappedSubResource.RowPitch;
				BitmapBuffer += BitmapWidthRows;
			}
			return BitmapBufferReturn;
		}
		catch (BYTE*)
		{
			ScreenshotResetVariables();
			return NULL;
		}
	}
}