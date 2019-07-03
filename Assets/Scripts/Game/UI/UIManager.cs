using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PropertyChangeNotifier gamePropertyChangeNotifier;
    public GameObject waitingUI;
    public GameObject connectingUI;
    public enum GameState
    {
        CONNECTING,
        WAITING_FOR_MATCH,
        PLAYING
    }
    GameState state = GameState.CONNECTING;

    void Awake()
    {
        gamePropertyChangeNotifier.propertyChangedEvent.AddListener(Handler);
    }

    private void Handler(object dummy, string prop, object newval, object oldval)
    {
        if (prop == "numPlayers")
        {
            NumPlayersChanged((int)newval, (int)oldval);
        }
    }

    private void NumPlayersChanged(int newval, int oldval)
    {
        Debug.Log("NumPlayersChanged... newval: " + newval + ",oldval: " + oldval);
        if (newval == 1)
        {
            if (oldval == 0)
            {
                Debug.Log("setting state to waiting");
                state = GameState.WAITING_FOR_MATCH;
            }
        } else if (newval == 2)
        {
            Debug.Log("setting state to playing");
            state = GameState.PLAYING;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Game>() == null)
        {
            state = GameState.CONNECTING;
        }

        waitingUI.SetActive(state == GameState.WAITING_FOR_MATCH);
        connectingUI.SetActive(state == GameState.CONNECTING);
    }
}
