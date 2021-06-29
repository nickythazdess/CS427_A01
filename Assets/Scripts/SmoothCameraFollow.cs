using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    void Update() {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, 2f, 6.2f),
            Mathf.Clamp(target.position.y, -3.05f,-1.9f),
            transform.position.z);
    }
}
