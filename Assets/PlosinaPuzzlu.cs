using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlosinaPuzzlu : MonoBehaviour
{
    [Header("Propojení - LEVEL 1")]
    public SpravceMistnosti mujSpravce;

    [Header("Propojení - LEVEL 2")]
    public UVText tajnyTextNaZdi;

    [Header("Obtížnost")]
    public float casPotrebnyKActivaci = 2.0f;
    private float aktualniCas = 0f;
    private bool figurinaNaPlosine = false;
    private bool uzJeAktivni = false;

    [Header("Vizuální nastavení")]
    public GameObject cisloObjekt;
    public Material materialAktivni;
    public Material materialNeaktivni;

    private MeshRenderer rendererPlosiny;
    private GameObject taFigurina;

    void Awake()
    {
        rendererPlosiny = GetComponent<MeshRenderer>();

        if (cisloObjekt != null)
        {
            cisloObjekt.SetActive(false);
        }
    }

    void Update()
    {
        if (figurinaNaPlosine && !uzJeAktivni && taFigurina != null)
        {
            aktualniCas += Time.deltaTime;

            if (aktualniCas >= casPotrebnyKActivaci)
            {
                AktivujCisloNaporad();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Figurina") && !uzJeAktivni)
        {
            figurinaNaPlosine = true;
            taFigurina = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Figurina") && !uzJeAktivni)
        {
            figurinaNaPlosine = false;
            taFigurina = null;
            aktualniCas = 0f;
        }
    }

    void AktivujCisloNaporad()
    {
        uzJeAktivni = true;

        if (cisloObjekt != null) cisloObjekt.SetActive(true);
        if (materialAktivni != null) rendererPlosiny.material = materialAktivni;

        if (taFigurina != null)
        {
            NavMeshAgent agent = taFigurina.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                if (agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.isStopped = true;
                }
                agent.enabled = false;
            }

            taFigurina.transform.position = new Vector3(transform.position.x, taFigurina.transform.position.y, transform.position.z);
        }

        string jmenoSceny = SceneManager.GetActiveScene().name.ToLower();

        if (jmenoSceny.Contains("level2") || jmenoSceny.Contains("level 2"))
        {
            if (tajnyTextNaZdi != null)
            {
                tajnyTextNaZdi.odemcenoPlosinami = true;
            }
        }
        else
        {
            if (mujSpravce != null)
            {
                mujSpravce.PlosinaAktivovana();
            }
        }
    }
}