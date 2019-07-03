using UnityEngine;
using UnityEngine.Networking;

public class GameLoader : MonoBehaviour
{
    // prefabs to clone
    public GameNetworkManager gameNetworkManager;
    public GameMatchMaker gameMatchMaker;

    void Awake()
    {
        Debug.Log("Instantiating network manager and match maker");
        Instantiate(gameNetworkManager);
        Instantiate(gameMatchMaker);
    }
}
