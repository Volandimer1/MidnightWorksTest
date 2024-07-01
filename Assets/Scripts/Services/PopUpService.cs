using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpService
{
    private const string SingleButtonPopUpPath = "SingleButtonPopUp";
    private const string TwoButtonsPopUpPath = "TwoButtonsPopUp";
    private const string OptionsPopUpPath = "UI/Popups/OptionsCanvas";

    public void ShowSingleButtonPopUp(string messageText, string buttonText, Action onButtonPressed)
    {
        GameObject singleButtonPopUpPrefab = Resources.Load<GameObject>(SingleButtonPopUpPath);
        if (singleButtonPopUpPrefab == null)
        {
            Debug.LogError($"Failed to load {SingleButtonPopUpPath}");
            return;
        }
        GameObject popup = UnityEngine.Object.Instantiate(singleButtonPopUpPrefab);

        popup.GetComponentInChildren<TextMeshProUGUI>().text = messageText;

        Button button1 = popup.GetComponentInChildren<Button>();
        button1.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        button1.onClick.AddListener(() =>
        {
            onButtonPressed?.Invoke();

            UnityEngine.Object.Destroy(popup);
            Resources.UnloadUnusedAssets();
        });
    }

    public void ShowTwoButtonsPopUp(string messageText, string button1Text, string button2Text, Action onButton1Pressed, Action onButton2Pressed)
    {
        GameObject TwoButtonsPopUpPrefab = Resources.Load<GameObject>(TwoButtonsPopUpPath);
        if (TwoButtonsPopUpPrefab == null)
        {
            Debug.LogError($"Failed to load {TwoButtonsPopUpPath}");
            return;
        }
        GameObject popup = UnityEngine.Object.Instantiate(TwoButtonsPopUpPrefab);

        popup.GetComponentInChildren<TextMeshProUGUI>().text = messageText;

        Button[] buttons = popup.GetComponentsInChildren<Button>();

        buttons[0].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = button1Text;
        buttons[0].onClick.AddListener(() =>
        {
            onButton1Pressed?.Invoke();

            UnityEngine.Object.Destroy(popup);
            Resources.UnloadUnusedAssets();
        });

        buttons[1].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = button2Text;
        buttons[1].onClick.AddListener(() =>
        {
            onButton2Pressed?.Invoke();

            UnityEngine.Object.Destroy(popup);
            Resources.UnloadUnusedAssets();
        });
    }

    public void ShowOptionsPopUp(GameStateMachine gameStateMachine)
    {
        GameObject optionsPrefab = Resources.Load<GameObject>(OptionsPopUpPath);
        if (optionsPrefab == null)
        {
            Debug.LogError($"Failed to load {OptionsPopUpPath}");
            return;
        }
        GameObject popup = UnityEngine.Object.Instantiate(optionsPrefab);

        Button[] buttons = popup.GetComponentsInChildren<Button>();
        Slider[] sliders = popup.GetComponentsInChildren<Slider>();

        Settings optionSettings = OptionSettings.LoadSettings();
        
        sliders[0].value = optionSettings.SFXVolume;
        sliders[1].value = optionSettings.MusicVolume;

        buttons[0].onClick.AddListener(() =>
        {
            UnityEngine.Object.Destroy(popup);
            Resources.UnloadUnusedAssets();
        });

        AudioManager audioManager = gameStateMachine.AudioManager;

        sliders[0].onValueChanged.AddListener((float value) =>
        {
            audioManager.SetSFXVolume(value);
            OptionSettings.SaveSettings(value, sliders[1].value);
        });

        sliders[1].onValueChanged.AddListener((float value) =>
        {
            audioManager.SetMusicVolume(value);
            OptionSettings.SaveSettings(sliders[0].value, value);
        });
    }
}
