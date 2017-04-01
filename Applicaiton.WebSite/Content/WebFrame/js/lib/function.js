function isObject(obj) {
    return (typeof obj == 'object') && obj.constructor == Object;
}

function isArray(obj) {
    return (typeof obj == 'object') && obj.constructor == Array;
}

function isString(str) {
    return (typeof str == 'string') && str.constructor == String;
}

function isNumber(obj) {
    return (typeof obj == 'number') && obj.constructor == Number;
}

function isDate(obj) {
    return (typeof obj == 'object') && obj.constructor == Date;
}

function isFunction(obj) {
    return (typeof obj == 'function') && obj.constructor == Function;
}

function extend(destination, source) {
    for (var property in source) {
        destination[property] = source[property];
    }
    return destination;
}

function extendValue(destination, source) {
    for (var property in source) {
        if (typeof source[property] == 'function') continue;
        destination[property] = source[property];
    }
    return destination;
}

function decimalToColorArray(decNum) {
    var colorArray = [];
    colorArray[0] = (Math.floor(decNum / 65536)) / 256;
    colorArray[1] = (Math.floor(decNum % 65536 / 256)) / 256;
    colorArray[2] = (decNum % 256) / 256;
    colorArray[3] = 1;
    return colorArray;
}