using UnityEngine.Networking;
using UnityEngine;
using TMPro;


/**
 * This is an object whose properties can not be assigned on a client, and property
 * changes must all take place on the server.  The client tracks changed data, and
 * if a property changes on the client will send notifications to listeners that
 * data has changed.  This class should be code generated.
 **/
public class Player : NetworkBehaviour
{
    public PropertyChangedEvent propertyChangedEvent = new PropertyChangedEvent();

    void Start()
    {
        if (isLocalPlayer)
        {
            PlayerProfile profile = FindObjectOfType<PlayerProfile>();

            // send over profile attributes to the other client so
            // they can be displayed.  
            id = profile.id;
            elo = profile.elo;
            playerName = profile.playerName;
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

        propertyChangedEvent.AddListener(prop_changed);
    }

    private void prop_changed(string prop, object newval, object oldval)
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

    void Update()
    {
        // need to check for updates for all instances of the Player object
        // on the client or the server/client, not just the local player.
        notifyIdPropChange();
        notifyEloPropChange();
        notifyInventoryPropChange();
        notifyPlayerNamePropChange();
        notifyScorePropChange();
        notifyMyTurnPropChange();

    }

    void SendNotification(string propName, object newval, object prevval)
    {
        Debug.Log("Received change notification: " + propName + " new: " + newval + " old: " + prevval);
        propertyChangedEvent.Invoke(propName, newval, prevval);
    }
}
