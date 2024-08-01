using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Don't forget to set this to TRUE or expose it to the Inspector else it will always be false and the script will not work
    [SerializeField]
    private float timeToWait = 0.3f;

    private bool isDraggable = false;

    private bool draggingSlot;
    private ScrollRect scrollRect;
    private Vector3 offset;

    public event Action<Vector3> OnReleasedObject;
    public event Action OnGrabbedObject;

    private void Awake()
    {
        scrollRect = transform.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void SetDraggable(bool isDraggable)
    {
        this.isDraggable = isDraggable;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }

        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);
        draggingSlot = true;

        offset = transform.position - Input.mousePosition;
        OnGrabbedObject?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingSlot)
        {
            transform.position = Input.mousePosition + offset;
        }
        else
        {
            //OR DO THE SCROLLRECT'S
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
        if (draggingSlot)
        {
            //END YOUR DRAGGING HERE
            draggingSlot = false;
            OnReleasedObject?.Invoke(transform.position);
        }
    }
}