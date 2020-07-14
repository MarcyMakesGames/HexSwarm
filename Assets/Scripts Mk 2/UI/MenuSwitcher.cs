﻿using UnityEngine;


enum Derp
{
    State1 = 1,
    State2 = 2,
    State3 = 3
}
public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] protected GameObject mainCanvas;
    [SerializeField] protected GameObject optionsCanvas;
    [SerializeField] protected GameObject audioPanel;
    [SerializeField] protected GameObject cameraPanel;
    [SerializeField] protected GameObject gameoptionsPanel;

    protected bool optionsActive = false;

    public bool OptionsActive { get => optionsActive; }

    public void ToggleCanvases()
    {
        optionsActive = !optionsActive;
        
        optionsCanvas.SetActive(optionsActive);
        mainCanvas.SetActive(!optionsActive);
    }

    public void SwapOptionsPanel(int x)
    {
        if(x == 1)
        {
            audioPanel.SetActive(true);
            cameraPanel.SetActive(false);
            gameoptionsPanel.SetActive(false);
        }
        if(x == 2)
        {
            audioPanel.SetActive(false);
            cameraPanel.SetActive(true);
            gameoptionsPanel.SetActive(false);
        }
        if(x == 3)
        {
            audioPanel.SetActive(false);
            gameoptionsPanel.SetActive(false);
            cameraPanel.SetActive(true);
        }
    }

    protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvases();
        }
    }
}
