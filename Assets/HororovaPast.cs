using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class HororovaPast : MonoBehaviour
{
    [Header("ROZCESTNÍK")]
    public bool jsemVLevelu2 = false; 

    [Header("Nastavení pro Level 1")]
    public GameObject[] figuriny;

    [Header("Stav - pro L2 zbytečné")]
    public bool pastUzSklapla = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (jsemVLevelu2)
        {
            RestartujLevel();
            return;
        }
        if (pastUzSklapla)
        {
            RestartujLevel();
        }
        else
        {
            pastUzSklapla = true;
            SpustSekvenciVmistnosti();
            gameObject.SetActive(false);
        }
    }

    private void SpustSekvenciVmistnosti()
    {
        SpravceMistnosti mistnost = NajdiNejblizsiMistnost();
        if (mistnost != null)
        {
            if (mistnost.dvereOdMistnosti != null)
                mistnost.dvereOdMistnosti.ZabouchniAZamkni();

            if (mistnost.puzzleVMistnosti != null)
                mistnost.puzzleVMistnosti.SetActive(true);
            if (figuriny != null)
            {
                foreach (GameObject f in figuriny) { if (f != null) f.SetActive(false); }
                int limit = Mathf.Min(new int[] { 2, figuriny.Length, mistnost.bodyProFiguriny.Length });

                for (int i = 0; i < limit; i++)
                {
                    if (figuriny[i] != null)
                    {
                        NavMeshAgent agent = figuriny[i].GetComponent<NavMeshAgent>();
                        if (agent != null)
                        {
                            agent.Warp(mistnost.bodyProFiguriny[i].position);
                        }
                        else
                        {
                            figuriny[i].transform.position = mistnost.bodyProFiguriny[i].position;
                        }

                        figuriny[i].SetActive(true);
                        Debug.Log("Spawnuji figurínu č. " + (i + 1));
                    }
                }
            }
        }
        GhostAI mozek = GetComponent<GhostAI>();
        if (mozek != null) mozek.DuchNalezen();
    }

    private void RestartujLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private SpravceMistnosti NajdiNejblizsiMistnost()
    {
        SpravceMistnosti[] vsechnyMistnosti = Object.FindObjectsOfType<SpravceMistnosti>();
        SpravceMistnosti nejblizsi = null;
        float nejmensiVzdalenost = Mathf.Infinity;
        foreach (var m in vsechnyMistnosti)
        {
            float vzd = Vector3.Distance(transform.position, m.transform.position);
            if (vzd < nejmensiVzdalenost) { nejmensiVzdalenost = vzd; nejblizsi = m; }
        }
        return nejblizsi;
    }
}