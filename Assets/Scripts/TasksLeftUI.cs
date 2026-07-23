using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TasksLeftUI : MonoBehaviour
{
    private TextMeshProUGUI textDisplayer;

    private void Awake()
    {
        textDisplayer = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textDisplayer.text = GameManager.Instance.getCurrentTask() + " / " + GameManager.Instance.getTotalTasks();
    }
}
