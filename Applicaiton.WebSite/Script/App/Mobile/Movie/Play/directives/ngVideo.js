/* Used by user and role permission settings. 
 */
appModule.directive('ngVideo', ['infrastructure.services.app.movie', '$sce',
      function (movieService, $sce) {
          return {
              scope: {
                  movieid: '='
              },
              link: function ($scope, element, attr) {
                  function loadPlugin(options) {
                      var _this = this;
                      var player = this;
                      var button = document.createElement('div');
                      button.className = 'vjs-big-play-button';
                      player.el_.appendChild(button);
                      button.onclick = function () {
                          movieService.getMoviePlayPathForUser({ id: $scope.movieid }).success(function (result) {
                              var path = result;
                              player.src(path);
                              player.load();
                          });
                      };

                      player.on('loadstart', function () {
                          player.el_.classList.add("vjs-waiting");
                          $(button).remove();
                      });

                      player.on('loadeddata', function () {
                          player.el_.classList.remove("vjs-waiting");
                          player.play();
                      });
                  };
                  videojs.plugin('loadPlugin', loadPlugin);

                  var player = videojs(element.get(0), {
                      fluid: true,
                      techOrder: ["html5", "flash"],
                      bigPlayButton:false
                  }, function () {

                  }).ready(function () {
                      var player = this;
                      player.loadPlugin();
                  });
                  // 检测播放时间
                  player.on('timeupdate', function () {

                  });
                  // 开始或恢复播放
                  player.on('play', function () {
                      element.turnOffLight();
                  });
                  // 暂停播放
                  player.on('pause', function () {
                      element.turnOnLight();
                  });
              }
          };
      }]);