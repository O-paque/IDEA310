using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    [Tooltip("Gravity acceleration toward the planet center.")]
    public float gravity = 15f;

    [Tooltip("How quickly the player rotates to align with the planet.")]
    public float alignSpeed = 8f;
}
