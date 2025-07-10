using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class RunReceiveTransformSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<NetworkTransformUpdateEvent, NetworkEntityComponent, TransformComponent>, Exc<OwnComponent>> _filter = default;
        readonly EcsPoolInject<NetworkLerpTransformComponent> _targetPool = default;
        readonly EcsPoolInject<NetworkTransformUpdateEvent> _updateEventPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var update = ref _updateEventPool.Value.Get(entity);

                ref var targetComp = ref _targetPool.Value.Has(entity)
                    ? ref _targetPool.Value.Get(entity)
                    : ref _targetPool.Value.Add(entity);

                targetComp.TargetPosition = update.Position;
                targetComp.TargetRotation = update.Quaternion; 
            }
        }
    }
}