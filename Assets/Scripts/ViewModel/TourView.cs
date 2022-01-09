using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight;
using MarkLight.Views.UI;
using System.IO;
using System.Xml;
using System;

public class TourView : View, TourObserver {

    //view Ids
    public UserInterface PlayerUI;
    public UserInterface SelectionUI;
    public Label playerText;
    public Region selectionRegion;
    public Button closeButton;

    //set in editor
    public AudioSource audioSource;

    private TourController tourController;
    private int tourColumnCounter = 0;

    private SwitchCallback switchCallback;

    private int tourRowCounter = 0;
    private int numberOfTours = 0;

    public override void Initialize()
    {
        base.Initialize();
        tourController = new TourController(this);
        tourController.registerListeners(this);
        String path =
            @"C:\Users\sciss\Documents\UnivVR\Assets\Tours.xml";
        String libraryxml = File.ReadAllText(path);

        using (XmlReader reader = XmlReader.Create(new StringReader(libraryxml)))
        {
            reader.ReadToFollowing("Root");
            reader.Read(); // next
            while (reader.Name != "Root" && reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.Name == "Tour")
                {
                    
                    TourButton TourButton = ViewData.CreateView<TourButton>(selectionRegion, selectionRegion);
                    TourButton.InitializeViews();
                    String iconPath = reader.GetAttribute("icon");
                    String title = reader.GetAttribute("title");
                    TourButton.setup(tourController, numberOfTours, activateTourPlayer, iconPath, title, 60, 60,
                        20 + 60 * (tourColumnCounter-1), 20 + 60 * (tourRowCounter-1));
                    tourColumnCounter++;
                    numberOfTours++;
                    if (tourColumnCounter>=4)
                    {
                        tourColumnCounter = 0;
                        tourRowCounter++;
                    }
                    tourController.createTour(reader);
                }
                reader.Read(); // position on next element
            }   
        }
    }

    public void setup(SwitchCallback switchCallback)
    {
        this.switchCallback = switchCallback;
    }

    // Update is called once per frame
    void Update()
    {
        SelectionUI.RectTransform.localScale = new Vector3(1, 1, 1);
        SelectionUI.RectTransform.eulerAngles = new Vector3(0, 90, 0);
        SelectionUI.RectTransform.position = new Vector3(3, 1, 0);

        PlayerUI.RectTransform.localScale = new Vector3(1, 1, 1);
        PlayerUI.RectTransform.eulerAngles = new Vector3(0, 135, 0);
        PlayerUI.RectTransform.position = new Vector3(3, 1, -2);
    }

    public void clickClose()
    {
        switchCallback();
    }

    private void activateTourPlayer()
    {
        SelectionUI.Deactivate();
        PlayerUI.Activate();
    }
    private void activateSelection()
    {
        SelectionUI.Activate();
        PlayerUI.Deactivate();
    }

    public void cancelTour()
    {
        tourController.cancelTour();
    }
    public void stepForward()
    {
        tourController.stepForward();
    }
    public void stepBackward()
    {
        tourController.stepBackward();
    }
    public void pauseButton()
    {
        tourController.pause();
    }
    
    public void tourChangeHandler(TourStateDTO tourState)
    {
        playerText.Text.Value = tourState.text;
        if(audioSource.clip != tourState.clip)
        {
            audioSource.clip = tourState.clip;
            audioSource.Play();
        }
            

        if (tourState.paused && audioSource.isPlaying)
        {
            audioSource.Pause();
        } else if (!tourState.paused && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
    public void tourEnded()
    {
        activateSelection();
        audioSource.Stop();
        audioSource.clip = null;
    }

}
