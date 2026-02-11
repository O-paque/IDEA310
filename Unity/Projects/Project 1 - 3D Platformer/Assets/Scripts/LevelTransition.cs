using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{

    public string nextLevel;
    public CharacterController player;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            int pizzaCount = gameManager.getPizzaCount();
            int pizzasToCollect = gameManager.getPizzasToCollect();
            if (pizzaCount >= pizzasToCollect)
            {
                player.gameObject.SetActive(false); 
                SceneManager.LoadScene(nextLevel);
            }
        }
    }
}
