using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light svetlo;
    public Color normalni = Color.white;
    public Color uvBarva = new Color(0.5f, 0f, 1f);

    private bool jeZapnuto = true;
    private bool jeUV = false;

    public static bool UV_Aktivni = false;

    void Start()
    {
        if (svetlo != null)
        {
            svetlo.enabled = jeZapnuto;
            svetlo.color = normalni;
        }
    }

    void Update()
    {
        if (svetlo == null) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            jeZapnuto = !jeZapnuto;
            svetlo.enabled = jeZapnuto;
            AktualizujUVStav();
        }

        if (Input.GetKeyDown(KeyCode.U) && jeZapnuto)
        {
            jeUV = !jeUV;
            svetlo.color = jeUV ? uvBarva : normalni;
            AktualizujUVStav();
            Debug.Log("UV mód: " + jeUV);
        }
    }

    void AktualizujUVStav()
    {
        UV_Aktivni = jeZapnuto && jeUV;
    }
}