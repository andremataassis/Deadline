using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputRelaySync : MonoBehaviour
{
    [SerializeField] RectTransform CanvasTransform;

    GraphicRaycaster Raycaster;
    List<GameObject> DragTargets = new List<GameObject>();
    [SerializeField] List<GameObject> MouseOnObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Raycaster = GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCursorInput(Vector2 normalisedPosition)
    {
        // calculate the position in canvas space
        Vector3 mousePosition = new Vector3(CanvasTransform.sizeDelta.x * normalisedPosition.x,
                                            CanvasTransform.sizeDelta.y * normalisedPosition.y,
                                            0f);

        // construct our pointer event
        PointerEventData mouseEvent = new PointerEventData(EventSystem.current);
        mouseEvent.position = mousePosition;

        // perform a raycast using the graphics raycaster
        List<RaycastResult> results = new List<RaycastResult>();
        Raycaster.Raycast(mouseEvent, results);

        bool sendMouseDown = Input.GetMouseButtonDown(0);
        bool sendMouseUp = Input.GetMouseButtonUp(0);
        bool isMouseDown = Input.GetMouseButton(0);

        // send through end drag events as needed
        if (sendMouseUp)
        {
            foreach (var target in DragTargets)
            {
                if (ExecuteEvents.Execute(target, mouseEvent, ExecuteEvents.endDragHandler))
                    break;
            }
            DragTargets.Clear();
        }

        //List of GameObjects that are no longer being hovered on, so we must trigger pointerExitHandler 
        List<GameObject> MouseExitedList = new List<GameObject>(MouseOnObjects);

        // process the raycast results
        foreach (var result in results)
        {
            // setup the new event data
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePosition;
            eventData.pointerCurrentRaycast = eventData.pointerPressRaycast = result;

            // is the mouse down?
            if (isMouseDown)
                eventData.button = PointerEventData.InputButton.Left;

            var slider = result.gameObject.GetComponentInParent<UnityEngine.UI.Slider>();

            // potentially new drag targets?
            if (sendMouseDown)
            {
                if (ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.beginDragHandler))
                    DragTargets.Add(result.gameObject);

                if (slider != null)
                {
                    slider.OnInitializePotentialDrag(eventData);

                    if (!DragTargets.Contains(result.gameObject))
                        DragTargets.Add(result.gameObject);
                }
            } // need to update drag target
            else if (DragTargets.Contains(result.gameObject))
            {
                eventData.dragging = true;
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.dragHandler);
                if (slider != null)
                {
                    slider.OnDrag(eventData);
                }
            }

            // send a mouse down event?
            if (sendMouseDown)
            {
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerDownHandler);
            }
            else if (sendMouseUp)
            {
                bool didRun = ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerUpHandler);
                didRun |= ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }

            //Send mouse enter / exit
            if (MouseOnObjects.Contains(result.gameObject))
            {
                //Mouse is still hovering on it
                MouseExitedList.Remove(result.gameObject);
            }
            else
            {
                //Mouse just got hovered on
                MouseOnObjects.Add(result.gameObject);
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerEnterHandler);
                Debug.Log($"Mouse enter! - {result.gameObject.name}");
            }
        }

        //Trigger pointerExitHandler for remaining MouseExitedList objects and remove them from MouseOnObjects
        foreach (GameObject obj in MouseExitedList)
        {
            MouseOnObjects.Remove(obj);
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePosition;
            ExecuteEvents.Execute(obj, eventData, ExecuteEvents.pointerExitHandler);
            Debug.Log($"Mouse exit! - {obj.name}");
        }
    }
}


