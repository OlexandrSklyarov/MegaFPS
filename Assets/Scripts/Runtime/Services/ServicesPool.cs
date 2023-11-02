using System;
using System.Collections.Generic;
using Runtime.Services.WeaponsFactory;

namespace SA.FPS
{
    public sealed class ServicesPool : IDisposable
    {
        public static ServicesPool Instance 
        { 
            get
            {
                if (_instance == null) _instance = new ServicesPool();
                return _instance;
            }
        }

        private static ServicesPool _instance;
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
        private bool _isInit;


        public void Init(GameConfig config)
        {            
            if (_isInit)
            {
                throw new Exception($"Initialization has already been done!!!");
            }

            _services.Add(typeof(GameConfig), config);
            _services.Add(typeof(InputService), new InputService());
            _services.Add(typeof(IWeaponItemFactory), new WeaponItemFactory(config.WeaponData));
            _services.Add(typeof(IPoolManager), new PoolManager(config.PoolData));       

            _isInit = true;     
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


        public void Dispose() => _instance = null;       
    }
}