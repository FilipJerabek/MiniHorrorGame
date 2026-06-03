using UnityEngine;

public class ZdraviBosse : MonoBehaviour
{
    [Header("Nastavení životů a animací")]
    public int hp = 100;
    public Animator anim; 

    public void UtrzPoskozeni(int poskozeni)
    {
        if (hp <= 0) return; 

        hp -= poskozeni;
        Debug.Log($"Duch dostal hit, zbývá mu: {hp} HP");

        if (hp > 0)
        {
            anim.SetTrigger("Hit"); 
        }
        else
        {
            Zemri();
        }
    }

    void Zemri()
    {
        Debug.Log("Boss padl k zemi!");

        if (anim != null)
        {
            anim.SetTrigger("Die");
        }
        else
        {
            Debug.LogWarning("Ve skriptu ZdraviBosse chybí přiřazený Animátor!");
        }

       BossAI aiSkript = GetComponent<BossAI>();
        if (aiSkript != null)
        {
            aiSkript.enabled = false;
        }

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        Destroy(gameObject, 5f);
    }
}