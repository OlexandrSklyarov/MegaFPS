using UnityEngine;

namespace SA.FPS
{    
    public abstract class Cartridge
    {
        public int Ammo
        {
            get => _currentAmmo;
            private set
            {
                _currentAmmo = Mathf.Clamp(value, 0, _startAmmo);
            }
        }

        private int _startAmmo;
        private int _currentAmmo;

        public Cartridge(int startAmmo)
        {
            Ammo = _startAmmo = startAmmo;
        }

        public void Take() => Ammo--;        
    }
}