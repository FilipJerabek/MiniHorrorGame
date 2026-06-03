using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class ZdraviHrace : MonoBehaviour
{
    [Header("Nastavení Života")]
    public int maxZdravi = 60;
    private int aktualniZdravi;
    private bool jeMrtvy = false;

    [Header("Ochrana")]
    public float casNesmrtelnosti = 0.5f;
    private float casovacNesmrtelnosti;

    [Header("UI Prvky")]
    public Slider hpBar;

    void Start()
    {
        aktualniZdravi = maxZdravi;

        if (hpBar != null)
        {
            hpBar.maxValue = maxZdravi;
            hpBar.value = maxZdravi;
        }
    }

    void Update()
    {
        if (casovacNesmrtelnosti > 0f) casovacNesmrtelnosti -= Time.deltaTime;
    }

    public void UtrzPoskozeni(int kolik)
    {
        if (jeMrtvy) return;
        if (casovacNesmrtelnosti > 0f) return;

        aktualniZdravi -= kolik;
        Debug.Log("Hráč dostal hit! HP: " + aktualniZdravi);

        if (hpBar != null)
        {
            hpBar.value = aktualniZdravi;
        }

        casovacNesmrtelnosti = casNesmrtelnosti;

        if (aktualniZdravi <= 0)
        {
            Zemri();
        }
    }

    void Zemri()
    {
        jeMrtvy = true;
        Invoke("RestartujLevel", 1.0f);
    }

    void RestartujLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}