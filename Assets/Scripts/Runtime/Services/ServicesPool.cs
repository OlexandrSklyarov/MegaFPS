using System;
using System.Collections.Generic;
using Runtime.Services.WeaponsFactory;

namespace SA.FPS
{
    public sealed class ServicesPool
    {
        private readonly Dictionary<Type, IService> _services;

        public ServicesPool(GameConfig _config)
        {            
            _services = new Dictionary<Type, IService>()
            {
                {typeof(InputService), new InputService()},
                {typeof(IWeaponItemFactory), new WeaponItemFactory(_config.WeaponData)},
                {typeof(IPoolManager), new PoolManager(_config.PoolData)}
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