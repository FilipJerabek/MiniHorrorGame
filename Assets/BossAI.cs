using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Header("Zdraví Bossa")]
    public int maxZdraviBossa = 100;
    private int aktualniZdraviBossa;
    public UnityEngine.UI.Slider hpBarBossa;

    [Header("Nastavení Hráče")]
    public Transform hrac;
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Trasa (Patrolování)")]
    public Transform[] bodyTrasy;
    public float dohledVzdalenost = 15f;
    public float rychlostHlidkovani = 2f;
    private int cilovyBod = 0;

    [Header("Nastavení Útoku")]
    public int poskozeniHrace = 20;
    public float utocnaVzdalenost = 3.0f;
    public float rychlostSprintu = 5f;
    public float casMeziUtoky = 1.5f;
    private float casovacUtoku;

    private bool uzVidelHrace = false;

    private bool jeMrtvy = false;
    private float casovacOmrazeniHitom = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (bodyTrasy.Length > 0 && agent != null)
        {
            cilovyBod = 0;
            agent.SetDestination(bodyTrasy[cilovyBod].position);
        }
        aktualniZdraviBossa = maxZdraviBossa;
        if (hpBarBossa != null)
        {
            hpBarBossa.maxValue = maxZdraviBossa;
            hpBarBossa.value = maxZdraviBossa;
        }
    }

    void Update()
    {
        if (jeMrtvy) return;

        if (casovacOmrazeniHitom > 0f)
        {
            casovacOmrazeniHitom -= Time.deltaTime;
            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
            return; 
        }

        if (hrac == null || agent == null || !agent.isOnNavMesh) return;

        float vzdalenostOdHrace = Vector3.Distance(transform.position, hrac.position);

        if (vzdalenostOdHrace <= dohledVzdalenost)
        {
            uzVidelHrace = true;
        }

        if (casovacUtoku > 0f) casovacUtoku -= Time.deltaTime;

        // MOZEK
        if (vzdalenostOdHrace <= utocnaVzdalenost)
        {
            UtocnaLogika();
        }
        else if (uzVidelHrace == true)
        {
            PronasledovaniHrace();
        }
        else
        {
            Patrolovani();
        }
    }

    void Patrolovani()
    {
        if (bodyTrasy.Length == 0) return;

        agent.isStopped = false;
        agent.speed = rychlostHlidkovani;
        if (anim != null) anim.SetBool("isSprinting", false);

        float vzdalenostKBodu = Vector3.Distance(transform.position, bodyTrasy[cilovyBod].position);

        if (vzdalenostKBodu < 1.5f)
        {
            cilovyBod = (cilovyBod + 1) % bodyTrasy.Length;
            agent.SetDestination(bodyTrasy[cilovyBod].position);
        }
    }

    void PronasledovaniHrace()
    {
        agent.isStopped = false;
        agent.speed = rychlostSprintu;
        agent.SetDestination(hrac.position);

        if (anim != null) anim.SetBool("isSprinting", true);
    }

    void UtocnaLogika()
    {
        agent.isStopped = true;
        if (anim != null) anim.SetBool("isSprinting", false);

        if (casovacUtoku <= 0f)
        {
            if (anim != null)
            {
                anim.ResetTrigger("Attack");
                anim.SetTrigger("Attack");
            }

            StartCoroutine(ZpozdenePoskozeni(0.5f));
            casovacUtoku = casMeziUtoky;
        }
    }

    System.Collections.IEnumerator ZpozdenePoskozeni(float zpozdeni)
    {
        yield return new WaitForSeconds(zpozdeni);
        if (hrac != null && !jeMrtvy)
        {
            float vzdalenost = Vector3.Distance(transform.position, hrac.position);
            if (vzdalenost <= utocnaVzdalenost + 0.5f)
            {
                ZdraviHrace zdravi = hrac.GetComponent<ZdraviHrace>();
                if (zdravi != null) zdravi.UtrzPoskozeni(poskozeniHrace);
            }
        }
    }

    public void UtrzPoskozeniBossa(int kolik)
    {
        if (jeMrtvy) return; 

        aktualniZdraviBossa -= kolik;

        if (hpBarBossa != null)
        {
            hpBarBossa.value = aktualniZdraviBossa;
        }

        if (aktualniZdraviBossa <= 0)
        {
            ZemriBosse();
        }
        else
        {
            casovacOmrazeniHitom = 0.4f;
            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;

            if (anim != null)
            {
                anim.SetBool("isSprinting", false);
                anim.ResetTrigger("GetHit");
                anim.SetTrigger("GetHit");
            }
        }
    }

    void ZemriBosse()
    {
        jeMrtvy = true;

        if (agent != null) agent.isStopped = true;

        if (anim != null)
        {
            anim.SetBool("isSprinting", false);
            anim.ResetTrigger("Die");
            anim.SetTrigger("Die"); 
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level3")
        {
            StartCoroutine(VyhraAOchodDoMenu(3.0f));
        }
        else
        {
            Destroy(gameObject, 3.0f);
        }
    }

    System.Collections.IEnumerator VyhraAOchodDoMenu(float casCekani)
    {
        yield return new WaitForSeconds(casCekani);
        PlayerPrefs.DeleteKey("UlozenyLevelNazev");
        PlayerPrefs.DeleteKey("HracJeVArene");
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("HlavniMenu");
    }
}