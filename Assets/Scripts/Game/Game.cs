using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/**
 * Game object - only one per Game.  Sync'd to both player devices.
 */
public class Game : NetworkBehaviour
{
    public const int NUM_BALLS = 4;
    public const int NUM_PLAYERS = 2;
    public const int ROUND_OVER_DELAY = 3;
    public const int GAME_OVER_DELAY = 3;

    // properties
    #region numPlayers
    [SyncVar]
    private int mNumPlayers;
    private int mPreviousNumPlayers;
    public int numPlayers
    {
        get
        {
            return mNumPlayers;
        }
        set
        {
            if (mNumPlayers != value)
                CmdSetNumPlayers(value);
        }
    }
    [Command]
    void CmdSetNumPlayers(int value)
    {
        mNumPlayers = value;
    }
    void notifyNumPlayersPropChange()
    {
        if (mNumPlayers != mPreviousNumPlayers)
        {
            SendNotification("numPlayers", mNumPlayers, mPreviousNumPlayers);
            mPreviousNumPlayers = mNumPlayers;
        }
    }
    #endregion
    #region IsFighting
    [SyncVar]
    private bool mIsFighting;
    private bool mPreviousIsFighting;
    public bool isFighting
    {
        get
        {
            return mIsFighting;
        }
        set
        {
            if (mIsFighting != value)
                CmdSetIsFighting(value);
        }
    }
    [Command]
    void CmdSetIsFighting(bool value)
    {
        mIsFighting = value;
    }
    void notifyIsFightingPropChange()
    {
        if (mIsFighting != mPreviousIsFighting)
        {
            SendNotification("isFighting", mIsFighting, mPreviousIsFighting);
            mPreviousIsFighting = mIsFighting;
        }
    }
    #endregion
    #region IsGameActive
    [SyncVar]
    private bool mIsGameActive;
    private bool mPreviousIsGameActive;
    public bool isGameActive
    {
        get
        {
            return mIsGameActive;
        }
        set
        {
            if (mIsGameActive != value)
                CmdSetIsGameActive(value);
        }
    }
    [Command]
    void CmdSetIsGameActive(bool value)
    {
        mIsGameActive = value;
    }
    void notifyIsGameActivePropChange()
    {
        if (mIsGameActive != mPreviousIsGameActive)
        {
            SendNotification("isGameActive", mIsGameActive, mPreviousIsGameActive);
            mPreviousIsGameActive = mIsGameActive;
        }
    }
    #endregion
    #region isRoundOver
    [SyncVar]
    private bool mIsRoundOver;
    private bool mPreviousIsRoundOver;
    public bool isRoundOver
    {
        get
        {
            return mIsRoundOver;
        }
        set
        {
            if (mIsRoundOver != value)
                CmdSetIsRoundOver(value);
        }
    }
    [Command]
    void CmdSetIsRoundOver(bool value)
    {
        mIsRoundOver = value;
    }
    void notifyIsRoundOverPropChange()
    {
        if (mIsRoundOver != mPreviousIsRoundOver)
        {
            SendNotification("isRoundOver", mIsRoundOver, mPreviousIsRoundOver);
            mPreviousIsRoundOver = mIsRoundOver;
        }
    }
    #endregion
    #region isGameOver
    [SyncVar]
    private bool mIsGameOver;
    private bool mPreviousIsGameOver;
    public bool isGameOver
    {
        get
        {
            return mIsGameOver;
        }
        set
        {
            if (mIsGameOver != value)
                CmdSetIsGameOver(value);
        }
    }
    [Command]
    void CmdSetIsGameOver(bool value)
    {
        mIsGameOver = value;
    }
    void notifyIsGameOverPropChange()
    {
        if (mIsGameOver != mPreviousIsGameOver)
        {
            SendNotification("isGameOver", mIsGameOver, mPreviousIsGameOver);
            mPreviousIsGameOver = mIsGameOver;
        }
    }
    #endregion

    void SendNotification(string propName, object newval, object prevval)
    {
        Debug.Log("Received change notification: " + propName + " new: " + newval + " old: " + prevval);
        propertyChangeNotifier.propertyChangedEvent.Invoke(this, propName, newval, prevval);
    }

    private void Start()
    {
        if (mNumPlayers == 0)
        {
            numPlayers = NetworkManager.singleton.numPlayers;
        }
    }

    void Update()
    {
        #region Property Change Notifications
        notifyNumPlayersPropChange();
        notifyIsGameActivePropChange();
        notifyIsFightingPropChange();
        notifyIsRoundOverPropChange();
        notifyIsGameOverPropChange();
        #endregion 
    }

    public void BeginPlay()
    {
        isGameActive = true;
        isFighting = false;
        isGameOver = false;
        isRoundOver = false;

        NewRound();
    }

    void NewRound()
    {
        Debug.Log("Starting new round");
        foreach(Player player in FindObjectsOfType<Player>())
        {
            Debug.Log("Setting inventory to " + NUM_BALLS);
            player.inventory = NUM_BALLS;
        }
    }

    public void PlayerNumberChanged(int newNumPlayers)
    {
        Debug.Log("Game.PlayerNumberChanged new=: " + newNumPlayers);
        Debug.Log("Game.PlayerNumberChanged previous=: " + numPlayers);

        if (newNumPlayers == NUM_PLAYERS)
        {
            Debug.Log("2 players -- Beginning play");
            BeginPlay();
        } else if (newNumPlayers == 1 && numPlayers == 2)
        {
            EndGame();
        }
        numPlayers = newNumPlayers;
    }

    void EndRound()
    {
        Debug.Log("Ending round");
        Timeout.SetTimeout(this, ()=>NewRound(), ROUND_OVER_DELAY);
    }

    void EndGame()
    {
        Debug.Log("Ending game");
        FindObjectOfType<GameMatchMaker>().EndGame();
        SceneManager.LoadScene("Menu");
    }

    public PropertyChangeNotifier propertyChangeNotifier;
}
