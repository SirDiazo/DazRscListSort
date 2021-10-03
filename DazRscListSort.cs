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
  
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DazRscListSortData : KMonoBehaviour
    {
        [Serialize]
        public int CurrentHdrButton;
        //hardcode multiple lists for serialization simplicity
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

        public static void SyncRows(Dictionary<Tag, GameObject> KlieRows)
        {
            DazRows = KlieRows;
        }
        public void Initialise()
        {
            //Debug.Log("Daz dStore init");
            if (!init)
            {
                PinnedResourcesPanel inst = PinnedResourcesPanel.Instance;// PinnedResourcesPanel.Instance.PointerEnterActions += OnMouseEnter();
                inst.pointerEnterActions += new KScreen.PointerEnterActions(RscOnMouseEnter);
                inst.pointerExitActions += new KScreen.PointerExitActions(RscOnMouseExit);
                init = true;
            }
        }
        public void RscOnMouseEnter(PointerEventData data) => OnMouseOver();
        public void RscOnMouseExit(PointerEventData data) => OnMouseExit();

        public static void OnMouseOver()
        {
            //Debug.Log("Daz mouse over execute");
            //Debug.Log("Daz rows check"+DazRows.Count);
            foreach(KeyValuePair<Tag,GameObject> kvp in DazRows)
            {
                //Debug.Log("Daz foreach start");
                Transform tfr = kvp.Value.transform.Find("UpArrow");
                if(tfr != null)
                {
                    //Debug.Log("Daz foreach not null");
                    GameObject go4 = tfr.gameObject;
                    //Debug.Log("Daz foreach not null1");
                    tfr.gameObject.SetActive(true);
                   // Debug.Log("Daz foreach not null2");

                }
                else
                {
                   // Debug.Log("Daz foreach was null");
                }
                tfr = kvp.Value.transform.Find("DownArrow");
                if (tfr != null)
                {
                  //  Debug.Log("Daz foreach not null");
                    GameObject go4 = tfr.gameObject;
                    //Debug.Log("Daz foreach not null1");
                    tfr.gameObject.SetActive(true);
                  //  Debug.Log("Daz foreach not null2");

                }
                else
                {
                   // Debug.Log("Daz foreach was null");
                }
                tfr = kvp.Value.transform.Find("ValueLabel");
                if (tfr != null)
                {
                   // Debug.Log("Daz foreach not null");
                    GameObject go4 = tfr.gameObject;
                   // Debug.Log("Daz foreach not null1");
                    tfr.gameObject.SetActive(false);
                   // Debug.Log("Daz foreach not null2");

                }
                else
                {
                    //Debug.Log("Daz foreach was null");
                }
                //ValueLabel
                ////foreach(Transform trf in kvp.Value.transform.getch)
                //int i2 = 0;
                //for (int i = 0; i < kvp.Value.transform.childCount; i++)
                //{
                //    Debug.Log("Daz foreach2 start");
                //    Debug.Log("Daz fe3" + kvp.Value.transform.GetChild(i).gameObject.name);
                //    if (kvp.Value.transform.GetChild(i).gameObject.name == "UpArrow")
                //    {
                //        //i2 = i;
                //        break;
                //    }
                //}
                ////Debug.Log("Daz foreach end");
                //Debug.Log("Daz find check " + (kvp.Value.transform.Find("UpArrow")==null));
                //Transform tfrm = kvp.Value.transform.Find("UpArrow");
                //Debug.Log("Daz step1a");
                //RectTransform rtfrm = (RectTransform)tfrm;
                //Debug.Log("Daz step1aa");
                //GameObject go3 = kvp.Value.transform.GetChild(i2).gameObject;
                //Debug.Log("Daz step1b." + i2);
                //UpBtn ub = go3.GetComponent<UpBtn>();
                //Debug.Log("Daz step1c");
                //Debug.Log("Daz step1cc" + go3.name);
                //Debug.Log("daz check " + kvp.Value.)
            }
        }
        public static void OnMouseExit()
        {
            //Debug.Log("Daz mouse leave execute");
            //Debug.Log("Daz rows check" + DazRows.Count);
            foreach (KeyValuePair<Tag, GameObject> kvp in DazRows)
            {
                //Debug.Log("Daz foreach start");
                Transform tfr = kvp.Value.transform.Find("UpArrow");
                if (tfr != null)
                {
                    //Debug.Log("Daz foreach not null");
                    GameObject go4 = tfr.gameObject;
                   // Debug.Log("Daz foreach not null1");
                    tfr.gameObject.SetActive(false);
                   // Debug.Log("Daz foreach not null2");

                }
                else
                {
                  //  Debug.Log("Daz foreach was null");
                }
                tfr = kvp.Value.transform.Find("DownArrow");
                if (tfr != null)
                {
                   // Debug.Log("Daz foreach not null");
                    GameObject go4 = tfr.gameObject;
                   // Debug.Log("Daz foreach not null1");
                    tfr.gameObject.SetActive(false);
                   // Debug.Log("Daz foreach not null2");

                }
                else
                {
                  //  Debug.Log("Daz foreach was null");
                }
                tfr = kvp.Value.transform.Find("ValueLabel");
                if (tfr != null)
                {
                  //  Debug.Log("Daz foreach not null");
                    GameObject go4 = tfr.gameObject;
                  //  Debug.Log("Daz foreach not null1");
                    tfr.gameObject.SetActive(true);
                   // Debug.Log("Daz foreach not null2");

                }
                else
                {
                   // Debug.Log("Daz foreach was null");
                }
                //Debug.Log("Daz mouse leave execute");
                //foreach (UpBtn uBtn in PinnedResourcesPanel.Instance.headerButton.gameObject.transform.GetComponentsInChildren<UpBtn>())
                //{
                //    Debug.Log("Daz mouse leave execute2");
                //    uBtn.gameObject.SetActive(false);
                //}
            }
        }
        public void DumpLists()
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
        public List<Tag> GetList(int num)
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
                Debug.Log("Daz Pinned Resource DataStore missing List<Tag>");
                return new List<Tag>();
            }
        }

        public List<Tag> GetCurrentList()
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
            Debug.Log("Daz reorder reqesut, UP? " + Up + " tag " + tg);
            DazStatics.hdrButtonChangeInProgress = true;
            List<Tag> workingList = GetCurrentList();
            //int OldTagIndex = workingList.FindIndex(tg);//findindex needs predicate?
            int OldTagIndex = 0;
            for (int i = 0; i < workingList.Count; i++)
            {
                Debug.Log("Daz for loop count " + i + " and tg " + tg.ProperNameStripLink());
                if (workingList[i] == tg)
                {
                    Debug.Log("Daz for loop tag found at" + i );
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
            if (!(OldTagIndex == 0) && Up || !((OldTagIndex +1) == workingList.Count) &&!Up)
            {
                Debug.Log("Daz tag list before remove" + workingList.Count);
                workingList.Remove(tg);
                
                //Debug.Log("Daz tag list after remove" + workingList.Count);
                if (Up) //reinsert tag at new location
                {
                    //Debug.Log("Daz tag list move up" + workingList.Count + " " + (OldTagIndex - 1));
                    workingList.Insert(OldTagIndex - 1, tg);
                }
                else
                {
                    //Debug.Log("Daz tag list move down" + workingList.Count + " " + (OldTagIndex + 1));
                    workingList.Insert(OldTagIndex + 1, tg);
                }
            }
            else
            {
                Debug.Log("Daz can't move first item up or last itme down");
            }
                
            
            DazStatics.hdrButtonChangeInProgress = false;
            PinnedResourcesPanel.Instance.Refresh();
        }
    }
    public class HdrBtn : Button
    {
        public int BtnNum;
        public GameObject go;
        DazRscListSort.DazRscListSortData dataStore;


        public void Setup(int num, GameObject attachedGO)
        {

            dataStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>();

            BtnNum = num;
            go = attachedGO;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
           // Debug.Log("Daz click test 1");
            DazStatics.hdrButtonChangeInProgress = true;
          //  Debug.Log("Daz click start" + BtnNum);
            //DazStatics.DumpSavedLists();
            if (BtnNum != dataStore.CurrentHdrButton)
            {
                DazStatics.UpdateRscList(BtnNum); //must be called before updated RscListCurrentID
                dataStore.CurrentHdrButton = BtnNum;
                DazStatics.RscListRefreshHeader(); //updated header icons for new current list
            }
          //  Debug.Log("Daz click mid " + BtnNum);
            //DazStatics.DumpSavedLists();
          //  Debug.Log("Daz click end " + BtnNum);
            DazStatics.hdrButtonChangeInProgress = false;
        }
        
    }

    //public class UpBtn : KButton
    //{
    //    public Tag btnTag;
    //    public GameObject go;

    //    public void Setup(Tag tg, GameObject gobj)
    //    {
    //        Debug.Log("Daz upbtn setup 1");
    //        btnTag = tg;
    //        go = gobj;
    //        //PinnedResourcesPanel.Instance.pointerEnterActions += onMouseEnter();
    //        //PinnedResourcesPanel.Instance.OnPointerEnter() += entAct();
    //        //PinnedResourcesPanel instance = PinnedResourcesPanel.Instance;
    //        Debug.Log("Daz upbtn setup 2");
    //        //onPointerEnter += OnMouseOver();
    //        Debug.Log("Daz upbtn setup 3");
    //        //onPointerExit += OnMouseLeave();
    //        Debug.Log("Daz upbtn setup 4");
    //        //onClick += OnPointerClick();
    //        //this.closeButton.onClick += (System.Action) (() => this.Show(false));
    //        foreach(Component co in go.GetComponents<Component>())
    //        {
    //            Debug.Log("Daz co check " + co.GetType());
    //        }
    //        onClick += (System.Action)(() => OnPointerClick());
    //        Debug.Log("Daz upbtn setup 5");
    //        //instance.pointerEnterActions += new KScreen.PointerEnterActions(this.OnMouseLeave);
    //        Debug.Log("Daz stat 1 " + KInputManager.isFocused);
    //        Debug.Log("Daz stat 2 " + (this.GetComponent<ImageToggleState>() == null));
    //        //Debug.Log("Daz stat 3 " + this.intera);
    //        //Debug.Log("Daz stat 4 " + KInputManager.isFocused);
    //        //Debug.Log("Daz stat 5 " + KInputManager.isFocused);
    //        //Debug.Log("Daz stat 6 " + KInputManager.isFocused);

    //    }

    //    //

    //    public System.Action OnMouseOver()
    //    {
    //        Debug.Log("Daz mouse entered!");
    //        go.SetActive(true);
    //        //go.GetComponent<Image>().enabled);
    //        //RectTransform btnTfrm = go.AddComponent<RectTransform>();
    //        //btnTfrm.SetParent(go.transform, false); //set parent, false to use localspace
    //        //btnTfrm.sizeDelta = new Vector2(14, 14); //resize
    //        //btnTfrm.SetAsLastSibling(); //not 100% this is required
    //        //btnTfrm.Translate(new Vector3(70, -200, 0)); //move button into place
    //        //Destroy(go.GetComponent<Image>());
    //        //Image img = go.AddComponent<Image>();
    //        //img.sprite = Assets.GetSprite("arrow_forward");
    //        //img.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
    //        //img.color = Color.white;
    //        //go.GetComponent<UpBtn>().image.sprite = Assets.GetSprite("arrow_forwards");
    //        Debug.Log("daz pos2");// + go.GetComponent<UpBtn>().isActiveAndEnabled + " " + go.GetComponent<UpBtn>().colors + go.GetComponent<UpBtn>().image + go.GetComponent<UpBtn>().IsInteractable() + go.GetComponent<UpBtn>().IsActive() + go.GetComponent<UpBtn>().spriteState);
    //        return null;

    //    }
    //    public System.Action OnMouseLeave()
    //    {
    //        Debug.Log("Daz mouse leave!");
    //        go.SetActive(false);
    //        Debug.Log("daz pos1");
    //        return null;

    //    }

    //    public void OnPointerClick()
    //    {
    //        Debug.Log("Daz up arrow row click ");// + btnTag);
    //        //ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().ReOrdertag(btnTag, true);
    //        //return null;
    //    }
    //}
    public class UpBtn : Button
    {
        public Tag btnTag;

        public void Setup(Tag tg)
        {
            btnTag = tg;
          //  Debug.Log("dazup btn setup");
            //PinnedResourcesPanel instance = PinnedResourcesPanel.Instance;
            //instance.on
            //PinnedResourcesPanel inst = PinnedResourcesPanel.Instance;// PinnedResourcesPanel.Instance.PointerEnterActions += OnMouseEnter();
            //inst.pointerEnterActions += new KScreen.PointerEnterActions(OnMouseEnter);
            //this.pointerEnterActions = this.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
        }

        //public void OnMouseEnter(PointerEventData data) => Debug.Log("Daz why did this work"); 
        //{
        //    Debug.Log("daz Mouse enter");
        //    return null;
        //}
        public override void OnPointerClick(PointerEventData eventData)
        {
         //   Debug.Log("Daz down up row click " + btnTag);
            ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().ReOrdertag(btnTag, true);
        }
    }
    public class DownBtn : Button
    {
        public Tag btnTag;

        public void Setup(Tag tg)
        {
            btnTag = tg;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
           // Debug.Log("Daz down arrow row click " + btnTag);
            ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().ReOrdertag(btnTag, false);
        }
    }
    public static class DazStatics
    {
        public static Dictionary<int, HdrBtn> hdrButtons = new Dictionary<int,HdrBtn>();
        public static bool hdrButtonChangeInProgress = false;//apparently ONI is multithreaded and the PinnedResourcesWindow can try to update in teh middle of a button change

        //public static void DumpSavedLists() //troubleshooting only
        //{
        //    foreach(KeyValuePair<int,HdrBtn> kVp in DazStatics.hdrButtons)
        //    {
        //        Debug.Log("Daz dump button num " + kVp.Key);
        //        foreach(Tag tg in kVp.Value.sortedTags)
        //        {
        //            Debug.Log(" daz dump tag " + tg.ProperNameStripLink());
        //        }
        //    }
        //    foreach(Tag tg in ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory.pinnedResources)
        //    {
        //        Debug.Log(" daz dump world tag " + tg.ProperNameStripLink());
        //    }
        //}
        public static void UpdateRscList(int newList) //RscListCurrentID still holds old ID
        {
            //do not save current list, should already be saved when list was last updated
            //clear list, this is direct copy of PinnedResourcePanel.UnPinAll(), but that method is private so can't call it
            //rows list is private, can't access, so use our saved list as it should be the same, RscListCurrentID is still the old list, use it to remove
            WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
            DazRscListSort.DazRscListSortData dataStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>();
            List<Tag> newItems = new List<Tag>(); //don't want to save items tagged New, but can't remove while inside foreach
            foreach (Tag tg in dataStore.GetList(dataStore.CurrentHdrButton))
            {
                if (!worldInventory.pinnedResources.Contains(tg) && DiscoveredResources.Instance.newDiscoveries.ContainsKey(tg))
                {
                    newItems.Add(tg);
                }
                worldInventory.pinnedResources.Remove(tg);
                
                //Debug.Log("Daz remove " + DazStatics.hdrButtons[1].CurrentBtnNum + tg.ProperName());
            }
            foreach(Tag tg in newItems)
            {
                dataStore.GetList(dataStore.CurrentHdrButton).Remove(tg); //remove items tagged new from saved list
            }

            //add items from new list
            foreach (Tag tg in dataStore.GetList(newList))
            {
                worldInventory.pinnedResources.Add(tg);
                //Debug.Log("Daz add " + newList + tg.ProperName());
            }
            //RscListCurrentID = newList; update current button back in OnClick now
            PinnedResourcesPanel.Instance.Refresh();
        }

        public static void RscListRefreshHeader() //refresh header button colors
        {
            foreach(int i in DazStatics.hdrButtons.Keys)
            {
                if(DazStatics.hdrButtons[i].BtnNum == ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton)
                {
                    //set color green
                    Image btnImg = DazStatics.hdrButtons[i].GetComponent<Image>();
                    btnImg.color = Color.green;

                    
                }
                else
                {
                    //set color gray
                    Image btnImg = DazStatics.hdrButtons[i].GetComponent<Image>();
                    btnImg.color = Color.gray;
                }
            }
        }
        public static List<Tag> SortRscList(List<Tag> OldList, Dictionary<Tag, GameObject> rows) //primary work done here, compare list and sort, call from PinnedResourceList with Harmony
        {
            //Debug.Log("Daz sort list 1");
            List<Tag> ReturnList = new List<Tag>(); //have to add the inactive items back into list at end, so instatiate an object to modify and return
            if (DazStatics.hdrButtons.ContainsKey(1) && (DazStatics.hdrButtonChangeInProgress==false)) //race condition check, this method can run before buttons instantiate so skip if buttons don't exist yet
                //also skip if button change in progress so that doesn't screw data up
            {
                //Debug.Log("Daz sort list 2");
                List<Tag> OldListActive = new List<Tag>();
                foreach (KeyValuePair<Tag, GameObject> row in rows)
                {
                    if (row.Value.activeSelf)
                    {
                        OldListActive.Add(row.Key); //make list of active only objects
                    }
                }
                DazRscListSort.DazRscListSortData dataStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>();
                List<Tag> toRemove = new List<Tag>();
                foreach (Tag tag in dataStore.GetList(dataStore.CurrentHdrButton)) //check nothing has been removed from list
                {
                    if (!OldListActive.Contains(tag)) //removed item so...
                    {
                        //DazStatics.RscListCollection[DazStatics.RscListCurrentID].Remove(tag); //remove from saved list, this line is crash error, can't edit list being enumerated
                        toRemove.Add(tag); //save tags to remove
                    }
                }
                foreach (Tag tg in toRemove) //remove tags
                {
                    dataStore.GetList(dataStore.CurrentHdrButton).Remove(tg);
                    //Debug.Log("Daz remove item my list" + tg.ProperName());
                }

                foreach (Tag tag in OldListActive) //check list to see if anythign new added
                {
                    if (!dataStore.GetList(dataStore.CurrentHdrButton).Contains(tag)) //missing tag from saved list
                    {
                        dataStore.GetList(dataStore.CurrentHdrButton).Add(tag); //add tag to end of list
                       // Debug.Log("Daz add item my list" + tag.ProperName());
                    }
                }
                foreach (Tag tg in dataStore.GetList(dataStore.CurrentHdrButton)) //active items at start of returned list
                {
                    ReturnList.Add(tg);
                    SetupUpArrowButton(rows[tg], tg);
                    SetupDownArrowButton(rows[tg], tg);
                }
                foreach (Tag tg in OldList) //add inactive items
                {
                    if (!ReturnList.Contains(tg))
                    {
                        ReturnList.Add(tg);
                    }
                }
                //Debug.Log("Daz sort list 3");
            }

            else
            {
                //Debug.Log("Daz sort fail, return same list");
                ReturnList = OldList;
            }
            //Debug.Log("Daz sort list end");
            return ReturnList;
        }

       
        public static void SetupButton(GameObject parent, int btnnum, int posOffset)
        {
            if (!DazStatics.hdrButtons.ContainsKey(btnnum) || DazStatics.hdrButtons.ContainsKey(btnnum) && DazStatics.hdrButtons[btnnum] == null)
            {
                
                GameObject btn = new GameObject("RscList" + btnnum, typeof(RectTransform), typeof(KLayoutElement), typeof(HdrBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                HdrBtn btnImg = btn.GetComponent<HdrBtn>(); //setup the button
                btnImg.Setup(btnnum, btn);
                btnTfrm.sizeDelta = new Vector2(12, 12); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(posOffset, 0, 0)); //move button into place
                if (!DazStatics.hdrButtons.ContainsKey(btnnum))
                {
                    DazStatics.hdrButtons.Add(btnnum, btnImg);
                    Debug.Log("Daz full button instantiate " + btnnum);
                }
                else if (DazStatics.hdrButtons.ContainsKey(btnnum) && DazStatics.hdrButtons[btnnum] == null)
                {
                    DazStatics.hdrButtons[btnnum] = btnImg;
                    Debug.Log("Daz partial button instantiate " + btnnum);
                }
                else
                {
                    Debug.Log("Daz button instantiate fallthrough " + btnnum);
                }
            }
            else
            {
//              Debug.Log("Daz button already exists " + btnnum);
            }

        }
        public static void SetupUpArrowButton(GameObject parent, Tag tg)
        {
            //Debug.Log("Daz uparrow setup 1");
            if (parent.transform.Find("UpArrow")== null)
            {
                //Debug.Log("Daz uparrow setup 1a");
                GameObject btn = new GameObject("UpArrow", typeof(RectTransform), typeof(KLayoutElement), typeof(UpBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
               // Debug.Log("daz check ua " + btnTfrm.gameObject.name);
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                UpBtn btnImg = btn.GetComponent<UpBtn>(); //setup the button
                btnImg.Setup(tg);
                btnTfrm.sizeDelta = new Vector2(14, 14); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(70, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                //img.sprite = Assets.GetSprite("icon_priority_down");
                img.sprite = Assets.GetSprite("arrow_forward");
                img.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                img.color = Color.white;

               // Debug.Log("daz check u2a " + btnTfrm.gameObject.name);
               // Debug.Log("Daz uparrow setup 2");

            }
            else
            {
               //Debug.Log("Daz upArrow button already exists ");
            }

        }
        public static void SetupDownArrowButton(GameObject parent, Tag tg)
        {
            if (parent.transform.Find("DownArrow") == null)
            {
               // Debug.Log("Daz downarrow setup 1");
                GameObject btn = new GameObject("DownArrow", typeof(RectTransform), typeof(KLayoutElement), typeof(DownBtn), typeof(Image)); //make base gameobject
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                DownBtn btnImg = btn.GetComponent<DownBtn>(); //setup the button
                btnImg.Setup(tg);
                btnTfrm.sizeDelta = new Vector2(14, 14); //resize
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                btnTfrm.Translate(new Vector3(90, 0, 0)); //move button into place
                Image img = btn.GetComponent<Image>();
                //img.sprite = Assets.GetSprite("icon_priority_down");
                img.sprite = Assets.GetSprite("arrow_forward");
                img.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                img.color = Color.white;
               // Debug.Log("Daz downarrow setup 2");

            }
            else
            {

               // Debug.Log("Daz downArrow button already exists ");
            }

        }
    }

    public class DazPatches : UserMod2
    {
        //[SerializationConfig(MemberSerialization.OptIn)]
        //public class DazDataTest2 : KMonoBehaviour
        //{
        //    [Serialize]
        //    public int testNum;
        //}

        [HarmonyPatch(typeof(WorldInventory), "OnPrefabInit")]
        public static class WorldInventory_OnPrefabInit_Patch
        {
            /// <summary>
            /// Applied after OnPrefabInit runs.
            /// </summary>
            internal static void Postfix(WorldInventory __instance)
            {
                //Debug.Log("daz test added");
                DazRscListSort.DazRscListSortData dStore = __instance.gameObject.AddOrGet<DazRscListSort.DazRscListSortData>();
                //dStore.Initialise();
          
            }
        }
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            //Debug.Log("Daz Yay, working!");
            //SaveGame.Instance.FindOrAddComponent<DazRscListSortData>();
            //ebug.Log("Daz Yay, component added!");

        }

        //[HarmonyPatch(typeof(MainMenu))]
        //[HarmonyPatch("OnPrefabInit")]
        //public class MainMenuInit
        //{
        //    public static void Postfix()
        //    {
        //        Debug.Log("Daz main menu init");
        //        Debug.Log("daz check " + PinnedResourcesPanel.Instance.name);
        //    }
        //}

        [HarmonyPatch(typeof(PinnedResourcesPanel))]
        [HarmonyPatch("OnSpawn")]
        public class PinnedRscPanelPatchOnSpawn
        {
            public static void Postfix(Dictionary<Tag, GameObject> ___rows)
            {
                //Debug.Log("Daz onspawn setup 1");
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 1, 55);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 2, 71);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 3, 87);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 4, 103);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 5, 119);
                //Debug.Log("Daz onspawn setup 2");
                if (ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton==0)
                {
                    ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton = 1;
                }
                ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().Initialise();
                //Debug.Log("Daz onspawn setup 3");
                DazStatics.RscListRefreshHeader();
                //Debug.Log("Daz onspawn setup 4");
                DazRscListSortData.SyncRows(___rows);
            }

            
        }


        [HarmonyPatch(typeof(PinnedResourcesPanel))]
        [HarmonyPatch("SortRows")]
        public class PinnedRscPanelPatchSortRows
        {
            public static bool Prefix(PinnedResourcesPanel __instance, Dictionary<Tag, GameObject> ___rows)
            {
                //in theory a transpiler replace the one line needed should be used, i lack the skills to do so, copy-paste entire method as a workaround
                //start Klei code
               // Debug.Log("Daz native sortrwos 1");
                List<Tag> tagList = new List<Tag>(); //all items in ___rows
                foreach (KeyValuePair<Tag, GameObject> row in ___rows)
                {
                    tagList.Add(row.Key);
                }
                //end Klei code
                //below line is all i need to replace
                //tagList.Sort((Comparison<Tag>)((a, b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink())));
                //replace with my code, want to make this a transpiler?
                //Debug.Log("Daz native sortrwos 21");
                //List<Tag> passList = new List<Tag>(); //taglist being modified in other thread?
                List<Tag> RetList = DazStatics.SortRscList(tagList, ___rows); //need to modify order of tagList obejct without changing object name so below code doesn't need modification.
                //RetList; //can't pass a list by ref or out? workaround
                //resume Klei code
               // Debug.Log("Daz native sortrwos 3");
                foreach (Tag key in RetList)
                {
                    ___rows[key].transform.SetAsLastSibling();
                }
                PinnedResourcesPanel.Instance.clearNewButton.transform.SetAsLastSibling();
                PinnedResourcesPanel.Instance.seeAllButton.transform.SetAsLastSibling();
               // Debug.Log("Daz native sortrwos 4");
                //end of method, bypass original method with next line
                return false;
            }
        }
    }
}
