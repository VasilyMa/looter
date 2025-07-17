using System.Collections.Generic;

namespace Client 
{
    struct HolderWeaponComponent 
    {
        public List<WeaponData> Weapons;
    }

    public struct WeaponData
    {
        public int WeaponEntity;
    }
}