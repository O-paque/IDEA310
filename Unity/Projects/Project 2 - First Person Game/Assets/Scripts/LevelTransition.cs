using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextSceneName;

    private bool itemCollected = false;

    public GameObject player;
    
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && itemCollected)
        {
            player.SetActive(false);
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void CollectItem()
    {
        itemCollected = true;
        gameManager.showImage();
    }
}
