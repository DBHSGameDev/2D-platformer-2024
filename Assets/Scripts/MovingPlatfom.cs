using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatfom : MonoBehaviour
{
    [SerializeField] Transform[] points; // points to move between
    [SerializeField] int currentIndex = 0; // index of the current target
    [SerializeField] float speed;
    [SerializeField] float reachingThreshold; // how close the platform needs to be with the target before switching to the next target

    Vector2 delta; // difference before and after a physic frame

    // FixedUpdate is a frame-rate independent message called once before every physic calculation
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, points[currentIndex].position) < reachingThreshold)
        {
            currentIndex++;
            currentIndex %= points.Length;
        }

        Vector3 newPos = Vector3.MoveTowards(transform.position, points[currentIndex].position, speed * Time.fixedDeltaTime);
        delta = (Vector2)(newPos - transform.position) / Time.fixedDeltaTime;
        transform.position = newPos;
    }

    // message to be called when the player character landed on this
    public void LandedOn()
    {
        PlayerController.instance.VelocityModifiers += AddVelocity;
    }

    // velocity modifier
    void AddVelocity()
    {
        PlayerController.instance.rb.velocity += delta;
    }
}
