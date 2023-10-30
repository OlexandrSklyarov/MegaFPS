
using UnityEngine;

namespace SA.FPS
{
    public interface IWeaponView
    {
        Transform FirePoint { get; }
        Transform LeftHand { get; }
        Transform RightHand { get; }
    }
}