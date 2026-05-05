using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    [Header("Footstep Clips")]
    [SerializeField] private AudioClip[] stepClips;

    [Header("Sound Settings")]
    [SerializeField] private float volume = 0.8f;
    [SerializeField] private float pitchMin = 0.9f;
    [SerializeField] private float pitchMax = 1.1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        if (stepClips == null || stepClips.Length == 0) return;

        AudioClip clip = stepClips[Random.Range(0, stepClips.Length)];

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clip, volume);
    }
}
