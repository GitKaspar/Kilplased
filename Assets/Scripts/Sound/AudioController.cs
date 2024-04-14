using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController AudioInstance;

    public AudioClipGroup KauriPala;
    public AudioClipGroup Click;
    public AudioClipGroup SampleMusic;
    public AudioClipGroup SoDumb;
    public AudioClipGroup Mungad;
    public AudioClipGroup Harf;
    public AudioClipGroup Sigh;

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
