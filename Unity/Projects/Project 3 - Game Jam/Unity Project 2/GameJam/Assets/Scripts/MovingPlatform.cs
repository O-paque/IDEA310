using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Assign these points in the Inspector
    [SerializeField] private Transform[] _waypoints; 
    [SerializeField] private float _speed = 2f;

    private int _targetIndex = 0;

    void FixedUpdate()
    {
        if (_waypoints == null || _waypoints.Length == 0) return;

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_targetIndex].position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _waypoints[_targetIndex].position) < 0.1f)
        {
            _targetIndex = (_targetIndex + 1) % _waypoints.Length;
        }
    }
}
