#define WIN32_LEAN_AND_MEAN
#include "ScreenCapture.h"

BOOL CaptureResetVariables()
{
	try
	{
		//Capture Variables
		iDxgiDevice.Release();
		iDxgiAdapter.Release();
		iDxgiOutput.Release();
		iDxgiOutput6.Release();
		iDxgiOutputDuplication.Release();
		iD3DDevice.Release();
		iD3DDeviceContext.Release();

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
		iDxgiResource.Release();
		iD3DScreenCaptureTextureOriginal.Release();
		iD3DScreenCaptureTextureOutput.Release();
		iD3DScreenCaptureTextureResize.Release();
		iD3D11ShaderResourceView.Release();

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
	__declspec(dllexport) BOOL CaptureInitialize(UINT CaptureMonitor, BOOL* OutputHDR)
	{
		try
		{
			//Disable debug reporting
			_CrtSetReportMode(_CRT_ASSERT, 0);

			//Reset capture variables
			CaptureResetVariables();

			//Create D3D Device
			for (UINT FeatureIndex = 0; FeatureIndex < NumD3DFeatureLevels; FeatureIndex++)
			{
				D3D_FEATURE_LEVEL iD3DFeatureLevel;
				hResult = D3D11CreateDevice(nullptr, D3D_DRIVER_TYPE_HARDWARE, nullptr, 0, &ArrayD3DFeatureLevels[FeatureIndex], 1, D3D11_SDK_VERSION, &iD3DDevice, &iD3DFeatureLevel, &iD3DDeviceContext);
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
			hResult = iD3DDevice->QueryInterface(&iDxgiDevice);
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get DXGI Adapter
			hResult = iDxgiDevice->GetParent(IID_PPV_ARGS(&iDxgiAdapter));
			iDxgiDevice.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get DXGI Output
			hResult = iDxgiAdapter->EnumOutputs(CaptureMonitor, &iDxgiOutput);
			iDxgiAdapter.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Query DXGI Output
			hResult = iDxgiOutput->QueryInterface(&iDxgiOutput6);
			iDxgiOutput.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Create output duplicate
			hResult = iDxgiOutput6->DuplicateOutput(iD3DDevice, &iDxgiOutputDuplication);
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get output description
			DXGI_OUTPUT_DESC1 iDxgiOutputDescription;
			hResult = iDxgiOutput6->GetDesc1(&iDxgiOutputDescription);
			iDxgiOutput6.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

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

	__declspec(dllexport) BYTE* CaptureScreenshot(UINT* OutputWidth, UINT* OutputHeight, UINT* OutputSize, UINT ResizeMipLevel)
	{
		try
		{
			//Get output duplication frame
			DXGI_OUTDUPL_FRAME_INFO iDxgiOutputDuplicationFrameInfo;
			hResult = iDxgiOutputDuplication->AcquireNextFrame(INFINITE, &iDxgiOutputDuplicationFrameInfo, &iDxgiResource);
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Get screen texture
			hResult = iDxgiResource->QueryInterface(&iD3DScreenCaptureTextureOriginal);
			iDxgiResource.Release();
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Get screen description
			D3D11_TEXTURE2D_DESC iD3DTextureDescriptionOriginal;
			iD3DScreenCaptureTextureOriginal->GetDesc(&iD3DTextureDescriptionOriginal);

			//Check resize resolution
			UINT widthResize = iD3DTextureDescriptionOriginal.Width >> ResizeMipLevel;
			UINT heightResize = iD3DTextureDescriptionOriginal.Height >> ResizeMipLevel;
			if (widthResize < 300 || heightResize < 300)
			{
				ResizeMipLevel = 0;
			}

			//Create output texture
			if (ResizeMipLevel > 0)
			{
				//Create resize description
				iD3DTextureDescriptionOriginal.MipLevels = ResizeMipLevel + 1;
				iD3DTextureDescriptionOriginal.MiscFlags = D3D11_RESOURCE_MISC_GENERATE_MIPS;

				//Create resize texture
				hResult = iD3DDevice->CreateTexture2D(&iD3DTextureDescriptionOriginal, NULL, &iD3DScreenCaptureTextureResize);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Create resize shader view
				hResult = iD3DDevice->CreateShaderResourceView(iD3DScreenCaptureTextureResize, NULL, &iD3D11ShaderResourceView);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Create output description
				D3D11_TEXTURE2D_DESC iD3DTextureDescriptionOutput;
				iD3DTextureDescriptionOutput.Width = widthResize;
				iD3DTextureDescriptionOutput.Height = heightResize;
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
				hResult = iD3DDevice->CreateTexture2D(&iD3DTextureDescriptionOutput, NULL, &iD3DScreenCaptureTextureOutput);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Copy resize to output texture
				iD3DDeviceContext->CopySubresourceRegion(iD3DScreenCaptureTextureResize, 0, 0, 0, 0, iD3DScreenCaptureTextureOriginal, 0, NULL);
				iD3DDeviceContext->GenerateMips(iD3D11ShaderResourceView);
				iD3DDeviceContext->CopySubresourceRegion(iD3DScreenCaptureTextureOutput, 0, 0, 0, 0, iD3DScreenCaptureTextureResize, ResizeMipLevel, NULL);
			}
			else
			{
				//Create output description
				iD3DTextureDescriptionOriginal.Format = DXGI_FORMAT_B8G8R8A8_TYPELESS;
				iD3DTextureDescriptionOriginal.Usage = D3D11_USAGE_STAGING;
				iD3DTextureDescriptionOriginal.BindFlags = 0;
				iD3DTextureDescriptionOriginal.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
				iD3DTextureDescriptionOriginal.MiscFlags = 0;

				//Create output texture
				hResult = iD3DDevice->CreateTexture2D(&iD3DTextureDescriptionOriginal, NULL, &iD3DScreenCaptureTextureOutput);
				if (!SUCCEEDED(hResult))
				{
					ScreenshotResetVariables();
					return NULL;
				}

				//Copy original to output texture
				iD3DDeviceContext->CopySubresourceRegion(iD3DScreenCaptureTextureOutput, 0, 0, 0, 0, iD3DScreenCaptureTextureOriginal, 0, NULL);
			}

			//Release output duplication frame
			hResult = iDxgiOutputDuplication->ReleaseFrame();
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Map texture to subresource
			D3D11_MAPPED_SUBRESOURCE iD3DMappedSubResource;
			hResult = iD3DDeviceContext->Map(iD3DScreenCaptureTextureOutput, 0, D3D11_MAP_READ, 0, &iD3DMappedSubResource);
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Reset screenshot variables
			ScreenshotResetVariables();

			//Return image byte array
			*OutputWidth = iD3DMappedSubResource.RowPitch / 4;
			*OutputHeight = iD3DMappedSubResource.DepthPitch / iD3DMappedSubResource.RowPitch;
			*OutputSize = *OutputHeight * iD3DMappedSubResource.RowPitch;
			return (BYTE*)iD3DMappedSubResource.pData;
		}
		catch (BYTE*)
		{
			ScreenshotResetVariables();
			return NULL;
		}
	}
}