using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunDisposeInputActionSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputAimComponent, InputComponent, DisposeInputActionEvent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent, InActionComponent>> _playerFilter = default;
        readonly EcsPoolInject<DisposeActionEvent> _disposeAction = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                foreach (var playerEntity in _playerFilter.Value)
                {
                    _disposeAction.Value.Add(playerEntity);
                    Debug.Log("Cancel action on player");
                }
            }
        }
    }
}