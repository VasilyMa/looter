using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client 
{
    sealed class RunInputAimSystem : IEcsRunSystem
    { 
        readonly EcsFilterInject<Inc<InputAimComponent, InputComponent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent>> _playerFilter = default;
        readonly EcsPoolInject<InputAimComponent> _inputPool = default;
        readonly EcsPoolInject<AimDirectionComponent> _aimDirectionPool = default;
        readonly EcsPoolInject<AllowShootComponent> _allowShootPool = default;
        //readonly EcsPoolInject<SendTransformUpdateEvent> _sendTransformUpdatePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                foreach (var playerEntity in _playerFilter.Value)
                {
                    ref var inputComp = ref _inputPool.Value.Get(entity);

                    float aimHorizontal = inputComp.AimJoystick.Horizontal;
                    float aimVertical = inputComp.AimJoystick.Vertical;

                    float sqrMagnitude = aimHorizontal * aimHorizontal + aimVertical * aimVertical;

                    // Если палец двигает джойстик — записываем направление
                    if (sqrMagnitude > 0.01f) // любое движение, можно использовать 0.001f как порог
                    {
                        Vector3 aimDirection = new Vector3(aimHorizontal, 0f, aimVertical);

                        if (_aimDirectionPool.Value.Has(playerEntity))
                            _aimDirectionPool.Value.Get(playerEntity).Direction = aimDirection;
                        else
                            _aimDirectionPool.Value.Add(playerEntity).Direction = aimDirection;

                        // Только если игрок **реально прицелился**, добавляем AllowShootComponent
                        if (sqrMagnitude > 0.64f) // 0.8^2
                        { 
                            _allowShootPool.Value.Add(playerEntity);
                        }
                        else
                        {
                            _allowShootPool.Value.Del(playerEntity);
                        }
                    }
                    else
                    {
                        _aimDirectionPool.Value.Del(playerEntity);
                        _allowShootPool.Value.Del(playerEntity);
                    }

                    //_sendTransformUpdatePool.Value.Add(playerEntity);
                } 
            }
        }
    }
}