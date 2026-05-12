using UnityEngine;
using TMPro;

public class InventoryClearZone : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject upgradePromptUI;
    [SerializeField] private TMP_Text upgradeText;

    private bool playerInRange = false;

    private void Start()
    {
        if (upgradePromptUI != null)
            upgradePromptUI.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            EmptyInventory();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        ShowUpgradeUI();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (upgradePromptUI != null)
            upgradePromptUI.SetActive(false);
    }

    private void EmptyInventory()
    {
        if (InventoryManager.Instance == null)
        {
            UpdateText("inventoryManager not found.");
            return;
        }

        InventoryManager.Instance.ClearInventory();
        UpdateText("inventory cleared!");
    }

    private void ShowUpgradeUI()
    {
        if (upgradePromptUI != null)
            upgradePromptUI.SetActive(true);

        UpdateText("press e to\nclear inventory.");
    }

    private void UpdateText(string message)
    {
        if (upgradeText != null)
            upgradeText.text = message;
    }
}