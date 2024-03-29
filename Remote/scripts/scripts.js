﻿function sendLedSwitch() {
    sendSocket("LedSwitch");
}

function sendLedBrightness() {
    var sendBrightness = $("#LedBrightness").val();
    $("#LedBrightnessText").text("Change led brightness: " + sendBrightness + "%");
    sendSocket("LedBrightness‡" + sendBrightness);
}

function sendLedMode() {
    sendSocket("LedMode‡" + $("#LedMode").val());
}

function sendSolidLedColor() {
    sendSocket("SolidLedColor‡" + $("#SolidLedColor").val());
}

function sendSocket(SendData) {
    var ServerIp = $("#ServerIp").val();
    var ServerPort = $("#ServerPort").val();
    if (ServerIp == "" || ServerPort == "") { return; }
    var ServerUrl = "//" + ServerIp + ":" + ServerPort + "/?" + encodeURIComponent(SendData);

    var xHttpRequest = new XMLHttpRequest();
    xHttpRequest.open("GET", ServerUrl, true);
    xHttpRequest.timeout = 1000;

    if (!!navigator.userAgent.match(/Version\/[\d\.]+.*Safari/))
    {
        xHttpRequest.setRequestHeader("If-Unmodified-Since", new Date().getTime());
    }

    xHttpRequest.send();
}

function setCookie(key, value) {
    var expires = 1080 * 24 * 60 * 60 * 1000;
    var expires_date = new Date(new Date().getTime() + (expires)).toGMTString();
    document.cookie = key + '=' + value + ';expires=' + expires_date;
}

function getCookie(key) {
    var results = document.cookie.match('(^|;)?' + key + '=([^;]*)(;|$)');
    if (results == null) { return null; } else { return results[2]; }
}

function updateServerIp() {
    setCookie("ServerIp", $("#ServerIp").val());
}

function updateServerPort() {
    setCookie("ServerPort", $("#ServerPort").val());
}

function urlParameter(name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) { return null; } else { return results[1]; }
}

//Run on document ready
$(document).ready(function () {
    if (~window.location.href.indexOf("https://"))
    {
        window.location.replace(window.location.href.replace("https://", "http://"));
    }

    if (urlParameter("port") != null) {
        $("#ServerPort").val(urlParameter("port"));
        updateServerPort();
    }
    else if (getCookie("ServerPort") != null) { $("#ServerPort").val(getCookie("ServerPort")); }

    if (urlParameter("ip") != null) {
        $("#ServerIp").val(urlParameter("ip"));
        updateServerIp();
    }
    else if (getCookie("ServerIp") != null) { $("#ServerIp").val(getCookie("ServerIp")); }
});