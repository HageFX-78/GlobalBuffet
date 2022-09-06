using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Transform playerTR;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - playerTR.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTR.position + offset;
    }
}
