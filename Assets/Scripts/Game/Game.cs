using UnityEngine;
using UnityEngine.Networking;

public class Game : NetworkBehaviour
{
    public const int NUM_BALLS = 4;
    public const int NUM_PLAYERS = 2;

    public void BeginPlay()
    {
        isFighting = false;
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

    public PropertyChangedEvent propertyChangedEvent = new PropertyChangedEvent();

    void Update()
    {
        notifyIsFightingPropChange();
    }

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

    void SendNotification(string propName, object newval, object prevval)
    {
        Debug.Log("Received change notification: " + propName + " new: " + newval + " old: " + prevval);
        propertyChangedEvent.Invoke(propName, newval, prevval);
    }

}
