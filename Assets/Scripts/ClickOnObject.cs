using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ClickOnObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            UnityEngine.Debug.Log(transform.position);
            AudioController.AudioInstance.Click.Play()
        }
    }
}