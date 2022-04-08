#include "CaptureVariables.h"

namespace
{
	BOOL BitmapDataSaveBmpFile(BYTE* bitmapData, const char* filePath)
	{
		try
		{
			//Create bitmap info
			BITMAPINFO bmpInfo{};
			bmpInfo.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
			bmpInfo.bmiHeader.biWidth = vCaptureWidth;
			bmpInfo.bmiHeader.biHeight = vCaptureHeight;
			bmpInfo.bmiHeader.biPlanes = 1;
			bmpInfo.bmiHeader.biBitCount = 32; //B8G8R8A8=32 R16G16B16A16=64
			bmpInfo.bmiHeader.biCompression = BI_RGB;
			bmpInfo.bmiHeader.biSizeImage = vCaptureTotalByteSize;

			//Create bitmap file header
			BITMAPFILEHEADER bmpFileHeader{};
			bmpFileHeader.bfType = 'MB';
			bmpFileHeader.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + bmpInfo.bmiHeader.biSizeImage;
			bmpFileHeader.bfReserved1 = 0;
			bmpFileHeader.bfReserved2 = 0;
			bmpFileHeader.bfOffBits = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);

			//Write bitmap file
			FILE* bitmapFile = fopen(filePath, "wb");
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
};