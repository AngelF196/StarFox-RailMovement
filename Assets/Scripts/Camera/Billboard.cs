using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    void LateUpdate()
    {
        transform.forward = _camera.transform.forward;
    }
}
