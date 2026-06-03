using UnityEngine;
using UnityEngine.AI;

public class Hlidkovac : MonoBehaviour
{
    [Header("Všechny body trasy")]
    public Transform[] bodyTrasy;

    [Header("Rychlost chůze")]
    public float rychlostHlidkovani = 2f;

    private NavMeshAgent agent;
    private Animator anim;
    private int cilovyBod = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("Hlidkovac nenašel Animator");
        }
    }

    void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        agent.speed = rychlostHlidkovani;

        if (anim != null) anim.Rebind();

        if (bodyTrasy.Length > 0)
        {
            cilovyBod = 0;
            agent.SetDestination(bodyTrasy[cilovyBod].position);
        }
    }

    void Update()
    {
        if (bodyTrasy.Length == 0 || agent == null || !agent.isOnNavMesh) return;
        if (anim != null)
        {
            bool seHybe = agent.velocity.magnitude > 0.1f || (agent.hasPath && agent.remainingDistance > 0.5f);
            anim.SetBool("Jde", seHybe);
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            JdiNaDalsiBod();
        }
    }

    void JdiNaDalsiBod()
    {
        cilovyBod = (cilovyBod + 1) % bodyTrasy.Length;
        agent.SetDestination(bodyTrasy[cilovyBod].position);
    }
}