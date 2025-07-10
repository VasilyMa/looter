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

            EcsHandler = CreateHandler();

            InitCanvas();
        }

        public override void Start()
        {
            base.Start();

            InvokeCanvas<BattleCanvas>().OpenPanel<BattlePanel>();

            var runner = PhotonRunHandler.Instance.Runner;

            string networkKey = Guid.NewGuid().ToString();

            byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkUnitEntitySpawnEvent>(new NetworkUnitEntitySpawnEvent()
            {
                EntityKey = networkKey,
                SpawnKeyID = PlayerEntityBase.KEY_ID,
                PlayerOwner = runner.LocalPlayer.PlayerId
            });

            PhotonRunHandler.Instance.SendUnitEntitySpawnRPC(sendData);
        }
    }
}