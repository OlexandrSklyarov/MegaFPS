using System.Collections.Generic;
using System.Linq;
using SA.FPS;
using UnityEngine;

namespace Runtime.Services.WeaponsFactory
{
    public class WeaponItemFactory : IWeaponItemFactory
    {
        private readonly WeaponsConfig _config;
        private readonly Dictionary<WeaponType, WeaponView> _weaponsCache;

        public WeaponItemFactory(WeaponsConfig config)
        {
            _config = config;   
            _weaponsCache = new Dictionary<WeaponType, WeaponView>();
        }


        public WeaponView GetWeaponItem(WeaponType type, Transform parent)
        {
            var currentItem = _config.Weapons.First(x => x.Type == type);

            if (!_weaponsCache.TryGetValue(type, out var instance))
            {
                instance = UnityEngine.Object.Instantiate(currentItem.Prefab, parent); 
                _weaponsCache.Add(type, instance);
            }            

            instance.gameObject.SetActive(true);
            
            return instance;           
        }
    }
}