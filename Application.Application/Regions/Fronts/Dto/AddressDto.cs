﻿using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Regions.Fronts.Dto
{
    [AutoMapFrom(typeof(Address))]
    public class AddressDto:EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int Level { get; set; }

        public int Sort { get; set; }

        public string EnglishName { get; set; }

        public string ShortEnglishName { get; set; }
    }
}
