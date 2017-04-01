using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.DTO
{
    public class IdInput : IdInput<int>
    {

    }
    public class IdInput<TId>
    {
        public IdInput()
        {

        }

        public IdInput(TId id)
        {
            Id = id;
        }

        public TId Id { get; set; }
    }
}
