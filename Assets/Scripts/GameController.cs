using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public bool IsRandom;
    private string _oldSceneName;
    public int NumberOfScenes;
    
    public void LoadScene(string sceneName) {
        if (sceneName.IsNullOrEmpty() && IsRandom == false) sceneName = SceneManager.GetActiveScene().name;
        else if (sceneName == "random" || IsRandom)
        {
            if (sceneName != "random") _oldSceneName = sceneName;
            sceneName = "random";
            int i = Random.Range(0, NumberOfScenes);
            if (_oldSceneName.IsNotNullOrEmpty())
            {
                LSLController.Instance.LogControl($"GameController is set to load a random scene on restart. " +
                                                  $" if you are looking to load {_oldSceneName} make sure to uncheck that box!");
            }
            SceneManager.LoadScene(i);
        }
 
        AkUnitySoundEngine.StopAll();
        if (sceneName != "random" || !IsRandom) SceneManager.LoadScene(sceneName);
        LSLController.Instance.LogControl($"Loading scene {sceneName}");
    
    }
}