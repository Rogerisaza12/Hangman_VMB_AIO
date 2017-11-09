using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timeText;

    public bool isWatching
    {
        get; private set;
    }

    public float time
    {
        get; private set;
    }

    private float minutes;
    private float seconds;

    private IEnumerator watch;

    private void Awake()
    {
        timeText = GetComponent<Text>();
    }

    public void StartWatch()
    {
        if (watch != null)
            StopCoroutine(watch);

        watch = Watch();

        StartCoroutine(watch);
    }

    public void StopWatch()
    {
        isWatching = false;

        time = 0;
    }

    private IEnumerator Watch()
    {
        isWatching = true;

        while (isWatching)
        {
            time += Time.deltaTime;
            minutes = (int)time / 60;
            seconds = (int)time % 60;

            timeText.text = string.Format("Time: {0}:{1}", minutes.ToString("00"), seconds.ToString("00"));

            yield return null;
        }

        StopCoroutine(watch);
    }
}
