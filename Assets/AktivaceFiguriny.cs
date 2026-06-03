using UnityEngine;

public class AktivaceFiguriny : MonoBehaviour
{
    [Header("figuríny z pokoje")]
    public PliziciSeStin[] figurinyVPokoji;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProbudFiguriny();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProbudFiguriny();
        }
    }

    private void ProbudFiguriny()
    {
        foreach (PliziciSeStin figurina in figurinyVPokoji)
        {
            if (figurina != null && !figurina.jeAktivni)
            {
                figurina.jeAktivni = true;
            }
        }
        gameObject.SetActive(false);
    }
}