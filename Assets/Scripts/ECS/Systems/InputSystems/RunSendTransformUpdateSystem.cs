using Fusion;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunSendTransformUpdateSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<TransformComponent, TopTransformComponent, NetworkEntityComponent>> _filter = default;
        readonly EcsPoolInject<TopTransformComponent> _topTransformPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<NetworkEntityComponent> _networkEntityPool = default;

        public float _timeSync;

        public void Run(IEcsSystems systems)
        {
            _timeSync -= Time.deltaTime;

            if (_timeSync <= 0)
            {
                foreach (var entity in _filter.Value)
                {
                    ref var networkEntityComp = ref _networkEntityPool.Value.Get(entity);
                    ref var topTransformComp = ref _topTransformPool.Value.Get(entity);
                    ref var transformComp = ref _transformPool.Value.Get(entity);

                    Vector3 currentPosition = transformComp.Transform.position;

                    NetworkTransformEvent transformEvent = new NetworkTransformEvent()
                    {
                        EntityKey = networkEntityComp.EntityKey,
                        Position = currentPosition,
                        Rotation = transformComp.Transform.localRotation,
                        TopRotation = topTransformComp.Transform.localRotation
                    };

                    PhotonRunHandler.Instance.SendRequestTransformRPC(MemoryPack.MemoryPackSerializer.Serialize(transformEvent));

                    _timeSync = 0.1f;
                }
            }
        }
    }
}