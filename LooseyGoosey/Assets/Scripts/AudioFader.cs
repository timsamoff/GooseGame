using System.Collections;
using UnityEngine;

public class AudioFader : MonoBehaviour
{
    [Header("Settings Stuff")]
    [SerializeField] private float fadeTime = 2.0f;
    [SerializeField] private float targetVolume = 1.0f;

    void Start()
    {
        AudioSource backgroundMusic = GetComponent<AudioSource>();

        backgroundMusic.volume = 0f;
        StartCoroutine(FadeInMusic(backgroundMusic));
    }

    IEnumerator FadeInMusic(AudioSource audioSource)
    {
        float currentTime = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, currentTime / fadeTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}