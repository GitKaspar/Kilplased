using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController AudioInstance;
    private AudioSource buttonAudioSource;

    private void Awake()
    {

        if (AudioInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        AudioInstance = this;

        buttonAudioSource = GetComponent<AudioSource>(); //need own audiosource for buttons so that buttons sounds still work when game is paused
        buttonAudioSource.ignoreListenerPause = true;
    }
}
