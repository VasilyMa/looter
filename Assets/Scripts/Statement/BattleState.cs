using System.Collections.Generic;
using Client;
using Leopotam.EcsLite;

using UnityEngine;

namespace Statement
{
    public class BattleState : State
    {
        public static new BattleState Instance
        {
            get
            {
                return (BattleState)State.Instance;
            }
        }

        [HideInInspector] public EcsRunHandler EcsHandler;

        protected Dictionary<string, EcsPackedEntity> dictionaryEntities = new Dictionary<string, EcsPackedEntity>();

        public override void Awake() => EcsHandler = new MainEcsHandler(); 
        public override void Start() => EcsHandler.Init();
        public override void Update() => EcsHandler.Run();
        public override void FixedUpdate() => EcsHandler.FixedRun();
        public override void OnDestroy()
        {
            base.OnDestroy();   
            EcsHandler.Dispose();
        }
        public virtual void AddEntity(string key, int entity)
        {
            if (dictionaryEntities.ContainsKey(key)) return;

            dictionaryEntities.Add(key, EcsHandler.World.PackEntity(entity));
        }
        public virtual bool TryGetEntity(string key, out EcsPackedEntity packedEntity)
        {
            if (dictionaryEntities.ContainsKey(key))
            {
                packedEntity = dictionaryEntities[key];
                return true;
            }

            packedEntity = default(EcsPackedEntity);
            return false;
        }
        public virtual bool TryGetEntity(string key, out int unpackedEntity)
        {
            if (dictionaryEntities.ContainsKey(key))
            {
                if (dictionaryEntities[key].Unpack(EcsHandler.World, out int entity))
                {
                    unpackedEntity = entity;
                    return true;
                }
            }

            unpackedEntity = -1;
            return false;
        }

        public void SendRequest<TRequest>(TRequest request) where TRequest : struct, IRequestable
        {
            var world = EcsHandler.World;
            var entityEvent = world.NewEntity();

            world.GetPool<RequestEvent>().Add(entityEvent);
            ref var eventComp = ref world.GetPool<TRequest>().Add(entityEvent);

            eventComp = request;
        }
    }
}