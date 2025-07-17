using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunWeaponCooldownSystem : IEcsRunSystem 
    { 
        readonly EcsFilterInject<Inc<WeaponComponent, CooldownComponent>, Exc<InReloadComponent>> _filter = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _cooldownPool.Value.Get(entity).CurrentTime -= Time.deltaTime; 
            }
        }
    }
}