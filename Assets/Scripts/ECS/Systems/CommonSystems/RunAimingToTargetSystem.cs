using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunAimingToTargetSystem : IEcsRunSystem 
    { 
        readonly EcsFilterInject<Inc<AimComponent, TopTransformComponent>> _filter = default;
        readonly EcsPoolInject<AimComponent> _aimPool = default;
        readonly EcsPoolInject<TopTransformComponent> _topTransformPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var aimComp = ref _aimPool.Value.Get(entity);
                ref var topTransformComp = ref _topTransformPool.Value.Get(entity);

                topTransformComp.Transform.LookAt(aimComp.Target, Vector3.up);
            }
        }
    }
}