using Leopotam.EcsLite;
using Leopotam.EcsLite.Di; 

namespace Client 
{
    sealed class RunPrepareShootSystem : IEcsRunSystem 
    { 
        readonly EcsFilterInject<Inc<AimComponent, HolderWeaponComponent, AllowShootComponent>> _filter = default;
        readonly EcsPoolInject<AimComponent> _aimPool = default;
        readonly EcsPoolInject<HolderWeaponComponent> _holderWeaponPool = default;
        readonly EcsPoolInject<WeaponShootRequestEvent> _weaponShootPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var aimComp = ref _aimPool.Value.Get(entity);
                ref var holderWeaponComp = ref _holderWeaponPool.Value.Get(entity);

                _weaponShootPool.Value.Add(holderWeaponComp.Weapons[0].WeaponEntity).Target = aimComp.Target; 
            }
        }
    }
}