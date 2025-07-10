using Fusion;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class LobbyState : State
    {
        public static new LobbyState Instance
        {
            get
            {
                return (LobbyState)State.Instance;
            }
        }
        public override void FixedUpdate()
        { 
        }

        public override void Start()
        { 
        }

        public override void Update()
        { 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //LoadSceneAsync("TutorialScene");
                //TutorialStart(); 
            }
        }

        public async void StartMatchmaking()
        {
            var session = new SessionParams
            {
                Mode = GameMode.AutoHostOrClient,
                RoomName = $"Match_{Random.Range(0, 1000)}",
                SceneBuildIndex = 5,
                ProvideInput = true 
            };

            await PhotonInitializer.Instance.StartSession(session);
        }

        async void TutorialStart()
        { 
            var session = new SessionParams
            {
                Mode = GameMode.Single,
                RoomName = "TutorialRoom",
                SceneBuildIndex = 4, 
                ProvideInput = true 
            };

            await PhotonInitializer.Instance.StartSession(session);
        }
    }
}