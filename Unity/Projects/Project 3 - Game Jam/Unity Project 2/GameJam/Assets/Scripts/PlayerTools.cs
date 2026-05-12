using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    [Header("Sword Level Objects")]
    [SerializeField] private GameObject[] swordLevelObjects;

    [Header("Pickaxe Level Objects")]
    [SerializeField] private GameObject[] pickaxeLevelObjects;

    private void Start()
    {
        ApplyToolVisuals();
    }

    public void ApplyToolVisuals()
    {
        if (ToolUpgradeManager.Instance == null) return;

        int swordLevel = ToolUpgradeManager.Instance.CurrentSwordLevel;
        int pickaxeLevel = ToolUpgradeManager.Instance.CurrentPickaxeLevel;

        SetActiveLevel(swordLevelObjects, swordLevel);
        SetActiveLevel(pickaxeLevelObjects, pickaxeLevel);
    }

    private void SetActiveLevel(GameObject[] levelObjects, int activeLevel)
    {
        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (levelObjects[i] != null)
                levelObjects[i].SetActive(i == activeLevel);
        }
    }
}
