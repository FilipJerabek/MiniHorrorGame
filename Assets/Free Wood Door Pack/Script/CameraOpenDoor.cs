using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOpenDoor : MonoBehaviour
{
    public float DistanceOpen = 3f; 
    public GameObject text;         

    void Update()
    {
        RaycastHit hit;

        
        if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen))
        {
           
            if (hit.transform.GetComponent<Door>())
            {
                
                if (text != null) text.SetActive(true);

               
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.GetComponent<Door>().OpenDoor();
                }
            }
            else
            {
                
                if (text != null) text.SetActive(false);
            }
        }
        else
        {
           
            if (text != null) text.SetActive(false);
        }
    }
}