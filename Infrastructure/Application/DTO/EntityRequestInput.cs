using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.DTO
{
    public class EntityRequestInput : EntityRequestInput<int>, IEntityDto, IEntityDto<int>
    {
        public EntityRequestInput(int id):base(id)
        {
        }
    }

    public class EntityRequestInput<TPrimaryKey> : EntityDto<TPrimaryKey>
    {
        public EntityRequestInput(TPrimaryKey id)
        {
            Id = id;
        }
    }
}
