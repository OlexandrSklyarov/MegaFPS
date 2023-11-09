
using UnityEngine;

namespace SA.FPS
{
    public interface IWeaponAttack
    {
        DamageWeaponSettings DamageSettings {get;}
        Transform FirePoint { get; }
    }
}