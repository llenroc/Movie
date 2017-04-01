!(function (window, $, undefined) {
    'use strict';
    var PostStatus = {
        UnPost: 0,
        Posting: 1,
        Posted: 2,
        Failed:3
    };

    function CommentItem(data, comment, parentCommentItem) {
        this.data = data;
        this.comment = comment;
        this.parentCommentItem = parentCommentItem;
        this.initialize();
    };
    CommentItem.prototype.initialize = function () {
        this.createDom(this.data);
    };
    CommentItem.prototype.createDom = function (data) {
        var _this = this;
        this.$li = $("<li>").addClass("comment-header");
        var $commentBox = $("<div>").addClass("comment-box").appendTo(this.$li);
        var $commentLeft = $("<div>").addClass("comment-left").appendTo($commentBox);
        var $commentRight = $("<div>").addClass("comment-right").appendTo($commentBox);

        var $commentItemHeader = $("<div>").addClass("comment-item-header").appendTo($commentRight);
        var $authUserInfo = $("<div>").addClass("auth-user-info").appendTo($commentItemHeader);
        var $img = $("<img>").attr("src", data.user.avatar || this.comment.settings.defaultAvatar);

        if (this.parentCommentItem) {
            $img.appendTo($authUserInfo);
        }
        else {
            $img.appendTo($commentLeft);
        }
        
        var $username = $("<label>").addClass("username").text(data.user.userName).appendTo($authUserInfo);

        var $commentInfo = $("<div>").addClass("comment-info").appendTo($commentItemHeader);
        var $commentDatetime = $("<div>").addClass("comment-datetime").text(data.creationDatetime).appendTo($commentInfo);

        var $commentBody = $("<div>").addClass("comment-body").appendTo($commentRight);
        var $commentContent = $("<p>").html(data.content).appendTo($commentBody);

        this.$replyList = $("<ul>").addClass("reply-list").appendTo(this.$li);

        var $commentFooter = $("<div>").addClass("comment-footer").appendTo(this.$li);
        var $toolbar = $("<div>").addClass("toolbar").appendTo($commentFooter);
        var $replyButton = $("<button>").addClass("button-icon icon-comment-alt small").appendTo($toolbar).on("click", function () {
            _this.post.toggle();
        });

        this.post = new Post(this, this.comment);
        this.post.$postBox.appendTo(this.$li);
    };
    CommentItem.prototype.addItem = function (data) {
        var commentItem = new CommentItem(data, this.comment, this);
        this.$replyList.append(commentItem.$li);
    };

    function Post(commentItem,comment) {
        this.commentItem = commentItem;
        this.comment = comment;
        this.status = PostStatus.UnPost;
        this.initialize();
    }
    Post.prototype.initialize = function () {
        this.createDom();
    }
    Post.prototype.post = function () {
        var _this = this;

        if (!this.comment.isLogin) {
            this.comment.requestLogin();
            return false;
        }
        
        var content = this.$postInput.val();

        if (!content) {
            this.$postInput.focus();
            return false;
        }
        this.setPostingStatus();
        this.comment.settings.poster.call(this, {
            content: content,
            parentId: this.commentItem ? this.commentItem.data.id : null
        }).done(function (result) {

            if (_this.commentItem) {
                _this.commentItem.addItem(result);
                _this.hide();
            }
            else {
                _this.comment.createCommentItem(result);
            }
        });
    }
    Post.prototype.setPostingStatus = function () {
        this.status = PostStatus.Posting;
        this.$postButton.addClass("loading");
    };
    Post.prototype.setPostedStatus = function () {
        this.status = PostStatus.Posted;
        this.$postButton.removeClass("loading");
    };
    Post.prototype.createDom = function () {
        var _this = this;
        this.$postBox = $("<div>").addClass("post-box").hide();
        var $postInputWrapper = $("<div>").addClass("post-input-wrapper").appendTo(this.$postBox);
        this.$postInput = $("<textarea>").addClass("post-input").appendTo($postInputWrapper);
        var $postFooter = $("<div>").addClass("post-footer").appendTo(this.$postBox);

        this.$postButton = $("<button>").addClass("post-button").appendTo($postFooter).on("click", function () {
            _this.post();
        });
        return this.$postBox;
    };
    Post.prototype.show = function () {
        this.$postBox.show();
        return this;
    }
    Post.prototype.hide = function () {
        this.$postBox.hide();
        return this;
    };
    Post.prototype.toggle = function () {
        this.$postBox.toggle();
        return this;
    }

    function Comment($element,options) {
        this.settings = $.extend({}, this.defaultSettings, options);
        this.$element = $element;
        this.element = $element[0];

        this.isLogin = false;
        this.user = null;
        this.pageIndex = 0;
        this.pageCount = 0;

        this.initialize();
    };
    Comment.prototype.defaultSettings = {
        pageSize:10,
        defaultAvatar: "../../image/avatar.png",
        loader: null,
        poster:null
    };
    Comment.prototype.initialize = function () {
        this.createHeader();
        this.createCommentListBlock();
        this.loadComments();
    };
    Comment.prototype.setLogin = function (user) {
        this.isLogin = true;
        this.user = user;

        this.$avatar = $("<img>").addClass("avatar").appendTo(this.$userInfo);

        if (this.user.avatar) {
            this.$avatar.attr("src", this.user.avatar);
        }
        else {
            this.$avatar.attr("src", this.settings.defaultAvatar);
        }
    };
    Comment.prototype.setUnlogin = function () {
        this.isLogin = false;
        this.user = null;
        this.$login = $("<div>").addClass("login").text("Login").appendTo(this.$userInfo);
    }
    Comment.prototype.requestLogin=function(){

    }
    Comment.prototype.loadComments = function (commentItem) {
        var _this = this;

        if (!this.settings.loader) {
            return false;
        }
        var requestParameters = {
            pageIndex: this.pageIndex,
            pageSize:this.settings.pageSize
        };
        this.settings.loader.call(this, requestParameters).done(function (result) {
            _this.pageIndex = result.pageIndex;
            _this.pageCount = result.pageCount;

            result.items.forEach(function (item) {

                if (commentItem) {
                    commentItem.addItem(item);
                }
                else {
                    _this.createCommentItem(item);
                }
            });
        });
    };
    Comment.prototype.createHeader = function () {
        var $header = $("<div>").addClass("comment-header").appendTo(this.$element);
        this.$userInfo = $("<div>").addClass("user-info").appendTo($header);
        var post = new Post(null, this);
        post.show().$postBox.appendTo($header);
    };
    Comment.prototype.createCommentListBlock = function () {
        this.$commentListBlock = $("<div>").addClass("comment-list-block").appendTo(this.$element);
        this.$commentList = $("<ul>").addClass("comment-list").appendTo(this.$commentListBlock);
    };
    Comment.prototype.createPost = function () {
       
    };
    Comment.prototype.createCommentItem = function (data) {
        var commentItem = new CommentItem(data, this, null);
        commentItem.$li.appendTo(this.$commentList);
    }

    $.fn.comment = function (options) {
        this.eq(0).data("comment") || this.eq(0).data("comment", new Comment(this.eq(0), options));
        return this.eq(0).data("comment");
    };
})(window, jQuery);