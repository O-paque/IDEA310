using UnityEngine;
using TMPro;
using System.Collections;

public class CollectTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text pizzaText;

    private void Reset()
    {
        pizzaText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        // OnEnable happens reliably when the new scene activates UI
        StartCoroutine(RegisterWhenReady());
    }

    private IEnumerator RegisterWhenReady()
    {
        // Wait until GameManager exists (covers execution order issues)
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.RegisterPizzaUI(this);
    }

    public void SetText(int pizzas)
    {
        if (pizzaText != null)
            pizzaText.text = "Pizzas: " + pizzas;
    }
}
