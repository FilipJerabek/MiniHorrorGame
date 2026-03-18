using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light light;
    public bool hasFlashlight = true;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F) && hasFlashlight)
        {
            light.enabled = !light.enabled;
        }
    }
}
