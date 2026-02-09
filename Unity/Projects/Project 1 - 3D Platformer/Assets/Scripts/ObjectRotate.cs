using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 1, 0);
    
    void Update() {
        transform.Rotate(rotationAxis * Time.deltaTime * 50);    
    }
}
