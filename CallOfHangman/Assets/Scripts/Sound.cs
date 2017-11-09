using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;

    [Space(10f)]

    public AudioClip clip;

    [Space(10f)]

    public bool loop;
    [Range(0.0f, 1.0f)]
    public float volume;
    [Range(0.0f, 3.0f)]
    public float pitch;
}
