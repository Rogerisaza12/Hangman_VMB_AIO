using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int index
    {
        get; protected set;
    }

    public string word
    {
        get; protected set;
    }

    public int sucessCount
    {
        get; protected set;
    }

    public int errorsCount
    {
        get; protected set;
    }

    public char[] wordCharsArray
    {
        get; protected set;
    }
    //Word chars indexer
    public char this[int i]
    {
        get
        {
            return wordCharsArray[i];
        }
        private set
        {
            wordCharsArray[i] = value;
        }
    }

    public List<char> playedChars = new List<char>();

    public void SetIndex(int index)
    {
        this.index = index;

        if (index == 0)
        {
            Observer.Singleton.onPlayerTwoEndsTurn += Turn;
            Observer.Singleton.onPlayerOneEndsTurn += EndTurn;
        }
        else
        {
            Observer.Singleton.onPlayerOneEndsTurn += Turn;
            Observer.Singleton.onPlayerTwoEndsTurn += EndTurn;
        }

        Debug.Log(string.Format("Player {0} created!", index));
    }

    public void SetWord(string word)
    {
        this.word = word;

        wordCharsArray = word.ToCharArray();

        switch (GameManager.Singleton.gameMode)
        {
            case 1:
                SetWordCaseSingleplayer();
                break;

            case 2:
                SetWordCaseLocalMultiplayer();
                break;

            default:
                break;
        }

        Debug.Log(string.Format("Player {0} word: {1}", index, word));
    }

    public Dictionary<int, char> CheckForCharsInWord(char inputChar)
    {
        Dictionary<int, char> charsInWord = new Dictionary<int, char>();

        for (int i = 0; i < wordCharsArray.Length; i++)
        {
            if (inputChar == wordCharsArray[i])
                charsInWord.Add(i, wordCharsArray[i]);
        }

        return charsInWord;
    }

    public void IncreaseSuccessCount()
    {
        sucessCount++;
    }

    public void IncreaseErrorsCount()
    {
        errorsCount++;
    }

    private void SetWordCaseSingleplayer()
    {
        if (index == 0)
        {
            for (int i = UIFacade.Singleton.singleplayer.playerOneEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                UIFacade.Singleton.singleplayer.playerOneEmptyTexts[i].gameObject.SetActive(false);
        }
        else
        {
            for (int i = UIFacade.Singleton.singleplayer.playerTwoEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                UIFacade.Singleton.singleplayer.playerTwoEmptyTexts[i].gameObject.SetActive(false);
        }
    }

    private void SetWordCaseLocalMultiplayer()
    {
        if (index == 0)
        {
            for (int i = UIFacade.Singleton.localMultiplayer.playerOneEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                UIFacade.Singleton.localMultiplayer.playerOneEmptyTexts[i].gameObject.SetActive(false);
        }
        else
        {
            for (int i = UIFacade.Singleton.localMultiplayer.playerTwoEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                UIFacade.Singleton.localMultiplayer.playerTwoEmptyTexts[i].gameObject.SetActive(false);
        }
    }

    private void Turn()
    {
        gameObject.SetActive(true);

        if (GameManager.Singleton.turn > 1)
        {
            if (index == 0)
            {
                switch (GameManager.Singleton.gameMode)
                {
                    case 1:
                        UIFacade.Singleton.singleplayer.playersWordsObjects[1].SetActive(true);
                        break;

                    case 2:
                        UIFacade.Singleton.localMultiplayer.playersWordsObjects[1].SetActive(true);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (GameManager.Singleton.gameMode)
                {
                    case 1:
                        UIFacade.Singleton.singleplayer.playersWordsObjects[0].SetActive(true);
                        break;

                    case 2:
                        UIFacade.Singleton.localMultiplayer.playersWordsObjects[0].SetActive(true);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void EndTurn()
    {
        gameObject.SetActive(false);

        if (GameManager.Singleton.turn > 1)
        {
            if (index == 0)
            {
                switch (GameManager.Singleton.gameMode)
                {
                    case 1:
                        UIFacade.Singleton.singleplayer.playersWordsObjects[1].SetActive(false);
                        break;

                    case 2:
                        UIFacade.Singleton.localMultiplayer.playersWordsObjects[1].SetActive(false);
                        break;
                }
            }
            else
            {
                switch (GameManager.Singleton.gameMode)
                {
                    case 1:
                        UIFacade.Singleton.singleplayer.playersWordsObjects[0].SetActive(false);
                        break;

                    case 2:
                        UIFacade.Singleton.localMultiplayer.playersWordsObjects[0].SetActive(false);
                        break;
                }
            }
        }
    }
}
