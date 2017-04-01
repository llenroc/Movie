(function () {
    appModule.controller('app.manager.tenant.dashboard.index', [
        '$scope', 'infrastructure.services.app.tenantDashboard', 'infrastructure.services.app.systemMonitor',
        function ($scope, tenantDashboardService, systemMonitorService) {
            var vm = this;
            $scope.$on('$viewContentLoaded', function () {

            });

            //Get the common hub
            var systemHub = infrastructure.signalr.hubs.system;

            //Register to get notifications
            infrastructure.event.on('infrastructure.hubs.system.getMonitorInfo', function (result) {
                var chart = $('#cpuMonitor').highcharts(), point, newVal, inc;

                if (chart) {
                    point = chart.series[0].points[0];
                    point.update(result.cpuInfo);
                }
                chart = $('#memMonitor').highcharts();

                if (chart) {
                    point = chart.series[0].points[0];
                    point.update(result.memInfo);
                }
                chart = $('#tcpMonitor').highcharts();

                if (chart) {
                    point = chart.series[0].points[0];
                    point.update(result.tcpCount);
                }
                chart = $('#onlineMonitor').highcharts();

                if (chart) {
                    point = chart.series[0].points[0];
                    point.update(result.onlineCount);
                }
            });

            //Connect to the server
            infrastructure.signalr.connect = function () {
                $.connection.hub.start().done(function () {
                    infrastructure.event.trigger('infrastructure.signalr.connected');
                    commonHub.server.register().done(function () {
                        infrastructure.log.debug('Registered to the SignalR server!'); //TODO: Remove log
                    });
                });
            };
            var gaugeOptions = {
                chart: {
                    type: 'solidgauge'
                },
                title: null,
                pane: {
                    center: ['50%', '85%'],
                    size: '140%',
                    startAngle: -90,
                    endAngle: 90,
                    background: {
                        backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || '#EEE',
                        innerRadius: '60%',
                        outerRadius: '100%',
                        shape: 'arc'
                    }
                },
                tooltip: {
                    enabled: false
                },
                // the value axis
                yAxis: {
                    stops: [
                        [0.1, '#55BF3B'], // green
                        [0.5, '#DDDF0D'], // yellow
                        [0.9, '#DF5353'] // red
                    ],
                    lineWidth: 0,
                    minorTickInterval: null,
                    tickPixelInterval: 400,
                    tickWidth: 0,
                    title: {
                        y: -70
                    },
                    labels: {
                        y: 16
                    }
                },
                plotOptions: {
                    solidgauge: {
                        dataLabels: {
                            y: 5,
                            borderWidth: 0,
                            useHTML: true
                        }
                    }
                }
            };
            vm.getDashboardActivity = function () {
                tenantDashboardService.getDashboardActivity({}).success(function (result) {
                    var userCategories = [];
                    var data = [];
                    result.userActivity.newUsers.forEach(function (newUser) {
                        userCategories.push(newUser.date);
                        data.push(newUser.count);
                    });

                    $('#usersChart').highcharts({
                        chart: {
                            type: 'line'                         //指定图表的类型，默认是折线图（line）
                        },
                        title: {
                            text: 'Users'      //指定图表标题
                        },
                        xAxis: {
                            categories: userCategories   //指定x轴分组
                        },
                        yAxis: {
                            title: {
                                text: 'Count'                  //指定y轴的标题
                            }
                        },
                        series: [{                                 //指定数据列
                            name: 'NewUsers',                          //数据列名
                            data: data                      //数据
                        }]
                    });

                    var orderCategories = [];
                    var data = [];
                    result.orderActivity.newPayedOrders.forEach(function (newOrder) {
                        orderCategories.push(newOrder.date);
                        data.push(newOrder.count);
                    });

                    $('#orderChart').highcharts({
                        chart: {
                            type: 'line'                         //指定图表的类型，默认是折线图（line）
                        },
                        title: {
                            text: 'Orders'      //指定图表标题
                        },
                        xAxis: {
                            categories: orderCategories   //指定x轴分组
                        },
                        yAxis: {
                            title: {
                                text: 'Count'                  //指定y轴的标题
                            }
                        },
                        series: [{                                 //指定数据列
                            name: 'NewOrders',                          //数据列名
                            data: data                      //数据
                        }]
                    });
                });
            };
            vm.initalizeMonitor = function () {
                systemMonitorService.getSystemInfo().success(function (result) {
                    $('#cpuMonitor').highcharts(Highcharts.merge(gaugeOptions, {
                        yAxis: {
                            min: 0,
                            max: 100,
                            title: {
                                text: 'CPU'
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'CPU',
                            data: [result.cpuInfo],
                        }]
                    }));
                    $('#memMonitor').highcharts(Highcharts.merge(gaugeOptions, {
                        yAxis: {
                            min: 0,
                            max: 100,
                            title: {
                                text: 'Memory'
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'Memory',
                            data: [result.memInfo],
                        }]
                    }));
                    $('#tcpMonitor').highcharts(Highcharts.merge(gaugeOptions, {
                        yAxis: {
                            min: 0,
                            max: 1000,
                            title: {
                                text: 'TCP'
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'TCP',
                            data: [result.tcpCount],
                        }]
                    }));
                    $('#onlineMonitor').highcharts(Highcharts.merge(gaugeOptions, {
                        yAxis: {
                            min: 0,
                            max: 1000,
                            title: {
                                text: 'Online'
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'Online',
                            data: [result.onlineCount],
                        }]
                    }));
                    setInterval(function () {
                        systemHub.server.getMonitorInfo();
                    }, 1000);
                }).finally(function () {
                });
            };
            function intialize() {
                vm.getDashboardActivity();
                vm.initalizeMonitor();
            }
            intialize();
        }
    ]);
})();