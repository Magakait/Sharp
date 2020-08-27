using UnityEngine;

[RequireComponent(typeof(AudioSource))]
class AudioRandomizer : MonoBehaviour
{
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;
    [Space(10)]
    [SerializeField] bool randomize;
    [SerializeField] AudioClip[] clips;

    new AudioSource audio;
    public bool IsPlaying
    {
        get => audio.isPlaying;
        set
        {
            if (IsPlaying)
                audio.Play();
            else
                audio.Stop();
        }
    }

    void Awake() =>
        audio = GetComponent<AudioSource>();

    void Start()
    {
        audio.pitch = Random.Range(minPitch, maxPitch);
        if (randomize)
            audio.clip = clips[Random.Range(0, clips.Length)];
    }

    public void Play(int i)
    {
        audio.clip = clips[i];
        IsPlaying = true;
    }
}
