using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaBase : MonoBehaviour
{

    public Material highlightMaterial;
    public Material normalMaterial;
    private bool isHexBaseLocked = false;
    private MeshRenderer meshRenderer;
    private bool baseFilled;

    public bool IsThisBaseOccupied
    {
        get => baseFilled;

        set => baseFilled = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        baseFilled = false;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = normalMaterial;
    }
    public void HighLight(bool _highlight)
    {
        meshRenderer.sharedMaterial = _highlight ? highlightMaterial : normalMaterial;
       
    }
}
