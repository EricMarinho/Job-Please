using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private Toggle audioTogle;

    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;

    public int poolSize = 32;
    public AudioSource audioSourcePrefab;

    private List<AudioSource> audioSources;

    [SerializeField] private AudioClip paperSound;
    [SerializeField] private AudioClip signSound;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.instance.OnInterviewerLeaveAnimationEnd += CallPlayMusic;
        GameManager.instance.OnEndDialogueSentence += StopAllAudio;
        audioTogle.onValueChanged.AddListener(OnAudioToggleValueChanged);
        InitializePool();
    }

    private void OnAudioToggleValueChanged(bool unmute)
    {
        if (unmute)
        {
            audioTogle.image.sprite = unmutedSprite;
            UnmuteAudio();
        }
        else
        {
            audioTogle.image.sprite = mutedSprite;
            MuteAudio();
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.OnInterviewerLeaveAnimationEnd -= CallPlayMusic;
        GameManager.instance.OnEndDialogueSentence -= StopAllAudio;
    }

    private void MuteAudio() 
    {
        musicAudioSource.mute = true;
    }

    public void PlayPaperSound()
    {
        sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
        sfxAudioSource.PlayOneShot(paperSound);
    }

    public void PlaySignSound()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(signSound);
    }

    private void UnmuteAudio()
    {
        musicAudioSource.mute = false;
    }

    private void InitializePool()
    {
        audioSources = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AddToPool();
        }
    }

    private void AddToPool()
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform);
        audioSource.gameObject.SetActive(false);
        audioSources.Add(audioSource);
    }

    private void CallPlayMusic()
    {
        PlayMusic(GameManager.instance.GetCurrentInterview().music,1f);
    }

    public void PlayAudio(AudioClip audioClip)
    {
        AudioSource availableAudioSource = GetAvailableAudioSource();

        if (availableAudioSource != null)
        {
            availableAudioSource.gameObject.SetActive(true);
            availableAudioSource.pitch = Random.Range(0.9f, 1.1f);
            availableAudioSource.PlayOneShot(audioClip);
        }
        else
        {
            AddToPool();
            PlayAudio(audioClip);
        }
    }

    public void StopAllAudio()
    {
        audioSources.ForEach(audioSource => audioSource.Stop());
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        return null;
    }

    public void PlayMusic(AudioClip newAudioClip, float crossfadeDuration)
    {
        // Ensure there is currently a playing clip
        if (musicAudioSource.clip != null)
        {
            // Coroutine to handle the crossfade
            StartCoroutine(Crossfade(newAudioClip, crossfadeDuration));
        }
        else
        {
            // If no clip is currently playing, simply play the new clip
            musicAudioSource.clip = newAudioClip;
            musicAudioSource.Play();
        }
    }

    private IEnumerator Crossfade(AudioClip newAudioClip, float crossfadeDuration)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = newAudioClip;
        newAudioSource.time = musicAudioSource.time;
        newAudioSource.Play();

        float startTime = Time.time;

        while (Time.time < startTime + crossfadeDuration)
        {
            // Calculate the crossfade factor (0 to 1)
            float crossfadeFactor = (Time.time - startTime) / crossfadeDuration / 3;

            // Adjust the volume of the two audio sources
            musicAudioSource.volume = 0.3f - crossfadeFactor;
            newAudioSource.volume = crossfadeFactor;

            yield return null;
        }

        // Ensure a smooth transition by setting volumes explicitly at the end
        musicAudioSource.volume = 0f;
        newAudioSource.volume = 0.3f;

        // Stop and destroy the old AudioSource
        musicAudioSource.Stop();
        Destroy(musicAudioSource);

        // Set the new AudioSource as the primary one
        musicAudioSource = newAudioSource;
        musicAudioSource.loop = true;
    }
}
