using UnityEngine;

namespace SA.FPS
{
    public interface IAttackVisitable
    {
        void Visit(IWeaponAttack weapon);
        void Visit(IWeaponAttack weapon, RaycastHit hit);
    }
}