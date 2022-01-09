using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;

public class VisibilityView : UIView {

    public Group organizer;
    public Label infoText;

    public Region infoRegion;

    public Dictionary<String, VisibilityPane> visibilityPanes = new Dictionary<string, VisibilityPane>();

    public AudioSource audioSource;
    

    public void updatePanes(List<String> buttonVisIDs)
    {
        foreach (VisibilityPane pane in visibilityPanes.Values.ToArray<VisibilityPane>())
        {
            pane.Deactivate();
        }

        foreach(String id in buttonVisIDs)
        {
            visibilityPanes[id].Activate();
        }

    }

    public void presentInfo(String infoText, AudioClip infoClip)
    {
        if (infoText != "" && infoText != null)
        {
            infoRegion.Activate();
            this.infoText.Text.Value = infoText;
        } else
        {
            infoRegion.Deactivate();
        }
        audioSource.clip = infoClip;
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void onStopClick()
    {
        if (audioSource.clip != null) { 
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            } else
            {
                audioSource.Play();
            }
        }
    }

}
