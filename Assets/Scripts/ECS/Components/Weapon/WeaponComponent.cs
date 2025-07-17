using Leopotam.EcsLite;

namespace Client 
{
    public struct WeaponComponent : IWeapon
    {
        public WeaponType Type;
        [UnityEngine.HideInInspector] public string OwnerEntityKey;

        public void InitWeapon(EcsWorld world, int entity, string ownerEntityKey)
        {
            ref var weaponComp = ref world.GetPool<WeaponComponent>().Add(entity);
            weaponComp.OwnerEntityKey = ownerEntityKey;
            weaponComp.Type = Type;
        }
    }
}

public enum WeaponType { pistol, submachinegun, autorifle, scout, shotgun, launcher, machinegun, sniper }