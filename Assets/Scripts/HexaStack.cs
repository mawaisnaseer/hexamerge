using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HexaStack : MonoBehaviour
{
    public List<Hexa> hexagons = new List<Hexa>();
    [SerializeField] private float yOffset = 0.2f; // Vertical offset
    public float sphereRadius = 1f;
    public float maxDistance = 10f;
    [SerializeField] private List<HexaStack> adJacentHexaStacks = new List<HexaStack>();
    private Draggable myDragableComponent;
    private bool checkIFStackFillled;
    private void Start()
    {
        checkIFStackFillled = false;
        myDragableComponent = GetComponent<Draggable>();
        RepositionHexagons();
    }

    void RepositionHexagons()
    {
        // Ensure there are hexagons in the stack
        if (hexagons.Count == 0)
            return;

        // Calculate the initial position for the bottom hexagon
        Vector3 bottomHexPosition = transform.position;

        // Iterate through the hexagons list (excluding the bottom hexagon)
        for (int i = 1; i < hexagons.Count; i++)
        {
            // Calculate the new position for the current hexagon above the bottom one
            Vector3 newPosition = bottomHexPosition + Vector3.up * yOffset * i;

            // Set the position of the hexagon
            hexagons[i].transform.position = newPosition;
        }
    }
    public Hexa Peek()
    {
        if (hexagons.Count == 0)
            return null;

        return hexagons[hexagons.Count - 1];
    }

    public bool IsEmpty()
    {
        return hexagons.Count == 0;
    }

    public void Push(Hexa hexagon)
    {
        if (!IsEmpty())
        {
        }

        hexagons.Add(hexagon);

        hexagon.transform.SetParent(transform); // Make the hexagon a child of the stack
        // Update position and other necessary adjustments
        Vector3 newPosition = transform.position + Vector3.up * yOffset * (hexagons.Count - 1); // Calculate new position with offset
        hexagon.transform.DOMove(newPosition + new Vector3(0, 1, 0), 0.5f).OnComplete(() =>
        {
            hexagon.transform.DOMove(newPosition, 0.5f);
        });

    }

    public void Pop()
    {
        if (!IsEmpty())
        {
            Hexa hexagon = hexagons[hexagons.Count - 1];
            hexagons.Remove(hexagon);
            // Update position and other necessary adjustments
        }
    }

    bool IsMyStackIsSingleColor()
    {
        if (IsEmpty())
            return false;
        ColorCodes bottomHexaColor = hexagons[0].color;
        foreach (Hexa hexa in hexagons)
        {
            if (hexa.color != bottomHexaColor)
            {
                return false;
            }
        }

        return true;
    }

    bool isFindCoroutineRunning;
    public void FindHexaStack()
    {
        if (!isFindCoroutineRunning)
        {
            isFindCoroutineRunning = true;
            StartCoroutine(FindAdjacentStacks());
        }
    }

    IEnumerator FindAdjacentStacks()
    {
        // Perform the sphere caste
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, transform.forward, maxDistance);

        // Draw a debug sphere to visualize the sphere cast
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 5);

        // Clear the list before populating it again
        adJacentHexaStacks.Clear();

        // Iterate through all hits and collect the HexaStack objects
        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.GetComponent<HexaStack>())
            {
                adJacentHexaStacks.Add(hitObject.GetComponent<HexaStack>());
            }
        }

        // Now you have all HexaStack objects in the list
        // You can do whatever you want with them
        foreach (HexaStack hexaStackObject in adJacentHexaStacks)
        {
            if (hexaStackObject.Peek().color == Peek().color)
            {
                print("Matching stack found" + hexaStackObject.name);
                // Check if the current stack is single-colored
                if (IsMyStackIsSingleColor())
                {
                    // Transfer hexagons from the adjacent stack to your stack
                    yield return StartCoroutine(TransferHexagons(hexaStackObject));
                }
                else
                {
                    if (hexaStackObject.IsMyStackIsSingleColor())
                    {
                        print("i'm not single color but other one is " + hexaStackObject.name);
                        yield return StartCoroutine(TransferSimilarHexagons(hexaStackObject));

                      hexaStackObject.CheckIfStackCompleted();
                    }
                    else
                    {
                        print("both are not single color ");
                        yield return StartCoroutine(TransferHexagons(hexaStackObject));
                    }
                }
            }
            else
            {
                print("peak hexa color not matched");
            }

        }



        if (hexagons.Count >= 10 && IsMyStackIsSingleColor())
        {

            StartCoroutine(StackComplete());
        }
        else
        {
            isFindCoroutineRunning = false;
            print("stack not complete");
        }
    }

    public void CheckIfStackCompleted()
    {
        if (hexagons.Count >= 10 && IsMyStackIsSingleColor())
        {
            StartCoroutine(StackComplete());
        }
    }
    IEnumerator StackComplete()
    {


        yield return new WaitForSeconds(01f);
      
        print("stack  complete");
        checkIFStackFillled = false;
        int hexaCounts = hexagons.Count;

        for (int i = hexagons.Count - 1; i >= 0; i--)
        {
            if (hexagons[i])
            {
                hexagons[i].transform.DOScale(0, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }

            Pop();
        }


        print("stackComplete");

        UIController.UPdateScore(hexaCounts);
        isFindCoroutineRunning = false;
        DestroyThisEmptyStack();


    }

    IEnumerator TransferHexagons(HexaStack hexaStackObject)
    {
        // Iterate through each hexagon in the adjacent stack until a different color is found
        while (!hexaStackObject.IsEmpty() && hexaStackObject.Peek().color == Peek().color)
        {
            // Get the top hexagon of the adjacent stack
            Hexa topHexagon = hexaStackObject.Peek();

            // Transfer the top hexagon to your stack
            Push(topHexagon);

            // Remove the top hexagon from the adjacent stack
            hexaStackObject.Pop();
            yield return new WaitForSeconds(0.1f);

            if (hexaStackObject.IsEmpty())
            {
                print(hexaStackObject.name + " this is empty now");
                hexaStackObject.DestroyThisEmptyStack();
            }


        }

    }


    IEnumerator TransferSimilarHexagons(HexaStack hexaStackObject)
    {
        // Get the color of the top hexagon of the adjacent stack
        ColorCodes targetColor = hexaStackObject.Peek().color;

        // Transfer all similar-colored hexagons from the current stack to the single-colored adjacent stack
        while (!IsEmpty() && Peek().color == targetColor)
        {
            // Get the top hexagon of the current stack
            Hexa topHexagon = Peek();

            // Transfer the top hexagon to the adjacent stack
            hexaStackObject.Push(topHexagon);

            // Remove the top hexagon from the current stack
            Pop();

            // Wait for a short duration before proceeding to the next hexagon transfer
            yield return new WaitForSeconds(0.1f);
        }

        // Check for adjacent stacks with the same color on top as the transferred stack
        List<HexaStack> adjacentStacksWithSameColor = FindAdjacentStacksWithSameColor(hexaStackObject, targetColor);

        // Recursively transfer hexagons from adjacent stacks with the same color
        foreach (HexaStack adjacentStack in adjacentStacksWithSameColor)
        {
            yield return StartCoroutine(TransferSimilarHexagons(adjacentStack));
        }
    }

    List<HexaStack> FindAdjacentStacksWithSameColor(HexaStack hexaStackObject, ColorCodes color)
    {
        List<HexaStack> adjacentStacksWithSameColor = new List<HexaStack>();

        // Perform the sphere cast to find adjacent stacks
        RaycastHit[] hits = Physics.SphereCastAll(hexaStackObject.transform.position, hexaStackObject.sphereRadius, hexaStackObject.transform.forward, hexaStackObject.maxDistance);

        // Iterate through all hits and collect the HexaStack objects with the same color on top
        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            HexaStack adjacentStack = hitObject.GetComponent<HexaStack>();
            if (adjacentStack && adjacentStack.Peek().color == color && adjacentStack != hexaStackObject)
            {
                adjacentStacksWithSameColor.Add(adjacentStack);
            }
        }

        return adjacentStacksWithSameColor;
    }
    void DestroyThisEmptyStack()
    {
        if (!IsEmpty())
        {
            Debug.Log("This is not Empty and Cann't be Destryoed");
        }

        myDragableComponent.HexaBase.IsThisBaseOccupied = false;
        myDragableComponent.HexaBase.HighLight(false);

        Destroy(this.gameObject);

    }
}
