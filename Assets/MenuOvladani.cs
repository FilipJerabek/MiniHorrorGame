using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOvladani : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TlacitkoNovaHra()
    {
        PlayerPrefs.DeleteKey("UlozenyLevelNazev");
        PlayerPrefs.DeleteKey("HracJeVArene");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Level1");
    }

    public void TlacitkoPokracovat()
    {
        SpravceUlozeni.NactiUlozenouPozici();
    }

    public void TlacitkoUkoncitHru()
    {
        Debug.Log("Vypínám hru...");
        Application.Quit();
    }
}