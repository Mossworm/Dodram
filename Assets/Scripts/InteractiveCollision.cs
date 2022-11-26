using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCollision : MonoBehaviour
{
    private Vector3 movement;

    private void OnTriggerStay2D(Collider2D other)
    {
        var temp = (other.transform.position - transform.position).normalized;
        temp.z = 0.0f;
        movement = temp;
        
        other.transform.Translate(movement * (1f * Time.deltaTime));
    }


}
