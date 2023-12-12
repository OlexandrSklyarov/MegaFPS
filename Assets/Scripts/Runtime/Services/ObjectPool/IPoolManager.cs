using SA.FPS.Runtime.UI.HUD;
using UnityEngine;

namespace SA.FPS
{
    public interface IPoolManager : IService 
    {
        Decal GetDecal(DecalType type);
        UIWeaponView GetUIWeaponView();
        EnemyUnitView GetUnitView(UnitType type);
    }
}
