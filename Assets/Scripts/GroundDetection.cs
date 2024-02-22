using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public bool onGround;

    new BoxCollider2D collider; // hidden deprecated member collider

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // FixedUpdate is a frame-rate independent message called once before every physic calculation
    void FixedUpdate()
    {
        List<Collider2D> results = new();

        ContactFilter2D filter = new();
        filter.SetLayerMask(LayerMask.GetMask("ground"));

        collider.OverlapCollider(filter, results);

        if (results.Count != 0)
        {
            if (!onGround) {
                foreach (Collider2D other in results)
                {
                    other.SendMessage("LandedOn", SendMessageOptions.DontRequireReceiver);
                }
            }
            onGround = true;
        }
        else
        {
            PlayerController.instance.ClearVelocityModifiers();
            onGround = false;
        }
    }
}
