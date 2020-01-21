using UnityEngine;

namespace Assets.MonoBehaviours
{
    public class SoundEffects : MonoBehaviour
    {
        [SerializeField]
#pragma warning disable 0649
        private AudioClip journalOpen;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private AudioClip journalClose;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private AudioClip packOpen;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private AudioClip packClose;
#pragma warning restore 0649

        private static SoundEffects Instance;
        private AudioSource audioSource;

        private void Start()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        internal void PlayJournalOpen()
        {
            Instance.audioSource.clip = Instance.journalOpen;
            Instance.audioSource.Play();
        }

        internal void PlayJournalClose()
        {
            Instance.audioSource.clip = Instance.journalClose;
            Instance.audioSource.Play();
        }

        internal void PlayPackOpen()
        {
            Instance.audioSource.clip = Instance.packOpen;
            Instance.audioSource.Play();
        }

        internal void PlayPackClose()
        {
            Instance.audioSource.clip = Instance.packClose;
            Instance.audioSource.Play();
        }
    }
}