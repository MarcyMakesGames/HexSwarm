﻿using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] protected GameObject mainCanvas;
    [SerializeField] protected GameObject optionsCanvas;

    protected bool optionsActive = false;

    public bool OptionsActive { get => optionsActive; }

    public void ToggleCanvases()
    {
        optionsActive = !optionsActive;
        
        optionsCanvas.SetActive(optionsActive);
        mainCanvas.SetActive(!optionsActive);
        ConfigManager.instance.ToggleCameraControls(optionsActive);
    }

    protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvases();
        }
    }
}
