using Client;
using Fusion;
using Leopotam.EcsLite;
using System.Collections.Generic;
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

        public EntityBase PlayerEntityBase;
        public int PlayerEntity = -1;
        public int InputEntity = -1;

        protected Dictionary<string, EcsPackedEntity> dictionaryEntities = new Dictionary<string, EcsPackedEntity>();
        protected Dictionary<int, PlayerRef> dictionaryPlayers = new Dictionary<int, PlayerRef>();

        public virtual void OnStarted() { Debug.Log("[Start game] game started"); }
        public virtual void OnSceneLoaded() => EcsHandler = new MainEcsHandler();
        public virtual void ShutdownEcsHandler() => EcsHandler.Dispose();
        public override void Start() { }
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

        public void AddPlayer(NetworkPlayerData data)
        {
            if (!dictionaryPlayers.ContainsKey(data.PlayerOwn))
            {    
                var players = PhotonInitializer.Instance.Runner.ActivePlayers;

                foreach (var player in players)
                {
                    if (player.PlayerId == data.PlayerOwn)
                    {
                        dictionaryPlayers.Add(data.PlayerOwn, player);
                    }
                }
            }

            Debug.Log($"Player add {data.PlayerOwn}");

            if (PhotonRunHandler.Instance.SessionData.TargetPlayerCount == dictionaryPlayers.Count)
            {
                Debug.Log($"Player count is max {PhotonRunHandler.Instance.SessionData.TargetPlayerCount} and {PhotonRunHandler.Instance.SessionData.TargetPlayerCount == dictionaryPlayers.Count}");
                PhotonRunHandler.Instance.SendRequestStartGameRPC();
            }
        }

        protected void InitEcsHandler()
        {
            EcsHandler = PhotonRunHandler.Instance.Runner.IsServer ? new ServerRunHandler() : new ClientRunHandler();

            EcsHandler.Init();
        }
    }
}