using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using KMod;
using HarmonyLib;
using UnityEngine;
using KSerialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Klei;

namespace DazRscListSort
{
  //Primary object of this mod, is where date is saved
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DazRscListSortData : KMonoBehaviour
    {
        [Serialize]
        public int CurrentHdrButton;
        //hardcode multiple lists for serialization simplicity
        //is DLC multple lists somehow?
        [Serialize]
        public List<Tag> Button1List;
        [Serialize]
        public List<Tag> Button2List;
        [Serialize]
        public List<Tag> Button3List;
        [Serialize]
        public List<Tag> Button4List;
        [Serialize]
        public List<Tag> Button5List;
        public bool init = false;
        public static Dictionary<Tag, GameObject> DazRows = new Dictionary<Tag, GameObject>();
        public static bool DazShowSortArrows = false;


        public static void SyncRows(Dictionary<Tag, GameObject> KlieRows) //called on PinnedResourcePanel.OnSpawn, this is the list of objects displayed in Pinned Resource Panel, use this reference to manipulated it, careful of race conditions
        {
            DazRows = KlieRows; 
        }
        public void Initialise()
        {
            if (!init)
            {
                PinnedResourcesPanel inst = PinnedResourcesPanel.Instance;// PinnedResourcesPanel.Instance.PointerEnterActions += OnMouseEnter();
                //inst.pointerEnterActions += new KScreen.PointerEnterActions(RscOnMouseEnter);
                //inst.pointerExitActions += new KScreen.PointerExitActions(RscOnMouseExit);
                init = true;
            }
        }

        public void OnDestroy()
        {
            PinnedResourcesPanel inst = PinnedResourcesPanel.Instance;// PinnedResourcesPanel.Instance.PointerEnterActions += OnMouseEnter();
            //inst.pointerEnterActions -= RscOnMouseEnter;
            //inst.pointerExitActions -= RscOnMouseExit;
            base.OnDestroy();
        }
        //public void RscOnMouseEnter(PointerEventData data) => OnMouseOver(); //hook on mouseover of Pinned Resource Panel
        //public void RscOnMouseExit(PointerEventData data) => OnMouseExit(); //hook on mousexit of pinned resource panel //last patch added gap between headed and rows that triggers this


        public static void ShowSortArrows() //hide value list, show sort arrows
        {
            DazShowSortArrows = true;
            foreach(KeyValuePair<Tag,GameObject> kvp in DazRows)
            {
                Transform tfr = kvp.Value.transform.Find("UpArrow"); //find UpArrow game obect and show it
                if(tfr != null)
                {
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(true);
                }
                tfr = kvp.Value.transform.Find("DownArrow");//find DownArrow game obect and show it
                if (tfr != null)
                {
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(true);
                }
                tfr = kvp.Value.transform.Find("ValueLabel");//find ValueLabel game obect and hide it, not this is a Klei default UI object
                if (tfr != null)
                {
                    //GameObject go4 = tfr.gameObject;
                    tfr.gameObject.SetActive(false);
                }
            }
        }
        public static void HideSortArrows() //hide value list, show sort arrows
        {
            DazShowSortArrows = false;
            foreach (KeyValuePair<Tag, GameObject> kvp in DazRows)
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
        }
        //public static void OnMouseExit() //show value list, hide sort arrows
        //{
        //    if(!AllResourcesScreen.Instance.gameObject.activeSelf) //don't exit sort mode if All Resources screen is open
        //    { 
        //    DazShowSortArrows = false;
        //        foreach (KeyValuePair<Tag, GameObject> kvp in DazRows)
        //        {
        //            Transform tfr = kvp.Value.transform.Find("UpArrow");//find UpArrow game obect and hide it
        //            if (tfr != null)
        //            {
        //                GameObject go4 = tfr.gameObject;
        //                tfr.gameObject.SetActive(false);
        //            }
        //            tfr = kvp.Value.transform.Find("DownArrow");//find DownArrow game obect and hide it
        //            if (tfr != null)
        //            {
        //                GameObject go4 = tfr.gameObject;
        //                tfr.gameObject.SetActive(false);
        //            }
        //            tfr = kvp.Value.transform.Find("ValueLabel");//find ValueLabel game obect and show it
        //            if (tfr != null)
        //            {
        //                GameObject go4 = tfr.gameObject;
        //                tfr.gameObject.SetActive(true);
        //            }
        //        }
        //    }
        //}
        public void DumpLists() //troubleshooting, dump current data structure, never called in release .dll
        {

                Debug.Log("DazDump " + CurrentHdrButton);
                foreach (Tag tg in Button1List)
                {
                    Debug.Log("DazDump1 " + tg.ProperNameStripLink());
                }
                foreach (Tag tg in Button2List)
                {
                    Debug.Log("DazDump2 " + tg.ProperNameStripLink());
                }
                foreach (Tag tg in Button3List)
                {
                    Debug.Log("DazDump3 " + tg.ProperNameStripLink());
                }
                foreach (Tag tg in Button4List)
                {
                    Debug.Log("DazDump4 " + tg.ProperNameStripLink());
                }
                foreach (Tag tg in Button5List)
                {
                    Debug.Log("DazDump5 " + tg.ProperNameStripLink());
                }
            
        }
        public List<Tag> GetList(int num) //get List<Tag> associated with a button, specified by passed int.
        {
            if (num == 1)
            {
                if(Button1List==null)
                {
                    Button1List = new List<Tag>();
                }
                return Button1List;
            }
            else if (num == 2)
            {
                if (Button2List == null)
                {
                    Button2List = new List<Tag>();
                }
                return Button2List;
            }
            else if (num == 3)
            {
                if (Button3List == null)
                {
                    Button3List = new List<Tag>();
                }
                return Button3List;
            }
            else if (num == 4)
            {
                if (Button4List == null)
                {
                    Button4List = new List<Tag>();
                }
                return Button4List;
            }
            else if (num == 5)
            {
                if (Button5List == null)
                {
                    Button5List = new List<Tag>();
                }
                return Button5List;
            }
            else
            {
                Debug.Log("Daz Pinned Resource DataStore missing List<Tag> for " + num);
                return new List<Tag>();
            }
        }

        public List<Tag> GetCurrentList() //get List<Tag> of currently selected button
        {
            if (CurrentHdrButton == 1)
            {
                if (Button1List == null)
                {
                    Button1List = new List<Tag>();
                }
                return Button1List;
            }
            else if (CurrentHdrButton == 2)
            {
                if (Button2List == null)
                {
                    Button2List = new List<Tag>();
                }
                return Button2List;
            }
            else if (CurrentHdrButton == 3)
            {
                if (Button3List == null)
                {
                    Button3List = new List<Tag>();
                }
                return Button3List;
            }
            else if (CurrentHdrButton == 4)
            {
                if (Button4List == null)
                {
                    Button4List = new List<Tag>();
                }
                return Button4List;
            }
            else if (CurrentHdrButton == 5)
            {
                if (Button5List == null)
                {
                    Button5List = new List<Tag>();
                }
                return Button5List;
            }
            else
            {
                Debug.Log("Daz Pinned Resource DataStore missing Current List<Tag>");
                return new List<Tag>();
            }
        }
        public void ReOrdertag(Tag tg, bool Up)
        {
            DazStatics.hdrButtonChangeInProgress = true; //race condition exists, lock out PinnedResoucePanel.SortRows from running on true.
            List<Tag> workingList = GetCurrentList(); //get List<Tag> of current Header button
            int OldTagIndex = 0; //save Index location of Tag being moved
            for (int i = 0; i < workingList.Count; i++)
            {
                if (workingList[i] == tg)
                {
                    OldTagIndex = i;
                    break;
                }
            }
            if (DiscoveredResources.Instance.newDiscoveries.ContainsKey(tg)) //if resrouce was in new discoveries list and not pinned resources list, move it over
            {
                DiscoveredResources.Instance.newDiscoveries.Remove(tg);
                if (!ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tg))
                {
                    ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Add(tg);
                }

            }
            if (!(OldTagIndex == 0) && Up || !((OldTagIndex +1) == workingList.Count) &&!Up) //can't move top level item up, or bottom level item down, check here for that
            {
                workingList.Remove(tg);//row needs moving, remove it from the list
                if (Up) //reinsert tag at new location moving up
                {
                    workingList.Insert(OldTagIndex - 1, tg);
                }
                else //reinsert tag at new location, moving down
                {
                    workingList.Insert(OldTagIndex + 1, tg);
                }
            }
            DazStatics.hdrButtonChangeInProgress = false; //reenabled PinnedResourcePanel.SortRows
            PinnedResourcesPanel.Instance.Refresh(); //Refresh PinnedResourcePanel for new tag sort
        }
    }
    public class HdrBtn : Button
    {
        //Button class for the 5 header buttons to pick current list
        public int BtnNum; //which button are we? 1 to 5 left to right on screen
        public GameObject go; //our parent GameObject
        DazRscListSort.DazRscListSortData dataStore; //master dataStorage object

        public void Setup(int num, GameObject attachedGO) //called on button creation by mod, not Unity built-it
        {
            dataStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>(); //get our datastorage object
            BtnNum = num; //save our button number
            go = attachedGO; //save our gameObject
        }

        public override void OnPointerClick(PointerEventData eventData) //on click event, save data and change page
        {
            DazStatics.hdrButtonChangeInProgress = true; //disable PinnedResourcePanel.SortRows, will corrupt data if it runs during this method.
            if (BtnNum != dataStore.CurrentHdrButton)
            {
                DazStatics.UpdateRscList(BtnNum); //must be called before updated RscListCurrentID, updates our saved list for the old page before changing
                dataStore.CurrentHdrButton = BtnNum; //change the current page
                DazStatics.RscListRefreshHeader(); //updated header icons for new current list
            }
            DazStatics.hdrButtonChangeInProgress = false; //reenabled PinnedResourcePanel.SortRows
        }
        
    }
    public class UpBtn : Button
    {
        //UpArrow button on resource item in list
        public Tag btnTag; //Tag of our resource type

        public void Setup(Tag tg)
        {
            btnTag = tg; //save Tag of resource type, manually called by mod, not Unity
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().ReOrdertag(btnTag, true); //call method to move this data row up on button click
        }
    }
    public class DownBtn : Button
    {
        //DownArrow button on resource item in list
        public Tag btnTag; //Tag of our resource type

        public void Setup(Tag tg)
        {
            btnTag = tg;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().ReOrdertag(btnTag, false); //move this resource down a row
        }
    }

    public class SortBtn : Button
    {
        //Enable sort button
        public DazRscListSortData dStore;
        public void Setup()
        {
            dStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if(!DazRscListSortData.DazShowSortArrows)
            {
                DazRscListSortData.ShowSortArrows();
                DazRscListSortData.DazShowSortArrows = true;
            }
            else
            {
                DazRscListSortData.HideSortArrows();
                DazRscListSortData.DazShowSortArrows = false;
            }
        }
    }
    public static class DazStatics
    {
        //static methods for ease of programming, these could probably be moved into DazRscSortList data storage object as non-static without issues?
        public static Dictionary<int, HdrBtn> hdrButtons = new Dictionary<int,HdrBtn>(); //Save our header button objects for easy reference
        public static bool hdrButtonChangeInProgress = false;//apparently ONI is multithreaded and the PinnedResourcesWindow can try to update in teh middle of a button change, if TRUE, PinnedResourcePanel.SortRows is locked out and will by bypassed to avoid conflicts

        public static void UpdateRscList(int newList) //RscListCurrentID still holds old ID
        {
            //do not save current list, should already be saved when list was last updated
            //clear list, this is direct copy of PinnedResourcePanel.UnPinAll(), but that method is private so can't call it
            //rows list is private, can't access, so use our saved list as it should be the same, RscListCurrentID is still the old list, use it to remove
            //note that we need to remove Tags not in pinned list, but in New Discoveries list from this button's List<Tag> before saving
            WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory; //get the current game data
            DazRscListSort.DazRscListSortData dataStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>(); //get this mods datastorage object
            List<Tag> newItems = new List<Tag>(); //don't want to save items tagged New, but can't remove while inside foreach
            foreach (Tag tg in dataStore.GetList(dataStore.CurrentHdrButton))
            {
                if (!worldInventory.pinnedResources.Contains(tg) && DiscoveredResources.Instance.newDiscoveries.ContainsKey(tg))
                {
                    newItems.Add(tg);
                }
                worldInventory.pinnedResources.Remove(tg);//remove items tagged new from saved button list, does NOT remove them for Klei's new discoveries list so you can change buttons to add a new resource to a different page
            }
            foreach(Tag tg in newItems)
            {
                dataStore.GetList(dataStore.CurrentHdrButton).Remove(tg); //remove items tagged new from saved list
            }
            foreach (Tag tg in dataStore.GetList(newList)) //add saved tags from our new pages saved List<Tag>.
            {
                worldInventory.pinnedResources.Add(tg);
            }
            PinnedResourcesPanel.Instance.Refresh(); //refresh the PinnedResourcesPanel for the new list of items.
        }

        public static void RscListRefreshHeader() //refresh header button colors after page change
        {
            foreach(int i in DazStatics.hdrButtons.Keys)
            {
                if(DazStatics.hdrButtons[i].BtnNum == ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton)
                {
                    Image btnImg = DazStatics.hdrButtons[i].GetComponent<Image>();//set color green
                    btnImg.color = Color.green;
                }
                else
                {
                    Image btnImg = DazStatics.hdrButtons[i].GetComponent<Image>(); //set color gray
                    btnImg.color = Color.gray;
                }
            }
        }
        public static List<Tag> SortRscList(List<Tag> OldList, Dictionary<Tag, GameObject> rows) //primary work done here, compare list and sort, call from PinnedResourceList with Harmony
        {
            //Note this method REPLACES PinnedResourceWindow.SortRows in it's entirety, make sure it still does all the default Klei functions.
            List<Tag> ReturnList = new List<Tag>(); //have to add the inactive items back into list at end, so instatiate an object to modify and return
            if (DazStatics.hdrButtons.ContainsKey(1) && (DazStatics.hdrButtonChangeInProgress==false)) //race condition check, this method can run before buttons instantiate so skip if buttons don't exist yet
                //also skip if button change in progress so that doesn't screw data up
            {
                List<Tag> OldListActive = new List<Tag>();
                foreach (KeyValuePair<Tag, GameObject> row in rows) //we only need to sort objects actively being displayed, there seems to be no destruction routine for objects that were shown and hidden and they stay around until game load.
                {
                    if (row.Value.activeSelf) //is resource displayed?
                    {
                        OldListActive.Add(row.Key); //make list of active only objects
                    }
                }
                DazRscListSort.DazRscListSortData dataStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>(); //get this mod's datastore
                List<Tag> toRemove = new List<Tag>();
                foreach (Tag tag in dataStore.GetList(dataStore.CurrentHdrButton)) //check nothing has been removed from list
                {
                    if (!OldListActive.Contains(tag)) //removed item so...
                    {
                        //DazStatics.RscListCollection[DazStatics.RscListCurrentID].Remove(tag); //remove from saved list, this line is crash error, can't edit list being enumerated so use toRemove
                        toRemove.Add(tag); //save tags to remove
                    }
                }
                foreach (Tag tg in toRemove) //remove tags
                {
                    dataStore.GetList(dataStore.CurrentHdrButton).Remove(tg);
                }

                foreach (Tag tag in OldListActive) //check list to see if anythign new added to Klei's list.
                {
                    if (!dataStore.GetList(dataStore.CurrentHdrButton).Contains(tag)) //missing tag from saved list in mod
                    {
                        dataStore.GetList(dataStore.CurrentHdrButton).Add(tag); //add tag to end of list
                    }
                }
                foreach (Tag tg in dataStore.GetList(dataStore.CurrentHdrButton)) //active items at start of returned list
                {
                    ReturnList.Add(tg); //add items in sorted order to ReturnList
                    SetupUpArrowButton(rows[tg], tg); //check for, and add up arrow if needed
                    SetupDownArrowButton(rows[tg], tg); //check for, and add down arrow if needed
                }
                foreach (Tag tg in OldList) //add inactive items for compatibility so list passed to mod and list mod passes back for display contain the same items, if in a different order
                {
                    if (!ReturnList.Contains(tg))
                    {
                        ReturnList.Add(tg);
                    }
                }
            }

            else //method had to be bypassed, most likely due to header button change in progress (see if statement)
            {
                ReturnList = OldList;
            }
            return ReturnList;
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
                btnImg.Setup(); //call Setup in button class, pass this button's number and the HdrBtn object
                btnTfrm.sizeDelta = new Vector2(12, 12); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(posOffset, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                img.sprite = Assets.GetSprite("OverviewUI_priority_icon");
                //img.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                img.color = Color.white;
            }
            
        }
        public static void SetupUpArrowButton(GameObject parent, Tag tg) //create Up arrow button on each resource item in list
        {
            if (parent.transform.Find("UpArrow")== null) //this will be FALSE most of the time, this button creation method is called every second, only need to create button if it does not exist
            {
                GameObject btn = new GameObject("UpArrow", typeof(RectTransform), typeof(KLayoutElement), typeof(UpBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                UpBtn btnImg = btn.GetComponent<UpBtn>(); //setup the button
                btnImg.Setup(tg); //pass TAG of resource this UpArrow is attached to
                btnTfrm.sizeDelta = new Vector2(14, 14); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(70, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                img.sprite = Assets.GetSprite("arrow_forward");
                img.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                img.color = Color.white;
                if(!DazRscListSortData.DazShowSortArrows)
                {
                    btn.SetActive(false);
                }
            }
        }
        public static void SetupDownArrowButton(GameObject parent, Tag tg)//create Down arrow button on each resource item in list
        {
            if (parent.transform.Find("DownArrow") == null)//this will be FALSE most of the time, this button creation method is called every second, only need to create button if it does not exist
            {
                GameObject btn = new GameObject("DownArrow", typeof(RectTransform), typeof(KLayoutElement), typeof(DownBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                DownBtn btnImg = btn.GetComponent<DownBtn>(); //setup the button
                btnImg.Setup(tg); //pass TAG of this resource list item
                btnTfrm.sizeDelta = new Vector2(14, 14); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(90, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                img.sprite = Assets.GetSprite("arrow_forward");
                img.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                img.color = Color.white;
                if (!DazRscListSortData.DazShowSortArrows)
                {
                    btn.SetActive(false);
                }
            }
        }
    }

    public class DazPatches : UserMod2 //mod loading class
    {
        [HarmonyPatch(typeof(WorldInventory), "OnPrefabInit")]
        public static class WorldInventory_OnPrefabInit_Patch
        {
           
            //add our primary data storage object, needs to happen here so it exists in game world at load time, UI objects are created after game load.
            internal static void Postfix(WorldInventory __instance)
            {
                DazRscListSort.DazRscListSortData dStore = __instance.gameObject.AddOrGet<DazRscListSort.DazRscListSortData>(); //looks like WorldInventory is destroyed when game ends and return to Main Menu, so starting a new game instantiates a new instance of DazRscListSortData without having to code resetting to defaults on return to Main Menu
            }
        }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony); //tell ONI my mod exists
        }

        [HarmonyPatch(typeof(PinnedResourcesPanel))]
        [HarmonyPatch("OnSpawn")]
        public class PinnedRscPanelPatchOnSpawn
        {
            
            //hook PinnedResourcePanel for the UI objects being added.
            //use PostFix as no changes to panel, just adding stuff
            public static void Postfix(Dictionary<Tag, GameObject> ___rows)
            {
                Debug.Log("Daz pinned rez onspawn");
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 1, 55); //setup the 5 Page buttons on the header
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 2, 71);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 3, 87);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 4, 103);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 5, 119);
                DazStatics.SetupSortButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 35);
                if (ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton==0) //this will only be hit the first time the mod is loaded into a game, so set to first page, otherwise last display page will populate this field on game load, use that if present.
                {
                    ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton = 1;
                }
                ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().Initialise(); //call primary data object for setup
                DazStatics.RscListRefreshHeader(); //now have loaded, refresh button colors
                DazRscListSortData.SyncRows(___rows); //link rows object to primary data store
            }
        }
        [HarmonyPatch(typeof(PinnedResourcesPanel))]
        [HarmonyPatch("SortRows")]
        public class PinnedRscPanelPatchSortRows
        {
            //primary worker method of the mod, this is called every 1000ms by Klei, note total replacement required until I can figure out a Transpiler
            public static bool Prefix(PinnedResourcesPanel __instance, Dictionary<Tag, GameObject> ___rows)
            {
                //in theory a transpiler replace the one line needed should be used, i lack the skills to do so, copy-paste entire method as a workaround
                //start Klei code
                List<Tag> tagList = new List<Tag>(); //all items in ___rows
                foreach (KeyValuePair<Tag, GameObject> row in ___rows)
                {
                    tagList.Add(row.Key);
                }
                //end Klei code
                //below line is all i need to replace
                //tagList.Sort((Comparison<Tag>)((a, b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink())));
                //replace with my code, want to make this a transpiler?
                List<Tag> RetList = DazStatics.SortRscList(tagList, ___rows); //need to modify order of tagList obejct without changing object name so below code doesn't need modification.
                //RetList; //can't pass a list by ref or out? workaround with this
                //resume Klei code, using RetList instead of taglist
                foreach (Tag key in RetList)
                {
                    ___rows[key].transform.SetAsLastSibling();
                }
                PinnedResourcesPanel.Instance.clearNewButton.transform.SetAsLastSibling();
                PinnedResourcesPanel.Instance.seeAllButton.transform.SetAsLastSibling();
                //end of method, bypass original method with next line
                return false;
            }
        }
    }
}
