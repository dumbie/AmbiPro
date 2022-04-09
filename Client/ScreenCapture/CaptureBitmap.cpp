#include "CaptureVariables.h"

namespace
{
	BOOL BitmapDataSaveFileBmp(BYTE* bitmapData, WCHAR* filePath)
	{
		try
		{
			//Create bitmap
			Gdiplus::Bitmap gdiBitmap(vCaptureWidth, vCaptureHeight);
			Gdiplus::Rect nodRectangle(0, 0, vCaptureWidth, vCaptureHeight);

			//Copy data to bitmap
			Gdiplus::BitmapData* bitmapDataLock = new Gdiplus::BitmapData;
			gdiBitmap.LockBits(&nodRectangle, 0, PixelFormat32bppARGB, bitmapDataLock);
			memcpy(bitmapDataLock->Scan0, bitmapData, (size_t)vCaptureTotalByteSize);
			gdiBitmap.UnlockBits(bitmapDataLock);

			//Get save class identifier
			CLSID imageSaveClassId;
			hResult = CLSIDFromString(L"{557CF400-1A04-11D3-9A73-0000F81EF32E}", &imageSaveClassId);
			if (FAILED(hResult))
			{
				return false;
			}

			//Save bitmap to file
			gdiBitmap.Save(filePath, &imageSaveClassId, NULL);

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL BitmapDataSaveFileJpg(BYTE* bitmapData, WCHAR* filePath, UINT saveQuality)
	{
		try
		{
			//Create bitmap
			Gdiplus::Bitmap gdiBitmap(vCaptureWidth, vCaptureHeight);
			Gdiplus::Rect nodRectangle(0, 0, vCaptureWidth, vCaptureHeight);

			//Copy data to bitmap
			Gdiplus::BitmapData* bitmapDataLock = new Gdiplus::BitmapData;
			gdiBitmap.LockBits(&nodRectangle, 0, PixelFormat32bppARGB, bitmapDataLock);
			memcpy(bitmapDataLock->Scan0, bitmapData, (size_t)vCaptureTotalByteSize);
			gdiBitmap.UnlockBits(bitmapDataLock);

			//Get save class identifier
			CLSID imageSaveClassId;
			hResult = CLSIDFromString(L"{557CF401-1A04-11D3-9A73-0000F81EF32E}", &imageSaveClassId);
			if (FAILED(hResult))
			{
				return false;
			}

			//Save bitmap to file
			Gdiplus::EncoderParameters encoderParameters{};
			encoderParameters.Count = 1;
			encoderParameters.Parameter[0].Guid = Gdiplus::EncoderQuality;
			encoderParameters.Parameter[0].Type = Gdiplus::EncoderParameterValueTypeLong;
			encoderParameters.Parameter[0].NumberOfValues = 1;
			encoderParameters.Parameter[0].Value = &saveQuality;

			gdiBitmap.Save(filePath, &imageSaveClassId, &encoderParameters);

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL BitmapDataSaveFilePng(BYTE* bitmapData, WCHAR* filePath)
	{
		try
		{
			//Create bitmap
			Gdiplus::Bitmap gdiBitmap(vCaptureWidth, vCaptureHeight);
			Gdiplus::Rect nodRectangle(0, 0, vCaptureWidth, vCaptureHeight);
			Gdiplus::Graphics gdiGraphics(&gdiBitmap);

			//Copy data to bitmap
			Gdiplus::BitmapData* bitmapDataLock = new Gdiplus::BitmapData;
			gdiBitmap.LockBits(&nodRectangle, 0, PixelFormat32bppARGB, bitmapDataLock);
			memcpy(bitmapDataLock->Scan0, bitmapData, (size_t)vCaptureTotalByteSize);
			gdiBitmap.UnlockBits(bitmapDataLock);

			//Update alpha channel
			Gdiplus::ColorMatrix gdiColorMatrix =
			{
				1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
				0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
				0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
				0.0f, 0.0f, 0.0f, 255.0f, 0.0f,
				0.0f, 0.0f, 0.0f, 0.0f, 1.0f
			};
			Gdiplus::ImageAttributes gdiImageAttributes;
			gdiImageAttributes.SetColorMatrix(&gdiColorMatrix);
			gdiGraphics.DrawImage(&gdiBitmap, nodRectangle, 0, 0, vCaptureWidth, vCaptureHeight, Gdiplus::UnitPixel, &gdiImageAttributes);

			//Get save class identifier
			CLSID imageSaveClassId;
			hResult = CLSIDFromString(L"{557CF406-1A04-11D3-9A73-0000F81EF32E}", &imageSaveClassId);
			if (FAILED(hResult))
			{
				return false;
			}

			//Save bitmap to file
			gdiBitmap.Save(filePath, &imageSaveClassId, NULL);

			return true;
		}
		catch (...)
		{
			return false;
		}
	}
};