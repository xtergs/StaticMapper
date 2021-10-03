using System;
using System.Collections.Generic;

namespace StaticMapperGenerator.Implementation
{
    public abstract class StaticMapper
    {
        private readonly Dictionary<(Type Source, Type Destination), IMapper> _mappers;

        protected StaticMapper()
        {
            _mappers = new Dictionary<(Type, Type), IMapper>();

            Init();
        }

        public D Map<D>(object value)
        {
            return (D)_mappers[(value.GetType(), typeof(D))].Map(value);
        }

        protected void AddMap(Type source, Type destination, IMapper mapper)
        {
            _mappers[(source, destination)] = mapper;
        }

        protected virtual void Init()
        {
        }
    }
}