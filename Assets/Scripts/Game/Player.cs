using UnityEngine.Networking;
using UnityEngine;
using TMPro;


/**
 * Player object.  Spawned by GameNetworkManager, one per connected player.
 **/
public class Player : NetworkBehaviour
{
    public PropertyChangeNotifier propertyChangeNotifier;

    // Properties
    #region id
    [SyncVar]
    private string mId;
    private string mPreviousId;
    public string id
    {
        get
        {
            return mId;
        }
        set
        {
            if (mId != value)
                CmdSetId(value);
        }
    }
    [Command]
    void CmdSetId(string value)
    {
        mId = value;
    }
    void notifyIdPropChange()
    {
        if (mId != mPreviousId)
        {
            SendNotification("id", mId, mPreviousId);
            mPreviousId = mId;
        }
    }
    #endregion 
    #region elo
    [SyncVar]
    int mElo;
    private int mPreviousElo;
    public int elo
    {
        get
        {
            return mElo;
        }
        set
        {
            if (mElo != value)
                CmdSetElo(value);
        }
    }
    [Command]
    void CmdSetElo(int value)
    {
        mElo = value;
    }
    void notifyEloPropChange()
    {
        if (mElo != mPreviousElo)
        {
            SendNotification("elo", mElo, mPreviousElo);
            mPreviousElo = mElo;
        }
    }
    #endregion
    #region inventory
    [SyncVar]
    int mInventory;
    private int mPreviousInventory;
    public int inventory
    {
        get
        {
            return mInventory;
        }
        set
        {
            if (mInventory != value)
                CmdSetInventory(value);
        }
    }
    [Command]
    void CmdSetInventory(int value)
    {
        mInventory = value;
    }
    void notifyInventoryPropChange()
    {
        if (mInventory != mPreviousInventory)
        {
            SendNotification("inventory", mInventory, mPreviousInventory);
            mPreviousInventory = mInventory;
        }
    }
    #endregion
    #region playerName
    [SyncVar]
    private string mPlayerName = "";
    private string mPreviousPlayerName;
    public string playerName
    {
        get
        {
            return mPlayerName;
        }
        set
        {
            if (mPlayerName != value)
                CmdSetPlayerName(value);
        }
    }
    [Command]
    void CmdSetPlayerName(string value)
    {
        mPlayerName = value;
    }
    void notifyPlayerNamePropChange()
    {
        if (mPlayerName != mPreviousPlayerName)
        {
            SendNotification("playerName", mPlayerName, mPreviousPlayerName);
            mPreviousPlayerName = mPlayerName;
        }
    }
    #endregion
    #region score
    [SyncVar]
    private int mScore;
    private int mPreviousScore;
    public int score
    {
        get
        {
            return mScore;
        }
        set
        {
            if (mScore != value)
                CmdSetScore(value);
        }
    }
    [Command]
    void CmdSetScore(int value)
    {
        mScore = value;
    }
    void notifyScorePropChange()
    {
        if (mScore != mPreviousScore)
        {
            SendNotification("score", mScore, mPreviousScore);
            mPreviousScore = mScore;
        }
    }
    #endregion
    #region myTurn
    [SyncVar]
    private bool mMyTurn;
    private bool mPreviousMyTurn;
    public bool myTurn
    {
        get
        {
            return mMyTurn;
        }
        set
        {
            if (mMyTurn != value)
                CmdSetMyTurn(value);
        }
    }
    [Command]
    void CmdSetMyTurn(bool value)
    {
        mMyTurn = value;
    }
    void notifyMyTurnPropChange()
    {
        if (mMyTurn != mPreviousMyTurn)
        {
            SendNotification("myTurn", mMyTurn, mPreviousMyTurn);
            mPreviousMyTurn = mMyTurn;
        }
    }
    #endregion

    void Update()
    {
        #region Property Change Notifications
        notifyIdPropChange();
        notifyEloPropChange();
        notifyInventoryPropChange();
        notifyPlayerNamePropChange();
        notifyScorePropChange();
        notifyMyTurnPropChange();
        #endregion 
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            PlayerProfile profile = FindObjectOfType<PlayerProfile>();

            // send over profile attributes to the other client so
            // they can be displayed.  
            id = profile.id;
            if (profile.elo == 1200)
            {
                elo = Application.isEditor ? 1700 : 1822;
            } else
            {
                elo = profile.elo;
            }
            if (profile.playerName == "")
            {
                playerName = Application.isEditor ? "john" : "sara";
            }
            else
            {
                playerName = profile.playerName;
            }
        }

        // assign position and random rotation
        if (isServer)
        {
            if (isLocalPlayer)
                transform.position += Vector3.left * 2;
            transform.GetComponent<Rigidbody>().angularVelocity = new Vector3(1, 1, .7f) * 2;
        }

        // set up a context specific caption on the player objects
        Transform text = transform.Find("Canvas/Number");
        string caption = "";
        if (isServer)
        {
            if (isLocalPlayer)
                caption = "P1\n(me)";
            else
                caption = "P2";
        } else if (isClient)
        {
            if (isLocalPlayer)
                caption = "P2\n(me)";
            else
                caption = "P1";
        }
        text.GetComponent<TextMeshProUGUI>().text = caption;

        propertyChangeNotifier.propertyChangedEvent.AddListener(prop_changed);
    }

    private void prop_changed(object instance, string prop, object newval, object oldval)
    {
        if (prop == "playerName")
        {
            Transform text = transform.Find("Canvas/Name");
            text.GetComponent<TextMeshProUGUI>().text = "Name:\n" + playerName;
        }
        else if (prop == "elo")
        {
            Transform text = transform.Find("Canvas/ELO");
            text.GetComponent<TextMeshProUGUI>().text = "ELO:\n" + elo;
        }
    }

    void SendNotification(string propName, object newval, object prevval)
    {
        Debug.Log("Received change notification: " + propName + " new: " + newval + " old: " + prevval);
        propertyChangeNotifier.propertyChangedEvent.Invoke(this, propName, newval, prevval);
    }
}
