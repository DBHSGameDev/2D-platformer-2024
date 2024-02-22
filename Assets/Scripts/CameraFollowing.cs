using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float duration;

    void FixedUpdate()
    {
        Vector3 target = new(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime / duration);
    }
}
