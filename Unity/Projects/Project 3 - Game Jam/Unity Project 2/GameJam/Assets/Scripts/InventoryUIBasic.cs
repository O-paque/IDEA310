using UnityEngine;
using TMPro;
using System.Text;

public class InventoryUIBasic : MonoBehaviour
{
    [SerializeField] private TMP_Text inventoryText;

    private void Update()
    {
        UpdateInventoryDisplay();
    }

    private void UpdateInventoryDisplay()
    {
        if (inventoryText == null) return;
        if (InventoryManager.Instance == null) return;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"inventory: {InventoryManager.Instance.CurrentCapacity}/{InventoryManager.Instance.maxCapacity}");

        foreach (var resource in InventoryManager.Instance.resources)
        {
            sb.AppendLine($"{resource.Key}: {resource.Value}");
        }

        inventoryText.text = sb.ToString();
    }
}
