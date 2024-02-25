#pragma warning disable 649

using UnityEngine;
using UnityEngine.EventSystems;

// UI Module v0.9.0
public class UITouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isMouseDown = false;
    private bool isOneTimeClick;
    [SerializeField] Camera raycastCamera;
    public static bool Enabled { get; set; }

    public static bool CanReplay { get; set; }



    public void OnDrag(PointerEventData eventData)
    {
        if (!isMouseDown) return;

        CanReplay = true;

        isMouseDown = false;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        var hit = Physics2D.Raycast(raycastCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
       // Debug.DrawLine(raycastCamera.ScreenToWorldPoint(Input.mousePosition), Vector3.positiveInfinity);
        if (hit)
        {
            if (hit.collider.CompareTag("Tiles"))
            {
              
                Debug.Log("object clicked: " + hit.collider.gameObject.name);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {


    }


}
