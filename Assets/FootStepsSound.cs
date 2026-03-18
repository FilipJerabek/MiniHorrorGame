using UnityEngine;

public class FootStepsSound : MonoBehaviour
{
    
    public AudioSource footstepSound;

    
    public float stepRate = 0.5f;
    private float stepTimer = 0f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            
            stepTimer += Time.deltaTime;

            
            if (stepTimer >= stepRate)
            {
                footstepSound.Play(); 
                stepTimer = 0f;      
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}