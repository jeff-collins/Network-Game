using System;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public int defaultElo;

    public int elo;
    public string playerName;
    public string id;

    void Awake()
    {
        id = Guid.NewGuid().ToString();
        elo = defaultElo;

        if (Application.isEditor)
        {
            elo = 1700;
            playerName = "john";
        } else
        {
            elo = 1833;
            playerName = "sara";
        }
        // add myself to the persistence system - values will be overwritten if read
        FindObjectOfType<PersistenceSystem>().AddPersistentObject("profile", this);
    }
}