using MarkLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TourController {

    private TourPlayer tourPlayer;
    private TourView tourView;


    public TourController(TourView tv)
    {
        tourPlayer = new TourPlayer();
        tourView = tv;
    }


    public void startTour(int tourID)
    {
        //tourPlayer.start(tourID);
        tourView.StartCoroutine(tourPlayer.tourCoroutine(tourID));
    }

    public void createTour(XmlReader reader)
    {
        tourPlayer.createTour(reader);
       
    }

    public void registerListeners(TourObserver tourView)
    {
        tourPlayer.addObserver(tourView);
    }

    public void cancelTour()
    {
        tourPlayer.cancelTour();
    }

    public void stepForward()
    {
        tourPlayer.forward();
    }

    public void stepBackward()
    {
        tourPlayer.backward();
    }

    public void pause()
    {
        tourPlayer.pause();
    }
}
