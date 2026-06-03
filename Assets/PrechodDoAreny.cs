using UnityEngine;
using UnityEngine.AI;

public class PrechodDoAreny : MonoBehaviour
{
    [Header("Pozice v Aréně")]
    public Transform poziceHraceVArene;
    public Transform poziceBosseVArene;

    [Header("Odkazy na postavy")]
    public GameObject hrac;
    public GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportujVsechny();
        }
    }

    void TeleportujVsechny()
    {
        SpravceUlozeni.UlozVstupDoAreny();
        CharacterController cc = hrac.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        hrac.transform.position = poziceHraceVArene.position;
        hrac.transform.rotation = poziceHraceVArene.rotation;

        if (cc != null) cc.enabled = true;

        NavMeshAgent agent = boss.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false; 

        boss.transform.position = poziceBosseVArene.position;
        boss.transform.rotation = poziceBosseVArene.rotation;

        if (agent != null)
        {
            agent.enabled = true;
        }
    }
}