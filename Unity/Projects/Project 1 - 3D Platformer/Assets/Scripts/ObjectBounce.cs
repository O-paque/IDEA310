using UnityEngine;

public class ObjectBounce : MonoBehaviour {
    private Vector3 startPos;
    public Vector3 bounceDirection = Vector3.up;
    public float speed = 2f;
    public float height = 1f;

    void Start() {
        startPos = transform.position;
    }

    void Update() {
        float offset = Mathf.Sin(Time.time * speed) * height;
        transform.position = startPos + bounceDirection.normalized * offset;
    }
}
