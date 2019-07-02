using UnityEngine.Networking;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    public IntVariable playerCount;
    public GameMatchMaker gameMatchMaker;

    public void Start()
    {
        playerCount.ResetValue();
        Debug.Log("Started Network Manager!!!");
    }

    private void LogPlayers()
    {
        //Debug.Log("Logging players");
        //foreach(Player p in FindObjectsOfType<Player>())
        //{
        //    Debug.Log("Found player: " + p.playerControllerId);
        //    Debug.Log("  name: " + p.name);
        //    Debug.Log("  elo: " + p.elo);
        //    Debug.Log("  isClient: " + p.isClient);
        //    Debug.Log("  isLocalPlayer: " + p.isLocalPlayer);
        //    Debug.Log("  isServer: " + p.isServer);
        //}
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);

        Debug.Log("Added player: " + playerControllerId);
        LogPlayers();

        playerCount.value = NetworkManager.singleton.numPlayers;

        if (NetworkManager.singleton.numPlayers == Game.NUM_PLAYERS)
        {
            Debug.Log("2 players -- Beginning play");
            FindObjectOfType<Game>().BeginPlay();
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        Debug.Log("Server disconnected: " + conn);
        LogPlayers();

        playerCount.value = NetworkManager.singleton.numPlayers;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        Debug.Log("Client disconnected from server: " + conn);
        LogPlayers();

        playerCount.value = NetworkManager.singleton.numPlayers;
    }
}
