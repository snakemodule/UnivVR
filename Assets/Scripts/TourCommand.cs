using System;
using UnityEngine;

public class TourCommand {
    public AudioClip clip;
    public String cmd { get; private set; }
    public int delay { get; private set; }
    public String text { get; private set; }

    public TourCommand(String cmd, int delay, String text, AudioClip clip)
    {
        this.cmd = cmd;
        this.delay = delay;
        this.text = text;
        this.clip = clip;
    }

    
}