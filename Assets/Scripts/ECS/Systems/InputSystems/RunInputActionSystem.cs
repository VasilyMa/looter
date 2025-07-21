using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunInputActionSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<InputAimComponent, InputComponent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent>, Exc<InActionComponent>> _playerFilter = default;

        readonly EcsPoolInject<InputAimComponent> _inputPool = default;
        readonly EcsPoolInject<InActionComponent> _inActionPool = default;
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

                    if (sqrMagnitude > 0.63f) // 0.8^2
                    {
                        _inActionPool.Value.Add(playerEntity);
                        Debug.Log("Start action on player");
                    }
                }
            }
        }
    }
}