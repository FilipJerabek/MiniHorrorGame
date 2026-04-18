using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    public enum StavDucha { Patrani, PuzzleBezi, Lov }
    public StavDucha aktualniStav = StavDucha.Patrani;

    private NavMeshAgent agent;
    private Animator anim;

    [Header("Nastavení obtížnosti (1-3)")]
    [Range(1, 3)] public int urovenDucha = 1;

    [Header("Nastavení toulání")]
    [Range(0f, 10f)] public float polomerToulani = 3f;

    [Header("Reference")]
    public Transform hrac;
    public Transform[] mozneMistnosti;
    public GameObject vizualDucha;

    private Transform oblibenaMistnost;
    private float casDoDalsihoKroku = 0f;
    private bool dosahlStreduMistnosti = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        
        //agent.acceleration = 15f;
        //agent.angularSpeed = 300f;

        if (mozneMistnosti.Length > 0)
        {
            // 1. Duch si vybere náhodnou místnost ze seznamu
            oblibenaMistnost = mozneMistnosti[Random.Range(0, mozneMistnosti.Length)];

            //Duch se okamžitě zrodí přímo na tom bodu
            agent.Warp(oblibenaMistnost.position);

            // 3. "toulání"
            dosahlStreduMistnosti = true;

            Debug.Log("Zrodil jsem se přímo v místnosti: " + oblibenaMistnost.name);

            // 4. první cíl
            VyberNovyBod();
        }
    }

    void Update()
    {
        if (anim != null)
        {
            // Pokud má spočítanou cestu a cíl je dál než 20 centimetrů, prostě JDE.
            bool duchSeHybe = agent.hasPath && agent.remainingDistance > 0.2f;
            anim.SetBool("Jde", duchSeHybe);
        }
        switch (aktualniStav)
        {
            case StavDucha.Patrani:
                LogikaPatrani();
                break;
            case StavDucha.PuzzleBezi:
                // Zde duch stojí a neexistuje
                break;
            case StavDucha.Lov:
                agent.SetDestination(hrac.position);
                agent.speed = 3.0f + urovenDucha;
                break;
        }
    }

    void LogikaPatrani()
    {
        if (oblibenaMistnost == null) return;

        if (!dosahlStreduMistnosti)
        {
            if (agent.pathPending || agent.remainingDistance > 1f) return;
            dosahlStreduMistnosti = true;
            return;
        }

        if (agent.pathPending) return;

        if (agent.hasPath && agent.remainingDistance > 0.5f) return;

        casDoDalsihoKroku -= Time.deltaTime;
        if (casDoDalsihoKroku <= 0f)
        {
            VyberNovyBod();
        }
    }

    void VyberNovyBod()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 nahodnyKruh = Random.insideUnitCircle * polomerToulani;

            Vector3 nahodnyBod = new Vector3(
                oblibenaMistnost.position.x + nahodnyKruh.x,
                transform.position.y,
                oblibenaMistnost.position.z + nahodnyKruh.y
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(nahodnyBod, out hit, 2.0f, NavMesh.AllAreas))
            {
                // Zkontroluje, jestli je bod aspoň 0.5 metru daleko
                if (Vector3.Distance(transform.position, hit.position) > 0.5f)
                {
                    // POJISTKA PROTI ÚTĚKU Z POKOJE
                    NavMeshPath cesta = new NavMeshPath();
                    if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, cesta))
                    {
                        if (cesta.status == NavMeshPathStatus.PathComplete)
                        {
                            float delkaCesty = SpocitejDelkuCesty(cesta);
                            // Cesta nesmí být extrémní obcházka přes chodbu
                            if (delkaCesty < (polomerToulani * 2f))
                            {
                                agent.SetDestination(hit.position);
                                casDoDalsihoKroku = 0.5f;
                                Debug.Log("Našel jsem skvělý bod pěkně na zemi, vyrážím!");
                                return;
                            }
                        }
                    }
                }
            }
        }

        Debug.LogWarning("Je tu na mě moc těsno (nebo jsem uvízl), nenašel jsem bod! Za chvíli to zkusím znovu.");
        casDoDalsihoKroku = 1f;
    }

    float SpocitejDelkuCesty(NavMeshPath cesta)
    {
        float delka = 0f;
        if (cesta.corners.Length < 2) return 0f;

        for (int i = 0; i < cesta.corners.Length - 1; i++)
        {
            delka += Vector3.Distance(cesta.corners[i], cesta.corners[i + 1]);
        }
        return delka;
    }

    public void DuchNalezen()
    {
        if (aktualniStav == StavDucha.Patrani)
        {
            aktualniStav = StavDucha.PuzzleBezi;
            vizualDucha.SetActive(false);
            agent.isStopped = true;
            Debug.Log("Duch nalezen! Spouštím puzzle úrovně " + urovenDucha);
        }
    }

    public void PuzzleVyreseno()
    {
        aktualniStav = StavDucha.Lov;
        vizualDucha.SetActive(true);
        agent.isStopped = false;
        Debug.Log("Puzzle vyřešeno! UTÍKEJ!");
    }
}