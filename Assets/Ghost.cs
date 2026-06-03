using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GhostAI : MonoBehaviour
{
    public enum StavDucha { Patrani, PuzzleBezi, Lov }
    public StavDucha aktualniStav = StavDucha.Patrani;

    private NavMeshAgent agent;
    private Animator anim;

    [Header("Společné nastavení")]
    public Transform hrac;
    public GameObject vizualDucha;
    [Range(1, 3)] public int urovenDucha = 1;

    [Header("Level 1 - Pokoj")]
    public Transform[] mozneMistnosti;
    private Transform oblibenaMistnost;
    private bool dosahlStreduMistnosti = false;

    [Header("Level 2 - Chodba")]
    public Transform[] bodyChodbaLevel2;
    private int indexBodu = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

     
        anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        string jmenoSceny = SceneManager.GetActiveScene().name.ToLower();

        if (jmenoSceny == "level2")
        {
            aktualniStav = StavDucha.Lov;
            agent.speed = 2.5f;
            agent.isStopped = false;
            if (vizualDucha != null) vizualDucha.SetActive(true);

            if (bodyChodbaLevel2.Length > 0)
            {
                agent.Warp(bodyChodbaLevel2[0].position);
                NastavDalsiBodL2();
            }
            Debug.Log("Duch nastaven pro LEVEL 2 - Hlídkování.");
        }
        else
        {
            if (mozneMistnosti.Length > 0)
            {
                oblibenaMistnost = mozneMistnosti[Random.Range(0, mozneMistnosti.Length)];
                agent.Warp(oblibenaMistnost.position);
                dosahlStreduMistnosti = true;
                VyberNovyBodL1();
            }
        }
        if (agent != null && agent.isOnNavMesh)
        {
            VyberNovyBodL1();
        }
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh) return;

        if (anim != null)
        {
            bool seHybe = agent.velocity.magnitude > 0.1f || (agent.hasPath && agent.remainingDistance > 0.5f);
            anim.SetBool("Jde", seHybe);
        }

        string jmenoSceny = SceneManager.GetActiveScene().name.ToLower();

        if (jmenoSceny == "level2")
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                NastavDalsiBodL2();
            }
        }
        else
        {
            switch (aktualniStav)
            {
                case StavDucha.Patrani:
                    LogikaPatraniL1();
                    break;
                case StavDucha.Lov:
                    if (hrac != null) agent.SetDestination(hrac.position);
                    break;
            }
        }
    }
    void NastavDalsiBodL2()
    {
        if (bodyChodbaLevel2.Length == 0) return;
        agent.SetDestination(bodyChodbaLevel2[indexBodu].position);
        indexBodu = (indexBodu + 1) % bodyChodbaLevel2.Length;
    }
    void LogikaPatraniL1()
    {
        if (oblibenaMistnost == null) return;
        if (!dosahlStreduMistnosti)
        {
            if (agent.remainingDistance < 1f) dosahlStreduMistnosti = true;
            return;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f) VyberNovyBodL1();
    }

    void VyberNovyBodL1()
    {
        Vector2 nahodnyKruh = Random.insideUnitCircle * 5f;
        Vector3 nahodnyBod = new Vector3(oblibenaMistnost.position.x + nahodnyKruh.x, transform.position.y, oblibenaMistnost.position.z + nahodnyKruh.y);
        agent.SetDestination(nahodnyBod);
    }
    public void DuchNalezen()
    {
        if (SceneManager.GetActiveScene().name.ToLower() != "level2")
        {
            aktualniStav = StavDucha.PuzzleBezi;
            if (vizualDucha != null) vizualDucha.SetActive(false);
            agent.isStopped = true;
        }
    }

    public void PuzzleVyreseno()
    {
        aktualniStav = StavDucha.Lov;
        if (vizualDucha != null) vizualDucha.SetActive(true);
        agent.isStopped = false;
    }
    public void AktivujDuchaNaChodbe(Vector3 pozice)
    {
        aktualniStav = StavDucha.Lov;
        if (vizualDucha != null) vizualDucha.SetActive(true);
        agent.isStopped = false;
        agent.Warp(pozice);

        if (anim != null) anim.Rebind();
    }
}