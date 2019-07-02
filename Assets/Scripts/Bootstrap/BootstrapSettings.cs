using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bootstrap Settings", menuName = "Variables/Bootstrap Settings")]
public class BootstrapSettings : ScriptableObject
{
    public string firstSceneName;
    public List<GameObject> preloadObjects;
}