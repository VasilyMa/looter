using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunPrepareShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
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

                _weaponShootPool.Value.Add(holderWeaponComp.WeaponEntity).Target = aimComp.Target; 
            }
        }
    }
}