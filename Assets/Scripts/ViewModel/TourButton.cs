using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class TourButton : UIView
{
    //view Ids
    public Button innerButton;
    public Label title;

    private TourController tourController;
    private int tourID;
    private Action onClickCallback;

    public void setup(TourController tc, int tourID, Action onClickCallback, String iconPath, String title,
        int width, int height, int leftMargin, int topMargin)
    {
        this.tourController = tc;
        this.tourID = tourID;
        this.onClickCallback = onClickCallback;
        this.title.Text.Value = title;

        if (iconPath != null)
        {
            Texture2D tex = Resources.Load<Texture2D>(iconPath);
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0,0));
            UnityAsset asset = new UnityAsset(null, null);
            asset.Load(sprite);
            innerButton.BackgroundImage.Value = new SpriteAsset(asset);
           // innerButton.InitializeViews();
        }

        innerButton.Width.Value = width;
        innerButton.Height.Value = height;
        this.Alignment.Value = ElementAlignment.TopLeft;
        this.Offset.Value = new ElementMargin(new ElementSize(leftMargin),
                                                     new ElementSize(topMargin));


    }

    public void onClick()
    {
        onClickCallback();
        tourController.startTour(tourID);
    }

    public void setup(String title)
    {
        
    }

}
