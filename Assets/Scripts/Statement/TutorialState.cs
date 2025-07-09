using Client;
using System;
using UnityEngine;

namespace Statement
{
    public class TutorialState : BattleState
    {
        public EntityBase PlayerEntityBase;
        [SerializeField] private string gameSceneName = "GameScene";

        public override void Awake()
        {
            EcsHandler = new TutorEcsHandler();
        }

        public override void Start()
        {
            base.Start();

            Debug.Log("[TutorialState] Starting singleplayer session..."); 
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                string networkKey = Guid.NewGuid().ToString();

                byte[] sendData = MemoryPack.MemoryPackSerializer.Serialize<NetworkUnitEntitySpawnEvent>(new NetworkUnitEntitySpawnEvent()
                {
                    EntityKey = networkKey,
                    SpawnKeyID = PlayerEntityBase.KEY_ID,
                });

                PhotonRunHandler.Instance.SendUnitEntitySpawnRPC(sendData);
            }
        }
    }
}