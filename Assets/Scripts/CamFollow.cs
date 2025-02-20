
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 2f, playerTransform.position.z - 5.5f);
    }
}

