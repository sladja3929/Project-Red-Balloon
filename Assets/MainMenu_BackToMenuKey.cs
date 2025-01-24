using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_BackToMenuKey : MonoBehaviour
{
    public MainMenu mainMenu;

    private void Awake()
    {
        mainMenu = FindObjectOfType<MainMenu>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.BackToMenu();
        }
    }
}