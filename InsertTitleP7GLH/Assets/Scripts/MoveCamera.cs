using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y + 1, cameraPos.position.z);
    }
}
