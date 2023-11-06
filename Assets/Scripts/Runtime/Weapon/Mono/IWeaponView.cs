
using UnityEngine;

namespace SA.FPS
{
    public interface IWeaponView
    {
        Transform FirePoint { get; }
        WeaponSettings Settings {get;}
        bool TryReload(out float reloadTime);
    }
}