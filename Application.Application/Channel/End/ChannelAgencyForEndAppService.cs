using Application.Channel.ChannelAgencies;
using Application.Channel.End.Dto;
using Infrastructure.Application.Services;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.End
{
    public class ChannelAgencyForEndAppService : CrudAppService<ChannelAgency, ChannelAgencyDto>, IChannelAgencyForEndAppService
    {
        public ChannelAgencyForEndAppService(IRepository<ChannelAgency> respository) :base(respository)
        {

        }
    }
}
