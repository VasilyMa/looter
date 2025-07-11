using Client;
using Fusion;
using System;
using System.Collections;
using UnityEngine;

namespace Statement
{
    public class OnlineState : BattleState
    { 
        public static new OnlineState Instance
        {
            get
            {
                return (OnlineState)State.Instance;
            }
        }

        public override void Awake()
        {
            PlayerEntity = -1;
            InputEntity = -1;

            InitEcsHandler();

            InitCanvas();
        }

        public override void Start()
        {
            var runner = PhotonRunHandler.Instance.Runner;

            if (runner.IsServer)
            {
                BattleState.Instance.AddPlayer(new NetworkPlayerData(runner.LocalPlayer.PlayerId));
            }
            else
            {
                byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkPlayerData>(new NetworkPlayerData()
                {
                    PlayerOwn = runner.LocalPlayer.PlayerId
                });

                PhotonRunHandler.Instance.SendRequestReadyToStartRPC(sendData);
            }
        }

        public override void OnSceneLoaded()
        {
            
        }

        public override void OnStarted()
        { 
            base.OnStarted();

            InvokeCanvas<BattleCanvas>().OpenPanel<BattlePanel>();

            StartCoroutine(waitToSpawnPlayer());
        }

        IEnumerator waitToSpawnPlayer()
        {
            var runner = PhotonRunHandler.Instance.Runner;

            yield return new WaitUntil(() => runner.IsRunning);

            string networkKey = Guid.NewGuid().ToString();

            byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkUnitEntitySpawnEvent>(new NetworkUnitEntitySpawnEvent()
            {
                EntityKey = networkKey,
                SpawnKeyID = PlayerEntityBase.KEY_ID,
                PlayerOwner = runner.LocalPlayer.PlayerId
            });

            Debug.Log($"Send spawn event {networkKey}");
            PhotonRunHandler.Instance.SendUnitEntitySpawnRPC(sendData);
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var runner = PhotonRunHandler.Instance.Runner;

                string networkKey = Guid.NewGuid().ToString();

                byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkUnitEntitySpawnEvent>(new NetworkUnitEntitySpawnEvent()
                {
                    EntityKey = networkKey,
                    SpawnKeyID = PlayerEntityBase.KEY_ID,
                    PlayerOwner = runner.LocalPlayer.PlayerId
                });

                Debug.Log($"Send spawn event {networkKey}");
                PhotonRunHandler.Instance.SendUnitEntitySpawnRPC(sendData);
            }
        }
    }
}