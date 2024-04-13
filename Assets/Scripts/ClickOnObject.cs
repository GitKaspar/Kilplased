using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ClickOnObject : MonoBehaviour
{
    public int xGridStep;
    public int zGridStep;
    private float xIndeks;
    private float zIndeks;
    float[] coordinates = new float[2];

    private void Awake()
    {
        // Indeksi mate on vale. Vaja robustsemat lahendust.
        xIndeks = transform.position.x - xGridStep;
        zIndeks = transform.position.z - zGridStep;
        coordinates[0] = xIndeks;
        coordinates[1] = zIndeks;
    }

    void OnMouseDown()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            UnityEngine.Debug.Log(transform.position);
            AudioController.AudioInstance.Click.Play();
        }
    }

    public float[] ReturnLowerLeftSquareCoordinates()
    {
        return coordinates;
    }
}