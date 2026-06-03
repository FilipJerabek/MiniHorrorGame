using UnityEngine;

public class Strelba : MonoBehaviour
{
    [Header("Nastavení zbraně")]
    public float dostrel = 100f;
    public int poskozeni = 10; 
    public Camera hracovaKamera;

    [Header("Efekty")]
    public AudioSource zvukVystrelu;
    public ParticleSystem zableskZHlavne;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vystrel();
        }
    }

    void Vystrel()
    {
        if (zvukVystrelu != null) zvukVystrelu.Play();
        if (zableskZHlavne != null) zableskZHlavne.Play();

        RaycastHit zasah;

        if (Physics.Raycast(hracovaKamera.transform.position, hracovaKamera.transform.forward, out zasah, dostrel))
        {
            BossAI duch = zasah.transform.GetComponentInParent<BossAI>();

            if (duch != null)
            {
                duch.UtrzPoskozeniBossa(poskozeni);
                Debug.Log("Úspěšný zásah ducha! Poškození: " + poskozeni);
            }
            else
            {
                Debug.Log("Zásah do zdi/objektu: " + zasah.transform.name);
            }
        }
    }
}