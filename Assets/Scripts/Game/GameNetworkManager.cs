using UnityEngine.Networking;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    public void Start()
    {
        Debug.Log("Started Network Manager!!!");
    }

    private void LogPlayers()
    {
        Debug.Log("Logging players");
        foreach (Player p in FindObjectsOfType<Player>())
        {
            Debug.Log("Found player: " + p.id);
            Debug.Log("--name: " + p.name);
            Debug.Log("--elo: " + p.elo);
            Debug.Log("--isClient: " + p.isClient);
            Debug.Log("--isLocalPlayer: " + p.isLocalPlayer);
            Debug.Log("--isServer: " + p.isServer);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);

        Debug.Log("Added player: " + conn.connectionId);
        LogPlayers();

        Game game = FindObjectOfType<Game>();

        if (game != null)
        {
            game.PlayerNumberChanged(NetworkManager.singleton.numPlayers);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        Debug.Log("Server disconnected client: " + conn.connectionId);

        Game game = FindObjectOfType<Game>();

        game.PlayerNumberChanged(NetworkManager.singleton.numPlayers);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        Debug.Log("Client disconnected from server: " + conn.connectionId);
    }
}
