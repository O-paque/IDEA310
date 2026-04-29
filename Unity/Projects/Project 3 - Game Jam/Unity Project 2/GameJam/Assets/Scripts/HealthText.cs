using UnityEngine;
using TMPro;
using System.Collections;

public class HealthText : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text healthText;

    private void Reset()
    {
        healthText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(RegisterWhenReady());
    }

    private IEnumerator RegisterWhenReady()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }
        GameManager.Instance.RegisterHealthText(this);
    }
    
    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = $"Health: {currentHealth}/{maxHealth}";
    }
}
