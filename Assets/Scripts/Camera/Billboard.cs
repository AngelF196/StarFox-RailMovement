using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    private CinemachineBrain _brain;
    void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            _brain = _camera.gameObject.GetComponent<CinemachineBrain>();
        }



        if (_player == null)
            _player = transform.parent;
    }

    void LateUpdate()
    {
        transform.LookAt(_brain.ActiveVirtualCamera.State.RawPosition);
    }
}
