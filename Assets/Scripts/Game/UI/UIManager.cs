using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public IntVariable numPlayers;
    public GameObject waitingUI;

    void Awake()
    {
        numPlayers.onValueChange.AddListener(NumPlayersChanged);
    }

    private void NumPlayersChanged(int newval, int oldval)
    {
        Debug.Log("NumPlayersChanged... newval: " + newval + ",oldval: " + oldval);
        if (newval == 1)
        {
            waitingUI.SetActive(true);
        } else if (newval == 2)
        {
            waitingUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
