using Client;
using Fusion;
using System;
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

            CreateHandler();

            InitCanvas();
        } 

        public override void OnSceneLoaded()
        {
            
        }

        public override void OnStarted()
        {
            base.OnStarted();

            var runner = PhotonRunHandler.Instance.Runner;

            InvokeCanvas<BattleCanvas>().OpenPanel<BattlePanel>();

            string networkKey = Guid.NewGuid().ToString();

            byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkUnitEntitySpawnEvent>(new NetworkUnitEntitySpawnEvent()
            {
                EntityKey = networkKey,
                SpawnKeyID = PlayerEntityBase.KEY_ID,
                PlayerOwner = runner.LocalPlayer.PlayerId
            });

            PhotonRunHandler.Instance.SendUnitEntitySpawnRPC(sendData);
        }

        public override void Start()
        {
            var runner = PhotonRunHandler.Instance.Runner;

            if (runner.IsServer)
            {

                byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkPlayerData>(new NetworkPlayerData()
                {
                    PlayerOwn = runner.LocalPlayer.PlayerId
                });

                PhotonRunHandler.Instance.SendRequestReadyToStartRPC(sendData); 
            }
            else
            {
                BattleState.Instance.AddPlayer(new NetworkPlayerData(runner.LocalPlayer.PlayerId));
            }

        }
    }
}