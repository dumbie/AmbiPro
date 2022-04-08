#include "CaptureVariables.h"
#include "CaptureDetails.cpp"

namespace
{
	BOOL CaptureResetVariablesAll()
	{
		try
		{
			//Devices
			iDxgiDevice4.Release();
			iDxgiAdapter4.Release();
			iDxgiOutput0.Release();
			iDxgiOutput6.Release();
			iDxgiOutputDuplication0.Release();
			iD3D11Device0.Release();
			iD3D11Device5.Release();
			iD3D11DeviceContext0.Release();
			iD3D11DeviceContext4.Release();

			//Views
			iD3D11InputLayout0.Release();
			iD3D11RenderTargetView0.Release();
			iD3D11ShaderResourceView0.Release();

			//Shaders
			iD3D11Buffer0.Release();
			iD3DBlob0VertexShader.Release();
			iD3DBlob0PixelShader.Release();
			iD3D11VertexShader0.Release();
			iD3D11PixelShader0.Release();

			//Textures
			iDxgiResource0.Release();
			iD3D11Texture2D1CpuRead.Release();
			iD3D11Texture2D1Capture.Release();
			iD3D11Texture2D1RenderTargetView.Release();

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL CaptureResetVariablesScreenshot()
	{
		try
		{
			//Views
			iD3D11ShaderResourceView0.Release();

			//Textures
			iDxgiResource0.Release();
			iD3D11Texture2D1CpuRead.Release();
			iD3D11Texture2D1Capture.Release();

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL InitializeDirectX(UINT CaptureMonitorId, UINT MaxPixelDimension)
	{
		try
		{
			//Create D3D11 Device
			D3D_FEATURE_LEVEL iD3DFeatureLevel;
			hResult = D3D11CreateDevice(NULL, D3D_DRIVER_TYPE_HARDWARE, NULL, 0, D3DFeatureLevelsArray, D3DFeatureLevelsCount, D3D11_SDK_VERSION, &iD3D11Device0, &iD3DFeatureLevel, &iD3D11DeviceContext0);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Convert variables
			hResult = iD3D11Device0->QueryInterface(&iD3D11Device5);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Convert variables
			hResult = iD3D11Device5->QueryInterface(&iDxgiDevice4);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Convert variables
			hResult = iD3D11DeviceContext0->QueryInterface(&iD3D11DeviceContext4);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Get DXGI Adapter
			hResult = iDxgiDevice4->GetParent(IID_PPV_ARGS(&iDxgiAdapter4));
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Get DXGI Output
			hResult = iDxgiAdapter4->EnumOutputs(CaptureMonitorId, &iDxgiOutput0);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Convert variables
			hResult = iDxgiOutput0->QueryInterface(&iDxgiOutput6);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Get output duplicate (requires process dpi awareness)
			hResult = iDxgiOutput6->DuplicateOutput1(iD3D11Device5, 0, iDxgiFormatsCount, iDxgiFormatsArray, &iDxgiOutputDuplication0);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Get output description
			DXGI_OUTPUT_DESC1 iDxgiOutputDescription;
			hResult = iDxgiOutput6->GetDesc1(&iDxgiOutputDescription);
			if (FAILED(hResult))
			{
				CaptureResetVariablesAll();
				return false;
			}

			//Get duplicate description
			DXGI_OUTDUPL_DESC iDxgiOutputDuplicationDescription;
			iDxgiOutputDuplication0->GetDesc(&iDxgiOutputDuplicationDescription);

			//Get monitor HDR details
			vCaptureHDREnabled = iDxgiOutputDescription.ColorSpace == DXGI_COLOR_SPACE_RGB_FULL_G2084_NONE_P2020 || iDxgiOutputDescription.ColorSpace == DXGI_COLOR_SPACE_RGB_STUDIO_G2084_NONE_P2020;
			if (vCaptureHDREnabled)
			{
				//Fix Detect when changed in Windows settings
				vCaptureSDRWhiteLevel = GetMonitorSDRWhiteLevel(CaptureMonitorId);
			}

			//Update capture variables
			vCaptureWidth = iDxgiOutputDuplicationDescription.ModeDesc.Width;
			vCaptureHeight = iDxgiOutputDuplicationDescription.ModeDesc.Height;
			if (MaxPixelDimension != 0 && vCaptureWidth > MaxPixelDimension && vCaptureHeight > MaxPixelDimension)
			{
				DOUBLE resizedWidth = 0.01;
				DOUBLE resizedHeight = 0.01;
				UINT minDimension = min(vCaptureWidth, vCaptureHeight);
				//Find nearest full pixel dimensions to keep ratio
				while (resizedWidth != (UINT)resizedWidth || resizedHeight != (UINT)resizedHeight)
				{
					DOUBLE differenceRatio = (DOUBLE)minDimension / MaxPixelDimension;
					resizedWidth = iDxgiOutputDuplicationDescription.ModeDesc.Width / differenceRatio;
					resizedHeight = iDxgiOutputDuplicationDescription.ModeDesc.Height / differenceRatio;
					MaxPixelDimension++;
				}
				vCaptureWidth = resizedWidth;
				vCaptureHeight = resizedHeight;
			}
			vCapturePixelByteSize = 4; //B8G8R8A8=4 R16G16B16A16=8
			vCaptureWidthByteSize = vCaptureWidth * vCapturePixelByteSize;
			vCaptureTotalByteSize = vCaptureWidth * vCaptureHeight * vCapturePixelByteSize;

			//Release resources
			iD3D11Device0.Release();
			iD3D11DeviceContext0.Release();
			iDxgiDevice4.Release();
			iDxgiAdapter4.Release();
			iDxgiOutput0.Release();
			iDxgiOutput6.Release();

			return true;
		}
		catch (...)
		{
			CaptureResetVariablesAll();
			return false;
		}
	}

	BOOL InitializeRenderTargetView()
	{
		try
		{
			//Create render target view texture
			D3D11_TEXTURE2D_DESC1 iD3DTexture2D1DescRenderTargetView{};
			iD3DTexture2D1DescRenderTargetView.Width = vCaptureWidth;
			iD3DTexture2D1DescRenderTargetView.Height = vCaptureHeight;
			iD3DTexture2D1DescRenderTargetView.MipLevels = 1;
			iD3DTexture2D1DescRenderTargetView.ArraySize = 1;
			iD3DTexture2D1DescRenderTargetView.Format = vCaptureHDREnabled ? DXGI_FORMAT_B8G8R8A8_UNORM_SRGB : DXGI_FORMAT_B8G8R8A8_UNORM;
			iD3DTexture2D1DescRenderTargetView.SampleDesc.Count = 1;
			iD3DTexture2D1DescRenderTargetView.SampleDesc.Quality = 0;
			iD3DTexture2D1DescRenderTargetView.Usage = D3D11_USAGE_DEFAULT;
			iD3DTexture2D1DescRenderTargetView.BindFlags = D3D11_BIND_RENDER_TARGET;
			iD3DTexture2D1DescRenderTargetView.CPUAccessFlags = 0;
			iD3DTexture2D1DescRenderTargetView.MiscFlags = 0;
			hResult = iD3D11Device5->CreateTexture2D1(&iD3DTexture2D1DescRenderTargetView, NULL, &iD3D11Texture2D1RenderTargetView);
			if (FAILED(hResult))
			{
				return false;
			}

			//Create and set render target view
			hResult = iD3D11Device5->CreateRenderTargetView(iD3D11Texture2D1RenderTargetView, NULL, &iD3D11RenderTargetView0);
			if (FAILED(hResult))
			{
				return false;
			}
			iD3D11DeviceContext4->OMSetRenderTargets(1, &iD3D11RenderTargetView0, NULL);
			iD3D11DeviceContext4->ClearRenderTargetView(iD3D11RenderTargetView0, ColorRgbaBlack);

			//Release resources
			iD3D11RenderTargetView0.Release();

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL InitializeViewPort()
	{
		try
		{
			//Create and set viewport
			D3D11_VIEWPORT iD3D11ViewPort{};
			iD3D11ViewPort.Width = vCaptureWidth;
			iD3D11ViewPort.Height = vCaptureHeight;
			iD3D11DeviceContext4->RSSetViewports(1, &iD3D11ViewPort);

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL InitializeShaders()
	{
		try
		{
			//Load shaders from file
			hResult = D3DCompileFromFile(L"Resources\\VertexShader.hlsl", 0, 0, "main", "vs_5_0", 0, 0, &iD3DBlob0VertexShader, 0);
			if (FAILED(hResult))
			{
				return false;
			}
			hResult = D3DCompileFromFile(L"Resources\\PixelShader.hlsl", 0, 0, "main", "ps_5_0", 0, 0, &iD3DBlob0PixelShader, 0);
			if (FAILED(hResult))
			{
				return false;
			}

			//Create shaders from blob
			hResult = iD3D11Device5->CreateVertexShader(iD3DBlob0VertexShader->GetBufferPointer(), iD3DBlob0VertexShader->GetBufferSize(), NULL, &iD3D11VertexShader0);
			if (FAILED(hResult))
			{
				return false;
			}
			hResult = iD3D11Device5->CreatePixelShader(iD3DBlob0PixelShader->GetBufferPointer(), iD3DBlob0PixelShader->GetBufferSize(), NULL, &iD3D11PixelShader0);
			if (FAILED(hResult))
			{
				return false;
			}

			//Create and set input layout
			hResult = iD3D11Device5->CreateInputLayout(InputElementsArray, InputElementsCount, iD3DBlob0VertexShader->GetBufferPointer(), iD3DBlob0VertexShader->GetBufferSize(), &iD3D11InputLayout0);
			if (FAILED(hResult))
			{
				return false;
			}
			iD3D11DeviceContext4->IASetInputLayout(iD3D11InputLayout0);

			//Create buffer description
			D3D11_BUFFER_DESC bufferDescription{};
			bufferDescription.ByteWidth = sizeof(VertexVertice) * VertexVerticesCount;
			bufferDescription.Usage = D3D11_USAGE_DEFAULT;
			bufferDescription.BindFlags = D3D11_BIND_VERTEX_BUFFER;
			bufferDescription.CPUAccessFlags = 0;
			bufferDescription.MiscFlags = 0;
			bufferDescription.StructureByteStride = 0;

			//Create subresource data
			D3D11_SUBRESOURCE_DATA subresourceData{};
			subresourceData.pSysMem = VertexVerticesArray;

			//Create vertex buffer
			hResult = iD3D11Device5->CreateBuffer(&bufferDescription, &subresourceData, &iD3D11Buffer0);
			if (FAILED(hResult))
			{
				return false;
			}

			//Set vertex buffer
			UINT bufferOffsets = 0;
			UINT bufferStrides = sizeof(VertexVertice);
			iD3D11DeviceContext4->IASetVertexBuffers(0, 1, &iD3D11Buffer0, &bufferStrides, &bufferOffsets);
			iD3D11DeviceContext4->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);

			//Set shaders
			iD3D11DeviceContext4->VSSetShader(iD3D11VertexShader0, NULL, 0);
			iD3D11DeviceContext4->PSSetShader(iD3D11PixelShader0, NULL, 0);

			//Release resources
			iD3D11Buffer0.Release();
			iD3DBlob0VertexShader.Release();
			iD3DBlob0PixelShader.Release();
			iD3D11VertexShader0.Release();
			iD3D11PixelShader0.Release();
			iD3D11InputLayout0.Release();

			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	BOOL SetShaderVariables()
	{
		try
		{
			//Create shader variables
			ShaderVariables shaderVariables{};
			shaderVariables.HDREnabled = vCaptureHDREnabled;
			shaderVariables.SDRWhiteLevel = vCaptureSDRWhiteLevel;

			//Create buffer description
			D3D11_BUFFER_DESC bufferDescription{};
			bufferDescription.ByteWidth = sizeof(ShaderVariables);
			bufferDescription.Usage = D3D11_USAGE_DEFAULT;
			bufferDescription.BindFlags = 0;
			bufferDescription.CPUAccessFlags = 0;
			bufferDescription.MiscFlags = 0;
			bufferDescription.StructureByteStride = 0;

			//Create subresource data
			D3D11_SUBRESOURCE_DATA subresourceData{};
			subresourceData.pSysMem = &shaderVariables;

			//Create shader variables buffer
			hResult = iD3D11Device5->CreateBuffer(&bufferDescription, &subresourceData, &iD3D11Buffer0);
			if (FAILED(hResult))
			{
				return false;
			}

			//Set shader variables
			iD3D11DeviceContext4->PSSetConstantBuffers(0, 1, &iD3D11Buffer0);

			//Release resources
			iD3D11Buffer0.Release();

			return true;
		}
		catch (...)
		{
			return false;
		}
	}
};