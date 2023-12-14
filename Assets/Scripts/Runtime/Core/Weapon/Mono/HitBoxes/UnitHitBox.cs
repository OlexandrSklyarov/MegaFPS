using UnityEngine;

namespace SA.FPS
{
    public class UnitHitBox : BaseHitBox, IAttackVisitable
    {
        void IAttackVisitable.Visit(IWeaponAttack weapon)
        {
            if (!TryGetEntity(out var unitEntity)) return;

            ref var evt = ref GetOverlapDamageEvent(unitEntity);

            evt.DamageSource = weapon.FirePoint;
            evt.Damage = evt.Damage = weapon.DamageSettings.Damage;
            evt.Power = weapon.DamageSettings.MeleePushPower;
            evt.IsApplyPushForce = weapon.DamageSettings.IsUsedPushForce;
        }
        
        void IAttackVisitable.Visit(IWeaponAttack weapon, RaycastHit hit)
        {
            if (!TryGetEntity(out var unitEntity)) return;

            ref var evt = ref GetRaycastDamageEvent(unitEntity);

            evt.Hit = hit;
            evt.Damage = weapon.DamageSettings.Damage;
            evt.Power = weapon.DamageSettings.PushPower;
            evt.IsApplyPushForce = weapon.DamageSettings.IsUsedPushForce;
        } 
    }
}