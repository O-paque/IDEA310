using UnityEngine;
using TMPro;

public class PizzaCollector : MonoBehaviour
{
    public TMP_Text pizzaText;
    private int pizzaCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pizza")
        {
            Debug.Log("Pizza collected!");
            pizzaCount++;
            pizzaText.text = "Pizzas: " + pizzaCount; 
            Destroy(other.gameObject);
        }
    }
}
