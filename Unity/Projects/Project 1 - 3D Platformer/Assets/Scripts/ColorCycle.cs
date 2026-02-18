using UnityEngine;
using System.Collections;

public class ColorCycle : MonoBehaviour
{
    private const float MIN_DURATION = 0.1f;
    public Material targetMaterial;
    public Color red = Color.red;
    public Color orange = Color.orange;
    public Color yellow = Color.yellow;
    public Color green = Color.green;
    public Color blue = Color.blue;
    public Color indigo = new Color(0.29f, 0f, 0.51f); // Approximate indigo
    public Color violet = new Color(0.93f, 0.51f, 0.93f); // Approximate violet
    public float duration = 2.0f;
    private Coroutine _routine;

    void OnEnable()
    {
        if (_routine == null)
            _routine = StartCoroutine(CycleColorsLoop());
    }

    void OnDisable()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            _routine = null;
        }
    }

    private IEnumerator CycleColorsLoop()
    {
        // If you're using URP/Lit, "_BaseColor" is correct.
        // If you're on Standard shader, it's usually "_Color".
        int colorId = targetMaterial.HasProperty("_BaseColor")
            ? Shader.PropertyToID("_BaseColor")
            : Shader.PropertyToID("_Color");

        Color[] colors = { red, orange, yellow, green, blue, indigo, violet };

        int i = 0;
        while (true)
        {
            Color a = colors[i];
            Color b = colors[(i + 1) % colors.Length];

            float elapsed = 0f;
            float effectiveDuration = Mathf.Max(duration, MIN_DURATION);
            while (elapsed < effectiveDuration)
            {
                float t = elapsed / effectiveDuration;
                targetMaterial.SetColor(colorId, Color.Lerp(a, b, t));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // snap to exact end color
            targetMaterial.SetColor(colorId, b);

            i = (i + 1) % colors.Length;
        }
    }
}
