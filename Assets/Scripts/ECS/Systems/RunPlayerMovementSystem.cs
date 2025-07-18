using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunPlayerMovementSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<PlayerComponent, CharacterControllerComponent, MoveComponent, DirectionComponent, TransformComponent, NetworkEntityComponent, OwnComponent, TopTransformComponent>> _filter = default;
        readonly EcsPoolInject<CharacterControllerComponent> _characterPool = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        readonly EcsPoolInject<DirectionComponent> _directionPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default; 
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var characterComp = ref _characterPool.Value.Get(entity);
                ref var moveComp = ref _movePool.Value.Get(entity);
                ref var directionComp = ref _directionPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);

                Vector3 moveDelta = directionComp.Direction * moveComp.GetCurrentValue * Time.deltaTime;
                characterComp.CharacterController.Move(moveDelta);

            }
        }
    }
}