using System;
using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "SO/WeaponsConfig")]
    public class WeaponsConfig : ScriptableObject
    {
        [field: SerializeField] public WeaponItem[] Weapons {get; private set;}

        [Serializable]
        public class WeaponItem
        {
            [field: SerializeField] public WeaponType Type {get; private set;}
            [field: SerializeField] public WeaponView Prefab {get; private set;}
        }
    }
}