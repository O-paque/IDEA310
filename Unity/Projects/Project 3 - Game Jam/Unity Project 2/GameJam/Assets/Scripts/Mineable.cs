using UnityEngine;

public class Mineable : MonoBehaviour
{
    public int health = 3;

    public void Mine(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GetComponent<ResourceDrop>()?.Drop();
            Destroy(gameObject);
        }
    }
}