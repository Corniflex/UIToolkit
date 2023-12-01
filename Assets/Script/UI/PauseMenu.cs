using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIDocument optionsUI;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private PanelSettings panelSettings;
    [SerializeField] private ThemeStyleSheet[] themes;
    private VisualElement optionsRootElement;

    public bool isOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        optionsMenu.SetActive(isOpen);

        if (isOpen)
        {
            InitPauseMenuButtons();
        }
    }
    public void ToggleMenu(ClickEvent e)
    {
        isOpen = false;
        optionsMenu.SetActive(false);
    }
    
    private void InitPauseMenuButtons()
    {
        optionsRootElement = optionsUI.rootVisualElement;
        
        var dropDownToSet = optionsRootElement.Q<DropdownField>("ResolutionDropdown");
        var resList = new List<String>();
        foreach (var res in Screen.resolutions)
        {
            resList.Add(res.ToString());
        }
        dropDownToSet.choices = resList;
        dropDownToSet.value = dropDownToSet.choices[0];
        dropDownToSet.RegisterCallback<ChangeEvent<Enum>>(SetResolution);
        
        dropDownToSet = optionsRootElement.Q<DropdownField>("ThemeDropdown");
        dropDownToSet.choices = new List<string>(){ "Default" , "Christmas"} ;
        dropDownToSet.value = dropDownToSet.choices[0];
        dropDownToSet.RegisterValueChangedCallback(ChangeTheme);
        
        var toggleToSet = optionsRootElement.Q<Toggle>("FullscreenToggle");
        toggleToSet.RegisterCallback<ClickEvent>(ToggleFullscreen);
        toggleToSet.value = PlayerPrefs.GetInt("ToggleFullscreen") == 1;
            
        var sliderToSet = optionsRootElement.Q<Slider>("MasterSlider");
        sliderToSet.RegisterCallback<ChangeEvent<float>>(MasterSlider);
        sliderToSet.value = PlayerPrefs.GetInt("MasterVolume");
            
        sliderToSet = optionsRootElement.Q<Slider>("MusicSlider");
        sliderToSet.RegisterCallback<ChangeEvent<float>>(MusicSlider);
        sliderToSet.value = PlayerPrefs.GetInt("MusicVolume");

        sliderToSet = optionsRootElement.Q<Slider>("EffectsSlider");
        sliderToSet.RegisterCallback<ChangeEvent<float>>(EffectSlider);
        sliderToSet.value = PlayerPrefs.GetInt("EffectsVolume");
            
        var buttonToSet = optionsRootElement.Q<Button>("BackButton");
        buttonToSet.RegisterCallback<ClickEvent>(ToggleMenu);
        
        buttonToSet = optionsRootElement.Q<Button>("ReturnButton");
        buttonToSet.RegisterCallback<ClickEvent>(ReturnToMainMenu);
    }

    private void ReturnToMainMenu(ClickEvent e)
    {
        SceneManager.LoadScene(0);
    }
    
    private void SetResolution(ChangeEvent<Enum> e)
    {
        Debug.Log($"Resolution : {optionsRootElement.Q<EnumField>("ResolutionEnum").value}");
        //PlayerPrefs.SetString("Resolution", currentMenuRootElement.Q<EnumField>("ResolutionEnum").value.ToString());
    }
    
    private void ChangeTheme(ChangeEvent<string> e)
    {
        Debug.Log(e.newValue);
        if (e.newValue == "Default")
            panelSettings.themeStyleSheet = themes[0];
        if(e.newValue == "Christmas")
            panelSettings.themeStyleSheet = themes[1];
    }
    
    private void ToggleFullscreen(ClickEvent e)
    {
        bool fullscreenIsToggled = optionsRootElement.Q<Toggle>("FullscreenToggle").value;
        Debug.Log($"Fullscreen : {fullscreenIsToggled}");
        PlayerPrefs.SetInt("ToggleFullscreen", fullscreenIsToggled ? 1 : 0);
    }
    
    private void MasterSlider(ChangeEvent<float> e)
    {
        Debug.Log($"Master volume : {Mathf.RoundToInt(optionsRootElement.Q<Slider>("MasterSlider").value)}");
        PlayerPrefs.SetInt("MasterVolume", Mathf.RoundToInt(optionsRootElement.Q<Slider>("MasterSlider").value));;
    }

    private void MusicSlider(ChangeEvent<float> e)
    {
        Debug.Log($"Music volume : {Mathf.RoundToInt(optionsRootElement.Q<Slider>("MusicSlider").value)}");
        PlayerPrefs.SetInt("MusicVolume", Mathf.RoundToInt(optionsRootElement.Q<Slider>("MusicSlider").value));;
    }

    private void EffectSlider(ChangeEvent<float> e)
    {
        Debug.Log($"Effects volume : {Mathf.RoundToInt(optionsRootElement.Q<Slider>("EffectsSlider").value)}");
        PlayerPrefs.SetInt("EffectsVolume", Mathf.RoundToInt(optionsRootElement.Q<Slider>("EffectsSlider").value));;
    }
}
