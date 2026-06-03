using UnityEngine;
using TMPro;

public class UVText : MonoBehaviour
{
    private TextMeshPro textMesh;

    public bool odemcenoPlosinami = false;

    [Header("Nastavení vzhledu")]
    [Tooltip("(0 - 255)")]
    public float cilovaPruhlednost = 35f; 

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();

        if (textMesh != null)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0f);
        }
    }

    void Update()
    {
        if (textMesh == null) return;

       
        bool svitiUV = Flashlight.UV_Aktivni;

        if (odemcenoPlosinami && svitiUV)
        {
           
            float spravnaAlfa = cilovaPruhlednost / 255f;
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, spravnaAlfa);
        }
        else
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0f);
        }
    }
}