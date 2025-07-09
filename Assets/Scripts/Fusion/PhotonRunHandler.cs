using System.Collections;

using Client;

using Fusion;
using MemoryPack;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class PhotonRunHandler : NetworkBehaviour
{
    private static PhotonRunHandler _instance;
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

    private NetworkRunner runner;


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
        // ����� ��� ���������������� ECS-�������, �����, ������� � �.�.
    }

    

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void SendPlayerSpawnRPC(byte[] recieve)
    {
        NetworkPlayerSpawnEvent spawnEvent = MemoryPackSerializer.Deserialize<NetworkPlayerSpawnEvent>(recieve);
        Debug.Log($"{spawnEvent.Key} new key");
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

        // ������ �������� ���������� �����
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Single);

        // �������� ��������� ��������
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Scene loaded successfully: {handle.Result.Scene.name}");
            // ����� ������������� ������������������� ���-��
        }
        else
        {
            Debug.LogError("Failed to load scene from Addressables: " + scenePath);
        }
    }

}