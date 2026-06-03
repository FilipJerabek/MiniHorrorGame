using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PliziciSeStin : MonoBehaviour
{
    public float rychlost = 3.5f;
    public bool jeAktivni = false;

    private Transform hrac;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        if (agent != null) agent.speed = rychlost;

        GameObject hracObjekt = GameObject.FindGameObjectWithTag("Player");
        if (hracObjekt != null) hrac = hracObjekt.transform;

        AktivaceFiguriny triggerVPokoji = FindObjectOfType<AktivaceFiguriny>();

        if (triggerVPokoji != null)
        {
            jeAktivni = false;
        }
        else
        {
            jeAktivni = true;
        }
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        if (agent == null || !agent.isActiveAndEnabled || hrac == null || !jeAktivni)
        {
            if (agent != null && agent.isActiveAndEnabled) agent.isStopped = true;
            return;
        }

        float vzdalenostOdHrace = Vector3.Distance(transform.position, hrac.position);
        Vector3 bodNaMonitoru = Camera.main.WorldToViewportPoint(transform.position + Vector3.up);
        bool jeNaMonitoru = bodNaMonitoru.z > 0 && bodNaMonitoru.x > 0 && bodNaMonitoru.x < 1 && bodNaMonitoru.y > 0 && bodNaMonitoru.y < 1;

        if (jeNaMonitoru || vzdalenostOdHrace < 1.8f)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(hrac.position);
        }
    }
}