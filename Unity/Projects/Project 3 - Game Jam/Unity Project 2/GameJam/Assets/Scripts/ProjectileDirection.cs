using UnityEngine;

public class ProjectileDirection : MonoBehaviour
{
    public Camera playerCamera; 
    public GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projectile, playerCamera.transform.position, playerCamera.transform.rotation);
        }
    }
}
