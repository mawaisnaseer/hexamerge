using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaBoard : MonoBehaviour
{
    [SerializeField] List<HexaBase> hexaBoardList;
    // Start is called before the first frame update
    void Start()
    {

    }



    public bool AllBoardFilled()
    {
        foreach (HexaBase hexaBoard in hexaBoardList)
        {
            if (!hexaBoard.IsThisBaseOccupied)
            {
                return false;
              
            }

        }

        return true;
    }
}
