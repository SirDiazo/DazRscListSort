using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMod;
using KSerialization;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HarmonyLib;
using System.Reflection;

namespace DazRscListSort
{
    public static class DazStatics
    {
        //static methods for ease of programming, these could probably be moved into DazRscSortList data storage object as non-static without issues?
        public static Dictionary<int, HdrBtn> hdrButtons = new Dictionary<int, HdrBtn>(); //Save our header button objects for easy reference
        public static bool hdrButtonChangeInProgress = false;//apparently ONI is multithreaded and the PinnedResourcesWindow can try to update in teh middle of a button change, if TRUE, PinnedResourcePanel.SortRows is locked out and will by bypassed to avoid conflicts
        public static bool init = false;
        public static DazRscListSortData currentDstore; //current focus world
        public static DazRscListSortData baseDstore; //always World 0 for override lists

        public static bool ShowSortArrows = false; //show sort arrows? reset this to false on world change
        public static Dictionary<Tag, GameObject> MiddleManRows; //middleman object to access private list in PinnedResourcesWindow class
        public static bool ListRefreshRequired = false; //force manual list update after sort button clicked 1/2
        public static bool ListRefreshComplete = false; //force manual list update after sort button clicked 2/2
        public static bool PinnedResourceWindowOnSpawnComplete = false;
        //begin other mod checks, true if that mod found on game load
        public static bool CritterInventoryInstalled = false;
        //end other mods check

        //If CritterInventory present, populate following for reflection:
        public static Assembly CritInvAssembly;
        //public static Type CritInvCritterTypeEnum;
        public static object CritInvCritterEnumValueWild;
        public static object CritInvCritterEnumValueTame;
        public static object CritInvCritterEnumValueArtificial;
        public static Component CritInvPinnedCritterManager;
        public static FieldInfo CritInvPinnedObjectsField;
        public static IDictionary CritInvCurrentPinnedObject;
        public static IDictionary CritInvCurrentListWild;
        public static IDictionary CritInvCurrentListTame;
        public static IDictionary CritInvCurrentListArtificial;
        public static bool GetOverRideButtonState(int btnNum)
        {
            if (btnNum < 1 || btnNum > 5)
            {
                btnNum = 1;
                Debug.Log("Daz GetOverRideButtonState out of range, button set to 1");
            }
            if (btnNum == 1)
            {
                return baseDstore.Button1OverRide;
            }
            else if (btnNum == 2)
            {
                return baseDstore.Button2OverRide;
            }
            else if (btnNum == 3)
            {
                return baseDstore.Button3OverRide;
            }
            else if (btnNum == 4)
            {
                return baseDstore.Button4OverRide;
            }
            else if (btnNum == 5)
            {
                return baseDstore.Button5OverRide;
            }
            else
            {
                Debug.Log("Daz GetOverRideButtonState hit error catch ELSE");
                return false;
            }
        }

        public static void SetOverRideButtonState(int btnNum)
        {
            if (btnNum < 1 || btnNum > 5)
            {
                btnNum = 1;
                Debug.Log("Daz SetOverRideButtonState out of range, button set to 1");
            }
            if (btnNum == 1)
            {
                baseDstore.Button1OverRide = !baseDstore.Button1OverRide;
            }
            else if (btnNum == 2)
            {
                baseDstore.Button2OverRide = !baseDstore.Button2OverRide;
            }
            else if (btnNum == 3)
            {
                baseDstore.Button3OverRide = !baseDstore.Button3OverRide;
            }
            else if (btnNum == 4)
            {
                baseDstore.Button4OverRide = !baseDstore.Button4OverRide;
            }
            else if (btnNum == 5)
            {
                baseDstore.Button5OverRide = !baseDstore.Button5OverRide;
            }
            else
            {
                Debug.Log("Daz SetOverRideButtonState hit error catch ELSE");
            }
        }
        public static void ShowSortArrowsMethod() //hide value list, show sort arrows
        {
            //DazShowSortArrows = true;
            foreach (KeyValuePair<Tag, GameObject> kvp in DazStatics.MiddleManRows)
            {
                Transform tfr = kvp.Value.transform.Find("UpArrow"); //find UpArrow game obect and show it
                if (tfr == null)
                {
                    Debug.Log("Daz trf null");
                    SetupUpArrowButton(kvp.Value, kvp.Key, "base"); //version 1.1 - 1.2 upgrade can hit this, should never hit otherwise
                    Debug.Log("Daz trf null2");
                    tfr = kvp.Value.transform.Find("UpArrow");
                    Debug.Log("Daz trf null2a" + (tfr==null));
                }
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(true);
                Debug.Log("Daz trf null3");
                tfr = kvp.Value.transform.Find("DownArrow");//find DownArrow game obect and show it
                if (tfr == null)
                {
                    SetupDownArrowButton(kvp.Value, kvp.Key, "base");
                    tfr = kvp.Value.transform.Find("DownArrow");
                }
                //GameObject go4 = tfr.gameObject;
                tfr.gameObject.SetActive(true);
                Debug.Log("Daz trf null4");
                tfr = kvp.Value.transform.Find("ValueLabel");//find ValueLabel game obect and hide it, not this is a Klei default UI object
                if (tfr != null)
                {
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(false);
                }
            }
            Debug.Log("Daz trf null5");
            if (CritterInventoryInstalled)
            {
                foreach (HierarchyReferences href in CritInvCurrentListWild.Values)
                {
                    Transform tfr = href.gameObject.transform.Find("UpArrow"); //find UpArrow game obect and show it
                    if (tfr != null)
                    {
                        //GameObject go4 = tfr.gameObject;
                        tfr.gameObject.SetActive(true);
                    }
                    tfr = href.gameObject.transform.Find("DownArrow");//find DownArrow game obect and show it
                    if (tfr != null)
                    {
                        //GameObject go4 = tfr.gameObject;
                        tfr.gameObject.SetActive(true);
                    }
                    tfr = href.gameObject.transform.Find("ValueLabel");//find ValueLabel game obect and hide it, not this is a Klei default UI object
                    if (tfr != null)
                    {
                        //GameObject go4 = tfr.gameObject;
                        tfr.gameObject.SetActive(false);
                    }
                }
            }
        }
        public static void HideSortArrowsMethod() //hide value list, show sort arrows
        {
            //DazShowSortArrows = false;
            foreach (KeyValuePair<Tag, GameObject> kvp in DazStatics.MiddleManRows)
            {
                Transform tfr = kvp.Value.transform.Find("UpArrow"); //find UpArrow game obect and show it
                if (tfr != null)
                {
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(false);
                }
                tfr = kvp.Value.transform.Find("DownArrow");//find DownArrow game obect and show it
                if (tfr != null)
                {
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(false);
                }
                tfr = kvp.Value.transform.Find("ValueLabel");//find ValueLabel game obect and hide it, not this is a Klei default UI object
                if (tfr != null)
                {
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(true);
                }
            }
            if (CritterInventoryInstalled)
            {
                foreach (HierarchyReferences href in CritInvCurrentListWild.Values)
                {
                    Transform tfr = href.gameObject.transform.Find("UpArrow"); //find UpArrow game obect and show it
                    if (tfr != null)
                    {
                        //GameObject go4 = tfr.gameObject;
                        tfr.gameObject.SetActive(false);
                    }
                    tfr = href.gameObject.transform.Find("DownArrow");//find DownArrow game obect and show it
                    if (tfr != null)
                    {
                        //GameObject go4 = tfr.gameObject;
                        tfr.gameObject.SetActive(false);
                    }
                    tfr = href.gameObject.transform.Find("ValueLabel");//find ValueLabel game obect and hide it, not this is a Klei default UI object
                    if (tfr != null)
                    {
                        //GameObject go4 = tfr.gameObject;
                        tfr.gameObject.SetActive(true);
                    }
                }
            }
        }
    
    
        public static void SyncRows(Dictionary<Tag, GameObject> KlieRows)
        {
            MiddleManRows = KlieRows;
        }
        public static void GameStartDataLoad() //inital load of data from DataStore to static object as static object can't save across sessions
        {
            Debug.Log("Daz static load" + (ClusterManager.Instance.activeWorld.worldInventory.GetComponent("CritterInventory") == null));
            //ClusterManager.Instance.activeWorld.FindOrAddComponent<DazRscListSort.DazRscListSortData>();
            baseDstore = ClusterManager.Instance.GetWorld(0).FindOrAddComponent<DazRscListSort.DazRscListSortData>(); //always use world 0 for override data, not world we load into
            currentDstore = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).FindOrAddComponent<DazRscListSort.DazRscListSortData>(); //get dStore of current world
            ShowSortArrows = false;

            //If Critter Inventory is installed:
            if(CritterInventoryInstalled)
            {
                CritInvPinnedCritterManager = PinnedResourcesPanel.Instance.GetComponent("PinnedCritterManager");
                CritInvAssembly = CritInvPinnedCritterManager.GetType().Assembly;
                Type CritterEnumType = CritInvAssembly.GetType("PeterHan.CritterInventory.CritterType");
                CritInvCritterEnumValueWild = CritterEnumType.GetEnumValues().GetValue(0);
                CritInvCritterEnumValueTame = CritterEnumType.GetEnumValues().GetValue(1);
                CritInvCritterEnumValueArtificial = CritterEnumType.GetEnumValues().GetValue(2);
                CritInvPinnedObjectsField = CritInvPinnedCritterManager.GetType().GetField("pinnedObjects", BindingFlags.Instance | BindingFlags.NonPublic);
                CritInvCurrentPinnedObject = (IDictionary)CritInvPinnedObjectsField.GetValue(CritInvPinnedCritterManager);
                CritInvCurrentListWild = (IDictionary)CritInvCurrentPinnedObject[CritInvCritterEnumValueWild];
                CritInvCurrentListTame = (IDictionary)CritInvCurrentPinnedObject[CritInvCritterEnumValueTame];
                CritInvCurrentListArtificial = (IDictionary)CritInvCurrentPinnedObject[CritInvCritterEnumValueArtificial];




            }
        }

        public static void OnGameSpawn()
        {
            Game.Instance.Subscribe((int)GameHashes.ActiveWorldChanged, OnWorldChanged);
            Debug.Log("Daz gamespawn");

        }

        public static void OnGameDeSpawn()
        {
            Game.Instance.Unsubscribe((int)GameHashes.ActiveWorldChanged);
            Debug.Log("Daz gamedespawn");
            CritInvPinnedObjectsField = null;
            CritInvCurrentPinnedObject = null;
            CritInvCurrentListWild = null;
            CritInvCurrentListTame = null;
            CritInvCurrentListArtificial = null;
            PinnedResourceWindowOnSpawnComplete = false;
        }
        public static void OnWorldChanged(object obj) //think obj is a tuple with old and new worldIDs?
        {
            DazStatics.hdrButtonChangeInProgress = true;
            currentDstore.SanitizeList(); //still old world's dstore
            Debug.Log("Daz dstore check " + (ClusterManager.Instance.activeWorld.GetComponent<DazRscListSortData>()==null));
            currentDstore = ClusterManager.Instance.activeWorld.FindOrAddComponent<DazRscListSort.DazRscListSortData>();
            currentDstore.Initialise(ClusterManager.Instance.activeWorldId); //needed if first time we've visited this world, check for this inside method
            DazStatics.currentDstore.UpdatePinnedResources(); //add resources to list for new page
            DazStatics.RscListRefreshHeader(); //updated header icons for new current list
            DazStatics.hdrButtonChangeInProgress = false; //reenabled PinnedResourcePanel.SortRows
        }

 
        public static void RscListRefreshHeader() //refresh header button colors after page change
        {
            foreach (int i in DazStatics.hdrButtons.Keys)
            {
                if (DazStatics.hdrButtons[i].BtnNum == currentDstore.CurrentHdrButton)
                {
                    if (GetOverRideButtonState(i))
                    {
                        Image btnImg = DazStatics.hdrButtons[i].GetComponent<Image>();//set color cyan
                        btnImg.color = Color.magenta;
                    }
                    else
                    {
                        Image btnImg = DazStatics.hdrButtons[i].GetComponent<Image>();//set color green
                        btnImg.color = Color.green;
                    }
                }
                else
                {
                    Image btnImg = DazStatics.hdrButtons[i].GetComponent<Image>(); //set color gray
                    btnImg.color = Color.gray;
                }
            }
        }
        
        public static void SortRscList(Dictionary<Tag, GameObject> rows) //primary work done here, compare list and sort, call from PinnedResourceList with Harmony
        {
            //Debug.Log("Daz new discs" + DiscoveredResources.Instance.newDiscoveries.Count);
            //foreach(KeyValuePair<Tag,float> kvp in DiscoveredResources.Instance.newDiscoveries)
            //{
            //    Debug.Log("Daz new discoveries check " + kvp.Key);
            //}
            //Debug.Log("Daz sattic rsc list A" + (ClusterManager.Instance.activeWorld.worldInventory.GetComponent("CritterInventory")==null));

            
            List<TagSourceItem> ReturnList = new List<TagSourceItem>(); //have to add the inactive items back into list at end, so instatiate an object to modify and return

            if (DazStatics.hdrButtons.ContainsKey(1) && (DazStatics.hdrButtonChangeInProgress == false) && DazStatics.PinnedResourceWindowOnSpawnComplete) //race condition check, this method can run before buttons instantiate so skip if buttons don't exist yet
                                                                                                         //also skip if button change in progress so that doesn't screw data up
            {
                //start rocket etst logging
                Debug.Log("Daz base dstore " + ClusterManager.Instance.GetWorld(0).worldInventory.GetComponent<DazRscListSortData>().Button1List.Count);
                Debug.Log("Daz 9 dstore a" + (ClusterManager.Instance.GetWorld(9) == null));
                Debug.Log("Daz 9 dstore b" + (ClusterManager.Instance.GetWorld(9).worldInventory == null));
                Debug.Log("Daz 9 dstore c" + (ClusterManager.Instance.GetWorld(9).worldInventory.GetComponent<DazRscListSortData>() == null));
                Debug.Log("Daz 9 dstore d" + (ClusterManager.Instance.GetWorld(9).worldInventory.GetComponent<DazRscListSortData>().Button1List == null));
                //Debug.Log("Daz 9 dstore " + ClusterManager.Instance.GetWorld(9).worldInventory.GetComponent<DazRscListSortData>().Button1List.Count);
                //Debug.Log("Daz 10 dstore " + ClusterManager.Instance.GetWorld(10).worldInventory.GetComponent<DazRscListSortData>().Button1List.Count);
                //Debug.Log("Daz 11 dstore " + ClusterManager.Instance.GetWorld(11).worldInventory.GetComponent<DazRscListSortData>().Button1List.Count);
                Debug.Log("Daz this store list 1 check " + ClusterManager.Instance.activeWorld.worldInventory.GetComponent<DazRscListSortData>().Button1List.Count);

                //end rocket test logging

                //Debug.Log("Daz sattic rsc list A1");
                List<TagSourceItem> OldListActive = new List<TagSourceItem>(); //list of active items
                foreach (KeyValuePair<Tag, GameObject> row in rows) //we only need to sort objects actively being displayed, there seems to be no destruction routine for objects that were shown and hidden and they stay around until game quit.
                {
                    if (ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(row.Key) && row.Value.activeSelf) //is resource displayed?
                    {
                        //Debug.Log("Daz base check!" + row.Key);
                        OldListActive.Add(new TagSourceItem(row.Key, "base")); //make list of active only objects, includes NEW objects we don't want to save to sorted list
                        //currentDstore.AddItemToCurList(new TagSourceItem(row.Key, "base"));
                    }
                }
                //Debug.Log("Daz sattic rsc list B");
                if (CritterInventoryInstalled) //pull rows from Critter Inventory if installed
                {
                   //Debug.Log("Daz sattic rsc list C" + (CritInvCurrentListWild.Count));
                    foreach (Tag tg in CritInvCurrentListWild.Keys)
                    {
                        HierarchyReferences href = (HierarchyReferences)CritInvCurrentListWild[tg];
                        if (href.gameObject.activeSelf)
                        {
                            OldListActive.Add(new TagSourceItem(tg, "ciwild"));
                        }
                    }
                    //Debug.Log("Daz sattic rsc list d");
                    foreach (Tag tg in CritInvCurrentListTame.Keys)
                    {
                        HierarchyReferences href = (HierarchyReferences)CritInvCurrentListTame[tg];
                        if (href.gameObject.activeSelf)
                        {
                            OldListActive.Add(new TagSourceItem(tg, "citame"));
                        }
                    }
                    foreach (Tag tg in CritInvCurrentListArtificial.Keys)
                    {
                        HierarchyReferences href = (HierarchyReferences)CritInvCurrentListArtificial[tg];
                        if (href.gameObject.activeSelf)
                        {
                            OldListActive.Add(new TagSourceItem(tg, "ciart"));
                        }
                    }

                }
                //Debug.Log("Daz act list old:" + OldListActive.Count); //why does adding Criter to empty list not work?
                List<TagSourceItem> toRemove = new List<TagSourceItem>();
                List<TagSourceItem> currentList = currentDstore.GetCurrentList();
                foreach (TagSourceItem tagSrc in currentList)  //check nothing has been removed from list by the game
                {
                    if (!OldListActive.Contains(tagSrc)) //item exists in this page's list, but not in game's list, remove it from this page's list
                    {
                        //DazStatics.RscListCollection[DazStatics.RscListCurrentID].Remove(tag); //remove from saved list, this line is crash error, can't edit list being enumerated so use toRemove
                        toRemove.Add(tagSrc); //save tags to remove
                    }
                }
                foreach (TagSourceItem tg in toRemove) //remove tags from saved list for this page
                {
                    currentDstore.RemoveItem(tg);
                }
                currentList = currentDstore.GetCurrentList(); //refresh list from data storage (tag/source)
                foreach (TagSourceItem tg in OldListActive) //check list to see if anythign new added to Klei's list.
                {
                    if (!currentList.Contains(tg)) //missing tag from saved list in mod that exists in game list
                    {
                        currentDstore.AddItemToCurList(tg); //add tag to end of list
                        if(tg.source == "base")
                        {
                            Debug.Log("Daz new item base " + tg.tag);
                            SetupUpArrowButton(rows[tg.tag], tg.tag, tg.source); //check for, and add up arrow if needed
                            SetupDownArrowButton(rows[tg.tag], tg.tag,tg.source); //check for, and add down arrow if needed
                        }
                        else if(tg.source == "ciwild")
                        {
                            Debug.Log("Daz new item wild " + tg.tag);
                            HierarchyReferences href = (HierarchyReferences)CritInvCurrentListWild[tg.tag];
                            SetupUpArrowButton(href.gameObject, tg.tag, tg.source); //check for, and add up arrow if needed
                            SetupDownArrowButton(href.gameObject, tg.tag, tg.source); //check for, and add down arrow if needed
                        }
                        else if (tg.source == "citame")
                        {
                            Debug.Log("Daz new item tame " + tg.tag);
                            HierarchyReferences href = (HierarchyReferences)CritInvCurrentListTame[tg.tag];
                            SetupUpArrowButton(href.gameObject, tg.tag, tg.source); //check for, and add up arrow if needed
                            SetupDownArrowButton(href.gameObject, tg.tag, tg.source); //check for, and add down arrow if needed
                        }
                        else if (tg.source == "ciart")
                        {
                            Debug.Log("Daz new item art" + tg.tag);
                            HierarchyReferences href = (HierarchyReferences)CritInvCurrentListArtificial[tg.tag];
                            SetupUpArrowButton(href.gameObject, tg.tag, tg.source); //check for, and add up arrow if needed
                            SetupDownArrowButton(href.gameObject, tg.tag, tg.source); //check for, and add down arrow if needed
                        }
                    }
                }
                //foreach (TagSourceItem tg in currentList) //active items at start of returned list  //version 1.2, return list removed, setlastsibling now in this method
                //{
                //    ReturnList.Add(tg); //add items in sorted order to ReturnList
                //    SetupUpArrowButton(rows[tg], tg); //check for, and add up arrow if needed
                //    SetupDownArrowButton(rows[tg], tg); //check for, and add down arrow if needed
                //}
                //foreach (Tag tg in OldList) //add inactive items for compatibility so list passed to mod and list mod passes back for display contain the same items, if in a different order
                //{
                //    if (!ReturnList.Contains(tg))
                //    {
                //        ReturnList.Add(tg);
                //    }
                //}

                foreach(TagSourceItem tgItem in currentDstore.GetCurrentList())
                {
                    //Debug.Log("Daz List dump for display " + tgItem.tag + "?" + tgItem.source);
                    if(tgItem.source=="base")
                    {
                        rows[tgItem.tag].transform.SetAsLastSibling();
                    }
                    else if(tgItem.source=="ciwild")
                    {
                        object obj = CritInvCurrentListWild[tgItem.tag];
                        HierarchyReferences hrRef = (HierarchyReferences)obj;
                        hrRef.gameObject.transform.SetAsLastSibling();
                    }
                    else if (tgItem.source == "citame")
                    {
                        object obj = CritInvCurrentListTame[tgItem.tag];
                        HierarchyReferences hrRef = (HierarchyReferences)obj;
                        hrRef.gameObject.transform.SetAsLastSibling();
                    }
                    else if (tgItem.source == "ciart")
                    {
                        object obj = CritInvCurrentListArtificial[tgItem.tag];
                        HierarchyReferences hrRef = (HierarchyReferences)obj;
                        hrRef.gameObject.transform.SetAsLastSibling();
                    }
                }
                foreach(Tag newTag in DiscoveredResources.Instance.newDiscoveries.Keys)
                {
                    if (rows.Keys.Contains(newTag))
                    {
                        if (rows[newTag].activeSelf)
                        {
                            rows[newTag].transform.SetAsLastSibling();
                            SetupUpArrowButton(rows[newTag], newTag, "base"); //check for, and add up arrow if needed
                            SetupDownArrowButton(rows[newTag], newTag, "base"); //check for, and add down arrow if needed

                        }

                    }
                }
                PinnedResourcesPanel.Instance.clearNewButton.transform.SetAsLastSibling();
                PinnedResourcesPanel.Instance.seeAllButton.transform.SetAsLastSibling();

                //add dummy game object to list to force list update
                if (DazStatics.ListRefreshRequired && !DazStatics.ListRefreshComplete) //i can not get LayerRebuilder.MarkLayoutForRebuild to work, so add and then remove a dummy game object to force list refresh
                                                                                       //this IF statement runs over 2 consecutive list updates
                {
                    Transform placeHolder = PinnedResourcesPanel.Instance.seeAllButton.transform.parent.Find("DazPlaceHolder"); //does our dummy gameobject already exist?
                    GameObject placeHoldergo;
                    if (placeHolder == null)
                    {
                        //make dummy game object
                        placeHoldergo = new GameObject("DazPlaceHolder", typeof(RectTransform), typeof(KLayoutElement));
                        placeHoldergo.transform.SetParent(PinnedResourcesPanel.Instance.seeAllButton.transform.parent, false);
                    }
                    else
                    {
                        //dummy object exists, assign
                        placeHoldergo = placeHolder.gameObject;
                    }
                    placeHoldergo.SetActive(true); //first step, make dummy object active so it's "in" the list, despite the player not seeing it
                    placeHoldergo.transform.SetAsLastSibling(); //add to group, this forces list refresh
                    DazStatics.ListRefreshComplete = true; //done, set true so else if below runs next List update
                }
                else if (DazStatics.ListRefreshComplete)
                {
                    //dummy obejct has done it's job, disable it until next sort action
                    PinnedResourcesPanel.Instance.seeAllButton.transform.parent.Find("DazPlaceHolder").gameObject.SetActive(false); //have to find dummy object again, but we know it exists for sure
                    DazStatics.ListRefreshComplete = false; //reset refresh state so it can be called again
                    DazStatics.ListRefreshRequired = false;
                }
            }
            //else //method had to be bypassed, most likely due to header button change in progress (see if statement) //version 1.2 this method now returns void
            //{
            //    ReturnList = OldList;
            //}
            //return ReturnList;
        }
        public static void SetupButton(GameObject parent, int btnnum, int posOffset) //create header buttons for page switching
        {
            if (!DazStatics.hdrButtons.ContainsKey(btnnum) || DazStatics.hdrButtons.ContainsKey(btnnum) && DazStatics.hdrButtons[btnnum] == null) //error check, this should always return TRUE as it runs in PinnedResrouceWindow.OnSpawn
            {
                GameObject btn = new GameObject("RscList" + btnnum, typeof(RectTransform), typeof(KLayoutElement), typeof(HdrBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                HdrBtn btnImg = btn.GetComponent<HdrBtn>(); //setup the button
                btnImg.Setup(btnnum, btn); //call Setup in button class, pass this button's number and the HdrBtn object
                btnTfrm.sizeDelta = new Vector2(12, 12); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(posOffset, 0, 0)); //move button into place
                if (!DazStatics.hdrButtons.ContainsKey(btnnum)) //error check, this should alwasy return true and run first statement
                {
                    DazStatics.hdrButtons.Add(btnnum, btnImg);
                }
                else if (DazStatics.hdrButtons.ContainsKey(btnnum) && DazStatics.hdrButtons[btnnum] == null) //might run on game reload?
                {
                    DazStatics.hdrButtons[btnnum] = btnImg;
                }
                else //also might hit this on game reload, but button still exists so no problem
                {
                }
            }
        }
        public static void SetupSortButton(GameObject parent, int posOffset) //create header buttons for page switching
        {
            if (parent.FindComponent<SortBtn>() == null)
            {
                GameObject btn = new GameObject("SortListBtn", typeof(RectTransform), typeof(KLayoutElement), typeof(SortBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                SortBtn btnImg = btn.GetComponent<SortBtn>(); //setup the button
                btnTfrm.sizeDelta = new Vector2(12, 12); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(posOffset, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                img.sprite = Assets.GetSprite("OverviewUI_priority_icon");
                //img.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                img.color = Color.white;
            }

        }
        public static void SetupUpArrowButton(GameObject parent, Tag tg, string source) //create Up arrow button on each resource item in list
        {
            if (parent.transform.Find("UpArrow") == null) //this will be FALSE most of the time, this button creation method is called every second, only need to create button if it does not exist
            {
                GameObject btn = new GameObject("UpArrow", typeof(RectTransform), typeof(KLayoutElement), typeof(UpBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                UpBtn btnImg = btn.GetComponent<UpBtn>(); //setup the button
                btnImg.Setup(tg, source); //pass TAG of resource this UpArrow is attached to
                btnTfrm.sizeDelta = new Vector2(14, 14); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(70, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                img.sprite = Assets.GetSprite("arrow_forward");
                img.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                img.color = Color.white;
                if (!DazStatics.ShowSortArrows)
                {
                    btn.SetActive(false);
                }
            }
        }
        public static void SetupDownArrowButton(GameObject parent, Tag tg, string source)//create Down arrow button on each resource item in list
        {
            if (parent.transform.Find("DownArrow") == null)//this will be FALSE most of the time, this button creation method is called every second, only need to create button if it does not exist
            {
                GameObject btn = new GameObject("DownArrow", typeof(RectTransform), typeof(KLayoutElement), typeof(DownBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                DownBtn btnImg = btn.GetComponent<DownBtn>(); //setup the button
                btnImg.Setup(tg,source); //pass TAG of this resource list item
                btnTfrm.sizeDelta = new Vector2(14, 14); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(90, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                img.sprite = Assets.GetSprite("arrow_forward");
                img.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                img.color = Color.white;
                if (!DazStatics.ShowSortArrows)
                {
                    btn.SetActive(false);
                }
            }
        }
    }
}
