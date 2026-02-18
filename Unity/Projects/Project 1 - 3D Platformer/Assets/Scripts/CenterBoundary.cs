using UnityEngine;

public class CenterBoundary : MonoBehaviour
{
    [ContextMenu("Center To Children Bounds")]
    void Center()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 newCenter = bounds.center;
        Vector3 offset = newCenter - transform.position;

        foreach (Transform child in transform)
        {
            child.position -= offset;
        }

        transform.position = newCenter;
    }
}
