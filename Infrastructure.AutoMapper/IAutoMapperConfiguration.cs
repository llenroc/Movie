using System;
using System.Collections.Generic;
using AutoMapper;

namespace Infrastructure.AutoMapper
{
    public interface IAutoMapperConfiguration
    {
        List<Action<IMapperConfigurationExpression>> Configurators { get; }
    }
}
