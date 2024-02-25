using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{

    private Vector3 firstPosition = Vector3.zero;
    private Vector3 gridPosition = Vector3.zero;
    private Quaternion gridRotation = Quaternion.identity;


    private bool isdragging = false;

    private HexaBase hexaBase;
    private bool canBeDragged;

    public HexaBase HexaBase => hexaBase;
    public bool CanBeDgragged => canBeDragged;
    // Start is called before the first frame update
    private HexaStack hexaStack;
    void Start()
    {
        canBeDragged = true;
        firstPosition = transform.position;

        hexaStack = GetComponent<HexaStack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isdragging)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position + new Vector3(0, -1, 0), transform.forward * 2, Color.red);
            if (Physics.Raycast(transform.position + new Vector3(0, -1, 0), transform.forward, out hit, 2))
            {
                if (hit.collider && hit.collider.GetComponent<HexaBase>())
                {


                    if (hexaBase == null)
                    {
                        hexaBase = hit.collider.GetComponent<HexaBase>();
                        hexaBase.HighLight(true);
                        gridPosition = hexaBase.transform.position;
                        gridRotation = hexaBase.transform.rotation;
                    }
                    else
                    {
                        hexaBase.HighLight(false);
                        hexaBase = null;

                        hexaBase = hit.collider.GetComponent<HexaBase>();
                        hexaBase.HighLight(true);
                        gridPosition = hexaBase.transform.position;
                        gridRotation = hexaBase.transform.rotation;
                    }

                }
                else if (hexaBase != null)
                {
                    hexaBase.HighLight(false);
                    gridPosition = Vector3.zero;
                    hexaBase = null;
                }
                else
                {
                    gridPosition = Vector3.zero;
                }
            }
        }
    }

    public void SetDraggable(bool _isdragable)
    {

        if (_isdragable == false)
        {
            if (gridPosition == Vector3.zero)
            {

                transform.position = firstPosition;
                canBeDragged = true;
            }
            else
            {
                if (!hexaBase.IsThisBaseOccupied)
                    hexaBase.IsThisBaseOccupied = true;
                else
                {
                    transform.position = firstPosition;
                    canBeDragged = true;
                    return;
                }

                canBeDragged = false;
                transform.position = gridPosition;
                transform.rotation = gridRotation;
                transform.SetParent(hexaBase.transform);
                GameController.OnSpawnStackPlaced?.Invoke(this);
                hexaStack.FindHexaStack();
            }
        }

        isdragging = _isdragable;

    }
}
