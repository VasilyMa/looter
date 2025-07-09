
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class TutorialState : BattleState
    { 
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

        async void InitSession()
        {

        }
    }
}