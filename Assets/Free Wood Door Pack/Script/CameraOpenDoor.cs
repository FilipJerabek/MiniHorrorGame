//skript pro baterku i dveře
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
            Door door = hit.transform.GetComponent<Door>();

            FlashlightPickUp baterka = hit.transform.GetComponent<FlashlightPickUp>();

            //dveře/baterka
            if (door != null || baterka != null)
            {
                //if (text != null) text.SetActive(true);   --nastavit později

                if (Input.GetKeyDown(KeyCode.E))
                {
                    
                    if (door != null)
                    {
                        door.OpenDoor();
                    }
                    else if (baterka != null)
                    {
                        baterka.PickUp();
                    }
                }
            }
            else
            {
               // if (text != null) text.SetActive(false); --později
            }
        }
        else
        {
            //if (text != null) text.SetActive(false); --později
        }
    }
}