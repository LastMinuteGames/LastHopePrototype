﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button start;
    public Button help;
    public Button settings;
    public Button credits;
    public Button exit;
    public Text startText;
    public Text helpText;
    public Text settingsText;
    public Text creditsText;
    public Text exitText;
    public int focus;
    //Confirm exit menu
    public bool isConfirmExit = false;
    public Canvas confirmExit;
    public Button yes;
    public Button no;
    public Text yesText;
    public Text noText;
    public int focusExit;
    //Help
    public bool isHelp = false;
    public Canvas helpCanvas;
    //Settings
    public bool isSettings = false;
    public Canvas settingsCanvas;
    //Credits
    public bool isCredits = false;
    public Canvas creditsCanvas;

    public Color unselectedTextColor;
    public Color selectedTextColor;

    public Sprite unselectedBGMenuSprite;
    public Sprite selectedBGMenuSprite;
    public Sprite unselectedBGExitSprite;
    public Sprite selectedBGExitSprite;

    //Input
    bool upInUse = false;
    bool downInUse = false;
    bool leftInUse = false;
    bool rightInUse = false;

    //Audio
    private int swapSelectionFxId;
    private int applySelectionFxId;
    private int mainThemeMusicId;


    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1.0F;

        //Hide cursor
        Cursor.visible = false;

        unselectedTextColor = new Color(0.4F, 0.486F, 0.6235F, 1);
        selectedTextColor = new Color(0.612F, 0.937F, 1F, 1);

        //Boton Iniciar Partida - Initial state: Selected
        start = start.GetComponent<Button>();
        start.GetComponent<Image>().sprite = selectedBGMenuSprite;
        startText = startText.GetComponent<Text>();
        startText.color = selectedTextColor;

        //Boton Level Select - Initial state: Unselected
        help = help.GetComponent<Button>();
        help.GetComponent<Image>().sprite = unselectedBGMenuSprite;
        helpText = helpText.GetComponent<Text>();
        helpText.color = unselectedTextColor;


        //Boton Controles - Initial state: Unselected
        settings = settings.GetComponent<Button>();
        settings.GetComponent<Image>().sprite = unselectedBGMenuSprite;
        settingsText = settingsText.GetComponent<Text>();
        settingsText.color = unselectedTextColor;

        //Boton Controles - Initial state: Unselected
        credits = credits.GetComponent<Button>();
        credits.GetComponent<Image>().sprite = unselectedBGMenuSprite;
        creditsText = creditsText.GetComponent<Text>();
        creditsText.color = unselectedTextColor;

        //Boton Salida - Initial state: Unselected
        exit = exit.GetComponent<Button>();
        exit.GetComponent<Image>().sprite = unselectedBGMenuSprite;
        exitText = exitText.GetComponent<Text>();
        exitText.color = unselectedTextColor;

        //focus manages button focus logic - initiate 0
        focus = 0;

        //Manages the confirm exit canvas (sets it to invisible)
        confirmExit = confirmExit.GetComponent<Canvas>();
        confirmExit.gameObject.SetActive(false);

        //Flag isConfirmExit to false
        isConfirmExit = false;

        //Boton SI salir
        yes = yes.GetComponent<Button>();
        yes.GetComponent<Image>().sprite = unselectedBGExitSprite;
        yesText = yesText.GetComponent<Text>();
        yesText.color = unselectedTextColor;

        //Boton NO salir
        no = no.GetComponent<Button>();
        no.GetComponent<Image>().sprite = unselectedBGExitSprite;
        noText = noText.GetComponent<Text>();
        noText.color = unselectedTextColor;

        //focus exit menu
        focusExit = 1;

        //HelpCanvas
        helpCanvas = helpCanvas.GetComponent<Canvas>();
        helpCanvas.gameObject.SetActive(false);

        //Flag isHelp to false
        isHelp = false;

        //SettingsCanvas
        settingsCanvas = settingsCanvas.GetComponent<Canvas>();
        settingsCanvas.gameObject.SetActive(false);

        //Flag isSettings to false
        isSettings = false;

        //CreditCanvas
        creditsCanvas = creditsCanvas.GetComponent<Canvas>();
        creditsCanvas.gameObject.SetActive(false);

        //Flag isHelp to false
        isCredits = false;

        if (!AudioSources.instance)
        {
            Debug.LogWarning("PUT AUDIOSOURCES PREFAB IN SCENE!");
        }

        swapSelectionFxId = (int)AudiosSoundFX.Menu_SwapSelection;
        applySelectionFxId = (int)AudiosSoundFX.Menu_ApplySelection;
        mainThemeMusicId = (int)AudiosMusic.MainTheme;

        AudioSources.instance.PlayMusic(mainThemeMusicId);
    }

    //Move focus up
    public int MoveFocusUp(int focus)
    {

        if (focus != 0)
        {
            focus = focus - 1;
            AudioSources.instance.PlaySound(swapSelectionFxId);
        }

        return focus;
    }

    //Move focus down
    public int MoveFocusDown(int focus)
    {

        if (focus != 4)
        {
            focus = focus + 1;
            AudioSources.instance.PlaySound(swapSelectionFxId);
        }

        return focus;
    }

    //Move focus right in exit menu
    public int MoveFocusRight(int focus)
    {

        if (focus == 0)
        {
            focus = 1;
            AudioSources.instance.PlaySound(swapSelectionFxId);
        }

        return focus;
    }

    //Move focus left in exit menu
    public int MoveFocusLeft(int focus)
    {

        if (focus == 1)
        {
            focus = 0;
            AudioSources.instance.PlaySound(swapSelectionFxId);
        }

        return focus;
    }

    //Redraw the buttons if focus has changed
    void UpdateFocus(int focus)
    {

        switch (focus)
        {
            case 0:
                start.GetComponent<Image>().sprite = selectedBGMenuSprite;
                help.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                settings.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                credits.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                exit.GetComponent<Image>().sprite = unselectedBGMenuSprite;

                startText.color = selectedTextColor;
                helpText.color = unselectedTextColor;
                settingsText.color = unselectedTextColor;
                creditsText.color = unselectedTextColor;
                exitText.color = unselectedTextColor;
                break;
            case 1:
                start.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                help.GetComponent<Image>().sprite = selectedBGMenuSprite;
                settings.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                credits.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                exit.GetComponent<Image>().sprite = unselectedBGMenuSprite;

                startText.color = unselectedTextColor;
                helpText.color = selectedTextColor;
                settingsText.color = unselectedTextColor;
                creditsText.color = unselectedTextColor;
                exitText.color = unselectedTextColor;
                break;
            case 2:
                start.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                help.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                settings.GetComponent<Image>().sprite = selectedBGMenuSprite;
                credits.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                exit.GetComponent<Image>().sprite = unselectedBGMenuSprite;

                startText.color = unselectedTextColor;
                helpText.color = unselectedTextColor;
                settingsText.color = selectedTextColor;
                creditsText.color = unselectedTextColor;
                exitText.color = unselectedTextColor;
                break;
            case 3:
                start.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                help.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                settings.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                credits.GetComponent<Image>().sprite = selectedBGMenuSprite;
                exit.GetComponent<Image>().sprite = unselectedBGMenuSprite;

                startText.color = unselectedTextColor;
                helpText.color = unselectedTextColor;
                settingsText.color = unselectedTextColor;
                creditsText.color = selectedTextColor;
                exitText.color = unselectedTextColor;
                break;
            case 4:
                start.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                help.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                settings.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                credits.GetComponent<Image>().sprite = unselectedBGMenuSprite;
                exit.GetComponent<Image>().sprite = selectedBGMenuSprite;

                startText.color = unselectedTextColor;
                helpText.color = unselectedTextColor;
                settingsText.color = unselectedTextColor;
                creditsText.color = unselectedTextColor;
                exitText.color = selectedTextColor;
                break;
        }
    }
    //Redraw the buttons of the confirm exit menu if focus has changed
    void UpdateFocusExit(int focus)
    {

        switch (focus)
        {
            case 0:
                yes.GetComponent<Image>().sprite = selectedBGExitSprite;
                no.GetComponent<Image>().sprite = unselectedBGExitSprite;

                yesText.color = selectedTextColor;
                noText.color = unselectedTextColor;
                break;
            case 1:
                yes.GetComponent<Image>().sprite = unselectedBGExitSprite;
                no.GetComponent<Image>().sprite = selectedBGExitSprite;

                yesText.color = unselectedTextColor;
                noText.color = selectedTextColor;
                break;
        }
    }

    //When Enter is pressed, we've selected an item
    /*
		0: Start
		1: Help
        2: Settings
		3: Credits
		4: Exit
	*/
    void EnterPress(int focus)
    {
        if (!isConfirmExit)
        {
            switch (focus)
            {
                case 0:
                    SceneManager.LoadScene("CityMapUpdate");
                    break;
                case 1:
                    openHelp();
                    break;
                case 2:
                    openSettings();
                    break;
                case 3:
                    openCredits();
                    break;
                case 4:
                    openExitMenu();
                    break;
            }
        }
        else
        {
            switch (focus)
            {
                case 0:
                    Debug.Log("Application quit");
                    Application.Quit();
                    break;
                case 1:
                    closeExitMenu();
                    break;
            }
        }
        
    }

    //Open the confirm exit menu
    void openExitMenu()
    {
        confirmExit.gameObject.SetActive(true);
        isConfirmExit = true;
        focusExit = 1;
        UpdateFocusExit(focusExit);
    }

    //Open the confirm exit menu
    void closeExitMenu()
    {
        confirmExit.gameObject.SetActive(false);
        isConfirmExit = false;
        focusExit = 1;
    }

    //Open Help
    void openHelp()
    {
        helpCanvas.gameObject.SetActive(true);
        isHelp = true;
        start.gameObject.SetActive(false);
        help.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
    }

    //Close Help
    void closeHelp()
    {
        helpCanvas.gameObject.SetActive(false);
        isHelp = false;
        start.gameObject.SetActive(true);
        help.gameObject.SetActive(true);
        settings.gameObject.SetActive(true);
        credits.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
    }

    //Open Settings
    void openSettings()
    {
        settingsCanvas.gameObject.SetActive(true);
        isSettings = true;
        start.gameObject.SetActive(false);
        help.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
    }

    //Close Settings
    void closeSettings()
    {
        settingsCanvas.gameObject.SetActive(false);
        isSettings = false;
        start.gameObject.SetActive(true);
        help.gameObject.SetActive(true);
        settings.gameObject.SetActive(true);
        credits.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
    }

    //Open Credits
    void openCredits()
    {
        creditsCanvas.gameObject.SetActive(true);
        isCredits = true;
        start.gameObject.SetActive(false);
        help.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
    }

    //Close Credits
    void closeCredits()
    {
        creditsCanvas.gameObject.SetActive(false);
        isCredits = false;
        start.gameObject.SetActive(true);
        help.gameObject.SetActive(true);
        settings.gameObject.SetActive(true);
        credits.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        if (isHelp || isCredits || isSettings)
        {
            //Go back to main menu
            if (InputManager.Interact() || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
            {
                if (isHelp) { closeHelp(); AudioSources.instance.PlaySound(applySelectionFxId); }
                if (isCredits) { closeCredits(); AudioSources.instance.PlaySound(applySelectionFxId); }
                if (isSettings) { closeSettings(); AudioSources.instance.PlaySound(applySelectionFxId); }
            }
        }
        else
        {
            if (!isConfirmExit)
            {
                //Main menu
                if (InputManager.LeftJoystickUp() || Input.GetKeyDown("up"))
                {
                    if (!upInUse)
                    {
                        upInUse = true;
                        focus = MoveFocusUp(focus);
                        UpdateFocus(focus);
                        
                    }
                }
                else
                {
                    upInUse = false;
                }
                if (InputManager.LeftJoystickDown() || Input.GetKeyDown("down"))
                {
                    if (!downInUse)
                    {
                        downInUse = true;
                        focus = MoveFocusDown(focus);
                        UpdateFocus(focus);
                        
                    }
                }
                else
                {
                    downInUse = false;
                }
            }
            else
            {
                //Exit menu
                if (InputManager.LeftJoystickLeft() || Input.GetKeyDown("left"))
                {
                    if (!leftInUse)
                    {
                        leftInUse = true;
                        focusExit = MoveFocusLeft(focusExit);
                        UpdateFocusExit(focusExit);
                    }
                }
                else
                {
                    leftInUse = false;
                }

                if (InputManager.LeftJoystickRight() || Input.GetKeyDown("right"))
                {
                    if (!rightInUse)
                    {
                        rightInUse = true;
                        focusExit = MoveFocusRight(focusExit);
                        UpdateFocusExit(focusExit);
                    }
                }
                else
                {
                    rightInUse = false;
                }
            }
        }
        if (InputManager.Dodge() || Input.GetKeyDown("return"))
        {
            if (isConfirmExit)
            {
                EnterPress(focusExit);
                AudioSources.instance.PlaySound(applySelectionFxId);
            }
            else
            {
                EnterPress(focus);
                AudioSources.instance.PlaySound(applySelectionFxId);
            }
        }

    }


    
}
