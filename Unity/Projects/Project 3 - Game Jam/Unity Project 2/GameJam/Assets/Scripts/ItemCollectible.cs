using UnityEngine;

public class ItemCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LevelTransition levelTransition = FindFirstObjectByType<LevelTransition>();

            if (levelTransition != null)
            {
                levelTransition.CollectItem();
            }
            else 
            {
                Debug.LogWarning("No LevelTransition found in the scene to notify about item collection.");
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
