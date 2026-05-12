using UnityEngine;
using TMPro;

public class ToolUpgradeArea : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private ToolUpgradeType toolType;
    [SerializeField] private UpgradeLevelCost[] costsByLevel;

    [Header("UI")]
    [SerializeField] private GameObject upgradePromptUI;
    [SerializeField] private TMP_Text upgradeText;

    private bool playerInRange = false;
    private InventoryManager playerInventory;
    private ToolUpgradeManager ToolUpgradeManager;

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
            TryUpgrade();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        playerInventory = other.GetComponent<InventoryManager>();
        ToolUpgradeManager = other.GetComponent<ToolUpgradeManager>();

        ShowUpgradeUI();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        playerInventory = null;
        ToolUpgradeManager = null;

        if (upgradePromptUI != null)
            upgradePromptUI.SetActive(false);
    }

    private void TryUpgrade()
    {
        InventoryManager inventory = InventoryManager.Instance;
        ToolUpgradeManager upgradeManager = ToolUpgradeManager.Instance;

        if (inventory == null)
        {
            UpdateText("inventoryManager not found.");
            return;
        }

        if (upgradeManager == null)
        {
            UpdateText("ToolUpgradeManager not found.");
            return;
        }

        if (upgradeManager.IsMaxLevel(toolType))
        {
            UpdateText($"{toolType} is already max level.");
            return;
        }

        if (!CanAffordUpgrade())
        {
            UpdateText("not enough resources.\n\ncost:\n" + GetCostText());
            return;
        }

        SpendResources();
        upgradeManager.UpgradeTool(toolType);
        FindFirstObjectByType<PlayerTools>()?.ApplyToolVisuals();

        UpdateText($"{toolType} upgraded!\n\ncurrent level: {upgradeManager.GetCurrentLevel(toolType) + 1}");
    }

    private bool CanAffordUpgrade()
    {
        ResourceCost[] currentCosts = GetCurrentUpgradeCosts();

        if (currentCosts == null)
            return false;

        foreach (ResourceCost cost in currentCosts)
        {
            if (!InventoryManager.Instance.HasResource(cost.resourceType, cost.amount))
                return false;
        }

        return true;
    }

    private void SpendResources()
    {
        ResourceCost[] currentCosts = GetCurrentUpgradeCosts();

        foreach (ResourceCost cost in currentCosts)
        {
            InventoryManager.Instance.SpendResource(cost.resourceType, cost.amount);
        }
    }

    private void ShowUpgradeUI()
    {
        if (upgradePromptUI != null)
            upgradePromptUI.SetActive(true);

        if (ToolUpgradeManager != null && ToolUpgradeManager.IsMaxLevel(toolType))
        {
            UpdateText($"{toolType} upgrade\nmax level reached.");
        }
        else
        {
            UpdateText($"{toolType} upgrade\npress e to upgrade\ncost:\n{GetCostText()}");
        }
    }

    private string GetCostText()
    {
        ResourceCost[] currentCosts = GetCurrentUpgradeCosts();

        if (currentCosts == null)
            return "no upgrade available.";

        string text = "";

        foreach (ResourceCost cost in currentCosts)
        {
            text += $"{cost.resourceType}: {cost.amount}\n";
        }

        return text;
    }

    private void UpdateText(string message)
    {
        if (upgradeText != null)
            upgradeText.text = message;
    }

    private ResourceCost[] GetCurrentUpgradeCosts()
    {
        int currentLevel = ToolUpgradeManager.Instance.GetCurrentLevel(toolType);

        if (currentLevel < 0 || currentLevel >= costsByLevel.Length)
            return null;

        return costsByLevel[currentLevel].costs;
    }
}
