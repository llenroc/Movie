using Application.Authorization;
using Application.Authorization.Users;
using Application.Orders.Entities;
using Application.Tenants.Dashboard.Dto;
using Infrastructure.Authorization;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Tenants.Dashboard
{
    [InfrastructureAuthorize(AppPermissions.PagesAdministrationTenant)]
    public class TenantDashboardAppService : ApplicationAppServiceBase, ITenantDashboardAppService
    {
        public IRepository<User, long> UserRepository { get; set; }
        public IRepository<Order> OrderRepository { get; set; }
        public DashboardOutput GetDashboardActivity()
        {
            DashboardOutput dashboardOutput = new DashboardOutput();
            DateTime startDateTime = DateTime.Now.AddDays(-30);
            dashboardOutput.UserActivity = new UserActivityOutput()
            {
                NewUsers = new List<CommonDateCount>()
            };
            var userGroup =
                from user in UserRepository.GetAll().Where(model=>model.CreationTime>= startDateTime)
                group user by new { user.CreationTime.Year, user.CreationTime.Month, user.CreationTime.Day } into userGroupItem
                select new
                {
                    Date =  userGroupItem.Key.Year + "/" + userGroupItem.Key.Month + "/" + userGroupItem.Key.Day ,
                    Count = userGroupItem.Count()
                };
            foreach(var groupItem in userGroup)
            {
                dashboardOutput.UserActivity.NewUsers.Add(new CommonDateCount()
                {
                    Date= groupItem.Date,
                    Count=groupItem.Count
                });
            }

            dashboardOutput.OrderActivity = new OrderActivityOutput()
            {
                NewPayedOrders = new List<CommonDateCount>()
            };
            var orderGroup =
                from order in OrderRepository.GetAll().Where(model => model.CreationTime >= startDateTime&&model.PaymentStatus==PaymentStatus.Payed)
                group order by new { order.CreationTime.Year, order.CreationTime.Month, order.CreationTime.Day } into orderGroupItem
                select new
                {
                    Date = orderGroupItem.Key.Year + "/" + orderGroupItem.Key.Month + "/" + orderGroupItem.Key.Day,
                    Count = orderGroupItem.Count()
                };
            foreach (var groupItem in orderGroup)
            {
                dashboardOutput.OrderActivity.NewPayedOrders.Add(new CommonDateCount()
                {
                    Date = groupItem.Date,
                    Count = groupItem.Count
                });
            }
            return dashboardOutput;
        }
    }
}