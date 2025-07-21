using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunInvokeActionSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<InputAimComponent, InputComponent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent, InActionComponent>, Exc<PreRequestActionEvent>> _playerFilter = default;

        readonly EcsPoolInject<InputAimComponent> _inputPool = default;
        readonly EcsPoolInject<PreRequestActionEvent> _preRequestPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                foreach (var playerEntity in _playerFilter.Value)
                {
                    ref var inputComp = ref _inputPool.Value.Get(entity);

                    float aimHorizontal = inputComp.AimJoystick.Horizontal;
                    float aimVertical = inputComp.AimJoystick.Vertical;

                    float sqrMagnitude = aimHorizontal * aimHorizontal + aimVertical * aimVertical;

                    if (sqrMagnitude > 0.91f) // 0.8^2
                    {
                        _preRequestPool.Value.Add(playerEntity);
                        Debug.Log("Invoke current action on player");
                    }
                }
            }
        }
    }
}