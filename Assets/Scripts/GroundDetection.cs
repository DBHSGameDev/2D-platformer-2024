using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public bool onGround;

    PlayerController player;
    new BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        collider = GetComponent<BoxCollider2D>();
    }

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
