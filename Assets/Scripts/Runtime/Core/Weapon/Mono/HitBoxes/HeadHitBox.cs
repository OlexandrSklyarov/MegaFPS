using UnityEngine;

namespace SA.FPS
{
    public class HeadHitBox : BaseHitBox, IAttackVisitable
    {
        void IAttackVisitable.Visit(IWeaponAttack weapon)
        {
            if (!TryGetEntity(out var unitEntity)) return;

            ref var evt = ref GetOverlapDamageEvent(unitEntity);

            evt.DamageSource = weapon.FirePoint;
            evt.Damage = evt.Damage = weapon.DamageSettings.Damage * weapon.DamageSettings.HeadShotDamageMultiplier;
            evt.Power = weapon.DamageSettings.PushPower;
        }
        
        void IAttackVisitable.Visit(IWeaponAttack weapon, RaycastHit hit)
        {
            if (!TryGetEntity(out var unitEntity)) return;

            ref var evt = ref GetRaycastDamageEvent(unitEntity);

            evt.Hit = hit;
            evt.Damage = weapon.DamageSettings.Damage * weapon.DamageSettings.HeadShotDamageMultiplier;
            evt.Power = weapon.DamageSettings.PushPower;
        } 
    }
}