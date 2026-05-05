using UnityEngine;

public class Mineable : MonoBehaviour
{

    [Header("Sounds")]
    public TriggeredSoundEffect mineSoundEffect;

    public int health = 3;

    public void Mine(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Mineable object destroyed: " + gameObject.name);
            mineSoundEffect?.PlayAndDetach();
            GetComponent<ResourceDrop>()?.Drop();
            Destroy(gameObject);
        }
    }
}