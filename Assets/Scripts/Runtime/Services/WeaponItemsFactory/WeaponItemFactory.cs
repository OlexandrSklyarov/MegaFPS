using System.Collections.Generic;
using System.Linq;
using SA.FPS;
using UnityEngine;

namespace Runtime.Services.WeaponsFactory
{
    public class WeaponItemFactory : IWeaponItemFactory
    {
        private readonly WeaponsConfig _config;
        private readonly Dictionary<WeaponType, FireWeaponView> _weaponsCache;

        public WeaponItemFactory(WeaponsConfig config)
        {
            _config = config;   
            _weaponsCache = new Dictionary<WeaponType, FireWeaponView>();
        }


        public FireWeaponView CreateWeaponItem(WeaponType type, Transform parent)
        {
            var prefab = _config.Weapons.First(x => x.Key == type).Value;

            if (!_weaponsCache.TryGetValue(type, out var instance))
            {
                instance = UnityEngine.Object.Instantiate(prefab, parent); 
                _weaponsCache.Add(type, instance);
            }            

            instance.gameObject.SetActive(true);
            
            return instance;           
        }
    }
}