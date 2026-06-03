using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    public bool open;
    public float smooth = 1.0f;
    float DoorOpenAngle = -90.0f;
    private Quaternion closedRotation;
    private Quaternion openedRotation;
    public bool jeZamceno = false;


    public AudioSource asource;
    public AudioClip openDoor, closeDoor;

    void Start()
    {
        closedRotation = transform.localRotation;

        openedRotation = closedRotation * Quaternion.Euler(0, DoorOpenAngle, 0);

        asource = GetComponent<AudioSource>();

    }

    void Update()
    {

        if (open)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, openedRotation, Time.deltaTime * 5 * smooth);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, closedRotation, Time.deltaTime * 5 * smooth);
        }
    }


    public void OpenDoor()
    {
        if (jeZamceno)
        {
            Debug.Log("Zavřeno! Kliku nelze stisknout.");
            return; 
        }
        open = !open;
        if (asource != null)
        {
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
        }


    }

    public void ZabouchniAZamkni()
    {
        jeZamceno = true; 
        if (open)
        {
            open = false; 

            if (asource != null && closeDoor != null)
            {
                asource.clip = closeDoor;
                asource.Play();
            }
        }
    }


}