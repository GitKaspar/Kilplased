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

    public void SetDestination(Vector3 newPos, Sequence seq = null, bool line = true)
    {
        destination = newPos;
        if(seq != null)
        {
            if (line)
            {
                seq.Append(transform.DOMove(destination, 1f).SetEase(Ease.InOutQuad));
            }
            else{
                seq.Join(transform.DOMove(destination, 1f).SetEase(Ease.InOutQuad));
            }
            
        }
        else
        {
            transform.DOMove(destination, 0.1f).SetEase(Ease.InOutQuad);
        }
        //Debug.Log("move box to:" + destination);
    }
    public bool isMoving(){
        return (DOTween.IsTweening(transform));
    }

}