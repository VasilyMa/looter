using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class NetworkSceneManagerNone : MonoBehaviour, INetworkSceneManager
{
    public bool IsBusy => throw new System.NotImplementedException();

    public UnityEngine.SceneManagement.Scene MainRunnerScene => throw new System.NotImplementedException();

    UnityEngine.SceneManagement.Scene INetworkSceneManager.MainRunnerScene => throw new System.NotImplementedException();

    public void Shutdown()
    { 
    }

    public bool IsRunnerScene(UnityEngine.SceneManagement.Scene scene)
    {
        return false;
    }

    public bool TryGetPhysicsScene2D(out PhysicsScene2D scene2D)
    {
        scene2D = default(PhysicsScene2D);

        return true;
    }

    public bool TryGetPhysicsScene3D(out PhysicsScene scene3D)
    {
        scene3D = default(PhysicsScene);

        return true;
    }

    public void MakeDontDestroyOnLoad(GameObject obj)
    { 
    }

    public bool MoveGameObjectToScene(GameObject gameObject, SceneRef sceneRef)
    {
        return true;
    }

    public NetworkSceneAsyncOp LoadScene(SceneRef sceneRef, NetworkLoadSceneParameters parameters)
    {
        return default(NetworkSceneAsyncOp);
    }

    public NetworkSceneAsyncOp UnloadScene(SceneRef sceneRef)
    {
        return default(NetworkSceneAsyncOp);
    }

    public SceneRef GetSceneRef(GameObject gameObject)
    {  
        return default(SceneRef);
    }

    public SceneRef GetSceneRef(string sceneNameOrPath)
    { 
        return default(SceneRef);
    }

    public bool OnSceneInfoChanged(NetworkSceneInfo sceneInfo, NetworkSceneInfoChangeSource changeSource)
    {
        return default(bool);
    }

    public void Initialize(NetworkRunner runner)
    { 
    } 
}