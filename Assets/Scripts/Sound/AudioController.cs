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

    private void Awake()
    {
        if (AudioInstance != null)
        {
            return;
        }

        DontDestroyOnLoad(gameObject);
        AudioInstance = this;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            KauriPala.Play();
        }
    }
}
