using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunDisposeInputMovementSystem : IEcsRunSystem 
    { 
        readonly EcsFilterInject<Inc<InputMovementComponent, InputComponent, DisposeInputMovementEvent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent>> _playerFilter = default;
        readonly EcsPoolInject<DisposeMovementEvent> _disposeMovePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                foreach (var playerEntity in _playerFilter.Value)
                {
                    _disposeMovePool.Value.Add(playerEntity);
                }
            }
        }
    }
}