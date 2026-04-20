using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private HealthText healthText;
    [SerializeField]
    private Image imageComponent;

    private void Awake()
    {
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

    public void RegisterHealthText(HealthText text)
    {
        healthText = text;
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.UpdateHealthText(currentHealth, maxHealth);
        }
    }

    public void showImage()
    {
        imageComponent.gameObject.SetActive(true);
    }
}
