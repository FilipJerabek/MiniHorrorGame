using UnityEngine;
using UnityEngine.SceneManagement;

public static class SpravceUlozeni
{
    public static void UlozNazevSceny(string nazevPristihoLevelu)
    {
        PlayerPrefs.SetString("UlozenyLevelNazev", nazevPristihoLevelu);
        PlayerPrefs.SetInt("HracJeVArene", 0);
        PlayerPrefs.Save();
    }

    public static void UlozVstupDoAreny()
    {
        PlayerPrefs.SetString("UlozenyLevelNazev", SceneManager.GetActiveScene().name); 
        PlayerPrefs.SetInt("HracJeVArene", 1); 
        PlayerPrefs.Save();

        Debug.Log("Hra uložena, aréna Levelu 3.");
    }

    public static void NactiUlozenouPozici()
    {
        string levelKNačteni = PlayerPrefs.GetString("UlozenyLevelNazev", "Level1");
        SceneManager.LoadScene(levelKNačteni);
    }
}