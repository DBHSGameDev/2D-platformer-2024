using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float duration;

    // FixedUpdate is a frame-rate independent message called once before every physic calculation
    void FixedUpdate()
    {
        Vector3 target = new(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime / duration);
    }
}
