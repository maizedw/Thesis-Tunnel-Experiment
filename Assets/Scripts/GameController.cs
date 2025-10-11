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
 
        AkUnitySoundEngine.StopAll();
        LSLController.Instance.LogControl($"Loading scene {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}