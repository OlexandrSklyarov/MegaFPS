using System;
using System.Collections.Generic;
using SA.FPS.Runtime.UI.HUD;

namespace SA.FPS
{
    public class PoolManager : IPoolManager
    {
        private Dictionary<DecalType, BaseGOPool<Decal>> DecalPools
        {
            get
            {
                if (_decalPools == null)
                {
                    _decalPools = new Dictionary<DecalType, BaseGOPool<Decal>>();

                    foreach(var item in _config.Decals)
                    {
                        _decalPools.Add
                        (
                            item.Type, 
                            new BaseGOPool<Decal>(item.Prefab, item.StartCount, item.MaxPoolCount, $"DECAL_POOL[{item.Type}]")
                        );
                    }
                }

                return _decalPools;
            }
        }


        private Dictionary<UnitType, BaseGOPool<EnemyUnitView>> UnitViewPools
        {
            get
            {
                if (_unitViewPools == null)
                {
                    _unitViewPools = new Dictionary<UnitType, BaseGOPool<EnemyUnitView>>();

                    foreach(var item in _config.Units)
                    {
                        _unitViewPools.Add
                        (
                            item.Type, 
                            new BaseGOPool<EnemyUnitView>(item.Prefab, item.StartCount, item.MaxPoolCount, $"UNITS_POOL[{item.Type}]")
                        );
                    }
                }

                return _unitViewPools;
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


        private Dictionary<DecalType, BaseGOPool<Decal>> _decalPools;
        private Dictionary<UnitType, BaseGOPool<EnemyUnitView>> _unitViewPools;
        private BaseGOPool<UIWeaponView> _uiWeaponViewPool;
        private PoolObjectConfig _config;


        public PoolManager(PoolObjectConfig config)
        {
            _config = config;
        }


        public Decal GetDecal(DecalType type) => DecalPools[type].Get();
        

        public UIWeaponView GetUIWeaponView() => UIWeaponViewPool.Get();


        public EnemyUnitView GetUnitView(UnitType type)
        {
            if (UnitViewPools.TryGetValue(type, out var pool))
            {
                return pool.Get();
            }

            throw new ArgumentException($"Not found unit with this type {type}, probably not added in the config.");
        }
    }
}