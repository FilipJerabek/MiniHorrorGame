using UnityEngine;

public class ZabouchnutiDveri : MonoBehaviour
{
    [Header("Dveře, které se mají zabouchnout")]
    public Door vchodoveDvere;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (vchodoveDvere != null)
            {
                vchodoveDvere.ZabouchniAZamkni();
            }
            gameObject.SetActive(false);
        }
    }
}