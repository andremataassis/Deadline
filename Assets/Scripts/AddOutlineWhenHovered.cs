using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOutlineWhenHovered : MonoBehaviour
{
    Outline outline;
    
    // Start is called before the first frame update
    void Start()
    {
        outline = this.gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.white;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
