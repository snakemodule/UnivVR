using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight;
using MarkLight.Views.UI;
using System.Text;
using System;
using System.Xml;
using System.IO;

public class MainView : View
{

    public ViewSwitcher Switcher;

    public List<LibraryPane> switcherPanes;

    public override void Initialize()
    {
        base.Initialize();

        String path =
            @"C:\Users\sciss\Documents\UnivVR\Assets\SomeLibraryXML.xml";
        String libraryxml = File.ReadAllText(path);

        switcherPanes = new List<LibraryPane>();
        switcherPanes.Add(ViewData.CreateView<LibraryPane>(Switcher, Switcher));
        switcherPanes[0].InitializeViews();
        //switcherPanes.Add(ViewData.CreateView<LibraryPane>(Switcher, Switcher));

        //switcherPanes.Add(ViewData.CreateView<LibraryPane>(Switcher, Switcher));
        //switcherPanes[0].BackgroundColor.Value = Color.cyan;
        switcherPanes[0].backbutton.Deactivate();
        //    switcherPanes[0].backbutton.IsActive.Value = false;
        //     Debug.Log(switcherPanes[0].backbutton.IsActive.Value);

        // Create an XmlReader
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
                reader.Read(); // position on next menu element
                //Debug.Log("The XMLReader has looped and is currently on: " + reader.Name + " which is a(n) " + reader.NodeType + " node.");
                //Debug.Log("The XMLReader has returned from ReadSubtree and is Currently on: " + reader.Name + " which is a(n) " + reader.NodeType + " node.");
            }
        }



        //Switcher.DeactiveViews.Value = true;
        //Switcher.SwitchToDefault.Value = true;
        Switcher.SetDefaultValues();
        InitializeViews();
    }


    void createLibraryList(XmlReader subreader, LibraryPane pane, int backIndex)
    {
        ButtonWrapper button = ViewData.CreateView<ButtonWrapper>(pane.organizer, pane.organizer);
        button.InitializeViews();
        subreader.ReadToDescendant("Label");
        button.innerButton.Text.Value = subreader.ReadElementContentAsString();
        button.Switcher = this.Switcher;
        button.navigationPopupEnabled = false;


        String flyto;
        String jumpto;
        String target;
        String surface;

        readButtonInformation(subreader, out flyto, out jumpto, out target, out surface, out button.navigationPopupEnabled);

        subreader.ReadToFollowing("Content");
        if (!subreader.IsEmptyElement)
        {
            subreader.Read(); //position on first content element
            //ny content nod betyder ny region för det innehållet, men bara om det finns grejer i den
            switcherPanes.Add(ViewData.CreateView<LibraryPane>(Switcher, Switcher));
            switcherPanes[switcherPanes.Count - 1].InitializeViews();
            switcherPanes[switcherPanes.Count - 1].backIndex = backIndex;
            switcherPanes[switcherPanes.Count - 1].Switcher = this.Switcher;
            button.switchIndex = switcherPanes.Count - 1; //vår knapp byter till den nya regionen av content
            do
            {
                if(subreader.Name == "LibraryList")
                {
                    createLibraryList(subreader.ReadSubtree(), switcherPanes[switcherPanes.Count - 1],
                                                             switcherPanes.Count - 1);
                    Debug.Log("The XMLReader has returned from ReadSubtree and is Currently on: " + subreader.Name + " which is a(n) " + subreader.NodeType + " node. Depth: "+ subreader.Depth);
                }
                else if(subreader.Name == "TreeList")
                {

                }
                subreader.Read(); //position on next content element
                Debug.Log("" + subreader.Name + " which is a(n) " + subreader.NodeType + " node. Depth: " + subreader.Depth);
            } while (subreader.Name != "Content" && subreader.NodeType != XmlNodeType.EndElement);
        }

        subreader.Close();


    }

    void createTreeList(XmlReader subreader, LibraryPane pane, int backIndex)
    {
        ButtonWrapper button = ViewData.CreateView<ButtonWrapper>(pane.organizer, pane.organizer);
        button.InitializeViews();
        subreader.ReadToDescendant("Label");
        button.innerButton.Text.Value = subreader.ReadElementContentAsString();
        button.Switcher = this.Switcher;
        button.navigationPopupEnabled = false;


        String flyto;
        String jumpto;
        String target;
        String surface;
        readButtonInformation(subreader, out flyto, out jumpto, out target, out surface, out button.navigationPopupEnabled);

        subreader.ReadToFollowing("Content");
        if (!subreader.IsEmptyElement)
        {
            subreader.Read(); //position on first content element
            //ny content nod betyder ny region för det innehållet, men bara om det finns grejer i den
            switcherPanes.Add(ViewData.CreateView<LibraryPane>(Switcher, Switcher));
            switcherPanes[switcherPanes.Count - 1].InitializeViews();
            switcherPanes[switcherPanes.Count - 1].backIndex = backIndex;
            switcherPanes[switcherPanes.Count - 1].Switcher = this.Switcher;
            button.switchIndex = switcherPanes.Count - 1; //vår knapp byter till den nya regionen av content
            do
            {
                if (subreader.Name == "TreeListButton")
                {
                    createTreeListButton(subreader.ReadSubtree(), switcherPanes[switcherPanes.Count - 1]);
                }
                subreader.Read(); //position on next content element
                Debug.Log("looking for treelist buttons at:" + subreader.Name + " which is a(n) " + subreader.NodeType + " node. Depth: " + subreader.Depth);
            } while (subreader.Name != "Content" && subreader.NodeType != XmlNodeType.EndElement);
        }

        subreader.Close();
        
    }

    void createTreeListButton(XmlReader subreader, LibraryPane pane)
    {
        //Tar in XMLreadern(subreader) och parent LibraryPane(pane) för att veta var knappen ska ligga.
        //Parent LibraryPane är alltid en TreeList. Har ingen backbutton.
        TreeListButton button = ViewData.CreateView<TreeListButton>(pane.organizer, pane.organizer);
        button.InitializeViews();
        subreader.ReadToDescendant("Label");
        button.innerButton.Text.Value = subreader.ReadElementContentAsString();
        button.navigationPopupEnabled = false;

        String flyto;
        String jumpto;
        String target;
        String surface;
        readButtonInformation(subreader, out flyto, out jumpto, out target, out surface, out button.navigationPopupEnabled);

        subreader.ReadToFollowing("Content");
        if (!subreader.IsEmptyElement)
        {
            subreader.Read(); //position on first content element
            //ny content nod betyder ny region för det innehållet, men bara om det finns grejer i den
            //switcherPanes.Add(ViewData.CreateView<LibraryPane>(Switcher, Switcher));
            //switcherPanes[switcherPanes.Count - 1].InitializeViews();
            //switcherPanes[switcherPanes.Count - 1].backIndex = backIndex;
            //switcherPanes[switcherPanes.Count - 1].Switcher = this.Switcher;
            //button.switchIndex = switcherPanes.Count - 1; //vår knapp byter till den nya regionen av content
            
            do
            {
                if (subreader.Name == "Location")
                {
                    createLocation(subreader.ReadSubtree(), button.SubItemGroup);
                } else if (subreader.Name == "Subtree") {
                    createSubtree(subreader.ReadSubtree(), button.SubItemGroup);
                }

                subreader.Read(); //position on next content element
                Debug.Log("looking for treelist buttons at:" + subreader.Name + " which is a(n) " + subreader.NodeType + " node. Depth: " + subreader.Depth);
            } while (subreader.Name != "Content" && subreader.NodeType != XmlNodeType.EndElement);
        }
        subreader.Close();
    }

    void createLocation(XmlReader subreader, Group subItemGroup)
    {
        ButtonWrapper button = ViewData.CreateView<ButtonWrapper>(subItemGroup, subItemGroup);
        button.InitializeViews();
        subreader.ReadToDescendant("Label");
        button.innerButton.Text.Value = subreader.ReadElementContentAsString();
        button.navigationPopupEnabled = false;

        String flyto;
        String jumpto;
        String target;
        String surface;
        readButtonInformation(subreader, out flyto, out jumpto, out target, out surface, out button.navigationPopupEnabled);

        subreader.Close();

    }

    void createSubtree(XmlReader subreader, Group itemGroup)
    {
        Subtree subtreeButton = ViewData.CreateView<Subtree>(itemGroup, itemGroup);
        subtreeButton.InitializeViews();
        subreader.ReadToDescendant("Label");
        
        subtreeButton.innerButton.Text.Value = subreader.ReadElementContentAsString();
        subtreeButton.navigationPopupEnabled = false;


        String flyto;
        String jumpto;
        String target;
        String surface;
        readButtonInformation(subreader, out flyto, out jumpto, out target, out surface, out subtreeButton.navigationPopupEnabled);

        subreader.ReadToFollowing("SubtreeContent");
        if (!subreader.IsEmptyElement)
        {
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
        }

        subreader.Close();

    }

    void createCatalog(XmlReader subreader, Group itemGroup)
    {
        Debug.Log("Create catalog here");
        subreader.Close();
    }

    public void readButtonInformation(XmlReader reader, out string flyto, out string jumpto, out string target, out string surface, out bool popupenabled)
    {
        popupenabled = false;
        flyto = "";
        jumpto = "";
        target = "";
        surface = "";
        reader.ReadToNextSibling("FlyTo");
        if (reader.GetAttribute("Enabled") == "true")
        {
            flyto = reader.ReadElementContentAsString();
            popupenabled = true;
        }

        reader.ReadToNextSibling("JumpTo");
        if (reader.GetAttribute("Enabled") == "true")
        {
            jumpto = reader.ReadElementContentAsString();
            popupenabled = true;
        }

        reader.ReadToNextSibling("Target");
        if (reader.GetAttribute("Enabled") == "true")
        {
            target = reader.ReadElementContentAsString();
            popupenabled = true;
        }

        reader.ReadToNextSibling("Surface");
        if (reader.GetAttribute("Enabled") == "true")
        {
            surface = reader.ReadElementContentAsString();
            popupenabled = true;
        }
    }

}
