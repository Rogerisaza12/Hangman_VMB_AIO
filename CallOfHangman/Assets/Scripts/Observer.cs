using System;
using UnityEngine;

public class Observer : MonoBehaviour {

    //Actions
    public Action onSingleplayer;
    public Action onLocalMultiplayer;
    public Action onReadme;
    public Action onMatchData;
    public Action onPlayerOneEndsTurn;
    public Action onPlayerTwoEndsTurn;

    //Input field Actions for singleplayer
    public Action onWordInputFieldEnterSingleplayer;
    public Action onLetterInputFieldEnterSingleplayer;
    //Input field Actions for local multiplayer
    public Action onWordInputFieldEnterLocalMultiplayer;
    public Action onLetterInputFieldEnterLocalMultiplayer;

    //Singleton!
    public static Observer Singleton
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

    public void Singleplayer()
    {
        Debug.Log("Singleplayer");

        if (onSingleplayer != null)
            onSingleplayer();
    }

    public void LocalMultiplayer()
    {
        Debug.Log("LocalMultiplayer");

        if (onLocalMultiplayer != null)
            onLocalMultiplayer();
    }

    public void Readme()
    {
        Debug.Log("Readme");

        if (onReadme != null)
            onReadme();
    }

    public void MatchData()
    {
        Debug.Log("MatchData");

        if (onMatchData != null)
            onMatchData();
    }

    public void PlayerOneEndsTurn()
    {
        Debug.Log("PlayerOneEndsTurn");

        if (onPlayerOneEndsTurn != null)
            onPlayerOneEndsTurn();
    }

    public void PlayerTwoEndsTurn()
    {
        Debug.Log("PlayerTwoEndsTurn");

        if (onPlayerTwoEndsTurn != null)
            onPlayerTwoEndsTurn();
    }

    public void WordInputFieldEnterSingleplayer()
    {
        if (UIFacade.Singleton.singleplayer.currentInputFieldText == "")
        {
            AudioManager.Singleton.PlayEffect("Error");
            return;
        }

        if (onWordInputFieldEnterSingleplayer != null)
            onWordInputFieldEnterSingleplayer();
    }

    public void LetterInputFieldEnterSingleplayer()
    {
        if (UIFacade.Singleton.singleplayer.currentInputFieldText == "")
        {
            AudioManager.Singleton.PlayEffect("Error");
            return;
        }

        if (onLetterInputFieldEnterSingleplayer != null)
            onLetterInputFieldEnterSingleplayer();
    }

    public void WordInputFieldEnterLocalMultiplayer()
    {
        if (UIFacade.Singleton.localMultiplayer.currentInputFieldText == "")
        {
            AudioManager.Singleton.PlayEffect("Error");
            return;
        }

        if (onWordInputFieldEnterLocalMultiplayer != null)
            onWordInputFieldEnterLocalMultiplayer();
    }

    public void LetterInputFieldEnterLocalMultiplayer()
    {
        if (UIFacade.Singleton.localMultiplayer.currentInputFieldText == "")
        {
            AudioManager.Singleton.PlayEffect("Error");
            return;
        }

        if (onLetterInputFieldEnterLocalMultiplayer != null)
            onLetterInputFieldEnterLocalMultiplayer();
    }
}
