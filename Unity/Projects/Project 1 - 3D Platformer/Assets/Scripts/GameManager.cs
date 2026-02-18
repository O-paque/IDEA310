using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentPizzas = 0;
    private CollectTextUI collectTextUI;

    private void Awake()
    {
        // Singleton logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void CollectPizza(int pizzaValue)
    {
        currentPizzas += pizzaValue;
        UpdateUI();
    }

    public void RegisterPizzaUI(CollectTextUI ui)
    {
        collectTextUI = ui;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (collectTextUI != null)
        {
            collectTextUI.SetText(currentPizzas);
        }
    }

    public int getPizzaCount()
    {
        return currentPizzas;
    }
}
