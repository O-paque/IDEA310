using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int currentPizzas = 0;
    public int pizzasToCollect;
    public TMP_Text pizzaText;
    
    public void CollectPizza(int pizzaValue)
    {
        currentPizzas += pizzaValue;
        pizzaText.text = "Pizzas: " + currentPizzas;
    }

    public int getPizzaCount()
    {
        return currentPizzas;
    }

    public int getPizzasToCollect()
    {
        return pizzasToCollect;
    }
}
