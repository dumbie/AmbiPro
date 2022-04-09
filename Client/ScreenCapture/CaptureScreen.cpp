#include "CaptureVariables.h"
#include "CaptureBitmap.cpp"
#include "CaptureInitialize.cpp"
#include "CaptureTexture.cpp"

namespace
{
	extern "C"
	{
		__declspec(dllexport) BOOL CaptureInitialize(UINT CaptureMonitorId, UINT* OutputWidth, UINT* OutputHeight, UINT* OutputTotalByteSize, UINT* OutputHDREnabled, UINT MaxPixelDimension)
		{
			try
			{
				//Disable assert reporting
				_CrtSetReportMode(_CRT_ASSERT, 0);

				//Reset all used variables
				CaptureResetVariablesAll();

				//Initialize DirectX
				if (!InitializeDirectX(CaptureMonitorId, MaxPixelDimension)) { return false; }

				//Initialize render target view
				if (!InitializeRenderTargetView()) { return false; }

				//Initialize view port
				if (!InitializeViewPort()) { return false; }

				//Initialize shaders
				if (!InitializeShaders()) { return false; }

				//Set shader variables
				if (!SetShaderVariables()) { return false; }

				//Set out parameters
				*OutputWidth = vCaptureWidth;
				*OutputHeight = vCaptureHeight;
				*OutputTotalByteSize = vCaptureTotalByteSize;
				*OutputHDREnabled = vCaptureHDREnabled;
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
				return CaptureResetVariablesAll();
			}
			catch (...)
			{
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

		__declspec(dllexport) BOOL CaptureSaveFileBmp(BYTE* bitmapData, WCHAR* filePath)
		{
			try
			{
				return BitmapDataSaveFileBmp(bitmapData, filePath);
			}
			catch (...)
			{
				return false;
			}
		}

		__declspec(dllexport) BOOL CaptureSaveFileJpg(BYTE* bitmapData, WCHAR* filePath, UINT saveQuality)
		{
			try
			{
				return BitmapDataSaveFileJpg(bitmapData, filePath, saveQuality);
			}
			catch (...)
			{
				return false;
			}
		}

		__declspec(dllexport) BOOL CaptureSaveFilePng(BYTE* bitmapData, WCHAR* filePath)
		{
			try
			{
				return BitmapDataSaveFilePng(bitmapData, filePath);
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
				if (FAILED(hResult))
				{
					CaptureResetVariablesScreenshot();
					return NULL;
				}

				//Convert variables
				hResult = iDxgiResource0->QueryInterface(&iD3D11Texture2D1Capture);
				if (FAILED(hResult))
				{
					CaptureResetVariablesScreenshot();
					return NULL;
				}

				//Apply shaders to texture
				if (!ApplyShadersToTexture2D(iD3D11Texture2D1Capture))
				{
					CaptureResetVariablesScreenshot();
					return NULL;
				}

				//Convert to cpu read texture
				if (!ConvertTexture2DtoCpuRead(iD3D11Texture2D1RenderTargetView))
				{
					CaptureResetVariablesScreenshot();
					return NULL;
				}

				//Convert texture to bitmap data
				BYTE* bitmapData = ConvertTexture2DtoBitmapData(iD3D11Texture2D1CpuRead);

				//Release output duplication frame
				hResult = iDxgiOutputDuplication0->ReleaseFrame();
				if (FAILED(hResult))
				{
					CaptureResetVariablesScreenshot();
					return NULL;
				}

				//Release resources
				CaptureResetVariablesScreenshot();

				//Return bitmap data
				return bitmapData;
			}
			catch (...)
			{
				CaptureResetVariablesScreenshot();
				return NULL;
			}
		}
	}
};