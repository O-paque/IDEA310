using UnityEngine;

public class RayCastInteractor : MonoBehaviour
{
    public Camera playerCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit; 
            if (Physics.Raycast(transform.position, playerCamera.transform.forward, out hit, 100f))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    rigidbody.AddForce(playerCamera.transform.forward * 1000f);
                }
            }
        }
    }
}
