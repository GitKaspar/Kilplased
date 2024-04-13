using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class InstantiateScriptTesting : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(cube, new Vector3 (4.67f, 1.72f, 0f), transform.rotation);
        Instantiate(cube, new Vector3 (-2.83f, -1.29f, -3.09f), transform.rotation);
    }
}
