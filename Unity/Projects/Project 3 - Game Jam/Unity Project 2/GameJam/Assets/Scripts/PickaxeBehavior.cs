using UnityEngine;
using System.Collections;

public class PickaxeBehavior : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private float returnDuration = 0.15f;
    [SerializeField] private Vector3 swingRotation = new Vector3(-80f, 0f, 0f);

    [Header("Mining Settings")]
    [SerializeField] private int miningDamage = 1;
    [SerializeField] private float miningRange = 2.5f;

    [Header("Sounds")]
    public TriggeredSoundEffect mineSoundEffect;
    public TriggeredSoundEffect swingSoundEffect;

    private Quaternion startLocalRotation;
    private bool isSwinging = false;

    private void Start()
    {
        startLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            swingSoundEffect?.PlaySound();
            StartCoroutine(SwingPickaxe());
        }
    }

    private IEnumerator SwingPickaxe()
    {
        isSwinging = true;

        Quaternion targetRotation = startLocalRotation * Quaternion.Euler(swingRotation);
        float elapsed = 0f;

        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;
            transform.localRotation = Quaternion.Slerp(startLocalRotation, targetRotation, t);
            yield return null;
        }

        MineResource();

        elapsed = 0f;

        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / returnDuration;
            transform.localRotation = Quaternion.Slerp(targetRotation, startLocalRotation, t);
            yield return null;
        }

        transform.localRotation = startLocalRotation;
        isSwinging = false;
    }

    private void MineResource()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, miningRange))
        {
            if (!hit.collider.CompareTag("Resource"))
            {
                return;
            }
            mineSoundEffect?.PlaySound();
            Mineable mineable = hit.collider.GetComponentInParent<Mineable>();

            if (mineable != null)
            {
                mineable.Mine(miningDamage);
            }
        }
    }

    public bool isBusy()
    {
        return isSwinging;
    }
}