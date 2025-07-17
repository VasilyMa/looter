using Client; 
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
            InitEcsHandler();  
        }

        public override void Start()
        {
            StartCoroutine(AwaitingReadyStart());
        }

        IEnumerator AwaitingReadyStart()
        {
            var runner = PhotonRunHandler.Instance.Runner;
            
            yield return new WaitUntil(() => runner.IsRunning);

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

            if (UIModule.OpenCanvas<BattleCanvas>(out var battleCanvas))
            {
                battleCanvas.OpenPanel<BattlePanel>();
            }
            
            SendPlayerSpawnEvent();
        }

        public void SendPlayerSpawnEvent()
        { 
            var runner = PhotonRunHandler.Instance.Runner;

            if (runner.IsServer)
            {
                foreach (var player in dictionaryPlayers)
                {
                    string networkKey = Guid.NewGuid().ToString();

                    byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkUnitEntitySpawnEvent>(new NetworkUnitEntitySpawnEvent()
                    {
                        EntityKey = networkKey,
                        SpawnKeyID = PlayerEntityBase.KEY_ID,
                        PlayerOwner = player.Value.PlayerId,
                        SpawnPos = new Vector3(UnityEngine.Random.Range(-1, 1), 0.5f, UnityEngine.Random.Range(-1f, 1f))
                    });

                    Debug.Log($"Send spawn event {networkKey}");
                    PhotonRunHandler.Instance.SendUnitEntitySpawnRPC(sendData);
                }
            }
        }
    }
}