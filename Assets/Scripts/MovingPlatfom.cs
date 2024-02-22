using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatfom : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] int currentIndex = 0;
    [SerializeField] float speed;
    [SerializeField] float reachingThreshold;

    Vector2 direction;
    Vector2 delta;

    private void Start()
    {
        CalculateDirection();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, points[currentIndex].position) < reachingThreshold)
        {
            currentIndex++;
            currentIndex %= points.Length;
        }
        var newPos = Vector3.MoveTowards(transform.position, points[currentIndex].position, speed * Time.fixedDeltaTime);
        delta = (Vector2)(newPos - transform.position) / Time.fixedDeltaTime;
        transform.position = newPos;
        Debug.Log(delta);
    }

    void CalculateDirection()
    {
        Vector3 direction3D = points[currentIndex].position - transform.position;
        direction = new Vector2(direction3D.x, direction3D.y).normalized;
    }

    public void LandedOn()
    {
        PlayerController.instance.VelocityModifiers += AddVelocity;
    }

    void AddVelocity()
    {
        PlayerController.instance.rb.velocity += delta;
    }
}
