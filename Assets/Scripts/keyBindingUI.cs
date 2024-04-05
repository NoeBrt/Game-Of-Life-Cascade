using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyBindingUI : MonoBehaviour
{
    public GameObject resetButton;
    public GameObject zTimeCascadeButton;
    public GameObject gToogleGridButton;
    public GameObject eChangeViewButton;
    public GameObject tToogleUIButton;
    public GameObject spacePauseButton;

    public GameObject keyUi;
    public bool pause=true;

    // Update is called once per frame
    void Update()
    {
        // Set alpha to 1 when key is pressed down
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetButton.GetComponent<CanvasGroup>().alpha = 1;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            zTimeCascadeButton.GetComponent<CanvasGroup>().alpha = 1;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gToogleGridButton.GetComponent<CanvasGroup>().alpha = 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            eChangeViewButton.GetComponent<CanvasGroup>().alpha = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;
        }
        if (pause)
        {
                spacePauseButton.GetComponent<CanvasGroup>().alpha = 1;
        
        }
        else
            {
                spacePauseButton.GetComponent<CanvasGroup>().alpha = 0.3f;
            }
        if (Input.GetKeyDown(KeyCode.T))
        {
            tToogleUIButton.GetComponent<CanvasGroup>().alpha = 1;
            keyUi.SetActive(!keyUi.activeSelf);
        }

        // Set alpha back to 0.3 when key is released
        if (Input.GetKeyUp(KeyCode.R))
        {
            resetButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            zTimeCascadeButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            gToogleGridButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            eChangeViewButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            tToogleUIButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        }
    }
}
