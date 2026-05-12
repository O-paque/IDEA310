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

    [Header("Tool Holder Objects")]
    public GameObject swordObject;
    public GameObject pickaxeObject;

    [Header("Tool Visuals")]
    [SerializeField] private PlayerTools PlayerTools;

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

    private void Awake()
    {
        if (PlayerTools == null)
            PlayerTools = GetComponent<PlayerTools>();
    }

    private void Start()
    {
        EquipToolInstant(currentTool);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isSwitching && !CurrentToolIsBusy())
        {
            ToggleTool();
        }
    }

    private bool CurrentToolIsBusy()
    {
        if (currentTool == ToolType.Sword && swordObject != null && swordObject.activeInHierarchy)
        {
            SwordBehavior sword = swordObject.GetComponentInChildren<SwordBehavior>();

            if (sword != null)
                return sword.isBusy();
        }

        if (currentTool == ToolType.Pickaxe && pickaxeObject != null && pickaxeObject.activeInHierarchy)
        {
            PickaxeBehavior pickaxe = pickaxeObject.GetComponentInChildren<PickaxeBehavior>();

            if (pickaxe != null)
                return pickaxe.isBusy();
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
            swordSoundEffect?.PlaySound();
        else
            pickaxeSoundEffect?.PlaySound();

        GameObject currentObject = currentTool == ToolType.Sword ? swordObject : pickaxeObject;
        GameObject nextObject = nextTool == ToolType.Sword ? swordObject : pickaxeObject;

        if (currentObject == null || nextObject == null)
        {
            isSwitching = false;
            yield break;
        }

        yield return MoveTool(currentObject.transform, currentObject.transform.localPosition, hiddenLocalPosition);

        currentObject.SetActive(false);

        nextObject.SetActive(true);
        nextObject.transform.localPosition = hiddenLocalPosition;

        currentTool = nextTool;
        UpdateToolIcon();

        if (PlayerTools != null)
            PlayerTools.ApplyToolVisuals();

        yield return MoveTool(nextObject.transform, hiddenLocalPosition, equippedLocalPosition);

        if (PlayerTools != null)
            PlayerTools.ApplyToolVisuals();

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

        if (swordObject != null)
        {
            swordObject.SetActive(tool == ToolType.Sword);
            swordObject.transform.localPosition =
                tool == ToolType.Sword ? equippedLocalPosition : hiddenLocalPosition;
        }

        if (pickaxeObject != null)
        {
            pickaxeObject.SetActive(tool == ToolType.Pickaxe);
            pickaxeObject.transform.localPosition =
                tool == ToolType.Pickaxe ? equippedLocalPosition : hiddenLocalPosition;
        }

        UpdateToolIcon();

        if (PlayerTools != null)
            PlayerTools.ApplyToolVisuals();
    }

    private void UpdateToolIcon()
    {
        if (currentToolIcon == null) return;

        currentToolIcon.sprite = currentTool == ToolType.Sword
            ? swordIcon
            : pickaxeIcon;
    }
}