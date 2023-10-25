using System;
using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "SO/WeaponsConfig")]
    public class WeaponsConfig : ScriptableObject
    {
        [field: SerializeField] public WeaponKeyValuePair[] Weapons {get; private set;}

        [Serializable]
        public class WeaponKeyValuePair
        {
            [field: SerializeField] public WeaponType Key {get; private set;}
            [field: SerializeField] public FireWeaponView Value {get; private set;}

        }
    }
}