using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void LoadScene(string sceneName) {
        if (sceneName.IsNullOrEmpty()) sceneName = SceneManager.GetActiveScene().name;
        else if (sceneName == "random")
        {
            int i = Random.Range(0, 2);
            SceneManager.LoadScene(i);
        }
 
        AkUnitySoundEngine.StopAll();
        LSLController.Instance.LogControl($"Loading scene {sceneName}");
        if (sceneName != "random") SceneManager.LoadScene(sceneName);
    
    }
}