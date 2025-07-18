using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunDisposeInputAimSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputAimComponent, InputComponent, DisposeInputAimEvent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent>> _playerFilter = default;
        readonly EcsPoolInject<CancelShootingEvent> _cancelShootPool = default;
        readonly EcsPoolInject<DisposeAimEvent> _disposeAimPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                foreach (var playerEntity in _playerFilter.Value)
                {
                    _cancelShootPool.Value.Add(playerEntity);   
                    _disposeAimPool.Value.Add(playerEntity);
                }
            }
        }
    }
}