using UnityEngine;

public class PersistGameObject : MonoBehaviour
{
    public static PersistGameObject Instance;
    
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
}
