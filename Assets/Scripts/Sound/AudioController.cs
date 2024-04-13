using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController AudioInstance;
    private AudioSource m_AudioSource;

    public AudioClipGroup KauriPala;
    public AudioClipGroup Click;

    private void Start()
    {
        if (AudioInstance == null)
        {
            AudioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (AudioInstance != this)
        {
            Destroy(AudioInstance.gameObject);
        }
    }

}
