<!DOCTYPE html>
<html>
<head>
    <title>AmbiPro Remote</title>
    <meta charset="UTF-8">
    <meta content="AmbiPro Remote" name="description">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=0" name="viewport">
    <!-- jQuery -->
    <script src="//code.jquery.com/jquery.min.js" type="text/javascript"></script>
    <script src="scripts/scripts.js" type="text/javascript"></script>
    <!-- Styles -->
    <link href="styles/styles.css" media="all" rel="stylesheet" type="text/css">
	<!-- Icons -->
    <link href="images/AmbiPro.ico" rel="shortcut icon">
	<meta name="application-name" content=" "/>
</head>
<body>
    <div class="buttonAccent marginCenterTop"><a href="javascript:sendLedSwitch()"><span class="txtButton">Switch the leds on or off</span></a></div>

    <div class="clearBoth marginCenterTop">
        <span class="txtSettings marginCenter">Change led brightness:</span>
        <input id="LedBrightness" class="slider" type="range" min="5" max="100" step="5" value="50" oninput="sendLedBrightness();">
    </div>

    <div class="clearBoth marginCenterTop">
        <span class="txtSettings marginCenter">Change led display mode:</span>
        <select id="LedMode" class="select" onchange="sendLedMode();">
			<option hidden>Please select a mode...</option>
			<option value="0">Screen capture</option>
			<option value="1">Solid color</option>
			<option value="2">Color loop</option>
			<option value="3">Color spectrum</option>
        </select>
    </div>

	<div class="Bottom">
		<div class="clearBoth marginCenterTop effOpacity">
			<span class="txtSettings marginCenter">Remote connection settings:</span>
			<input id="ServerIp" type="text" placeholder="Server ip" oninput="updateServerIp();" size="16" />
			<input id="ServerPort" type="number" placeholder="Server port" oninput="updateServerPort();" size="8" />
		</div>

		<div class="clearBoth marginCenterTop effOpacity">
			<img width="120" src="images/AmbiPro.png" />
		</div>
    </div>
</body>
</html>