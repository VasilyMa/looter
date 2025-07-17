using Leopotam.EcsLite;

namespace Client 
{
    public struct WeaponComponent : IWeapon
    {
        public WeaponType Type;
        public int Index;
        [UnityEngine.HideInInspector] public string OwnerEntityKey;

        public void InitWeapon(EcsWorld world, int entity, string ownerEntityKey)
        {
            ref var weaponComp = ref world.GetPool<WeaponComponent>().Add(entity);
            weaponComp.OwnerEntityKey = ownerEntityKey;
            weaponComp.Type = Type;
            
            ref var attackComp = ref world.GetPool<AttackComponent>().Add(entity);
            attackComp.Value = 1f;
        }
    }

    public struct AttackComponent
    {
        public float Value;
    }
}
public enum DamageType { physic, solar, lightning, stasis, twilight }
public enum WeaponType { pistol, submachinegun, autorifle, scout, shotgun, launcher, machinegun, sniper }