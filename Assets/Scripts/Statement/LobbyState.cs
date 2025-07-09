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
                TutorialStart(); 
            }
        }
        async void TutorialStart()
        { 

            var session = new SessionParams
            {
                Mode = GameMode.Single,
                RoomName = "TutorialRoom",
                ScenePath = "TutorialScene" // םמגמו ןמכו, גלוסעמ SceneIndex
            };

            await PhotonInitializer.Instance.StartSession(session);
        }
    }
}