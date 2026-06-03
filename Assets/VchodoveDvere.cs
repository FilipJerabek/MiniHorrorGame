using UnityEngine;
using UnityEngine.SceneManagement;

public class VchodoveDvere : MonoBehaviour
{
    public string nazevDalsihoLevelu = "LEVEL2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HororovaPast duch = Object.FindObjectOfType<HororovaPast>();

            if (duch != null && duch.pastUzSklapla == true)
            {
                SpravceUlozeni.UlozNazevSceny(nazevDalsihoLevelu);
                SceneManager.LoadScene(nazevDalsihoLevelu);
            }
            else
            {
                Debug.Log("Dveře jsou zamčené, nejdřív je nutno prozkoumat dům.");
            }
        }
    }
}