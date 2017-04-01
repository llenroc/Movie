using Application.Authorization.Users;
using Application.Channel.ChannelAgencies;
using Application.Channel.ChannelAgents;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.ChananlAgencys
{
    public class ChannelAgencyManager : ApplicationDomainServiceBase
    {
        public IRepository<User,long> UserRepository { get; set; }

        public IRepository<ChannelAgency> ChannelAgencyRepository { get; set; }

        public IRepository<ChannelAgencyApply> ChannelAgencyApplyRepository { get; set; }

        public ChannelAgency GetChannelAgencyOfUser(long userId)
        {
            return ChannelAgencyRepository.GetAll().Where(model => model.UserId == userId).FirstOrDefault();
        }

        public ChannelAgencyApply GetApplyingChannelAgencyApplyOfUser(long userId)
        {
            return ChannelAgencyApplyRepository.GetAll().Where(model => model.UserId == userId&&model.Status==ChannelAgencyApplyStatus.Applying).FirstOrDefault();
        }

        public ChannelAgencyApply CreateChannelAgencyApply(int channelAgentId,long userId,int orderId)
        {
            ChannelAgencyApply channelAgencyApply = new ChannelAgencyApply()
            {
                UserId = userId,
                OrderId = orderId,
                ChannelAgentId = channelAgentId,
                Status= ChannelAgencyApplyStatus.Applying
            };
            ChannelAgencyApplyRepository.Insert(channelAgencyApply);
            CurrentUnitOfWork.SaveChanges();
            return channelAgencyApply;
        }

        public void DeleteChannelAgencyApply(int orderId)
        {
            ChannelAgencyApply channelAgencyApply = ChannelAgencyApplyRepository.GetAll().Where(model => orderId == orderId).FirstOrDefault();
            ChannelAgencyApplyRepository.Delete(channelAgencyApply);
        }

        [UnitOfWork]
        public ChannelAgencyApply PassChannelAgencyApply(ChannelAgencyApply channelAgencyApply)
        {
            using (CurrentUnitOfWork.SetTenantId(channelAgencyApply.TenantId))
            {
                CreateChannelAgency(channelAgencyApply.ChannelAgent, channelAgencyApply.User);
                channelAgencyApply.Status = ChannelAgencyApplyStatus.Success;
                ChannelAgencyApplyRepository.Update(channelAgencyApply);
                CurrentUnitOfWork.SaveChanges();
                return channelAgencyApply;
            } 
        }

        [UnitOfWork]
        public ChannelAgency CreateChannelAgency(ChannelAgent channelAgent, User user)
        {
            using (CurrentUnitOfWork.SetTenantId(user.TenantId))
            {
                ChannelAgency channelAgency = new ChannelAgency()
                {
                    UserId = user.Id,
                    ChannelAgentId = channelAgent.Id,
                    RebateRatio = channelAgent.RebateRatio
                };
                ChannelAgencyRepository.Insert(channelAgency);
                CurrentUnitOfWork.SaveChanges();

                user.IsChannelAgency = true;
                user.UserChannelAgencyId = channelAgency.Id;
                UserRepository.Update(user);

                ProcessChannlRelationChain(channelAgency);
                return channelAgency;
            }
        }

        [UnitOfWork]
        public async Task ProcessChannlRelationChain(ChannelAgency channelAgency)
        {
            using (CurrentUnitOfWork.SetTenantId(channelAgency.TenantId))
            {
                User user = UserRepository.Get(channelAgency.UserId);

                await Task.Run(() =>
                {
                    ProcessChannlRelationChainRecursion(user, channelAgency);
                });
            }  
        }

        private void ProcessChannlRelationChainRecursion(User user, ChannelAgency channelAgency, int depth = 1)
        {
            if (user.Children.Count > 0)
            {
                foreach (User childUser in user.Children)
                {
                    childUser.ChannelAgentDepth = depth;
                    childUser.ChannelAgencyId = channelAgency.Id;

                    if (!childUser.IsChannelAgency)
                    {
                        ProcessChannlRelationChainRecursion(childUser, channelAgency, depth + 1);
                    }
                }
            }
        }

        public ChannelAgency AddNewCustomer(int channelAgencyId,int depth)
        {
            ChannelAgency channelAgency = ChannelAgencyRepository.Get(channelAgencyId);
            channelAgency.ChildCount++;

            if (depth > channelAgency.Depth)
            {
                channelAgency.Depth = depth;
            }
            ChannelAgencyRepository.Update(channelAgency);
            return channelAgency;
        }
    }
}
