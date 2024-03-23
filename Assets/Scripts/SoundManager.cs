using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completeds
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource efxSource;
        public AudioSource musicSource;
        public static SoundManager instance = null;

        public float lowPitchRange = .95f;
        public float highPitchRange = 1.05f;

        void Awake()
        {
            if(instance== null)
            {
                instance = this;
            }
            else if(instance != this)
            
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            
        }

        public void PlaySingle(AudioClip clip)
        {
            efxSource.clip = clip;
            efxSource.Play();
        }

        public void RandomizeSfx(params AudioClip [] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPithch = Random.Range(lowPitchRange, highPitchRange);

            efxSource.pitch = randomPithch;
            efxSource.clip = clips[randomIndex];
            efxSource.Play();
        }
      
    }
}