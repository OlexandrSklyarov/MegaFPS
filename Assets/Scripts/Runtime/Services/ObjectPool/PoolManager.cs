using System;
using System.Collections.Generic;
using SA.FPS.Runtime.UI.HUD;
using UnityEngine;

namespace SA.FPS
{
    public class PoolManager : IPoolManager
    {
        private Dictionary<DecalType, BaseGOPool<Decal>> DecalPools
        {
            get
            {
                if (decalPools == null)
                {
                    decalPools = new Dictionary<DecalType, BaseGOPool<Decal>>();

                    foreach(var item in _config.Decals)
                    {
                        decalPools.Add
                        (
                            item.Type, 
                            new BaseGOPool<Decal>(item.Prefab, item.StartCount, item.MaxPoolCount, "DECAL_POOL")
                        );
                    }
                }

                return decalPools;
            }
        }

        private BaseGOPool<UIWeaponView> UIWeaponViewPool
        {
            get
            {
                if (_uiWeaponViewPool == null)
                {
                    _uiWeaponViewPool = new BaseGOPool<UIWeaponView>
                    (
                        _config.UIWeaponView.Prefab, 
                        _config.UIWeaponView.StartCount, 
                        _config.UIWeaponView.MaxPoolCount, 
                        "UI_WEAPON_VIEW_POOL"
                    );                    
                }

                return _uiWeaponViewPool;
            }
        }


        private Dictionary<DecalType, BaseGOPool<Decal>> decalPools;
        private BaseGOPool<UIWeaponView> _uiWeaponViewPool;
        private PoolObjectConfig _config;


        public PoolManager(PoolObjectConfig config)
        {
            _config = config;
        }


        public Decal GetDecal(DecalType type) => DecalPools[type].Get();
        

        public UIWeaponView GetUIWeaponView() => UIWeaponViewPool.Get();
    }
}