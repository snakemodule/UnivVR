using System;
using UnityEngine;

public class TourStateDTO
{
    public String text { get; private set; }
    public AudioClip clip { get; private set; }
    public bool paused { get; private set; }

    public TourStateDTO(string text, AudioClip clip, bool paused)
    {
        this.text = text;
        this.clip = clip;
        this.paused = paused;
    }

}