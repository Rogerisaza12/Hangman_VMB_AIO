using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Networking;

public class UIFacade : MonoBehaviour {

    [Header("Menus")]

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject modeLocalMultiplayer;
    [SerializeField]
    private GameObject modeOnlineMultiplayer;
    [SerializeField]
    private GameObject readme;

    [Space(10F)]

    [SerializeField]
    private GameObject[] localMultiplayerScreens;
    [SerializeField]
    private GameObject[] onlineMultiplayerScreens;

    [Space(10f)] [Header("Input Fields")]

    [SerializeField]
    private InputField wordInputField;
    [SerializeField]
    private InputField letterInputField;

    [Space(10f)] [Header("Game Screen")]

    public Text localMultiplayerInfo;
    public Text playerOneErrors;
    public Text playerTwoErrors;

    [Space(10f)] [Header("WinScreen")]

    [SerializeField]
    private Text playerWinner;
    [SerializeField]
    private Text[] playersWords;
    [SerializeField]
    private Text[] playersErros;

    [Space(10f)] [Header("Others")]

    public GameObject[] playersWordsObject;
    public Text[] playerOneEmptyTexts;
    public Text[] playerTwoEmptyTexts;

    [HideInInspector]
    public string currentInputFieldText;

    //Singleton!
    public static UIFacade Singleton
    {
        get; private set;
    }

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;
    }

    private void Start()
    {
        wordInputField.characterLimit = 10;
        wordInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateChar(addedChar);
        };

        letterInputField.characterLimit = 1;
        letterInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateChar(addedChar);
        };

        //Subscribing to the input field events
        Observer.Singleton.onWordInputFieldEnter += ClearInputFields;
        Observer.Singleton.onLetterInputFieldEnter += ClearInputFields;

        //Subscribing to the readme events
        Observer.Singleton.onReadme += DoneReadme;
    }

    public void SetActiveMainMenu(bool state)
    {
        mainMenu.SetActive(state);
    }

    public void SetActiveLocalMultiplayer(bool state)
    {
        modeLocalMultiplayer.SetActive(state);
    }

    public void SetActiveOnlineMultiplayer(bool state)
    {
        modeOnlineMultiplayer.SetActive(state);
    }

    public void SetActiveLocalMultiplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > localMultiplayerScreens.Length - 1)
            return;

        localMultiplayerScreens[screen].SetActive(state);
    }

    public void SetActiveOnlineMultiplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > onlineMultiplayerScreens.Length - 1)
            return;

        onlineMultiplayerScreens[screen].SetActive(state);
    }

    public void OnWordInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public void OnWordInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public void OnLetterInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public void OnLetterInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public void ClearInputFields()
    {
        /*
        wordInputField.text = "";
        letterInputField.text = "";
        */
    }

    public void UpdateWinScreen()
    {   
        SetActiveLocalMultiplayerScreen(3, true);

        playersErros[0].text = string.Format("Player 1 errors: {0}/10", GameManagerNetworking.Singleton.players[0].errorsCount);
        playersErros[1].text = string.Format("Player 2 errors: {0}/10", GameManagerNetworking.Singleton.players[1].errorsCount);

        playersWords[0].text = string.Format("{0}", GameManagerNetworking.Singleton.players[0].word);
        playersWords[1].text = string.Format("{0}", GameManagerNetworking.Singleton.players[1].word);
    }

    public void Done()
    {
        SceneManager.LoadScene(0);
    }

    public void DoneReadme()
    {
        if (readme.activeInHierarchy)
            readme.SetActive(false);
        else
            readme.SetActive(true);
    }

    private char ValidateChar(char charToValidate)
    {
        if (!char.IsLetter(charToValidate) && !char.IsWhiteSpace(charToValidate))
            charToValidate = ' ';

        return char.ToUpper(charToValidate);
    }
}
