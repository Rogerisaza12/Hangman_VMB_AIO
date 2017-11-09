using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour {

    //Screens
    public GameObject gameModeScreen;
    public GameObject[] screens;

    [Space(10f)] [Header("Input Fields")]

    public InputField wordInputField;
    public InputField letterInputField;

    [Space(10f)] [Header("Word Screen")]

    public Text playerTurnWordScreen;

    [Space(10f)] [Header("Game Screen")]

    public Text playerTurnGameScreen;
    public Text playerOneErrors;
    public Text playerTwoErrors;

    [Space(10f)] [Header("Win Screen")]

    public Text playerWinner;
    public Text[] playersWords;
    public Text[] playersErrors;

    [Space(10f)] [Header("Others")]

    public GameObject[] playersWordsObjects;
    public Text[] playerOneEmptyTexts;
    public Text[] playerTwoEmptyTexts;

    [Space(10f)]

    [SerializeField]
    private Image letterInputFieldImage;
    [SerializeField]
    private Image letterButonImage;
    [SerializeField]
    private Color playerOneColor;
    [SerializeField]
    private Color playerTwoColor;

    [Space(10f)]

    public Timer timer;

    [Space(10f)]

    public Score scoreAsset;

    [HideInInspector]
    public string currentInputFieldText;

    protected void Start()
    {
        if (wordInputField == null || letterInputField == null)
            return;

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

        UpdateLetterSectionColor();
    }

    public void Setup()
    {
        Observer.Singleton.onPlayerOneEndsTurn += SetTurnTextPlayerTwo;
        Observer.Singleton.onPlayerTwoEndsTurn += SetTurnTextPlayerOne;
    }

    public virtual void SetWinnerScreen(int winner)
    {
        if (winner > 1 || winner < 0)
            Debug.LogError("Winner out of index");

        int losser = (winner + 1) % 2;

        playerWinner.text = string.Format("PLAYER {0} WIN!", winner + 1);

        playersWords[winner].text = GameManager.Singleton.players[winner].word;
        playersWords[losser].text = GameManager.Singleton.players[losser].word;

        playersErrors[0].text = playerOneErrors.text;
        playersErrors[1].text = playerTwoErrors.text;
    }

    public virtual void OnWordInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public virtual void OnWordInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public virtual void OnLetterInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public virtual void OnLetterInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public virtual void UpdateErrors(int playerInTurn)
    {
        GameManager.Singleton.players[playerInTurn].IncreaseErrorsCount();

        if (playerInTurn == 0)
            playerOneErrors.text = string.Format("Player 1 Errors: {0}/10", GameManager.Singleton.players[0].errorsCount);
        else
            playerTwoErrors.text = string.Format("Player 2 Errors: {0}/10", GameManager.Singleton.players[1].errorsCount);
    }

    public virtual void UpdateSuccess(int playerInTurn, Dictionary<int, char> correctChars)
    {
        int otherPlayer = (playerInTurn + 1) % 2;

        for (int i = 0; i < GameManager.Singleton.players[otherPlayer].wordCharsArray.Length; i++)
        {
            if (correctChars.ContainsKey(i))
            {
                GameManager.Singleton.players[playerInTurn].IncreaseSuccessCount();

                if (otherPlayer == 0)
                    playerOneEmptyTexts[i].text = GameManager.Singleton.players[0].wordCharsArray[i].ToString();
                else
                    playerTwoEmptyTexts[i].text = GameManager.Singleton.players[1].wordCharsArray[i].ToString();
            }
        }
    }

    public virtual void ClearInputFields()
    {
        wordInputField.text = "";
        letterInputField.text = "";
    }

    public virtual void UpdateLetterSectionColor()
    {
        if (GameManager.Singleton.playerInTurn == 0)
        {
            letterInputFieldImage.color = playerOneColor;
            letterButonImage.color = playerOneColor;
        }
        else
        {
            letterInputFieldImage.color = playerTwoColor;
            letterButonImage.color = playerTwoColor;
        }
    }

    public virtual void SaveMatchData(int winner)
    {
        string playerWinner = (GameManager.Singleton.gameMode == 1 && winner == 1) ? "Player AI" : string.Format("Player {0}", (winner + 1).ToString());

        scoreAsset.Save(
            timer.timeText.text,
            GameManager.Singleton.players[0].errorsCount,
            GameManager.Singleton.players[1].errorsCount,
            playerWinner);

        ScoreHandler.Singleton.UpdateMatchTexts();
    }

    protected void SetTurnTextPlayerOne()
    {
        playerTurnGameScreen.text = "Player 1 Turn";
    }

    protected void SetTurnTextPlayerTwo()
    {
        playerTurnGameScreen.text = "Player 2 Turn";
    }

    protected char ValidateChar(char charToValidate)
    {
        if (!char.IsLetter(charToValidate) && !char.IsWhiteSpace(charToValidate))
            charToValidate = ' ';

        return char.ToUpper(charToValidate);
    }
}
