using UnityEngine;

public class ToolUpgradeManager : MonoBehaviour
{
    public static ToolUpgradeManager Instance { get; private set; }

    public int CurrentSwordLevel { get; private set; } = 0;
    public int CurrentPickaxeLevel { get; private set; } = 0;

    [SerializeField] private int maxSwordLevel = 2;
    [SerializeField] private int maxPickaxeLevel = 2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool UpgradeTool(ToolUpgradeType toolType)
    {
        if (toolType == ToolUpgradeType.Sword)
        {
            if (CurrentSwordLevel >= maxSwordLevel) return false;
            CurrentSwordLevel++;
            return true;
        }

        if (toolType == ToolUpgradeType.Pickaxe)
        {
            if (CurrentPickaxeLevel >= maxPickaxeLevel) return false;
            CurrentPickaxeLevel++;
            return true;
        }

        return false;
    }

    public int GetCurrentLevel(ToolUpgradeType toolType)
    {
        return toolType == ToolUpgradeType.Sword
            ? CurrentSwordLevel
            : CurrentPickaxeLevel;
    }

    public bool IsMaxLevel(ToolUpgradeType toolType)
    {
        return toolType == ToolUpgradeType.Sword
            ? CurrentSwordLevel >= maxSwordLevel
            : CurrentPickaxeLevel >= maxPickaxeLevel;
    }
}