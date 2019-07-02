using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("Play button clicked");
        SceneManager.LoadScene("Game");
    }
}
