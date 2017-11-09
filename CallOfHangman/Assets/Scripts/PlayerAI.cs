using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : Player {

    private char playableLetter = ' ';

    [SerializeField]
    private string[] words;
    [SerializeField]
    private AnimationCurve behaviourDistribution;

    //Char list
    private List<char> vocals = new List<char>();
    private List<char> commonConsonants = new List<char>();
    private List<char> unusualConsonants = new List<char>();

    //Coroutine
    private IEnumerator playCoroutine;

    private int playableTurn;
    private float behaviourFactor;
    private float behaviourGap = 5f;

    private float whileCount;

    private bool allVocalsWasPlayed = false;

    private void Start()
    {
        AddVocals();
        AddCommonConsonants();
        AddUnusualConsonants();
    }

    public void SelectWord()
    {
        string selectedWord = words[Random.Range(0, words.Length)];

        SetWord(selectedWord);

        GameManager.Singleton.NextTurn();
    }

    public void Play()
    {
        if (playCoroutine != null)
            StopCoroutine(playCoroutine);

        playCoroutine = PlayCoroutine();

        StartCoroutine(playCoroutine);
    }

    private IEnumerator PlayCoroutine()
    {
        UIFacade.Singleton.singleplayer.SetActiveLetterSection(false);

        yield return new WaitForSeconds(0.25f);

        playableTurn = (int)((GameManager.Singleton.turn * 0.5) + 0.5f);

        //Behaviour factor decides what play the AI
        behaviourFactor = playableTurn + Random.Range(-behaviourGap, behaviourGap) * behaviourDistribution.Evaluate(Random.Range(0, 1));

        if (behaviourFactor >= -behaviourGap && behaviourFactor <= behaviourGap && !allVocalsWasPlayed)
            PlayVocal();
        else if (behaviourFactor >= 0 && behaviourFactor <= 2 * behaviourGap)
            PlayCommonConsonant();
        else if (behaviourFactor >= behaviourGap && behaviourFactor <= 3 * behaviourGap)
            PlayUnusualConsonant();

        GameManager.Singleton.CheckForCharOnRivalPlayerWordAI(playableLetter);

        yield return new WaitForSeconds(0.75f);

        UIFacade.Singleton.singleplayer.SetActiveLetterSection(true);

        yield return null;

        GameManager.Singleton.NextTurn();

        yield return null;

        UIFacade.Singleton.singleplayer.UpdateLetterSectionColor();
    }

    private void PlayVocal()
    {
        int index = Random.Range(0, vocals.Count);

        while (playedChars.Contains(playableLetter))
        {
            index = Random.Range(0, vocals.Count);

            playableLetter = vocals[index];

            whileCount++;

            if (whileCount == vocals.Count)
            {
                PlayCommonConsonant();

                allVocalsWasPlayed = true;

                whileCount = 0;
                
                return;
            }
        }

        whileCount = 0;

        Debug.Log(string.Format("AI Letter: {0}", playableLetter));
    }

    private void PlayCommonConsonant()
    {
        int index = Random.Range(0, commonConsonants.Count);

        while (playedChars.Contains(playableLetter))
        {
            index = Random.Range(0, commonConsonants.Count);

            playableLetter = commonConsonants[index];

            whileCount++;
        }

        Debug.Log(string.Format("AI Letter: {0}", playableLetter));
    }

    private void PlayUnusualConsonant()
    {
        int index = Random.Range(0, unusualConsonants.Count);

        while (playedChars.Contains(playableLetter))
        {
            index = Random.Range(0, unusualConsonants.Count);

            playableLetter = unusualConsonants[index];

            whileCount++;
        }

        Debug.Log(string.Format("AI Letter: {0}", playableLetter));
    }

    private void AddVocals()
    {
        vocals.Add('A');
        vocals.Add('E');
        vocals.Add('I');
        vocals.Add('O');
        vocals.Add('U');
    }

    private void AddCommonConsonants()
    {
        commonConsonants.Add(' ');
        commonConsonants.Add('T');
        commonConsonants.Add('N');
        commonConsonants.Add('S');
        commonConsonants.Add('H');
        commonConsonants.Add('R');
        commonConsonants.Add('D');
        commonConsonants.Add('L');
        commonConsonants.Add('C');
        commonConsonants.Add('M');
        commonConsonants.Add('W');
    }

    private void AddUnusualConsonants()
    {
        unusualConsonants.Add('F');
        unusualConsonants.Add('G');
        unusualConsonants.Add('Y');
        unusualConsonants.Add('P');
        unusualConsonants.Add('B');
        unusualConsonants.Add('V');
        unusualConsonants.Add('K');
        unusualConsonants.Add('J');
        unusualConsonants.Add('X');
        unusualConsonants.Add('Q');
        unusualConsonants.Add('Z');
    }
}
