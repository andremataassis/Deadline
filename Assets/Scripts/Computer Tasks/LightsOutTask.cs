using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightsOutTask : MonoBehaviour
{
    bool[] grid = new bool[9];
    public Color on;
    public Color off;

    // Start is called before the first frame update
    void Start()
    {
        createGrid();
        updateAllButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkAllSameColor())
        {
            GameManager.Instance.taskDone();
        }
    }
    public void buttonClicked(Button btn)
    {
        PlayAudioHandler.Instance.playRandomAudio("ComputerClick", 5);
        int square = int.Parse(btn.name);
        
        //reverse button pressed
        grid[square] = !grid[square];

        int above = square - 3;
        int below = square + 3;
        int left = square - 1;
        int right = square + 1;

        if(square != 2 && square != 5 && square != 8) //reverse right if not on right row
        {
            grid[right] = !grid[right];
        }
        if(square != 0 && square != 3 && square != 6) //reverse left if not on left row
        {
            grid[left] = !grid[left];
        }
        if (above >= 0) //reverse top if valid
        {
            grid[above] = !grid[above];
        }
        if (below < 9) //reverse below if valid
        {
            grid[below] = !grid[below];
        }

        updateAllButtons();
    }

    //updates buttons in game to reflect current grid status
    private void updateAllButtons()
    {
        for(int i = 0; i < grid.Length; i++)
        {
            Image button = transform.GetChild(i).GetComponentInChildren<Image>();
            if (grid[i])
            {
                button.color = on;
            }
            else
            {
                button.color = off;
            }
        }
    }
    private void createGrid()
    {
        bool validity = false;
        while(validity == false)
        {
            for (int i = 0; i < 9; i++)
            {
                grid[i] = Random.Range(0, 2) == 1;
            }
            validity = !(checkAllSameColor());
        }
    }

    //returns true if grid is all zeroes
    private bool checkAllSameColor()
    {
        int trues = 0;
        int falses = 0;
        for (int i = 0; i < 9; i++)
        {
            if(grid[i] == false) falses++;
            if (grid[i] == true) trues++;
        }
        return trues == 9 || falses == 9;
    }
}
