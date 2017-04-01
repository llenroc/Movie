function Head(level, title, element, $content) {
    this.level = level;
    this.title = title;
    this.element = element;
    this.$content = $content;
    this.children = [];
    this.openStatus = true;
    this.toggleStatus = function () {
        this.openStatus = !this.openStatus;
    }
}
Head.prototype.scrollToView = function () {
    this.element.scrollIntoView(true);
}

function Article($element) {
    this.$element = $element;
    this.startLevel = 1;
}
Article.prototype.getOutline = function () {
    var _this = this;
    var tree = [];
    var $startHs = this.$element.find("h" + this.startLevel);
 
    if ($startHs.length) {
        this.$element.find("h" + this.startLevel).each(function (index, element) {
            var head = new Head(_this.startLevel, this.innerText, element, _this.$element);
            tree.push(head);
            _this.getOutlineCircle(head, $(this));
        });
        return tree;
    }
    else if(this.startLevel<6){
        this.startLevel++;
        return this.getOutline();
    }
}
Article.prototype.getOutlineCircle = function (parent, $parentHead) {
    var _this = this;
    var tagName = $parentHead.get(0).tagName;
    var parentLevel = tagName.substr(1, 1);
    var nextLevel = parentLevel;

    $parentHead.nextAll(":header").each(function (index, element) {
        var level = this.tagName.substr(1, 1);

        if (level <= parentLevel) {
            return false;
        }
        else if (level > nextLevel && nextLevel > parentLevel) {
            return true;
        }
        else {
            nextLevel = level;
            var head = new Head(level, this.innerText, element, _this.$element);
            parent.children.push(head);
            _this.getOutlineCircle(head, $(this));
        }
    });
}