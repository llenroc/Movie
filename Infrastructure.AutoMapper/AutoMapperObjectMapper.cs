﻿using AutoMapper;
using Infrastructure.Dependency;
using IObjectMapper = Infrastructure.ObjectMapping.IObjectMapper;

namespace Infrastructure.AutoMapper
{
    public class AutoMapperObjectMapper : IObjectMapper, ISingletonDependency
    {
        private readonly IMapper _mapper;

        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
