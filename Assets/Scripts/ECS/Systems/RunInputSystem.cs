using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunInputSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InputComponent>> _filter = default;
        readonly EcsPoolInject<InputComponent> _inputPool = default;
        readonly EcsPoolInject<DirectionComponent> _directionPool = default;
        readonly EcsPoolInject<AimDirectionComponent> _aimDirectionPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var playerEntity = BattleState.Instance.PlayerEntity;

                if (playerEntity == -1) continue;

                ref var inputComp = ref _inputPool.Value.Get(entity);

                float horizontal = inputComp.MovementJoystick.Horizontal;
                float vertical = inputComp.MovementJoystick.Vertical;

                if (horizontal != 0 || vertical != 0)
                {
                    Vector3 direction = new Vector3(horizontal, 0f, vertical);

                    if (_directionPool.Value.Has(playerEntity))
                    {
                        ref var directionComp = ref _directionPool.Value.Get(playerEntity);
                        directionComp.Direction = direction;
                    }
                    else
                    {
                        ref var directionComp = ref _directionPool.Value.Add(playerEntity);
                        directionComp.Direction = direction;
                    }
                }
                else
                {
                    if (_directionPool.Value.Has(playerEntity))
                    {
                        _directionPool.Value.Del(playerEntity);
                    }
                }

                // === Aim input ===
                float aimHorizontal = inputComp.AimJoystick.Horizontal;
                float aimVertical = inputComp.AimJoystick.Vertical;

                if (aimHorizontal != 0 || aimVertical != 0)
                {
                    Vector3 aimDirection = new Vector3(aimHorizontal, 0f, aimVertical);

                    if (_aimDirectionPool.Value.Has(playerEntity))
                    {
                        _aimDirectionPool.Value.Get(playerEntity).Direction = aimDirection;
                    }
                    else
                    {
                        _aimDirectionPool.Value.Add(playerEntity).Direction = aimDirection;
                    }
                }
                else
                {
                    _aimDirectionPool.Value.Del(playerEntity);
                }
            }
        }
    }
}
