using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System;
using UnityEngine.SceneManagement;

public class GameMatchMaker : MonoBehaviour
{
    public Game gamePrefab;

    const int MAX_RETRIES = 3;

    PlayerProfile playerProfile;
    MatchInfo currentMatchInfo;
    Game currentGame;

    int currentTries;

    // if true will create a match name unique to this machine so that only clients from the same
    // machine can join
    static bool TEST_MODE = true;

    // Start is called before the first frame update
    void Start()
    {
        playerProfile = FindObjectOfType<PlayerProfile>();

        Debug.Log("Starting MatchMaker");
        NetworkManager.singleton.StartMatchMaker();
        FindInternetMatch();
    }

    // Create a new match, success/failure is in callback -- OnInternetMatchCreate
    public void CreateInternetMatch()
    {
        string matchName = TEST_MODE ? SystemInfo.deviceUniqueIdentifier : Guid.NewGuid().ToString();

        Debug.Log("creating match with name " + matchName);
        NetworkManager.singleton.matchMaker.CreateMatch(matchName, /*max players*/ 2, /* advertise */true, /* pwd */ "",
            /* public client addr */ "", /* private client addr */ "",
            playerProfile.elo, /* domain */0, OnInternetMatchCreate);
    }

    // Start host after creating match
    public void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo info)
    {
        if (success)
        {
            currentMatchInfo = info;
            NetworkServer.Listen(currentMatchInfo, 9000);
            NetworkManager.singleton.StartHost(currentMatchInfo);

            SetupGame();

            Debug.Log("Match successfully created");
        }
        else
        {
            Debug.LogError("Create match failed, trying again ...");
            FindInternetMatch();
        }
    }

    void SetupGame()
    {
        // only happens on server
        currentGame = Instantiate(gamePrefab);
        NetworkServer.Spawn(currentGame.gameObject);
        currentGame.numPlayers = 1;
    }

    // Attempt to join the match -- success/failure is in callback -- OnJoinInternetMatch
    public void AttemptJoinMatch(MatchInfoSnapshot match)
    {
        NetworkManager.singleton.matchMaker.JoinMatch(match.networkId, /* pwd */ "", /* public client addr */"",
            /* private client addr */ "", playerProfile.elo, /* domain */ 0, OnJoinInternetMatch);
    }

    // Tries to join an open match, starting with match[0]
    // On success, send user to waiting room
    // On failure, try to join any other open match
    public void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo info)
    {
        if (success)
        {
            currentMatchInfo = info;
            NetworkManager.singleton.StartClient(currentMatchInfo);

            Debug.Log("Join match succeeded");
        }
        else
        {
            Debug.Log("Join match failed - trying again");
            FindInternetMatch();
        }
    }

    // List all active matches
    public void FindInternetMatch()
    {
        currentTries++;
        if (currentTries == MAX_RETRIES + 1)
        {
            Debug.Log("Max retries exceeded...");
            SceneManager.LoadScene("Menu");
            return;
        }

        NetworkManager.singleton.matchMaker.ListMatches(0, 10, /* match name filter */ "", /* no private */ true,
            playerProfile.elo, /* domain */ 0, OnInternetMatchList);
    }

    // Using the list of matches (if any exist), either create or join a match
    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            foreach (MatchInfoSnapshot m in matches)
            {
                if (m.currentSize == 1)
                {
                    Debug.Log("attempting to join " + m);
                    AttemptJoinMatch(m);
                    return;
                }
                else
                {
                    Debug.Log("match " + m + " has players: " + m.currentSize);
                }
            }
            Debug.Log("No valid matches exist-- Creating new match");
            CreateInternetMatch();
        }
        else
        {
            Debug.LogError("Couldn't connect to match maker");
            FindInternetMatch();
        }
    }

    public void EndGame()
    {
        Debug.Log("GameMatchMaker.EndGame");
        if (currentMatchInfo != null)
        {
            Debug.Log("destroying game");
            NetworkManager.singleton.matchMaker.DropConnection(currentMatchInfo.networkId, currentMatchInfo.nodeId, /* domain */0, OnMatchDestroy);
            if (currentGame != null)
            {
                NetworkManager.singleton.matchMaker.DestroyMatch(currentMatchInfo.networkId, /* domain */0, OnMatchDestroy);
            }
            currentMatchInfo = null;
        } else
        {
            Debug.Log("no current game to destroy");
        }
    }

    void OnApplicationQuit()
    {
        EndGame();
    }

    private void OnMatchDestroy(bool success, string extendedInfo)
    {
        Debug.Log("match destroyed");
    }
}
