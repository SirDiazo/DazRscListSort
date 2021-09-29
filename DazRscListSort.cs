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

namespace DazRscListSort
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class HdrBtn : Button
    {
        public int BtnNum;
        public GameObject go;
        public int CurrentBtnNum; //curently selected button, this should be in sync across all 5, put here for serialization
        public List<Tag> sortedTags;

        public void Setup(int num, GameObject attachedGO)
        {
            //Debug.Log("Daz setup start " + num);
            BtnNum = num;
            CurrentBtnNum = 1;
            DazStatics.hdrButtons.Add(num,this);
            go = attachedGO;
            if(sortedTags == null)
            {
                sortedTags = new List<Tag>();
            }
           // Debug.Log("Daz setup end " + BtnNum + "." + num);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            DazStatics.hdrButtonChangeInProgress = true;
            Debug.Log("Daz click start" + BtnNum);
            DazStatics.DumpSavedLists();
            if (BtnNum != CurrentBtnNum)
            {
                DazStatics.UpdateRscList(BtnNum); //must be called before updated RscListCurrentID

                foreach(int i in DazStatics.hdrButtons.Keys)
                {
                    DazStatics.hdrButtons[i].CurrentBtnNum = BtnNum; //set current button value on all buttons
                }

                DazStatics.RscListRefreshHeader(); //updated header icons for new current list
            }
            Debug.Log("Daz click mid " + BtnNum);
            DazStatics.DumpSavedLists();
            Debug.Log("Daz click end " + BtnNum);
            //go.GetComponent<RectTransform>().Translate(new Vector3(20, 0, 0));
            DazStatics.hdrButtonChangeInProgress = false;


        }

    }

 
    public static class DazStatics
    {
        //public static Dictionary<int, List<Tag>> RscListCollection = new Dictionary<int, List<Tag>>(); //master storage, need to save/serialize this, moved to HdrBtn class
        //public static int RscListCurrentID = 1; //current tag list id, need to save/serialize moved to HdrBtn class
        public static Dictionary<int, HdrBtn> hdrButtons = new Dictionary<int,HdrBtn>();
        public static bool hdrButtonChangeInProgress = false;//apparently ONI is multithreaded and the PinnedResourcesWindow can try to update in teh middle of a button change

        public static void DumpSavedLists() //troubleshooting only
        {
            foreach(KeyValuePair<int,HdrBtn> kVp in DazStatics.hdrButtons)
            {
                Debug.Log("Daz dump button num " + kVp.Key);
                foreach(Tag tg in kVp.Value.sortedTags)
                {
                    Debug.Log(" daz dump tag " + tg.ProperNameStripLink());
                }
            }
            foreach(Tag tg in ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory.pinnedResources)
            {
                Debug.Log(" daz dump world tag " + tg.ProperNameStripLink());
            }
        }
        public static void UpdateRscList(int newList) //RscListCurrentID still holds old ID
        {
            //do not save current list, should already be saved when list was last updated
            //clear list, this is direct copy of PinnedResourcePanel.UnPinAll(), but that method is private so can't call it
            //rows list is private, can't access, so use our saved list as it should be the same, RscListCurrentID is still the old list, use it to remove
            WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
            foreach (Tag tg in DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags)
            {
                worldInventory.pinnedResources.Remove(tg);
                Debug.Log("Daz remove " + DazStatics.hdrButtons[1].CurrentBtnNum + tg.ProperName());
            }

            //add items from new list
            foreach (Tag tg in DazStatics.hdrButtons[newList].sortedTags)
            {
                worldInventory.pinnedResources.Add(tg);
                Debug.Log("Daz add " + newList + tg.ProperName());
            }
            //RscListCurrentID = newList; update current button back in OnClick now
            PinnedResourcesPanel.Instance.Refresh();
        }

        public static void RscListRefreshHeader() //refresh header button colors
        {
            foreach(int i in DazStatics.hdrButtons.Keys)
            {
                if(DazStatics.hdrButtons[i].BtnNum == DazStatics.hdrButtons[i].CurrentBtnNum)
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
        //got ahead of myself, need to get buttons working first
        public static List<Tag> SortRscList(List<Tag> OldList, List<Tag> OldListActive) //primary work done here, compare list and sort, call from PinnedResourceList with Harmony
        {
            //Debug.Log("Daz sort routine fired");
            //foreach(Tag tg in OldListActive)
            //{
            //    Debug.Log("Daz sort check 1 " + tg.ProperName());
            //}
            //foreach (Tag tg in OldList)
            //{
            //    Debug.Log("Daz sort check 1b " + tg.ProperName());
            //}
            //foreach (Tag tg in RscListCollection[RscListCurrentID])
            //{
            //    Debug.Log("Daz sort check 2 " + tg.ProperName());
            //}
            //if (!DazStatics.RscListCollection.ContainsKey(DazStatics.RscListCurrentID)) //null check
            //{
            //    DazStatics.RscListCollection.Add(DazStatics.RscListCurrentID, new List<Tag>()); //was null, instance new list
            //}
           // Debug.Log("Daz sort start");
            List<Tag> ReturnList = new List<Tag>(); //have to add the inactive items back into list
            if (DazStatics.hdrButtons.ContainsKey(1) && (DazStatics.hdrButtonChangeInProgress==false)) //race condition check, this method can run before buttons instantiate so skip if buttons don't exist yet
            {
                List<Tag> toRemove = new List<Tag>();
                //Debug.Log("Daz sort a1" + DazStatics.hdrButtons[1].CurrentBtnNum);
                //Debug.Log("Daz sort a2" + DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum]);
                if (DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags == null)
                {
                    DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags = new List<Tag>();
                }

                foreach (Tag tag in DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags) //check nothing has been removed from list
                {
                    if (!OldListActive.Contains(tag)) //removed item so...
                    {
                        //DazStatics.RscListCollection[DazStatics.RscListCurrentID].Remove(tag); //remove from saved list, this line is crash error, can't edit list being enumerated
                        toRemove.Add(tag); //save tags to remove
                                           //Debug.Log("Daz remove item my list" + RscListCurrentID + tag.ProperName());
                    }
                }
                //Debug.Log("Daz sort 1");
                foreach (Tag tg in toRemove) //remove tags
                {
                    DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags.Remove(tg);
                    Debug.Log("Daz remove item my list" + tg.ProperName());
                }

                foreach (Tag tag in OldListActive) //check list to see if anythign new added
                {
                    if (!DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags.Contains(tag)) //missing tag from saved list
                    {
                        DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags.Add(tag); //add tag to end of list
                        Debug.Log("Daz add item my list" + tag.ProperName());
                    }
                }
                //Debug.Log("Daz sort 2");
                //foreach (Tag tg in OldList)
                //{
                //    Debug.Log("Daz sort check 3 " + tg.ProperName());
                //}
                //foreach (Tag tg in RscListCollection[RscListCurrentID])
                //{
                //    Debug.Log("Daz sort check 3 " + tg.ProperName());
                //}
                //List<Tag> ReturnList = new List<Tag>(); //have to add the inactive items back into list
                foreach (Tag tg in DazStatics.hdrButtons[DazStatics.hdrButtons[1].CurrentBtnNum].sortedTags) //active items at start of returned list
                {
                    ReturnList.Add(tg);
                }
                //Debug.Log("Daz sort 4");
                foreach (Tag tg in OldList) //add inactive items
                {
                    if (!ReturnList.Contains(tg))
                    {
                        ReturnList.Add(tg);
                    }
                }
            }
            else
            {
                Debug.Log("Daz sort fail, return same list");
                ReturnList = OldList;
            }
           // Debug.Log("Daz sort end");
            //foreach (Tag tg in ReturnList) //add inactive items
            //{
            //    Debug.Log("daz retlist item " + tg.ProperName());
            //}
            return ReturnList;
        }

    }



    public class DazPatches : UserMod2
    {
       



        
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("Daz Yay, working!");
            

        }

        //public void SetupButton(GameObject parent, int btnnum, int posOffset)
        //{
        //    GameObject btn = new GameObject("RscList"+btnnum, typeof(RectTransform), typeof(KLayoutElement), typeof(HdrBtn), typeof(Image));
        //    RectTransform btnTfrm = btn.GetComponent<RectTransform>();
        //    btnTfrm.SetParent(parent.transform, false);
        //    btn.GetComponent<KLayoutElement>().ignoreLayout = true;
        //    HdrBtn btnImg = btn.GetComponent<HdrBtn>();
        //    btnImg.Setup(btnnum,btn);
        //    btnTfrm.sizeDelta = new Vector2(12, 12);
        //    btnTfrm.SetAsLastSibling();
        //    btnTfrm.Translate(new Vector3(20, 0, 0));
        //}

        [HarmonyPatch(typeof(PinnedResourcesPanel))]
        [HarmonyPatch("OnSpawn")]
        public class PinnedRscPanelPatchOnSpawn
        {
            public static void Postfix()
            {
                //Debug.Log("Daz I execute after OnSpawn");
                SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 1, 55);
                SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 2, 71);
                SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 3, 87);
                SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 4, 103);
                SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 5, 119);
                DazStatics.RscListRefreshHeader();
                //GameObject TestGo = PinnedResourcesPanel.Instance.headerButton.gameObject;
                //Debug.Log("daz " + TestGo.name);
                //Debug.Log("daz " + TestGo.rectTransform().rect.ToString());
                //foreach(Component co in TestGo.GetComponents<Component>())
                //{
                //    Debug.Log("daz " + co.GetType());
                //}
                //GameObject btn1 = new GameObject("RscList1", typeof(RectTransform), typeof(KLayoutElement), typeof(HdrBtn), typeof(Image));
                //RectTransform btn1rTfm = btn1.GetComponent<RectTransform>();
                //btn1rTfm.SetParent(TestGo.transform, false);
                //btn1.GetComponent<KLayoutElement>().ignoreLayout = true;
                // HdrBtn btn1img = btn1.GetComponent<HdrBtn>();
                //btn1img.Setup(0);
                //KButton btn1btn = btn1.AddComponent<KButton>();
                //btn1btn.onClick += Clicked(); 
                //Debug.Log("daz " + btn1rTfm.sizeDelta.ToString());
                //btn1rTfm.sizeDelta = new Vector2(10, 10);
                //Debug.Log("daz " + btn1rTfm.sizeDelta.ToString());
                //btn1.rectTransform().SetAsLastSibling();
                //Image btn1img2 = btn1.GetComponent<Image>();
                //Debug.Log("daz img " + btn1img2.mainTexture + btn1img2.color);
                //foreach (Component co in btn1.GetComponents<Component>())
                //{
                //    Debug.Log("daz2 " + co.GetType());
                //}


            }

            public static void SetupButton(GameObject parent, int btnnum, int posOffset)
            {
                //Debug.Log("Daz setup 1");
                GameObject btn = new GameObject("RscList" + btnnum, typeof(RectTransform), typeof(KLayoutElement), typeof(HdrBtn), typeof(Image)); //make base gameobject
                //Debug.Log("Daz setup 2");
                RectTransform btnTfrm = btn.GetComponent<RectTransform>(); //rectTransform for UI layer
                //Debug.Log("Daz setup 3");
                btnTfrm.SetParent(parent.transform, false); //set parent, false to use localspace
                //Debug.Log("Daz setup 4");
                btn.GetComponent<KLayoutElement>().ignoreLayout = true; //not part of layout group
                //Debug.Log("Daz setup 5");
                HdrBtn btnImg = btn.GetComponent<HdrBtn>(); //setup the button
                //Debug.Log("Daz setup 6");
                btnImg.Setup(btnnum, btn);
                //Debug.Log("Daz setup 7");
                btnTfrm.sizeDelta = new Vector2(12, 12); //resize
                //Debug.Log("Daz setup 8");
                btnTfrm.SetAsLastSibling(); //not 100% this is required
                //Debug.Log("Daz setup 9");
                btnTfrm.Translate(new Vector3(posOffset, 0, 0)); //move button into place
                //Debug.Log("Daz setup 10");
                //DazStatics.hdrButtons.Add(btnnum, btnImg);
                //Debug.Log("Daz setup 11");

                //Debug.Log("daz info " + btnImg.targetGraphic + " " + btnImg.spriteState);
                //foreach (HashedString hstr in Assets.Sprites.Keys)
                //{
                //    Debug.Log("daz sprite " + Assets.Sprites[hstr].name);
                //}
                //foreach (Component co in btn.GetComponents<Component>())
                //{
                //    Debug.Log("daz2 " + co.GetType());
                //}

            }
        }


        [HarmonyPatch(typeof(PinnedResourcesPanel))]
        [HarmonyPatch("SortRows")]
        public class PinnedRscPanelPatchSortRows
        {
            public static bool Prefix(PinnedResourcesPanel __instance, Dictionary<Tag, GameObject> ___rows)
            {
                Debug.Log("Daz I execute before SortRows");
                //in theory a transpiler replace the one line needed should be used, i lack the skills to do so, copy-paste entire method as a workaround
                //start Klei code
                List<Tag> tagList = new List<Tag>(); //all items in ___rows
                List<Tag> tagListActive = new List<Tag>(); //only active items in rows, only these display, still need to return inactiveobjects to be safe
                foreach (KeyValuePair<Tag, GameObject> row in ___rows)
                {
                    tagList.Add(row.Key);
                    if(row.Value.activeSelf)
                    {
                        tagListActive.Add(row.Key);
                    }
                }
                //end Klei code
                //below line is all i need to replace
                //tagList.Sort((Comparison<Tag>)((a, b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink())));
                //replace with my code
                List<Tag> newRows = DazStatics.SortRscList(tagList, tagListActive);

                //resume Klei code
                foreach (Tag key in newRows)
                    ___rows[key].transform.SetAsLastSibling();
                PinnedResourcesPanel.Instance.clearNewButton.transform.SetAsLastSibling();
                PinnedResourcesPanel.Instance.seeAllButton.transform.SetAsLastSibling();
                //end of method, bypass original method with next line
                return false;
            }
            //public static void Postfix(PinnedResourcesPanel __instance, Dictionary<Tag, GameObject> ___rows)
            //{
            //    Debug.Log("Daz I execute after SortRows");
            //    Debug.Log("Daz" + ___rows.Count);
            //}
        }
    }
}

//possible reference from PrioritizeRowtableColumn?
//private GameObject GetWidget(GameObject parent)
//{
//    GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowWidget, parent, true);
//    HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
//    this.ConfigureButton(component, "UpButton", 1, widget_go);
//    this.ConfigureButton(component, "DownButton", -1, widget_go);
//    return widget_go;
//}

//private void ConfigureButton(
//  HierarchyReferences refs,
//  string ref_id,
//  int delta,
//  GameObject widget_go)
//{
//    KButton reference = refs.GetReference(ref_id) as KButton;
//    reference.onClick += (System.Action)(() => this.onChangePriority((object)widget_go, delta));
//    ToolTip component = reference.GetComponent<ToolTip>();
//    if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
//        return;
//    component.OnToolTip = (Func<string>)(() => this.onHoverWidget((object)widget_go, delta));
//}
//}
