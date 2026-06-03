using UnityEngine;
using UnityEngine.SceneManagement;

public class PauzaMenu : MonoBehaviour
{
    [Header("UI Panel Pauzy")]
    public GameObject pauseMenuPanel;

    private bool jeZapauzovano = false;

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jeZapauzovano)
            {
                PokracovatVeHre();
            }
            else
            {
                ZapauzovatHru();
            }
        }
    }
    public void PokracovatVeHre()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f; 
        jeZapauzovano = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Hra pokračuje.");
    }

    void ZapauzovatHru()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);

        Time.timeScale = 0f; 
        jeZapauzovano = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Hra byla zapauzována.");
    }

    public void UlozitAktualniPozici()
    {
        string nazevTohotoLevelu = SceneManager.GetActiveScene().name;

        SpravceUlozeni.UlozNazevSceny(nazevTohotoLevelu);

        Debug.Log("Pozice v levelu " + nazevTohotoLevelu + " byla úspěšně uložena z pauzy");
    }

    public void OdejitDoHlavnihoMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("HlavniMenu");
    }
}