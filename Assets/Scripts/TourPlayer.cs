using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TourPlayer 
{
    private UniviewConnection univiewConnection;
    private List<TourObserver> observers = new List<TourObserver>();
    private List<Tour> tours;
    private bool tourPlaying;
    private bool stepForwardSignal;
    private bool stepBackwardSignal;
    private bool pauseSignal;

    private bool paused;

    public TourPlayer()
    {
        tours = new List<Tour>();
        univiewConnection = UniviewConnection.Instance;
    }

    public void addObserver(TourObserver observer)
    {
        observers.Add(observer);
    }

    public void forward()
    {
        stepForwardSignal = true;
    }

    public void backward()
    {
        stepBackwardSignal = true;
    }

    public void pause()
    {
        pauseSignal = true;
    }

    public void cancelTour()
    {
        tourPlaying = false;
    }

    public void createTour(XmlReader reader)
    {
        List<TourCommand> commands = new List<TourCommand>();
        reader.Read(); //step into tour element
        while (reader.Name != "Tour" && reader.NodeType != XmlNodeType.EndElement)
        {
            if (reader.Name == "Command")
            {
                String cmd = reader.GetAttribute("cmd");
                int delay = Int32.Parse(reader.GetAttribute("delay"));
                String text = reader.GetAttribute("text");
                String clipPath = reader.GetAttribute("sound");
                AudioClip clip = Resources.Load<AudioClip>(clipPath);
                commands.Add(new TourCommand(cmd,delay,text,clip));
            }
            reader.Read();
        }
        tours.Add(new Tour(commands));
    }

    public IEnumerator tourCoroutine(int tourID)
    {
        tourPlaying = true;
        paused = false;
        List<TourCommand> cmds = tours[tourID].commands;
        for (int i = 0; i < cmds.Count; i++)
        {
            univiewConnection.sendCommand(cmds[i].cmd + '\n');
            TourStateDTO tdto = new TourStateDTO(cmds[i].text, cmds[i].clip, paused);
            notifyTourStateChange(tdto);

            //busy waiting, checking for cancel of playback or pause
            float waitCounter = 0;
            while(waitCounter < (cmds[i].delay / 1000) || paused)
            {
                if (stepForwardSignal)
                {
                    stepForwardSignal = false;
                    break; //do not wait for time
                } else if (stepBackwardSignal)
                {
                    stepBackwardSignal = false;
                    if (!(i <= 0))
                    {
                        i = i - 2;
                        break;
                    }
                }

                if (!tourPlaying)
                {
                    notifyTourFinished();
                    yield break;
                }

                if(pauseSignal)
                {
                    pauseSignal = false;
                    paused = !paused;
                    tdto = new TourStateDTO(cmds[i].text, cmds[i].clip, paused);
                    notifyTourStateChange(tdto);
                }
                

                if (!paused)
                {
                    waitCounter += Time.deltaTime;
                }
                
                yield return null;
            }
                
        }

        notifyTourFinished();
    }

    private void notifyTourStateChange(TourStateDTO tdto)
    {
        foreach (TourObserver observer in observers)
        {
            observer.tourChangeHandler(tdto);
        }
    }

    private void notifyTourFinished()
    {
        foreach (TourObserver observer in observers)
        {
            observer.tourEnded();
        }
    }

}
