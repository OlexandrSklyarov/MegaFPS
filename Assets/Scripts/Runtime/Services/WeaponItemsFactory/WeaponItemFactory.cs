using System;
using System.Linq;
using Runtime.Services.Inventory;
using Runtime.Services.WeaponsFactory.Items;
using SA.FPS;

namespace Runtime.Services.WeaponsFactory
{
    public class WeaponItemFactory : IWeaponItemFactory
    {
        private readonly InventoryConfig _config;

        public WeaponItemFactory(InventoryConfig config)
        {
            _config = config;   
        }


        public IInventoryItem CreateWeaponItem(WeaponType type)
        {
            return type switch 
            {
                WeaponType.MachineGun => new MachineGun(_config.Items.First(x => x.Type == type)),
                
                _=> throw new ArgumentNullException($"This type of weapon {type} is not processed.")
            };
        }
    }
}