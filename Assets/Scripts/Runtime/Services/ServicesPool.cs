using System;
using System.Collections.Generic;

namespace SA.FPS
{
    public sealed class ServicesPool
    {
        private readonly Dictionary<Type, IService> _services;

        public ServicesPool()
        {
            _services = new Dictionary<Type, IService>()
            {
                {typeof(InputService), new InputService()}
            };
        }

        public T GetService<T>() where T : IService
        {
            var type = typeof(T);

            if (!_services.ContainsKey(type))
            {
                throw new Exception($"Not found this service {type}!!!");
            }

            return (T)_services[type];
        }
    }
}