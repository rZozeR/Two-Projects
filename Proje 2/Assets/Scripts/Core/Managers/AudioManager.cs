using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _perfectStack, _imperfectStack;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int combo)
    {
        if (combo >= 1)
        {
            _audioSource.pitch = Mathf.Lerp(0.8f, 2f, (float)combo / 15);
            _audioSource.PlayOneShot(_perfectStack);
        }
        else
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_imperfectStack);
        }
    }
}
