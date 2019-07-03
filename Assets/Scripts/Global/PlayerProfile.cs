using System;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    [NonSerialized]
    public int defaultElo;

    public int elo;
    public string playerName;
    public string id;

    void Awake()
    {
        // Do this before the Start cycle
        id = Guid.NewGuid().ToString();
        elo = defaultElo;

        // add myself to the persistence system - values will be overwritten if read
        FindObjectOfType<PersistenceSystem>().AddPersistentObject("profile", this);
    }
}