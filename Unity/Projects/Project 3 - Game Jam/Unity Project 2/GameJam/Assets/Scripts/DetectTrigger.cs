using UnityEngine;

public class DetectTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject triggerObject;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        triggerObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger stayed by: " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited by: " + other.gameObject.name);
        triggerObject.SetActive(false);
    }
}
