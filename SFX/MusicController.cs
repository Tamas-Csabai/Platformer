using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.SFX
{
    public class MusicController : MonoBehaviour
    {

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private bool playOnAwake = true;
        [SerializeField] private AudioClip defaultMusic;

        private void Awake()
        {
            if (playOnAwake)
                PlayDefaultMusic();
        }

        public void PlayDefaultMusic()
        {
            audioSource.PlayOneShot(defaultMusic);
        }

    }
}
