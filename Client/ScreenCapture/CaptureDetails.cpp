#include "CaptureVariables.h"

namespace
{
	FLOAT GetMonitorSDRWhiteLevel(UINT CaptureMonitorId)
	{
		try
		{
			UINT displayPathCount;
			UINT displayModeCount;
			LONG eResult = GetDisplayConfigBufferSizes(QDC_ONLY_ACTIVE_PATHS, &displayPathCount, &displayModeCount);
			if (eResult != 0)
			{
				return 240.0F;
			}

			DISPLAYCONFIG_PATH_INFO* pathInfos = new DISPLAYCONFIG_PATH_INFO[displayPathCount];
			DISPLAYCONFIG_MODE_INFO* modeInfos = new DISPLAYCONFIG_MODE_INFO[displayModeCount];
			eResult = QueryDisplayConfig(QDC_ONLY_ACTIVE_PATHS, &displayPathCount, pathInfos, &displayModeCount, modeInfos, NULL);
			if (eResult != 0)
			{
				return 240.0F;
			}

			DISPLAYCONFIG_PATH_INFO pathInfo = pathInfos[CaptureMonitorId];

			DISPLAYCONFIG_SDR_WHITE_LEVEL sdrWhiteLevel{};
			sdrWhiteLevel.header.size = sizeof(sdrWhiteLevel);
			sdrWhiteLevel.header.type = DISPLAYCONFIG_DEVICE_INFO_GET_SDR_WHITE_LEVEL;
			sdrWhiteLevel.header.adapterId = pathInfo.targetInfo.adapterId;
			sdrWhiteLevel.header.id = pathInfo.targetInfo.id;

			eResult = DisplayConfigGetDeviceInfo(&sdrWhiteLevel.header);
			if (eResult != 0)
			{
				return 240.0F;
			}

			return (sdrWhiteLevel.SDRWhiteLevel / 1000) * 80.0F;
		}
		catch (...)
		{
			return 240.0F;
		}
	}
};