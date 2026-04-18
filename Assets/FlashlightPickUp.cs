using UnityEngine;

public class FlashlightPickUp : MonoBehaviour
{

    public GameObject baterkaVRuce;


    public void PickUp()
    {
        if (baterkaVRuce != null)
        {
            baterkaVRuce.SetActive(true);
        }

        gameObject.SetActive(false);

    }

}
