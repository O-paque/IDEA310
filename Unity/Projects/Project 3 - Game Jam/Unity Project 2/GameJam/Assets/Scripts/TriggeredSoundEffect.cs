using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TriggeredSoundEffect : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volume = 1f;

    [Header("Timing")]
    [SerializeField] private float delay = 0f;

    [Header("Mode")]
    [SerializeField] private bool loop = false;

    private AudioSource audioSource;
    private Coroutine playCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (playCoroutine != null)
            StopCoroutine(playCoroutine);

        playCoroutine = StartCoroutine(PlayAfterDelay());
    }

    private IEnumerator PlayAfterDelay()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (clip == null)
            yield break;

        if (loop)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.volume = volume;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayAndDetach()
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.volume = volume;

        audioSource.transform.parent = null;
        audioSource.Play();

        Destroy(audioSource.gameObject, clip.length + 0.1f);
    }
    public void StartLoop()
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = volume;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopLoop()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}