using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ToolType
{
    Sword,
    Pickaxe
}


public class ToolSwitcher : MonoBehaviour
{
    [Header("Current Tool")]
    public ToolType currentTool = ToolType.Sword;

    [Header("Tool Objects")]
    public GameObject swordObject;
    public GameObject pickaxeObject;

    [Header("Tool Behaviors")]
    public SwordBehavior swordBehavior;
    public PickaxeBehavior pickaxeBehavior;

    [Header("Tool Positions")]
    public Vector3 equippedLocalPosition;
    public Vector3 hiddenLocalPosition = new Vector3(0f, -1.0f, 0f);

    [Header("Switch Settings")]
    public float switchDuration = 0.15f;
    private bool isSwitching = false;

    [Header("UI")]
    public Image currentToolIcon;
    public Sprite swordIcon;
    public Sprite pickaxeIcon;

    [Header("Sounds")]
    public TriggeredSoundEffect pickaxeSoundEffect;
    public TriggeredSoundEffect swordSoundEffect;

    private void Start()
    {
        EquipToolInstant(currentTool);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isSwitching && !currentToolIsBusy())
        {
            ToggleTool();
        }
    }

    private bool currentToolIsBusy()
    {
        if (currentTool == ToolType.Sword && swordBehavior != null)
        {
            return swordBehavior.isBusy();
        }

        if (currentTool == ToolType.Pickaxe && pickaxeBehavior != null)
        {
            return pickaxeBehavior.isBusy();
        }

        return false;
    }

    private void ToggleTool()
    {
        ToolType nextTool = currentTool == ToolType.Sword
            ? ToolType.Pickaxe
            : ToolType.Sword;

        StartCoroutine(SwitchToolSmooth(nextTool));
    }

    private IEnumerator SwitchToolSmooth(ToolType nextTool)
    {
        isSwitching = true;

        if (nextTool == ToolType.Sword)
        {
            swordSoundEffect?.PlaySound();
        }
        else
        {
            pickaxeSoundEffect?.PlaySound();
        }

        GameObject currentObject = currentTool == ToolType.Sword ? swordObject : pickaxeObject;
        GameObject nextObject = nextTool == ToolType.Sword ? swordObject : pickaxeObject;

        Transform currentTransform = currentObject.transform;
        Transform nextTransform = nextObject.transform;

        yield return MoveTool(currentTransform, equippedLocalPosition, hiddenLocalPosition);

        currentObject.SetActive(false);

        nextObject.SetActive(true);
        nextTransform.localPosition = hiddenLocalPosition;

        currentTool = nextTool;
        UpdateToolIcon();

        yield return MoveTool(nextTransform, hiddenLocalPosition, equippedLocalPosition);

        isSwitching = false;
    }

    private IEnumerator MoveTool(Transform tool, Vector3 start, Vector3 end)
    {
        float elapsed = 0f;

        while (elapsed < switchDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / switchDuration;

            tool.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }

        tool.localPosition = end;
    }

    private void EquipToolInstant(ToolType tool)
    {
        currentTool = tool;

        swordObject.SetActive(tool == ToolType.Sword);
        pickaxeObject.SetActive(tool == ToolType.Pickaxe);

        swordObject.transform.localPosition =
            tool == ToolType.Sword ? equippedLocalPosition : hiddenLocalPosition;

        pickaxeObject.transform.localPosition =
            tool == ToolType.Pickaxe ? equippedLocalPosition : hiddenLocalPosition;

        UpdateToolIcon();
    }

    private void UpdateToolIcon()
    {
        if (currentToolIcon == null) return;

        currentToolIcon.sprite = currentTool == ToolType.Sword
            ? swordIcon
            : pickaxeIcon;
    }
}