using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    // [SerializeField] private string sceneName;

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeState(GameStateManager.State state)
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameStateManager>().ChangeState(state);
    }

    public void ChangeStateToVictory()
    {
        ChangeState(GameStateManager.State.victory);
    }
}
