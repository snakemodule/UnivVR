using System.Collections;
using System.Collections.Generic;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class VisibilityPane : Group {
    public Label label;
    public Group checkboxGroup;

    public void onDeactive()
    { 
        foreach (CommandCheckBox cb in checkboxGroup.GetChildren<CommandCheckBox>(false))
        {
            cb.setDefault();
        }
    }


    public void onActive()
    {
        updateLayout();
    }

    private void updateLayout()
    {
        View temp = checkboxGroup;
        while (temp != null)
        {
            temp.LayoutChanged();
            temp = temp.Parent;
        }
    }

}
