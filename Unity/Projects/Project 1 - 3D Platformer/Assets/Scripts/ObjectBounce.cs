using UnityEngine;

public class ObjectBounce : MonoBehaviour {
    private Vector3 startPos;

    public float speed = 2f;
    public float height = 1f;

    void Start() {
        startPos = transform.position;
    }

    void Update() {
        float newY = Mathf.Cos(Time.time * speed) * height * startPos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
