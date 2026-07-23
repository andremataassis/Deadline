using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingUIHandler : MonoBehaviour
{
    public TextMeshProUGUI bulletsDisplay;
    public GameObject panel;

    public static ShootingUIHandler Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        panel.SetActive(false);
    }

    public void updateBullets(int a, int b)
    {
        bulletsDisplay.text = a.ToString() + " / " + b.ToString();
    }

    public void shootingUI(bool a)
    {
        if (a)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
