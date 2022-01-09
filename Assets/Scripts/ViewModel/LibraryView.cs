using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight;
using MarkLight.Views.UI;
using System.Text;
using System;
using System.Xml;
using System.IO;


public delegate void CloseSiblingsCallback();

public class LibraryView : View
{
    public UserInterface LibraryUI;
    public UserInterface VisibilityUI;
    public ViewSwitcher libraryViewSwitcher;

    public List<LibraryPane> switcherPanes;

    public VisibilityView visibilityView;

    private SwitchCallback closeCallback;
    

    public void setup(SwitchCallback switchCallback)
    {
        this.closeCallback = switchCallback;
    }

    void Update()
    {
        LibraryUI.RectTransform.localScale = new Vector3(1, 1, 1);
        LibraryUI.RectTransform.eulerAngles = new Vector3(0, 45, 0);
        LibraryUI.RectTransform.position = new Vector3(3, 1, 2);

        VisibilityUI.RectTransform.localScale = new Vector3(1, 1, 1);
        VisibilityUI.RectTransform.eulerAngles = new Vector3(0, 135, 0);
        VisibilityUI.RectTransform.position = new Vector3(3, 1, -2);
    }

    public void onSwitcherActive()
    {
        libraryViewSwitcher.SwitchTo(0);
    }

    public override void Initialize()
    {
        base.Initialize();

        String path =
            @"C:\Users\sciss\Documents\UnivVR\Assets\UnivVR_Library.xml";
        String libraryxml = File.ReadAllText(path);

        switcherPanes = new List<LibraryPane>();
        switcherPanes.Add(ViewData.CreateView<LibraryPane>(libraryViewSwitcher, libraryViewSwitcher));
        switcherPanes[0].InitializeViews();
        switcherPanes[0].backbutton.Deactivate();
        switcherPanes[0].setup(null, closeCallback);

        
        using (XmlReader reader = XmlReader.Create(new StringReader(libraryxml)))
        {
            reader.ReadToFollowing("Library");
            reader.Read(); // position on first menu element
            while (reader.Name != "Library" && reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.Name == "LibraryList")
                {
                    createLibraryList(reader.ReadSubtree(),
                        switcherPanes[0], 0);
                }
                else if (reader.Name == "TreeList")
                {
                    createTreeList(reader.ReadSubtree(), switcherPanes[0], 0);
                }
                else if (reader.Name == "Location")
                {
                    createLocation(reader.ReadSubtree(), switcherPanes[0].organizer);
                }
                reader.Read(); // position on next menu element
            }


            reader.ReadToFollowing("VisibilityClasses");
            reader.ReadToDescendant("VisibilityClass"); //position on first visibilityclass element
            while (reader.Name != "VisibilityClasses" && reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.Name == "VisibilityClass")
                {
                    createVisibilityPane(reader.ReadSubtree(), visibilityView);
                }
                reader.Read();
            }
        }
    }


    private void createLibraryList(XmlReader subreader, LibraryPane pane, int backIndex)
    {
        SwitcherButton button = ViewData.CreateView<SwitcherButton>(pane.organizer, pane.organizer);
        button.InitializeViews();
        String label;
        readButtonInformation(subreader, button.navPopup, out label);

        subreader.ReadToFollowing("Content");

        subreader.Read(); //position on first content element        
        switcherPanes.Add(ViewData.CreateView<LibraryPane>(libraryViewSwitcher, libraryViewSwitcher));
        switcherPanes[switcherPanes.Count - 1].InitializeViews();
        switcherPanes[switcherPanes.Count - 1].setup(makeSwitchCallback(backIndex), closeCallback);
        button.setup(label, makeSwitchCallback(switcherPanes.Count - 1));
        int thisPaneIndex = switcherPanes.Count - 1;
        do
        {
            if (subreader.Name == "LibraryList")
            {
                createLibraryList(subreader.ReadSubtree(), switcherPanes[thisPaneIndex],
                                                            thisPaneIndex);
            }
            else if (subreader.Name == "TreeList")
            {
                createTreeList(subreader.ReadSubtree(), switcherPanes[thisPaneIndex], thisPaneIndex);
            }
            else if (subreader.Name == "Location")
            {
                createLocation(subreader.ReadSubtree(), switcherPanes[thisPaneIndex].organizer);
            }
            subreader.Read(); //position on next content element
        } while (subreader.Name != "Content" && subreader.NodeType != XmlNodeType.EndElement);

        subreader.Close();
    }

    private void createTreeList(XmlReader subreader, LibraryPane pane, int backIndex)
    {
        SwitcherButton button = ViewData.CreateView<SwitcherButton>(pane.organizer, pane.organizer);
        button.InitializeViews();
        String label;
        readButtonInformation(subreader, button.navPopup, out label);

        subreader.ReadToFollowing("Content");

        subreader.Read(); //position on first content element
        switcherPanes.Add(ViewData.CreateView<LibraryPane>(libraryViewSwitcher, libraryViewSwitcher));
        switcherPanes[switcherPanes.Count - 1].InitializeViews();
        switcherPanes[switcherPanes.Count - 1].setup(makeSwitchCallback(backIndex), closeCallback);

        button.setup(label, makeSwitchCallback(switcherPanes.Count - 1));
        do
        {
            if (subreader.Name == "TreeListButton")
            {
                createTreeListButton(subreader.ReadSubtree(), switcherPanes[switcherPanes.Count - 1]);
            }
            if (subreader.Name == "Location")
            {
                createLocation(subreader.ReadSubtree(), switcherPanes[switcherPanes.Count - 1].organizer);
            }
            subreader.Read(); //position on next content element
        } while (subreader.Name != "Content" && subreader.NodeType != XmlNodeType.EndElement);


        subreader.Close();

    }

    private void createTreeListButton(XmlReader subreader, LibraryPane pane)
    {

        TreeListButton button = ViewData.CreateView<TreeListButton>(pane.organizer, pane.organizer);
        button.InitializeViews();
        String label;
        readButtonInformation(subreader, button.navPopup, out label);
        button.setup(label, pane.closeTreeListButtons);

        subreader.ReadToFollowing("Content");
        subreader.Read(); //position on first content element

        do
        {
            if (subreader.Name == "Location")
            {
                createLocation(subreader.ReadSubtree(), button.SubItemGroup);
            }
            else if (subreader.Name == "Subtree")
            {
                createSubtree(subreader.ReadSubtree(), button);
            }

            subreader.Read(); //position on next content element
        } while (subreader.Name != "Content" && subreader.NodeType != XmlNodeType.EndElement);
        subreader.Close();
    }

    private void createLocation(XmlReader subreader, Group subItemGroup)
    {
        LocationButton button = ViewData.CreateView<LocationButton>(subItemGroup, subItemGroup);
        button.InitializeViews();
        //subreader.ReadToDescendant("Label");
        String label;
        readButtonInformation(subreader, button.navPopup, out label);
        button.setup(label, libraryViewSwitcher, visibilityView);


        subreader.Close();

    }

    private void createSubtree(XmlReader subreader, TreeListButton tlb)
    {
        Subtree subtreeButton = ViewData.CreateView<Subtree>(tlb.SubItemGroup, tlb.SubItemGroup);
        subtreeButton.InitializeViews();

        String label;
        readButtonInformation(subreader, subtreeButton.navPopup, out label);
        subtreeButton.setup(label, tlb.closeSubtrees);

        subreader.ReadToFollowing("SubtreeContent");
        subreader.Read();
        do
        {
            if (subreader.Name == "Location")
            {
                createLocation(subreader.ReadSubtree(), subtreeButton.SubItemGroup);
            }
            else if (subreader.Name == "Catalog")
            {
                createCatalog(subreader.ReadSubtree(), subtreeButton.SubItemGroup);
            }
            subreader.Read(); //position on next content element
        } while (subreader.Name != "Content" && subreader.NodeType != XmlNodeType.EndElement);


        subreader.Close();

    }

    private void createCatalog(XmlReader subreader, Group itemGroup)
    {
        Debug.Log("Create catalog here");
        subreader.Close();
    }

    private void createVisibilityPane(XmlReader subreader, VisibilityView vis)
    {

        VisibilityPane visibilitypane = ViewData.CreateView<VisibilityPane>(vis.organizer, vis.organizer);
        visibilitypane.InitializeViews();

        subreader.ReadToDescendant("VisID");
        visibilityView.visibilityPanes.Add(subreader.ReadElementContentAsString(), visibilitypane);

        subreader.Read(); //read to PaneLabel
        visibilitypane.label.Text.Value = subreader.ReadElementContentAsString();

        subreader.ReadToNextSibling("CheckboxOption");
        while (subreader.Name != "VisibilityClass" && subreader.NodeType != XmlNodeType.EndElement)
        {
            CommandCheckBox newcheckbox = ViewData.CreateView<CommandCheckBox>(visibilitypane.checkboxGroup, visibilitypane.checkboxGroup);
            newcheckbox.InitializeViews();
            newcheckbox.setup(ElementAlignment.TopLeft, subreader["OptionLabel"], subreader["CheckCommand"] + '\n', subreader["UncheckCommand"] + '\n',
                subreader["InfoText"], Resources.Load<AudioClip>(subreader["InfoClip"]), subreader["CheckedByDefault"], 14, visibilityView);

            subreader.ReadToNextSibling("CheckboxOption");
        }
        subreader.Close();
    }

    private void readButtonInformation(XmlReader reader, NavigatorPopup popup, out String label)
    {
        reader.ReadToDescendant("Label");
        label = reader.ReadElementContentAsString();

        bool popupEnabled = false;
        String flyto = "";
        String jumpto = "";
        String target = "";
        String surface = "";
        AudioClip infoClip = null;
        String infoText = "";
        List<String> visibilityIDs = new List<String>();
        reader.ReadToNextSibling("FlyTo");
        if (reader.GetAttribute("Enabled") == "true")
        {
            flyto = reader.ReadElementContentAsString() + '\n';
            popupEnabled = true;
        }
        else
        {
            popup.flytoButton.IsDisabled.Value = true;
        }

        reader.ReadToNextSibling("JumpTo");
        if (reader.GetAttribute("Enabled") == "true")
        {
            jumpto = reader.ReadElementContentAsString() + '\n';
            popupEnabled = true;
        }
        else
        {
            popup.jumptoButton.IsDisabled.Value = true;
        }

        reader.ReadToNextSibling("Target");
        if (reader.GetAttribute("Enabled") == "true")
        {
            target = reader.ReadElementContentAsString() + '\n';
            popupEnabled = true;
        }
        else
        {
            popup.targetButton.IsDisabled.Value = true;
        }

        reader.ReadToNextSibling("Surface");
        if (reader.GetAttribute("Enabled") == "true")
        {
            surface = reader.ReadElementContentAsString() + '\n';
            popupEnabled = true;
        }
        else
        {
            popup.surfaceButton.IsDisabled.Value = true;
        }

        reader.ReadToNextSibling("Soundfile");
        if (reader.GetAttribute("Enabled") == "true")
        {
            infoClip = Resources.Load<AudioClip>(reader.ReadElementContentAsString());
        }

        reader.ReadToNextSibling("InfoText");
        if (reader.GetAttribute("Enabled") == "true")
        {
            infoText = reader.ReadElementContentAsString();
        }

        while (reader.ReadToNextSibling("ButtonVisID"))
        {
            visibilityIDs.Add(reader.ReadElementContentAsString());
        }

        popup.setup(flyto, jumpto, target, surface, visibilityIDs, popupEnabled, visibilityView, infoClip, infoText);

    }

    private SwitchCallback makeSwitchCallback(int index)
    {
        return delegate () { if (index >= 0) { libraryViewSwitcher.SwitchTo(index); } };
    }

}
