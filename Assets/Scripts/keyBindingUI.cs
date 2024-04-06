using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingUI : MonoBehaviour
{
    public GameObject ResetButton;
    public GameObject TimeCascadeButton;
    public GameObject ToggleGridButton;
    public GameObject ChangeViewButton;
    public GameObject ToggleUIButton;
    public GameObject PauseButton;
    public GameObject KeyUi;

    private bool isPaused = true;
    private bool timeCascade = false;
    private bool viewChange = false;

    private Dictionary<KeyCode, System.Action> keyDownActions;
    private Dictionary<KeyCode, System.Action> keyUpActions;
    public static KeyBindingUI instance;
    private void Awake()
    {
        keyDownActions = new Dictionary<KeyCode, System.Action>
        {
            { KeyCode.R, () => SetAlpha(ResetButton, 1) },
            { KeyCode.Z, () => ToggleTimeCascade() },
            { KeyCode.G, () => SetAlpha(ToggleGridButton, 1) },
            { KeyCode.Space, () => TogglePause() },
            { KeyCode.T, () => ToggleUI() }
        };

        keyUpActions = new Dictionary<KeyCode, System.Action>
        {
            { KeyCode.R, () => SetAlpha(ResetButton, 0.3f) },
            { KeyCode.Z, () => SetAlpha(TimeCascadeButton, timeCascade ? 1 : 0.3f) }, // Reflects current state on key release
            { KeyCode.G, () => SetAlpha(ToggleGridButton, 0.3f) },
            { KeyCode.T, () => SetAlpha(ToggleUIButton, 0.3f) }
        };
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public void ToggleViewChange()
    {
        viewChange = !viewChange;
        SetAlpha(ChangeViewButton, viewChange ? 1 : 0.3f);
    }

    private void Start()
    {
        // Initial update of buttons based on their state
        SetAlpha(PauseButton, isPaused ? 1 : 0.3f);
        SetAlpha(TimeCascadeButton, timeCascade ? 1 : 0.3f);
    }

    private void Update()
    {
        foreach (var action in keyDownActions)
        {
            if (Input.GetKeyDown(action.Key))
            {
                action.Value.Invoke();
            }
        }

        foreach (var action in keyUpActions)
        {
            if (Input.GetKeyUp(action.Key))
            {
                action.Value.Invoke();
            }
        }
    }

    private void ToggleTimeCascade()
    {
        timeCascade = !timeCascade;
        SetAlpha(TimeCascadeButton, timeCascade ? 1 : 0.3f);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        SetAlpha(PauseButton, isPaused ? 1 : 0.3f);
    }

    private void ToggleUI()
    {
        SetAlpha(ToggleUIButton, 1);
        KeyUi.SetActive(!KeyUi.activeSelf);
    }

    private void SetAlpha(GameObject button, float alpha)
    {
        button.GetComponent<CanvasGroup>().alpha = alpha;
    }
}
