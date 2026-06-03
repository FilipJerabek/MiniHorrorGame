using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI; 

public class KodZamek : MonoBehaviour
{
    [Header("Nastavení kódu")]
    public string spravnyKod = "1234";

    [Header("UI Prvky")]
    public GameObject zamekUI;
    public TMP_InputField vstupniPole;

    [Header("Události po otevření")]
    public Door dvere;
    public GameObject pronasledujiciDuch;
    public Transform mistoZrozeniDucha;

    private bool hracJeBlizko = false;
    private bool kodUzBylUhadnut = false;

    void Start()
    {
        if (zamekUI != null) zamekUI.SetActive(false);
        if (vstupniPole != null) vstupniPole.onValueChanged.AddListener(ZkontrolujKod);
    }

    void Update()
    {
        if (kodUzBylUhadnut) return;

        if (hracJeBlizko && Input.GetKeyDown(KeyCode.Q))
        {
            if (!zamekUI.activeSelf)
            {
                OtevriZamek();
            }
        }

        if (zamekUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) ZavriZamek();
    }

    void OtevriZamek()
    {
        zamekUI.SetActive(true);
        vstupniPole.text = "";
        vstupniPole.ActivateInputField();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ZavriZamek()
    {
        zamekUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ZkontrolujKod(string zadanyText)
    {
        if (kodUzBylUhadnut) return;

        if (zadanyText == spravnyKod)
        {
            kodUzBylUhadnut = true;
            ZavriZamek();
            this.enabled = false;
            if (SceneManager.GetActiveScene().name == "Level2")
            {
                SpravceUlozeni.UlozNazevSceny("Level3");
                SceneManager.LoadScene("Level3");
            }
            else
            {
                if (dvere != null)
                {
                    dvere.jeZamceno = false;
                    dvere.OpenDoor();
                }

                OpravAZapniDuchaNaChodbe();
            }
        }
        else if (zadanyText.Length >= spravnyKod.Length)
        {
            vstupniPole.text = "";
            vstupniPole.ActivateInputField();
        }
    }

    void OpravAZapniDuchaNaChodbe()
    {
        if (pronasledujiciDuch == null) return;
        pronasledujiciDuch.SetActive(true);
        foreach (Transform t in pronasledujiciDuch.GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.SetActive(true);
        }
        NavMeshAgent agent = pronasledujiciDuch.GetComponent<NavMeshAgent>();
        if (agent != null && mistoZrozeniDucha != null)
        {
            agent.Warp(mistoZrozeniDucha.position);
        }
        foreach (Renderer r in pronasledujiciDuch.GetComponentsInChildren<Renderer>(true))
        {
            r.enabled = true;
        }
        GhostAI staryMozek = pronasledujiciDuch.GetComponent<GhostAI>();
        if (staryMozek != null) staryMozek.enabled = false;

        HororovaPast past = pronasledujiciDuch.GetComponent<HororovaPast>();
        if (past != null) past.enabled = false;

        Hlidkovac novyMozek = pronasledujiciDuch.GetComponent<Hlidkovac>();
        if (novyMozek != null) novyMozek.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hracJeBlizko = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hracJeBlizko = false;
            ZavriZamek();
        }
    }
}