using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public float speed = 0.01f;
    private Vector3 destination;
    private bool isMoving;

    void Start()
    {
        destination = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, speed);
    }

    void SetDestination(Vector3 newPos)
    {
        destination = newPos;
    }
}