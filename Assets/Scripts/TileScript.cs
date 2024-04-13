using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileScript : MonoBehaviour
{
    private Vector3 destination;

    void Start()
    {
        destination = transform.position;
    }

    void Update()
    {
        
    }

    public void SetDestination(Vector3 newPos)
    {
        destination = newPos;
        transform.DOMove(destination, 1f);
        //Debug.Log("move box to:" + destination);
    }
    public bool isMoving(){
        return (DOTween.IsTweening(transform));
    }

}