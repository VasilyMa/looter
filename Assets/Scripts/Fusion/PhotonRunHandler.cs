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
        // ����� ��� ���������������� ECS-�������, �����, ������� � �.�.
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void SendUnitEntitySpawnRPC(byte[] recieve)
    {
        NetworkUnitEntitySpawnEvent networkSpawnEvent = MemoryPackSerializer.Deserialize<NetworkUnitEntitySpawnEvent>(recieve);
        Debug.Log($"Spawn entity: {networkSpawnEvent.SpawnKeyID}");
        BattleState.Instance.SendRequest(networkSpawnEvent);
    }

    [Rpc(RpcSources.Proxies, RpcTargets.StateAuthority)]
    public void SendRequestDamageEffectRPC(byte[] recieve)
    {
        NetworkDamageEffectEvent networkDamageEffectEvent = MemoryPackSerializer.Deserialize<NetworkDamageEffectEvent>(recieve);
        Debug.Log($"Damage from {networkDamageEffectEvent.EntityKeySource} to {networkDamageEffectEvent.EntityKeyTarget} is {networkDamageEffectEvent.Value}");
        BattleState.Instance.SendRequest(networkDamageEffectEvent);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    public void SendRequestHealthUpdateRPC(byte[] recieve)
    {
        NetworkHealthUpdateEvent networkHealthUpdateEvent = MemoryPackSerializer.Deserialize<NetworkHealthUpdateEvent>(recieve);
        Debug.Log($"Update health of entity {networkHealthUpdateEvent.EntityKey} to {networkHealthUpdateEvent}");
        BattleState.Instance.SendRequest(networkHealthUpdateEvent);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void SendRequestTransformRPC(byte[] recieve)
    {
        NetworkTransformEvent networkTransformEvent = MemoryPackSerializer.Deserialize<NetworkTransformEvent>(recieve);
        Debug.Log($"Update transform of entity {networkTransformEvent.EntityKey}");
        BattleState.Instance.SendRequest(networkTransformEvent);
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