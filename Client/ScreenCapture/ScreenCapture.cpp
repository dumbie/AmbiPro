#include "ScreenCapture.h"

BOOL CaptureResetVariables()
{
	try
	{
		//Capture Variables
		iDxgiDevice4.Release();
		iDxgiAdapter4.Release();
		iDxgiOutput0.Release();
		iDxgiOutput6.Release();
		iDxgiOutputDuplication0.Release();
		iD3DDevice0.Release();
		iD3DDevice5.Release();
		iD3DDeviceContext0.Release();
		iD3DDeviceContext4.Release();

		//Result Variables
		hResult = E_FAIL;
		return true;
	}
	catch (...)
	{
		return false;
	}
}

BOOL ScreenshotResetVariables()
{
	try
	{
		//Screenshot Variables
		iDxgiResource0.Release();
		iD3D11Texture2D1Original.Release();
		iD3D11Texture2D1Resize.Release();
		iD3D11Texture2D1Cpu.Release();
		iD3DBlob0.Release();
		iD3D11PixelShader0.Release();
		iD3D11ShaderResourceView0.Release();

		//Result Variables
		hResult = E_FAIL;
		return true;
	}
	catch (...)
	{
		return false;
	}
}

BOOL Texture2D1ResizeMip(CComPtr<ID3D11Texture2D1>& iD3D11Texture2D1Target)
{
	try
	{
		//Read texture description
		D3D11_TEXTURE2D_DESC1 iD3DTexture2D1DescResize;
		iD3D11Texture2D1Target->GetDesc1(&iD3DTexture2D1DescResize);
		iD3DTexture2D1DescResize.MipLevels = BitmapMipLevel + 1;
		iD3DTexture2D1DescResize.MiscFlags = D3D11_RESOURCE_MISC_GENERATE_MIPS;

		//Create resize texture
		iD3DDevice5->CreateTexture2D1(&iD3DTexture2D1DescResize, NULL, &iD3D11Texture2D1Resize);

		//Copy target to resize texture
		iD3DDeviceContext4->CopySubresourceRegion(iD3D11Texture2D1Resize, 0, 0, 0, 0, iD3D11Texture2D1Target, 0, NULL);

		//Create resize shader view
		iD3DDevice5->CreateShaderResourceView(iD3D11Texture2D1Resize, NULL, &iD3D11ShaderResourceView0);
		iD3DDeviceContext4->GenerateMips(iD3D11ShaderResourceView0);

		//Release texture resource
		iD3D11Texture2D1Target.Release();
		iD3D11ShaderResourceView0.Release();

		//Replace texture resource
		iD3D11Texture2D1Target = iD3D11Texture2D1Resize;
		return true;
	}
	catch (...)
	{
		return false;
	}
}

BYTE* Texture2D1ToBitmapData(CComPtr<ID3D11Texture2D1>& iD3D11Texture2D1Target)
{
	try
	{
		//Read texture description
		D3D11_TEXTURE2D_DESC1 iD3DTexture2D1DescCpu;
		iD3D11Texture2D1Target->GetDesc1(&iD3DTexture2D1DescCpu);

		//Check if texture has cpu read and convert
		if (iD3DTexture2D1DescCpu.Usage == D3D11_USAGE_STAGING && iD3DTexture2D1DescCpu.CPUAccessFlags & D3D11_CPU_ACCESS_READ)
		{
			//Replace texture resource
			iD3D11Texture2D1Cpu = iD3D11Texture2D1Target;
		}
		else
		{
			//Update cpu description
			iD3DTexture2D1DescCpu.Width = BitmapWidthPixels;
			iD3DTexture2D1DescCpu.Height = BitmapHeightPixels;
			iD3DTexture2D1DescCpu.MipLevels = 1;
			iD3DTexture2D1DescCpu.ArraySize = 1;
			iD3DTexture2D1DescCpu.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
			iD3DTexture2D1DescCpu.SampleDesc.Count = 1;
			iD3DTexture2D1DescCpu.SampleDesc.Quality = 0;
			iD3DTexture2D1DescCpu.Usage = D3D11_USAGE_STAGING;
			iD3DTexture2D1DescCpu.BindFlags = 0;
			iD3DTexture2D1DescCpu.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
			iD3DTexture2D1DescCpu.MiscFlags = 0;

			//Create cpu texture
			iD3DDevice5->CreateTexture2D1(&iD3DTexture2D1DescCpu, NULL, &iD3D11Texture2D1Cpu);

			//Copy to cpu texture
			iD3DDeviceContext4->CopySubresourceRegion(iD3D11Texture2D1Cpu, 0, 0, 0, 0, iD3D11Texture2D1Target, BitmapMipLevel, NULL);
		}

		//Map texture to subresource
		D3D11_MAPPED_SUBRESOURCE iD3DMappedSubResource;
		iD3DDeviceContext4->Map(iD3D11Texture2D1Cpu, 0, D3D11_MAP_READ, 0, &iD3DMappedSubResource);

		//Release texture resource
		iD3DDeviceContext4->Unmap(iD3D11Texture2D1Cpu, 0);
		iD3D11Texture2D1Cpu.Release();
		iD3D11Texture2D1Target.Release();

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
	catch (...)
	{
		return NULL;
	}
}

BOOL BitmapDataToBmpFile(BYTE* bitmapData, std::string filePath)
{
	try
	{
		//Create bitmap info
		BITMAPINFO bmpInfo;
		bmpInfo.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
		bmpInfo.bmiHeader.biWidth = BitmapWidthPixels;
		bmpInfo.bmiHeader.biHeight = BitmapHeightPixels;
		bmpInfo.bmiHeader.biPlanes = 1;
		bmpInfo.bmiHeader.biBitCount = 32;
		bmpInfo.bmiHeader.biCompression = BI_RGB;
		bmpInfo.bmiHeader.biSizeImage = BitmapByteSize;

		//Create bitmap file header
		BITMAPFILEHEADER bmpFileHeader;
		bmpFileHeader.bfType = 'MB';
		bmpFileHeader.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + bmpInfo.bmiHeader.biSizeImage;
		bmpFileHeader.bfReserved1 = 0;
		bmpFileHeader.bfReserved2 = 0;
		bmpFileHeader.bfOffBits = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);

		//Write bitmap file
		FILE* bitmapFile = fopen(filePath.c_str(), "wb");
		fwrite(&bmpFileHeader, sizeof(BITMAPFILEHEADER), 1, bitmapFile);
		fwrite(&bmpInfo.bmiHeader, sizeof(BITMAPINFO), 1, bitmapFile);
		fwrite(bitmapData, bmpInfo.bmiHeader.biSizeImage, 1, bitmapFile);
		fclose(bitmapFile);
		return true;
	}
	catch (...)
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

			//Reset all used variables
			CaptureResetVariables();
			ScreenshotResetVariables();

			//Create D3D Device
			D3D_FEATURE_LEVEL iD3DFeatureLevel;
			for (UINT FeatureIndex = 0; FeatureIndex < D3DFeatureLevelsCount; FeatureIndex++)
			{
				hResult = D3D11CreateDevice(NULL, D3D_DRIVER_TYPE_HARDWARE, NULL, 0, &D3DFeatureLevelsArray[FeatureIndex], 1, D3D11_SDK_VERSION, &iD3DDevice0, &iD3DFeatureLevel, &iD3DDeviceContext0);
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

			//Convert variables
			hResult = iD3DDevice0->QueryInterface(&iD3DDevice5);
			iD3DDevice0.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Convert variables
			hResult = iD3DDevice5->QueryInterface(&iDxgiDevice4);
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Convert variables
			hResult = iD3DDeviceContext0->QueryInterface(&iD3DDeviceContext4);
			iD3DDeviceContext0.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get DXGI Adapter
			hResult = iDxgiDevice4->GetParent(IID_PPV_ARGS(&iDxgiAdapter4));
			iDxgiDevice4.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Get DXGI Output
			hResult = iDxgiAdapter4->EnumOutputs(CaptureMonitor, &iDxgiOutput0);
			iDxgiAdapter4.Release();
			if (!SUCCEEDED(hResult))
			{
				CaptureResetVariables();
				return false;
			}

			//Convert variables
			hResult = iDxgiOutput0->QueryInterface(&iDxgiOutput6);
			iDxgiOutput0.Release();
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
			hResult = iDxgiOutput6->DuplicateOutput(iD3DDevice5, &iDxgiOutputDuplication0);
			iDxgiOutput6.Release();
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
			BitmapHDREnabled = iDxgiOutputDescription.ColorSpace == DXGI_COLOR_SPACE_RGB_FULL_G2084_NONE_P2020 || iDxgiOutputDescription.ColorSpace == DXGI_COLOR_SPACE_RGB_STUDIO_G2084_NONE_P2020;
			*OutputHDR = BitmapHDREnabled;
			return true;
		}
		catch (...)
		{
			CaptureResetVariables();
			return false;
		}
	}

	__declspec(dllexport) BOOL CaptureFreeMemory(BYTE* FreeMemory)
	{
		try
		{
			delete[] FreeMemory;
			return true;
		}
		catch (...)
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
		catch (...)
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

			//Convert variables
			hResult = iDxgiResource0->QueryInterface(&iD3D11Texture2D1Original);
			iDxgiResource0.Release();
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Resize the texture
			if (BitmapMipLevel > 0)
			{
				Texture2D1ResizeMip(iD3D11Texture2D1Original);
			}

			//Convert texture to bitmap data
			BYTE* bitmapData = Texture2D1ToBitmapData(iD3D11Texture2D1Original);
			//BitmapDataToBmpFile(bitmapData, "ScreenCapture.bmp");

			//Release output duplication frame
			hResult = iDxgiOutputDuplication0->ReleaseFrame();
			if (!SUCCEEDED(hResult))
			{
				ScreenshotResetVariables();
				return NULL;
			}

			//Reset screenshot variables
			ScreenshotResetVariables();

			//Return bitmap data
			return bitmapData;
		}
		catch (...)
		{
			ScreenshotResetVariables();
			return NULL;
		}
	}
}