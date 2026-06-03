using UnityEngine;

public class SpravceMistnosti : MonoBehaviour
{
    public Transform[] bodyProFiguriny;
    public Door dvereOdMistnosti;
    public GameObject puzzleVMistnosti;

  

    

    [Header("Nastavení Puzzlu")]
    public int pocetPotrebnychPlosin = 2; 
    private int pocetAktivovanych = 0;

    public GameObject uvTextNaZdi;

   

  
    public void PlosinaAktivovana()
    {
        pocetAktivovanych++;
        Debug.Log("Plošina aktivována - zbývá: " + (pocetPotrebnychPlosin - pocetAktivovanych));

        // Když jsou všechny hotové
        if (pocetAktivovanych >= pocetPotrebnychPlosin)
        {
            PuzzleDokoncen();
        }
    }

    void PuzzleDokoncen()
    {
        if (uvTextNaZdi != null)
        {
            UVText uvSkript = uvTextNaZdi.GetComponent<UVText>();

            if (uvSkript != null)
            {
                uvSkript.odemcenoPlosinami = true;
            }
            else
            {
                uvTextNaZdi.SetActive(true); // Nouzové zobrazení
            }
        }
        else
        {
            Debug.LogError("CHYBA: Ve SpravceMistnosti je kolonka 'Uv Text Na Zdi' prázdná");
        }
    }
}