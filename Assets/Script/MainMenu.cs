using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")] 
    [SerializeField] private UIDocument mainMenuUIDocument;
    [SerializeField] private GameObject mainMenu;

    [Header("Options Menu")] 
    [SerializeField] private UIDocument optionsMenuUIDocument;
    [SerializeField] private GameObject optionsMenu;

    [Header("Save Select Menu")] 
    [SerializeField] private UIDocument saveSelectMenuUIDocument;
    [SerializeField] private GameObject saveSelectMenu;
    
    private VisualElement currentMenuRootElement;

    private GameObject activeMenu;
    
    private void Start()
    {
        InitMainMenuButtons();
        InitGameObjects();
    }

    #region Init
        private void InitGameObjects()
        {
            activeMenu = optionsMenu;
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            saveSelectMenu.SetActive(false);
        }

        private void InitMainMenuButtons()
        {
            currentMenuRootElement = mainMenuUIDocument.rootVisualElement;
            
            var buttonToSet = currentMenuRootElement.Q<Button>("PlayButton");
            buttonToSet.RegisterCallback<ClickEvent>(OpenSaveSelectMenu);
        
            buttonToSet = currentMenuRootElement.Q<Button>("OptionsButton");
            buttonToSet.RegisterCallback<ClickEvent>(OpenOptionsMenu);

            buttonToSet = currentMenuRootElement.Q<Button>("QuitButton");
            buttonToSet.RegisterCallback<ClickEvent>(OpenMainMenu);
            
            Debug.Log("Main menu buttons inited");
        }

        private void InitOptionsMenuButtons()
        {
            currentMenuRootElement = optionsMenuUIDocument.rootVisualElement;
            
            var enumToSet = currentMenuRootElement.Q<EnumField>("ResolutionEnum");
            enumToSet.RegisterCallback<ChangeEvent<Enum>>(SetResolution);
            
            var toggleToSet = currentMenuRootElement.Q<Toggle>("FullscreenToggle");
            toggleToSet.RegisterCallback<ClickEvent>(ToggleFullscreen);
            
            var sliderToSet = currentMenuRootElement.Q<Slider>("MasterSlider");
            sliderToSet.RegisterCallback<ChangeEvent<float>>(MasterSlider);
            
            sliderToSet = currentMenuRootElement.Q<Slider>("MusicSlider");
            sliderToSet.RegisterCallback<ChangeEvent<float>>(MusicSlider);

            sliderToSet = currentMenuRootElement.Q<Slider>("EffectsSlider");
            sliderToSet.RegisterCallback<ChangeEvent<float>>(EffectSlider);
            
            var buttonToSet = currentMenuRootElement.Q<Button>("BackButton");
            buttonToSet.RegisterCallback<ClickEvent>(OpenMainMenu);
            
            Debug.Log("Options menu buttons inited");
        }

        private void InitSaveSelectMenuButtons()
        {
            currentMenuRootElement = saveSelectMenuUIDocument.rootVisualElement;

            var buttonToSet = currentMenuRootElement.Q<Button>("BackButton");
            buttonToSet.RegisterCallback<ClickEvent>(OpenMainMenu);
            
            for (int i = 1; i <= 3; i++)
            {
                buttonToSet = currentMenuRootElement.Q<Button>($"DeleteSlot{i}");
                buttonToSet.RegisterCallback<ClickEvent, int>(DeleteSlot, i);

                buttonToSet = currentMenuRootElement.Q<Button>($"File{i}Button");
                buttonToSet.RegisterCallback<ClickEvent, int>(LoadDataFromFile, i);
            }
            
            Debug.Log("Save select menu buttons inited");
        }
        
    #endregion

    #region OpenMenus

        private void OpenMainMenu(ClickEvent e)
        {
            Debug.Log("Open main");
            SwitchMenu(mainMenu);
            InitMainMenuButtons();
        }
        private void OpenOptionsMenu(ClickEvent e)
        {
            Debug.Log("Open opt");
            SwitchMenu(optionsMenu);
            InitOptionsMenuButtons();
        }
        
        private void OpenSaveSelectMenu(ClickEvent e)
        {
            Debug.Log("Open save select");
            SwitchMenu(saveSelectMenu);
            InitSaveSelectMenuButtons();
        }

        private void SwitchMenu(GameObject targetMenu)
        {
            Debug.Log($"Switch menu for {targetMenu.name}");
            activeMenu.SetActive(false);
            activeMenu = targetMenu;
            activeMenu.SetActive(true);
        }
    #endregion

    #region OptionsMenu

    private void SetResolution(ChangeEvent<Enum> e)
    {
        Debug.Log($"Resolution : {currentMenuRootElement.Q<EnumField>("ResolutionEnum").value}");
        PlayerPrefs.SetString("Resolution", currentMenuRootElement.Q<EnumField>("ResolutionEnum").value.ToString());
    }

    private void ToggleFullscreen(ClickEvent e)
    {
        bool fullscreenIsToggled = currentMenuRootElement.Q<Toggle>("FullscreenToggle").value;
        Debug.Log($"Fullscreen : {fullscreenIsToggled}");
        PlayerPrefs.SetInt("ToggleFullscreen", fullscreenIsToggled ? 1 : 0);
    }
    
    private void MasterSlider(ChangeEvent<float> e)
    {
        Debug.Log($"Master volume : {Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("MasterSlider").value)}");
        PlayerPrefs.SetInt("MasterVolume", Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("MasterSlider").value));;
    }

    private void MusicSlider(ChangeEvent<float> e)
    {
        Debug.Log($"Music volume : {Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("MusicSlider").value)}");
        PlayerPrefs.SetInt("MusicVolume", Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("MusicSlider").value));;
    }

    private void EffectSlider(ChangeEvent<float> e)
    {
        Debug.Log($"Effects volume : {Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("EffectsSlider").value)}");
        PlayerPrefs.SetInt("EffectsVolume", Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("EffectsSlider").value));;
    }
    #endregion

    #region SaveSelectMenu

    private void DeleteSlot(ClickEvent e, int i)
    {
        Debug.Log($"Delete save in slot : {i}");
    }

    private void LoadDataFromFile(ClickEvent e, int index)
    {
        var playerName = currentMenuRootElement.Q<Label>($"Slot{index}PlayerName").text;
        var playerLevel = currentMenuRootElement.Q<Label>($"Slot{index}PlayerLevel").text;
        var playerCurrentZone = currentMenuRootElement.Q<Label>($"Slot{index}ZoneName").text;
        var playerPlaytime = currentMenuRootElement.Q<Label>($"Slot{index}Playtime").text;
        Debug.Log($"Infos : {playerName}//{playerLevel}//{playerCurrentZone}//{playerPlaytime}");
    }
    
    private void ReadSaveData(PseudoSaveFileSO file, int slotIndex)
    {
        
    }
    
    #endregion
    
    private void QuitGame(ClickEvent e)
    {
        Debug.Log("Quit Game.");
        Application.Quit();
    }
    
}
