/**
 * Created by canknow on 2015/8/7.
 */

function parsePath(path){
    var pathItems=path.split("/");
    return pathItems;
}


//判断:当前元素是否是被筛选元素的子元素 
$.fn.isChildOf = function (b) {
    return (this.parents(b).length > 0);
};
//判断:当前元素是否是被筛选元素的子元素或者本身 
$.fn.isChildAndSelfOf = function (b) {
    return (this.closest(b).length > 0);
};

$.fn.emulateTransitionEnd = function (duration) {
    var called = false, $el = this;

    $(this).one('webkitTransitionEnd', function () {
        called = true;
    });
    var callback = function () {

        if (!called)
            $($el).trigger('webkitTransitionEnd');
    };
    setTimeout(callback, duration);
};

jQuery.fn.getReliablePosition = function () {
    thisLeft = this.offset().left;
    thisTop = this.offset().top;
    thisParent = this.parent();

    parentLeft = thisParent.offset().left;
    parentTop = thisParent.offset().top;

    return {
        left: thisLeft - parentLeft,
        top: thisTop - parentTop
    };
};