using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputRelaySource : MonoBehaviour
{
    [SerializeField] LayerMask RayCastMask = ~0; //defaults mask to do raycast hit checking on everything, change in inspector
    [SerializeField] float RaycastDistance = 10f;
    [SerializeField] UnityEvent<Vector2> OnCursorInput = new UnityEvent<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitResult;
        if(Physics.Raycast(mouseRay, out hitResult, RaycastDistance, RayCastMask, QueryTriggerInteraction.Ignore))
        {
            if(hitResult.collider.gameObject != gameObject)
            {
                return;
            }
            OnCursorInput.Invoke(hitResult.textureCoord);
        }
    }
}
