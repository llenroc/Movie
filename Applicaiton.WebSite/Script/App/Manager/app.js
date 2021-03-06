﻿(function () {
    'use strict';

    //Configuration for Angular UI routing.
    appModule.config(['$stateProvider', '$urlRouterProvider', configRouter]);

    function configRouter($stateProvider, $urlRouterProvider) {

        // Default route (overrided below if user has permission)
        $urlRouterProvider.otherwise("/welcome");

        //Welcome page
        $stateProvider.state('welcome', {
            url: '/welcome',
            templateUrl: '/Areas/Manager/Views/Home/index.cshtml'
        });

        // Shared routes
        $stateProvider.state('loginAttempt', {
            url: '/loginAttempt',
            templateUrl: '/Areas/Manager/Views/Shared/LoginAttempt.cshtml',
            menu: 'Common.User.LoginAttempt'
        });

        //COMMON routes
        $stateProvider.state('changePassword', {
            url: '/changePassword',
            templateUrl: '/Areas/Manager/Views/Common/Profile/changePassword.cshtml'
        });
        $stateProvider.state('user', {
            'abstract': true,
            url: '/user',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('user.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Common/User/index.cshtml',
            menu: 'Common.User'
        }).state('user.createOrEdit', {
            url: '/createOrEdit/{id}',
            templateUrl: '/Areas/Manager/Views/Common/User/createOrEdit.cshtml',
            menu: 'Common.User'
        });

        $stateProvider.state('role', {
            'abstract': true,
            url: '/role',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('role.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Common/Role/index.cshtml',
            menu: 'Manager.Role'
        }).state('role.createOrEdit', {
            url: '/createOrEdit/{id}',
            templateUrl: '/Areas/Manager/Views/Common/Role/createOrEdit.cshtml',
            menu: 'Common.Role'
        });

        $stateProvider.state('notifications', {
            url: '/notifications',
            templateUrl: '/Areas/Manager/Views/Common/Notifications/index.cshtml'
        });

        //HOST routes
        if (infrastructure.auth.hasPermission('Pages.Administration.Host')) {
            $urlRouterProvider.otherwise("/host/dashboard"); //Entrance page for a tenant
            $stateProvider.state('host.dashboard', {
                url: '/dashboard',
                templateUrl: '/Areas/Manager/Views/Host/Dashboard/Index.cshtml'
            });

            $stateProvider.state('host', {
                'abstract': true,
                url: '/host',
                template: '<div ui-view class="fade-in-up"></div>'
            });
            $stateProvider.state('host.setting', {
                url: '/setting',
                templateUrl: '/Areas/Manager/Views/Host/Setting/index.cshtml',
                menu: 'Host.Setting'
            });

            $stateProvider.state('host.tenant', {
                'abstract': true,
                url: '/tenant',
                template: '<div ui-view class="fade-in-up"></div>'
            });
            $stateProvider.state('host.tenant.index', {
                url: '/index',
                templateUrl: '/Areas/Manager/Views/Host/Tenant/index.cshtml',
                menu: 'Host.Tenant'
            }).state('host.tenant.create', {
                url: '/create',
                templateUrl: '/Areas/Manager/Views/Host/Tenant/create.cshtml',
                menu: 'Host.Tenant'
            }).state('host.tenant.edit', {
                url: '/edit/{id}',
                templateUrl: '/Areas/Manager/Views/Host/Tenant/edit.cshtml',
                menu: 'Host.Tenant'
            });
            $stateProvider.state('host.edition', {
                url: '/edition',
                templateUrl: '/Areas/Manager/Views/Host/Edition/index.cshtml',
                menu: 'Host.Edition'
            });
            $stateProvider.state('host.maintenance', {
                url: '/maintenance',
                templateUrl: '/Areas/Manager/Views/Host/Maintenance/index.cshtml',
                menu: 'Host.Maintenance'
            });

            $stateProvider.state('host.language', {
                url: '/language',
                templateUrl: '/Areas/Manager/Views/Host/Language/index.cshtml',
                menu: 'Manager.Language'
            }).state('languageTexts', {
                url: '/languages/texts/:languageName?sourceName&baseLanguageName&targetValueFilter&filterText',
                templateUrl: '/Areas/Manager/Views/Host/Language/texts.cshtml',
                menu: 'Host.Language'
            });

            $stateProvider.state('host.auditLog', {
                url: '/auditLog',
                templateUrl: '/Areas/Manager/Views/Host/AuditLog/index.cshtml',
                menu: 'Host.AuditLog'
            });

            $stateProvider.state('host.backgroundJob', {
                url: '/backgroundJob',
                templateUrl: '/Areas/Manager/Views/Host/BackgroundJob/index.cshtml',
                menu: 'Host.BackgroundJob'
            });
        }

        //tenant
        $stateProvider.state('tenant', {
            'abstract': true,
            url: '/tenant',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (infrastructure.auth.hasPermission('Pages.Administration.Tenant')) {
            $urlRouterProvider.otherwise("/tenant/dashboard"); //Entrance page for a tenant
            $stateProvider.state('tenant.dashboard', {
                url: '/dashboard',
                templateUrl: '/Areas/Manager/Views/Tenant/Dashboard/Index.cshtml'
            });
        }
    
        $stateProvider.state('tenant.setting', {
            url: '/setting',
            templateUrl: '/Areas/Manager/Views/Tenant/Setting/index.cshtml',
            menu: 'Tenant.Setting'
        });

        $stateProvider.state('publicWechat', {
            'abstract': true,
            url: '/publicWechat',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('publicWechat.setting', {
            url: '/setting',
            templateUrl: '/Areas/Manager/Views/Tenant/PublicWechat/Setting/index.cshtml',
            menu: 'Tenant.PublicWechat.Setting'
        });
        $stateProvider.state('publicWechat.menu', {
            url: '/menu',
            templateUrl: '/Areas/Manager/Views/Tenant/PublicWechat/Menu/index.cshtml',
            menu: 'Tenant.PublicWechat.Menu'
        });
        $stateProvider.state('publicWechat.autoReply', {
            'abstract': true,
            url: '/autoReply',
            template: '<div ui-view class="fade-in-up"></div>'
        }).state('publicWechat.autoReply.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/PublicWechat/AutoReply/index.cshtml',
            menu: 'Tenant.PublicWechat.AutoReply'
        }).state('publicWechat.autoReply.createOrEdit', {
            url: '/createOrEdit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/PublicWechat/AutoReply/createOrEdit.cshtml',
            menu: 'Tenant.PublicWechat.AutoReply'
        });

        $stateProvider.state('spreadPosterTemplate', {
            'abstract': true,
            url: '/SpreadPoster',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('spreadPosterTemplate.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/SpreadPosterTemplate/index.cshtml',
            menu: 'Tenant.SpreadPoster'
        }).state('spreadPosterTemplate.createOrEdit', {
            url: '/createOrEdit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/SpreadPosterTemplate/createOrEdit.cshtml',
            menu: 'Tenant.SpreadPoster'
        });

        $stateProvider.state('memberLevel', {
            'abstract': true,
            url: '/memberLevel',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('memberLevel.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/MemberLevel/index.cshtml',
            menu: 'Tenant.MemberLevel'
        }).state('memberLevel.create', {
            url: '/create',
            templateUrl: '/Areas/Manager/Views/Tenant/MemberLevel/create.cshtml',
            menu: 'Tenant.MemberLevel'
        }).state('memberLevel.edit', {
            url: '/edit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/MemberLevel/edit.cshtml',
            menu: 'Tenant.MemberLevel'
        });

        $stateProvider.state('memberCardPackage', {
            'abstract': true,
            url: '/memberCardPackage',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('memberCardPackage.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/MemberCardPackage/index.cshtml',
            menu: 'Tenant.MemberCardPackage'
        });
        $stateProvider.state('memberCardPackage.create', {
            url: '/create',
            templateUrl: '/Areas/Manager/Views/Tenant/MemberCardPackage/create.cshtml',
            menu: 'Tenant.MemberCardPackage'
        }).state('memberCardPackage.edit', {
            url: '/edi/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/MemberCardPackage/edit.cshtml',
            menu: 'Tenant.MemberCardPackage'
        });

        $stateProvider.state('memberCard', {
            url: '/memberCard',
            templateUrl: '/Areas/Manager/Views/Tenant/MemberCard/index.cshtml',
            menu: 'Tenant.MemberCard'
        });

        $stateProvider.state('productCategory', {
            url: '/productCategory',
            templateUrl: '/Areas/Manager/Views/Tenant/ProductCategory/index.cshtml',
            menu: 'Tenant.Product'
        });

        $stateProvider.state('specificationProperty', {
            'abstract': true,
            url: '/specificationProperty',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('specificationProperty.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/SpecificationProperty/index.cshtml',
            menu: 'Tenant.Product'
        }).state('specificationProperty.createOrEdit', {
            url: '/createOrEdit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/SpecificationProperty/createOrEdit.cshtml',
            menu: 'Tenant.Product'
        });

        $stateProvider.state('product', {
            'abstract': true,
            url: '/product',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('product.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/Product/index.cshtml',
            menu: 'Tenant.Product'
        }).state('product.createOrEdit', {
            url: '/createOrEdit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/Product/createOrEdit.cshtml',
            menu: 'Tenant.Product'
        });

        $stateProvider.state('virtualProduct', {
            'abstract': true,
            url: '/virtualProduct',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('virtualProduct.virtualCard', {
            'abstract': true,
            url: '/virtualCard',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('virtualProduct.virtualCard.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/VirtualProduct/VirtualCard/Index.cshtml',
            menu: 'Tenant.VirtualProduct'
        }).state('virtualProduct.virtualCard.import', {
            url: '/import',
            templateUrl: '/Areas/Manager/Views/Tenant/VirtualProduct/VirtualCard/Import.cshtml',
            menu: 'Tenant.VirtualProduct'
        }).state('virtualProduct.virtualCard.list', {
            url: '/list/{version}',
            templateUrl: '/Areas/Manager/Views/Tenant/VirtualProduct/VirtualCard/List.cshtml',
            menu: 'Tenant.VirtualProduct'
        });

        $stateProvider.state('expressCompany', {
            'abstract': true,
            url: '/expressCompany',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('expressCompany.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/ExpressCompany/Index.cshtml',
            menu: 'Tenant.ExpressCompany'
        }).state('expressCompany.create', {
            url: '/create',
            templateUrl: '/Areas/Manager/Views/Tenant/ExpressCompany/Create.cshtml',
            menu: 'Tenant.ExpressCompany'
        }).state('expressCompany.edit', {
            url: '/edit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/ExpressCompany/Edit.cshtml',
            menu: 'Tenant.ExpressCompany'
        });

        $stateProvider.state('article', {
            'abstract': true,
            url: '/article',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('article.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/Article/Index.cshtml',
            menu: 'Tenant.Article'
        }).state('article.create', {
            url: '/create',
            templateUrl: '/Areas/Manager/Views/Tenant/Article/Create.cshtml',
            menu: 'Tenant.Article'
        }).state('article.edit', {
            url: '/edit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/Article/Edit.cshtml',
            menu: 'Tenant.Article'
        });

        $stateProvider.state('walletRecord', {
            'abstract': true,
            url: '/walletRecord',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('walletRecord.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/WalletRecord/Index.cshtml',
            menu: 'Tenant.WalletRecord'
        });

        $stateProvider.state('channel', {
            'abstract': true,
            url: '/channel',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        $stateProvider.state('channel.channelAgent', {
            'abstract': true,
            url: '/channelAgent',
            template: '<div ui-view class="fade-in-up"></div>'
        }).state('channel.channelAgent.index', {
            url: '/channelAgent',
            templateUrl: '/Areas/Manager/Views/Tenant/Channel/ChannelAgent/Index.cshtml',
            menu: 'Tenant.Channel'
        }).state('channel.channelAgent.createOrEdit', {
            url: '/createOrEdit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/Channel/ChannelAgent/CreateOrEdit.cshtml',
            menu: 'Tenant.Channel'
        });

        $stateProvider.state('channel.channelAgency', {
            'abstract': true,
            url: '/channelAgency',
            template: '<div ui-view class="fade-in-up"></div>'
        }).state('channel.channelAgency.index', {
            url: '/channelAgency',
            templateUrl: '/Areas/Manager/Views/Tenant/Channel/ChannelAgency/Index.cshtml',
            menu: 'Tenant.Channel'
        }).state('channel.channelAgency.edit', {
            url: '/edit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/Channel/ChannelAgency/Edit.cshtml',
            menu: 'Tenant.Channel'
        });

        $stateProvider.state('channel.channelAgencyApply', {
            'abstract': true,
            url: '/channelAgencyApply',
            template: '<div ui-view class="fade-in-up"></div>'
        }).state('channel.channelAgencyApply.index', {
            url: '/channelAgencyApply',
            templateUrl: '/Areas/Manager/Views/Tenant/Channel/ChannelAgencyApply/Index.cshtml',
            menu: 'Tenant.Channel'
        }).state('channel.channelAgencyApply.edit', {
            url: '/edit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/Channel/channelAgencyApply/Edit.cshtml',
            menu: 'Tenant.Channel'
        });

        $stateProvider.state('order', {
            'abstract': true,
            url: '/order',
            template: '<div ui-view class="fade-in-up"></div>'
        }).state('order.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/Order/index.cshtml',
            menu: 'Tenant.Order'
        }).state('order.detail', {
            url: '/detail/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/Order/Detail.cshtml',
            menu: 'Tenant.Order'
        });

        $stateProvider.state('globalRebate', {
            'abstract': true,
            url: '/globalRebate',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('globalRebate.globalRebate', {
            'abstract': true,
            url: '/globalRebate',
            template: '<div ui-view class="fade-in-up"></div>'
        }).state('globalRebate.globalRebate.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/GlobalRebate/index.cshtml',
            menu: 'Tenant.GlobalRebate'
        });

        $stateProvider.state('movie', {
            'abstract': true,
            url: '/movie',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('movie.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/Movie/Index.cshtml',
            menu: 'Manager.Movie'
        }).state('movie.create', {
            url: '/create',
            templateUrl: '/Areas/Manager/Views/Tenant/Movie/Create.cshtml',
            menu: 'Manager.Movie'
        }).state('movie.edit', {
            url: '/edit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/Movie/Edit.cshtml',
            menu: 'Manager.Movie'
        });

        $stateProvider.state('movieCategory', {
            'abstract': true,
            url: '/movieCategory',
            template: '<div ui-view class="fade-in-up"></div>'
        });
        $stateProvider.state('movieCategory.index', {
            url: '/index',
            templateUrl: '/Areas/Manager/Views/Tenant/MovieCategory/Index.cshtml',
            menu: 'Manager.MovieCategory'
        }).state('movieCategory.create', {
            url: '/create',
            templateUrl: '/Areas/Manager/Views/Tenant/MovieCategory/Create.cshtml',
            menu: 'Manager.MovieCategory'
        }).state('movieCategory.edit', {
            url: '/edit/{id}',
            templateUrl: '/Areas/Manager/Views/Tenant/MovieCategory/Edit.cshtml',
            menu: 'Manager.edit'
        });
    };
})();