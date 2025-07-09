using UnityEngine;
using Fusion;
using System.Threading.Tasks;
using MemoryPack;

public class PhotonInitializer : MonoBehaviour
{
    public static PhotonInitializer Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private NetworkObject runHandlerPrefab;

    private NetworkRunner runner;
    private PhotonRunHandler currentHandler;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (runHandlerPrefab == null)
        {
            Debug.LogError("[PhotonInitializer] RunHandler prefab is not assigned!");
        }
    }

    public async Task StartSession(SessionParams session)
    {
        if (runner != null && runner.IsRunning)
            await runner.Shutdown();

        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = session.ProvideInput;

        var dummySceneManager = runner.gameObject.AddComponent<NetworkSceneManagerNone>();

        var args = new StartGameArgs
        {
            GameMode = session.Mode,
            SessionName = session.RoomName,
            Scene = SceneRef.None,
            SceneManager = dummySceneManager
        }; 

        var result = await runner.StartGame(args);

        if (result.Ok == false)
        {
            Debug.LogError($"[PhotonInitializer] Failed to start game: {result.ShutdownReason}");
            return;
        }
         
        var obj = runner.Spawn(runHandlerPrefab, Vector3.zero, Quaternion.identity, inputAuthority: runner.LocalPlayer);

        if (obj == null)
        {
            Debug.LogError("[PhotonInitializer] Failed to spawn RunHandler.");
            return;
        }

        currentHandler = obj.GetComponent<PhotonRunHandler>();
        if (currentHandler == null)
        {
            Debug.LogError("[PhotonInitializer] Spawned object does not have PhotonRunHandler.");
            return;
        }

        currentHandler.Init(runner);
         
        if (runner.IsSharedModeMasterClient) 
        { 
            currentHandler.StartGameSceneRPC(MemoryPackSerializer.Serialize<NetworkSessionData>(new NetworkSessionData(session.ScenePath))); 
        }
    }

    public async Task EndSession()
    {
        if (currentHandler != null)
        {
            if (currentHandler.Object != null && currentHandler.Object.IsValid)
                runner.Despawn(currentHandler.Object);

            currentHandler = null;
        }

        if (runner != null)
        {
            await runner.Shutdown();
            Destroy(runner);
            runner = null;
        }
    }
}


public struct SessionParams
{
    public GameMode Mode;
    public string RoomName;
    public string ScenePath; // Вместо SceneIndex
    public bool ProvideInput;
}

[MemoryPackable]
public partial struct NetworkSessionData
{
    public string ScenePath;

    public NetworkSessionData(string scenePath)
    {
        ScenePath = scenePath;
    }
}