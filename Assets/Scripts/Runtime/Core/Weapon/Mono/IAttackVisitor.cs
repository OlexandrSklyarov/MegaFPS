using UnityEngine;

namespace SA.FPS
{
    public interface IAttackVisitor
    {
        void Visit(IWeaponAttack weapon);
        void Visit(IWeaponAttack weapon, RaycastHit hit);
    }
}