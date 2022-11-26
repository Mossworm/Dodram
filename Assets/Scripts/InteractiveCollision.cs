using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCollision : MonoBehaviour
{
    private Vector3 movement;
    private bool isEnter;

    // Start is called before the first frame update
    void Start()
    {
        isEnter = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        isEnter = true;
        var temp = (other.transform.position - transform.position).normalized;
        temp.z = 0.0f;
        movement = temp;
        
        other.transform.Translate(movement * (1f * Time.deltaTime));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isEnter = false;
    }
}
