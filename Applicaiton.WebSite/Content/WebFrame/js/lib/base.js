function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}

function GetUrlRelativePath() {
    var url = document.location.toString();
    var arrUrl = url.split("//");

    var start = arrUrl[1].indexOf("/");
    var relUrl = arrUrl[1].substring(start);//stopʡ�ԣ���ȡ��start��ʼ����β�������ַ�

    if (relUrl.indexOf("?") != -1) {
        relUrl = relUrl.split("?")[0];
    }
    return relUrl;
}

function normalizeNumber(number, min, max, defaultValue) {

    if (number == undefined || number == null || isNaN(number)) {
        return defaultValue;
    }

    if (number < min) {
        return min;
    }

    if (number > max) {
        return max;
    }
    return number;
}