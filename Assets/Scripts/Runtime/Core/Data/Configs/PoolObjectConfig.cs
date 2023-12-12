using System;
using System.Collections.Generic;
using UnityEngine;
using SA.FPS.Runtime.UI.HUD;

namespace SA.FPS
{    
    [CreateAssetMenu(fileName = "PoolObjectConfig", menuName = "SO/PoolObjectConfig")]
    public class PoolObjectConfig : ScriptableObject 
    {
        [field: SerializeField] public List<DecalItem> Decals {get; private set;}
        [field: Space, SerializeField] public UIWeaponViewItem UIWeaponView {get; private set;}
        [field: Space, SerializeField] public List<UnitViewItem> Units {get; private set;}


        [Serializable]
        public class DecalItem
        {
            [field: SerializeField] public DecalType Type {get; private set;}
            [field: SerializeField] public Decal Prefab {get; private set;}
            [field: SerializeField, Min(4)] public int StartCount {get; private set;} = 10;
            [field: SerializeField, Min(4)] public int MaxPoolCount {get; private set;} = 32;
        }

        [Serializable]
        public class UIWeaponViewItem
        {
            [field: SerializeField] public UIWeaponView Prefab {get; private set;}
            [field: SerializeField, Min(4)] public int StartCount {get; private set;} = 3;
            [field: SerializeField, Min(4)] public int MaxPoolCount {get; private set;} = 3;
        }

        [Serializable]
        public class UnitViewItem
        {
            [field: SerializeField] public EnemyUnitView Prefab {get; private set;}
            [field: SerializeField] public UnitType Type {get; private set;}
            [field: SerializeField, Min(4)] public int StartCount {get; private set;} = 3;
            [field: SerializeField, Min(4)] public int MaxPoolCount {get; private set;} = 3;
        }
    }   
}