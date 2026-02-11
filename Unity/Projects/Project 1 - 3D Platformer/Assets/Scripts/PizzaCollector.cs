using UnityEngine;
using TMPro;

public class PizzaCollector : MonoBehaviour
{   
    public int value = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindFirstObjectByType<GameManager>().CollectPizza(1); // Increment pizza count in GameManager
            Destroy(gameObject);
        }
    }
}
