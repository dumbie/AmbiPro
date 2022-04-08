#include "CaptureVariables.h"

namespace
{
	BYTE* ConvertTexture2DtoBitmapData(CComPtr<ID3D11Texture2D1>& textureTarget)
	{
		try
		{
			//Map texture to subresource
			D3D11_MAPPED_SUBRESOURCE iD3DMappedSubResource;
			hResult = iD3D11DeviceContext4->Map(textureTarget, 0, D3D11_MAP_READ, 0, &iD3DMappedSubResource);
			if (FAILED(hResult))
			{
				return NULL;
			}

			//Create image byte array
			BYTE* BitmapBuffer = new BYTE[vCaptureTotalByteSize];
			BYTE* BitmapBufferReturn = BitmapBuffer;
			BYTE* SourceBuffer = (BYTE*)iD3DMappedSubResource.pData;
			for (int i = 0; i < vCaptureHeight; i++)
			{
				memcpy(BitmapBuffer, SourceBuffer, vCaptureWidthByteSize);
				SourceBuffer += iD3DMappedSubResource.RowPitch;
				BitmapBuffer += vCaptureWidthByteSize;
			}

			//Unmap texture from subresource
			iD3D11DeviceContext4->Unmap(textureTarget, 0);

			//Return image byte array
			return BitmapBufferReturn;
		}
		catch (...)
		{
			return NULL;
		}
	}

	BOOL ConvertTexture2DtoCpuRead(CComPtr<ID3D11Texture2D1>& textureTarget)
	{
		try
		{
			//Read texture description
			D3D11_TEXTURE2D_DESC1 iD3DTexture2D1DescCpuRead;
			textureTarget->GetDesc1(&iD3DTexture2D1DescCpuRead);

			//Check if texture already has cpu access
			if (iD3DTexture2D1DescCpuRead.Usage == D3D11_USAGE_STAGING && iD3DTexture2D1DescCpuRead.CPUAccessFlags & D3D11_CPU_ACCESS_READ)
			{
				iD3D11Texture2D1CpuRead = textureTarget;
				return true;
			}

			//Update texture description
			//iD3DTexture2D1DescCpuRead.Width = iD3DTexture2D1DescCpuRead.Width;
			//iD3DTexture2D1DescCpuRead.Height = iD3DTexture2D1DescCpuRead.Height;
			iD3DTexture2D1DescCpuRead.MipLevels = 1;
			iD3DTexture2D1DescCpuRead.ArraySize = 1;
			//iD3DTexture2D1DescCpuRead.Format = iD3DTexture2D1DescCpuRead.Format;
			iD3DTexture2D1DescCpuRead.SampleDesc.Count = 1;
			iD3DTexture2D1DescCpuRead.SampleDesc.Quality = 0;
			iD3DTexture2D1DescCpuRead.Usage = D3D11_USAGE_STAGING;
			iD3DTexture2D1DescCpuRead.BindFlags = 0;
			iD3DTexture2D1DescCpuRead.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
			iD3DTexture2D1DescCpuRead.MiscFlags = 0;

			//Create cpu texture
			hResult = iD3D11Device5->CreateTexture2D1(&iD3DTexture2D1DescCpuRead, NULL, &iD3D11Texture2D1CpuRead);
			if (FAILED(hResult))
			{
				return false;
			}

			//Copy target texture to cpu texture
			iD3D11DeviceContext4->CopySubresourceRegion(iD3D11Texture2D1CpuRead, 0, 0, 0, 0, textureTarget, 0, NULL);

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL ApplyShadersToTexture2D(CComPtr<ID3D11Texture2D1>& textureTarget)
	{
		try
		{
			//Create shader resource view
			hResult = iD3D11Device5->CreateShaderResourceView(textureTarget, NULL, &iD3D11ShaderResourceView0);
			if (FAILED(hResult))
			{
				return false;
			}

			//Set shader resource view
			iD3D11DeviceContext4->PSSetShaderResources(0, 1, &iD3D11ShaderResourceView0);

			//Draw texture with shaders
			iD3D11DeviceContext4->Draw(VertexVerticesCount, 0);

			return true;
		}
		catch (...)
		{
			return false;
		}
	}
};