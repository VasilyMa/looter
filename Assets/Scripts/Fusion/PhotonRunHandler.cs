using System.Collections;

using Client;

using Fusion;
using MemoryPack;
using Statement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class PhotonRunHandler : NetworkBehaviour
{
    private static PhotonRunHandler _instance;
    private NetworkRunner runner;

    public static PhotonRunHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PhotonRunHandler>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(PhotonRunHandler).Name);
                    _instance = obj.AddComponent<PhotonRunHandler>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }


    protected void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Init(NetworkRunner runner)
    {
        this.runner = runner;

        Debug.Log("PhotonRunHandler Initialized");
        // Можно тут инициализировать ECS-систему, сцены, префабы и т.п.
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void SendUnitEntitySpawnRPC(byte[] recieve)
    {
        NetworkUnitEntitySpawnEvent networkSpawnEvent = MemoryPackSerializer.Deserialize<NetworkUnitEntitySpawnEvent>(recieve);
         
        BattleState.Instance.SendRequest(networkSpawnEvent);
        Debug.Log($"{networkSpawnEvent.SpawnKeyID} spawn entity player");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void StartGameSceneRPC(byte[] recieve)
    {
        NetworkSessionData sessionData = MemoryPackSerializer.Deserialize<NetworkSessionData>(recieve);

        Debug.Log($"StartGameSceneRPC received: {sessionData.ScenePath}");

        StartCoroutine(LoadSceneAsync(sessionData.ScenePath));
    } 
    private IEnumerator LoadSceneAsync(string scenePath)
    {
        var ecsConfig = ConfigModule.GetConfig<EcsSetupConfig>();
        ecsConfig.InitMainEcsHandler();

        // Запуск загрузки адресуемой сцены
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Single);

        // Ожидание окончания загрузки
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Scene loaded successfully: {handle.Result.Scene.name}");
            // Можно дополнительно проинициализировать что-то
        }
        else
        {
            Debug.LogError("Failed to load scene from Addressables: " + scenePath);
        }
    }
}