using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;

    public enum State
    {
        running,
        paused,
        gameOver,
        victory
    }

    void Start()
    {
        Combat.Combatant player = GameObject.FindWithTag("Player").GetComponent<Combat.Combatant>();
        player.onDeath += (string killedBy) =>
        {
            ChangeState(State.gameOver);
            return false;
        };
    }

    public void ChangeState(State state)
    {
        switch(state)
        {
            case State.running:
            throw new System.Exception("Not implemented");
            // break;
            case State.paused:
            throw new System.Exception("Not implemented");
            // break;
            case State.gameOver:
            gameOverScreen.SetActive(true);
            break;
            // break;
            case State.victory:
            victoryScreen.SetActive(true);
            GameObject.FindWithTag("Player").SetActive(false);
            break;
            default:
            throw new System.Exception("Not implemented");
        }
    }
}
