using System;
using UnityEngine;

public class Observer : MonoBehaviour {

    //Actions
    public Action onOnlineMultiplayer;
    public Action onReadme;

    public Action onPlayerOneEndsTurn;
    public Action onPlayerTwoEndsTurn;

    public Action onWordInputFieldEnter;
    public Action onLetterInputFieldEnter;

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

    public void OnlineMultiplayer()
    {
        Debug.Log("OnlineMultiplayer");

        if (onOnlineMultiplayer != null)
            onOnlineMultiplayer();
    }

    public void Readme()
    {
        Debug.Log("Readme");

        if (onReadme != null)
            onReadme();
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

    public void WordInputFieldEnter()
    {
        if (onWordInputFieldEnter != null)
            onWordInputFieldEnter();
    }

    public void LetterInputFieldEnter()
    {
        if (onLetterInputFieldEnter != null)
            onLetterInputFieldEnter();
    }
}
