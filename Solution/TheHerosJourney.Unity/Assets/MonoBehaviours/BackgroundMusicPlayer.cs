using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicPlayer : MonoBehaviour
{
    [SerializeField]
#pragma warning disable 0649
    private AudioClip[] tracks;
#pragma warning restore 0649

    private AudioSource audioSource;

    private int currentTrack = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);

        if (tracks.Length > 0)
        {
            PlayCurrentTrack();
        }
    }

    // Check out this page for a less messy way to do this:
    // https://docs.unity3d.com/ScriptReference/AudioSource.PlayScheduled.html
    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            currentTrack += 1;
            if (currentTrack >= tracks.Length)
            {
                currentTrack = 0;
            }

            PlayCurrentTrack();
        }
    }

    private void PlayCurrentTrack()
    {
        audioSource.clip = tracks[currentTrack];
        audioSource.Play();
    }
}
