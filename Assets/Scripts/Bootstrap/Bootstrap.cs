using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    /**
     * Note to use this object in your project, you must add one object of type BootstrapSettings
     * into a Resources folder somewhere in your project.  The resource must also be named
     * "BootstrapSettings".
     **/
    const string BOOTSTRAP_SETTINGS = "BootstrapSettings";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnRuntimeInitializedBeforeSceneLoad()
    {
        BootstrapSettings bootstrapSettings = Resources.Load<BootstrapSettings>(BOOTSTRAP_SETTINGS);

        if (bootstrapSettings == null)
        {
            Debug.LogError("Bootstrap Settings can't be found in any Resources folder");
            return;
        }

        Debug.Log("Bootstrap.OnRuntimeInitializedBeforeSceneLoad");
        Debug.Log("Cloning preload global objects");
        foreach(GameObject o in bootstrapSettings.preloadObjects)
        {
            DontDestroyOnLoad(Instantiate(o));
        }
        SceneManager.LoadScene(bootstrapSettings.firstSceneName);
    }
}

