using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Camera mainCamera;

    private bool isDragging = false;
    private Vector3 offset;
    private GameObject objectToDrag;
    private Draggable dragabel;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if the mainCamera is assigned
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera is not assigned.");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            dragabel = hit.collider.gameObject.GetComponent<Draggable>();
            // Check if the hit object has a collider and is draggable
            if (hit.collider != null && dragabel != null)
            {
                if (!dragabel.CanBeDgragged)
                    return;
                // Start dragging
                dragabel.SetDraggable(true);
                isDragging = true;
                objectToDrag = hit.collider.gameObject;
                // Calculate the offset to maintain the relative position of the cursor to the object
                offset = objectToDrag.transform.position - hit.point;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && objectToDrag != null)
        {

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newPosition = hit.point + offset;
                // Keep the object on the X-Z plane
                newPosition.y = 12;// objectToDrag.transform.position.y;
                objectToDrag.transform.position = newPosition;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop dragging
        isDragging = false;
        objectToDrag = null;
        if (dragabel)
        {
            dragabel.SetDraggable(false);
            dragabel = null;
        }
    }


}
