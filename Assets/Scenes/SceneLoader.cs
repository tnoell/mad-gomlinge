using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // [SerializeField] private string sceneName;

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
