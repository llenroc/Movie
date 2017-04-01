using Application.Articles;
using Application.Authorization.Roles;
using Application.Authorization.Users;
using Application.Channel.ChannelAgencies;
using Application.Channel.ChannelAgents;
using Application.Customers;
using Application.Distributions;
using Application.Expresses;
using Application.Files;
using Application.GlobalRebates;
using Application.Members;
using Application.MovieCategorys;
using Application.Movies;
using Application.MultiTenancy;
using Application.Orders.Entities;
using Application.ProductCategorys;
using Application.Products;
using Application.Regions;
using Application.ShopCarts;
using Application.Spread.SpreadPosters.SpreadPosterTemplates;
using Application.VirtualProducts;
using Application.Wallets.Entities;
using Application.Wechats.AutoReplys;
using Application.Wechats.Qrcodes;
using Application.Wechats.Shares;
using Infrastructure.CommonFrame.EntityFramework;
using System.Data.Common;
using System.Data.Entity;

namespace Application.EntityFramework
{
    public class ApplicationDbContext: CommonFrameDbContext<Tenant, Role, User>
    {
        public virtual IDbSet<Article> Articles { get; set; }

        public virtual IDbSet<ArticleLike> ArticleLikes { get; set; }

        public virtual IDbSet<ArticleHint> ArticleHints { get; set; }

        public virtual IDbSet<VirtualCard> VirtualCards { get; set; }

        public virtual IDbSet<OrderItemVirtualCard> OrderItemVirtualCards { get; set; }

        public virtual IDbSet<InfrastructureFileInfo> InfrastructureFileInfos { get; set; }

        public virtual IDbSet<Movie> Movies { get; set; }

        public virtual IDbSet<MovieHint> MovieHints { get; set; }

        public virtual IDbSet<MovieCategory> MovieCategorys { get; set; }

        public virtual IDbSet<Qrcode> Qrcodes { get; set; }

        public virtual IDbSet<SpreadPosterTemplate> SpreadPosterTemplates { get; set; }

        public virtual IDbSet<SpreadPosterTemplateParameter> SpreadPosterTemplateParameters { get; set; }

        public virtual IDbSet<AutoReply> AutoReplys { get; set; }

        public virtual IDbSet<AutoReplyArticle> AutoReplyArticles { get; set; }

        public virtual IDbSet<Share> Shares { get; set; }

        public virtual IDbSet<ShareAccess> ShareAccesses { get; set; }

        public virtual IDbSet<MemberLevel> MemberLevels { get; set; }

        public virtual IDbSet<MemberCard> MemberCards { get; set; }

        public virtual IDbSet<ProductCategory> ProductCategorys { get; set; }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<Distribution> Distributions { get; set; }

        public virtual IDbSet<OrderDistribution> OrderDistributions { get; set; }
        
        public virtual IDbSet<ChannelAgentRebate> ChannelAgentRebates { get; set; }

        public virtual IDbSet<SpecificationProperty> SpecificationPropertys { get; set; }

        public virtual IDbSet<SpecificationPropertyValue> SpecificationPropertyValues { get; set; }

        public virtual IDbSet<Specification> Specifications { get; set; }

        public virtual IDbSet<Address> Addresss { get; set; }

        public virtual IDbSet<CustomerInfo> CustomerInfos { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public virtual IDbSet<ProductOrder> ProductOrders { get; set; }

        public virtual IDbSet<OrderItem> OrderItems { get; set; }

        public virtual IDbSet<OrderCustomerInfo> OrderCustomerInfos { get; set; }

        public virtual IDbSet<ShopCart> ShopCarts { get; set; }

        public virtual IDbSet<ExpressCompany> ExpressCompanys { get; set; }

        public virtual IDbSet<ShopCartItem> ShopCartItems { get; set; }

        public virtual IDbSet<MemberCardPackage> MemberCardPackages { get; set; }

        public virtual IDbSet<MemberCardPackageOrder> MemberCardPackageOrders { get; set; }

        public virtual IDbSet<Wallet> Wallets { get; set; }

        public virtual IDbSet<WalletRecord> WalletRecords { get; set; }

        public virtual IDbSet<ChannelAgent> ChannelAgents { get; set; }

        public virtual IDbSet<ChannelAgency> ChannelAgencies { get; set; }

        public virtual IDbSet<ChannelAgencyApply> ChannelAgencyApplys { get; set; }

        public virtual IDbSet<ChannelAgencyApplyOrder> ChannelAgencyApplyOrders { get; set; }

        public virtual IDbSet<GlobalRebate> GlobalRebates { get; set; }

        public virtual IDbSet<OrderGlobalRebate> OrderGlobalRebates { get; set; }


        static ApplicationDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Application.Migrations.Configuration>());
            //Database.SetInitializer(new DatabaseInitializerForCreateDatabaseIfNotExists());
        }

        public ApplicationDbContext(): base("Default")
        {

        }

        public ApplicationDbContext(string nameOrConnectionString): base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public ApplicationDbContext(DbConnection connection) : base(connection, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().HasOptional(order => order.OrderCustomerInfo).WithRequired(OrderCustomerInfo => OrderCustomerInfo.Order);
        }

    }
}
