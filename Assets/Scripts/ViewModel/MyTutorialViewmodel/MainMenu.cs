using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;

public class MainMenu : UIView
{

    public ViewSwitcher ContentViewSwitcher;


    public override void Initialize()
    {
        base.Initialize();
        Button popup = CreateView<Button>(-1);
        popup.InitializeViews();
        popup.Text.Value = "HELLO";
        popup.Click.Name = "StartGame";
        popup.Click.AddEntry(new ViewActionEntry { ParentView = this,
                                                   ViewActionFieldName = "Click",
                                                   ViewActionHandlerName = "StartGame" });
        
    }

    public void StartGame()
    {
        ContentViewSwitcher.SwitchTo(1);
    }

    public void Options()
    {
        ContentViewSwitcher.SwitchTo(2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}