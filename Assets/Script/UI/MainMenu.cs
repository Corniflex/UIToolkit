using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [Header("Panel Settings")] 
    [SerializeField] private PanelSettings panelSettings;
    [SerializeField] private ThemeStyleSheet[] themes;
    
    [Header("Main Menu")] 
    [SerializeField] private UIDocument mainMenuUIDocument;
    [SerializeField] private GameObject mainMenu;

    [Header("Options Menu")] 
    [SerializeField] private UIDocument optionsMenuUIDocument;
    [SerializeField] private GameObject optionsMenu;

    [Header("Save Select Menu")] 
    [SerializeField] private UIDocument saveSelectMenuUIDocument;
    [SerializeField] private GameObject saveSelectMenu;

    [Header("Pseudo Save files")] 
    [SerializeField] private PseudoSaveFileSO[] pseudoSaveFiles;
    
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
            buttonToSet.RegisterCallback<ClickEvent>(QuitGame);
            
            Debug.Log("Main menu buttons inited");
        }

        private void InitOptionsMenuButtons()
        {
            currentMenuRootElement = optionsMenuUIDocument.rootVisualElement;
            
            var dropDownToSet = currentMenuRootElement.Q<DropdownField>("ResolutionDropdown");
            
            dropDownToSet.choices = InitResolutions();
            dropDownToSet.value = dropDownToSet.choices[0];
            dropDownToSet.RegisterValueChangedCallback(SetResolution);
            
            dropDownToSet = currentMenuRootElement.Q<DropdownField>("ThemeDropdown");
            dropDownToSet.choices = new List<string>(){ "Default" , "Christmas"} ;
            dropDownToSet.value = dropDownToSet.choices[0];
            dropDownToSet.RegisterValueChangedCallback(ChangeTheme);
            
            
            var toggleToSet = currentMenuRootElement.Q<Toggle>("FullscreenToggle");
            toggleToSet.RegisterCallback<ClickEvent>(ToggleFullscreen);
            toggleToSet.value = PlayerPrefs.GetInt("ToggleFullscreen") == 1;
            
            var sliderToSet = currentMenuRootElement.Q<Slider>("MasterSlider");
            sliderToSet.RegisterCallback<ChangeEvent<float>>(MasterSlider);
            sliderToSet.value = PlayerPrefs.GetInt("MasterVolume");
            
            sliderToSet = currentMenuRootElement.Q<Slider>("MusicSlider");
            sliderToSet.RegisterCallback<ChangeEvent<float>>(MusicSlider);
            sliderToSet.value = PlayerPrefs.GetInt("MusicVolume");

            sliderToSet = currentMenuRootElement.Q<Slider>("EffectsSlider");
            sliderToSet.RegisterCallback<ChangeEvent<float>>(EffectSlider);
            sliderToSet.value = PlayerPrefs.GetInt("EffectsVolume");
            
            var buttonToSet = currentMenuRootElement.Q<Button>("BackButton");
            buttonToSet.RegisterCallback<ClickEvent>(OpenMainMenu);

            buttonToSet = currentMenuRootElement.Q<Button>("ReturnButton");
            buttonToSet.style.display = DisplayStyle.None;
            
            Debug.Log("Options menu buttons inited");
        }

        private void InitSaveSelectMenuButtons()
        {
            currentMenuRootElement = saveSelectMenuUIDocument.rootVisualElement;

            var buttonToSet = currentMenuRootElement.Q<Button>("BackButton");
            buttonToSet.RegisterCallback<ClickEvent>(OpenMainMenu);
            
            for (int i = 0; i < pseudoSaveFiles.Length; i++)
            {
                DisplaySaveSlot(i);
            }

            for (int i = pseudoSaveFiles.Length; i < 4; i++)
            {
                DisplayAvailableSlot(i);
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

    private List<String> InitResolutions()
    {
        var resList = new List<String>();
        foreach (var res in Screen.resolutions)
        {
            resList.Add(res.ToString());
        }

        return resList;
    }
    
    private void SetResolution(ChangeEvent<string> e)
    {
        Debug.Log($"Resolution : {currentMenuRootElement.Q<DropdownField>("ResolutionDropdown").value}");
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
        bool fullscreenIsToggled = currentMenuRootElement.Q<Toggle>("FullscreenToggle").value;
        PlayerPrefs.SetInt("ToggleFullscreen", fullscreenIsToggled ? 1 : 0);
    }
    
    private void MasterSlider(ChangeEvent<float> e)
    {
        PlayerPrefs.SetInt("MasterVolume", Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("MasterSlider").value));;
    }

    private void MusicSlider(ChangeEvent<float> e)
    {
        PlayerPrefs.SetInt("MusicVolume", Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("MusicSlider").value));;
    }

    private void EffectSlider(ChangeEvent<float> e)
    {
        PlayerPrefs.SetInt("EffectsVolume", Mathf.RoundToInt(currentMenuRootElement.Q<Slider>("EffectsSlider").value));;
    }
    #endregion

    #region SaveSelectMenu

    private void DisplaySaveSlot(int i)
    {
        var buttonToSet = currentMenuRootElement.Q<Button>($"DeleteSlot{i+1}");
        buttonToSet.RegisterCallback<ClickEvent, int>(DeleteSlot, i+1);

        buttonToSet = currentMenuRootElement.Q<Button>($"File{i+1}Button");
        buttonToSet.RegisterCallback<ClickEvent, int>(LoadGame, i+1);

        currentMenuRootElement.Q<Label>($"Slot{i+1}PlayerName").text = pseudoSaveFiles[i].CharacterName;
        currentMenuRootElement.Q<Label>($"Slot{i+1}PlayerLevel").text = pseudoSaveFiles[i].CharacterLevel.ToString();
        currentMenuRootElement.Q<Label>($"Slot{i+1}ZoneName").text = pseudoSaveFiles[i].CharacterCurrentZoneName;
        currentMenuRootElement.Q<Label>($"Slot{i+1}Playtime").text = pseudoSaveFiles[i].playTime;

        currentMenuRootElement.Q<GroupBox>($"File{i + 1}NoData").style.display = DisplayStyle.None;
        currentMenuRootElement.Q<Toggle>($"File{i + 1}Toggle").style.display = DisplayStyle.None;
        currentMenuRootElement.Q<GroupBox>($"File{i+1}Data").style.display = DisplayStyle.Flex;
        currentMenuRootElement.Q<Button>($"DeleteSlot{i + 1}").style.display = DisplayStyle.Flex;
    }

    private void DisplayAvailableSlot(int i)
    {
        var buttonToSet = currentMenuRootElement.Q<Button>($"File{i+1}Button");
        buttonToSet.RegisterCallback<ClickEvent>(LoadNewGame);
        currentMenuRootElement.Q<GroupBox>($"File{i+1}Data").style.display = DisplayStyle.None;
        currentMenuRootElement.Q<Button>($"DeleteSlot{i + 1}").style.display = DisplayStyle.None;
        currentMenuRootElement.Q<GroupBox>($"File{i + 1}NoData").style.display = DisplayStyle.Flex;
        currentMenuRootElement.Q<Toggle>($"File{i + 1}Toggle").style.display = DisplayStyle.Flex;
        currentMenuRootElement.Q<Toggle>($"File{i + 1}Toggle").RegisterCallback<ClickEvent>(EnablePermadeath);
                
    }
    
    private void DeleteSlot(ClickEvent e, int i)
    {
        DisplayAvailableSlot(i-1);
        Debug.Log($"Delete save in slot : {i}");
    }

    private void EnablePermadeath(ClickEvent e)
    {
        Debug.Log("Careful, permadeath means you only get one try.");
    }
    
    private void LoadDataFromFile(int index)
    {
        var playerName = currentMenuRootElement.Q<Label>($"Slot{index}PlayerName").text;
        var playerLevel = currentMenuRootElement.Q<Label>($"Slot{index}PlayerLevel").text;
        var playerCurrentZone = currentMenuRootElement.Q<Label>($"Slot{index}ZoneName").text;
        var playerPlaytime = currentMenuRootElement.Q<Label>($"Slot{index}Playtime").text;
        Debug.Log($"Infos : {playerName}//{playerLevel}//{playerCurrentZone}//{playerPlaytime}");
        
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("PlayerLevel", playerLevel);
        PlayerPrefs.SetString("PlayerCurrentZone", playerCurrentZone);
        PlayerPrefs.SetString("PlayerPlaytime", playerPlaytime);
    }
    
    private void ReadSaveData(PseudoSaveFileSO file, int slotIndex)
    {
        
    }
    
    #endregion

    private void LoadNewGame(ClickEvent e)
    {
        SceneManager.LoadScene(1);
    }
    private void LoadGame(ClickEvent e, int index)
    {
        LoadDataFromFile(index);
        SceneManager.LoadScene(1);
    }
    
    private void QuitGame(ClickEvent e)
    {
        Debug.Log("Quit Game.");
        Application.Quit();
    }
}
