Math.cot = function (x) {
    return 1 / Math.tan(x);
}

Math.square = function (x) {
    return x * x;
}

function getBit(value, position) {
    return (value & (1 << position));
}

function clamp(x, a, b) {
    return (x < a) ? a : ((x > b) ? b : x);
}