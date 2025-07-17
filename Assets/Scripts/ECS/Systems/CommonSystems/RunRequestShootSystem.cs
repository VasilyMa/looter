using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunRequestShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<WeaponComponent, WeaponShootRequestEvent, CooldownComponent>> _filter = default; 
        readonly EcsPoolInject<CooldownComponent> _cooldownPool = default;
        readonly EcsPoolInject<WeaponShootRequestEvent> _requestPool = default;
        readonly EcsPoolInject<ResolveShootEvent> _resolvePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var cooldownComp = ref _cooldownPool.Value.Get(entity);

                if (cooldownComp.CurrentTime <= 0)
                {
                    cooldownComp.CurrentTime = cooldownComp.Tick;

                    ref var requestComp = ref _requestPool.Value.Get(entity);

                    string targetKey = requestComp.Target.gameObject.name; 

                    _resolvePool.Value.Add(entity).TargetKey = targetKey;
                }
            }
        }
    }
}