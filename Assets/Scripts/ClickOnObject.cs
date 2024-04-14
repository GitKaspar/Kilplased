using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;

public class ClickOnObject : MonoBehaviour
{
    private int xGridStep;
    private int zGridStep;
    private int xIndeks;
    private int zIndeks;
    Vector3 coordinates;

    private void Awake()
    {
        // Indeksi mate on vale. Vaja robustsemat lahendust.
        xGridStep = gameObject.GetComponentInParent<loogika>().xGridStep;
        zGridStep = gameObject.GetComponentInParent<loogika>().zGridStep;
        xIndeks = (int)transform.position.x ;
        zIndeks = (int)transform.position.z ;
        coordinates = new Vector3(xIndeks*xGridStep+xGridStep - xGridStep*gameObject.GetComponentInParent<loogika>().gridSize[0]/2,gameObject.GetComponentInParent<loogika>().yoffset ,zIndeks*zGridStep+zGridStep - zGridStep*gameObject.GetComponentInParent<loogika>().gridSize[1]/2);;
        transform.SetPositionAndRotation(coordinates , Quaternion.identity);
    }

    public void OnClick()
    {
            gameObject.GetComponentInParent<loogika>().ImputPress(xIndeks,zIndeks);
    }
/*    void OnMouseDown()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            //UnityEngine.Debug.Log(transform.position);
            Debug.Log("Click responce");
            gameObject.GetComponentInParent<loogika>().ImputPress(xIndeks,zIndeks);
            AudioController.AudioInstance.SoDumb.Play();
        }
    }*/
}