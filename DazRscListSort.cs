//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace DazRscListSort
{
    //Primary object of this mod, is where date is saved
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DazRscListSortData : KMonoBehaviour
    {
        [Serialize]
        public int CurrentHdrButton;
        //hardcode multiple lists for serialization simplicity, can a dictionary be serialized?
        //dictionary can not be serialized, make a "dictionary" for each page manually, List<Tag> is key, List<string> is value
        [Serialize]
        public List<Tag> Button1ListOverRide;
        [Serialize]
        public List<string> Button1ListOverRideSource;
        [Serialize]
        public List<Tag> Button2ListOverRide;
        [Serialize]
        public List<string> Button2ListOverRideSource;
        [Serialize]
        public List<Tag> Button3ListOverRide;
        [Serialize]
        public List<string> Button3ListOverRideSource;
        [Serialize]
        public List<Tag> Button4ListOverRide;
        [Serialize]
        public List<string> Button4ListOverRideSource;
        [Serialize]
        public List<Tag> Button5ListOverRide;
        [Serialize]
        public List<string> Button5ListOverRideSource;
        [Serialize]
        public List<Tag> Button1List;
        [Serialize]
        public List<string> Button1ListSource;
        [Serialize]
        public List<Tag> Button2List;
        [Serialize]
        public List<string> Button2ListSource;
        [Serialize]
        public List<Tag> Button3List;
        [Serialize]
        public List<string> Button3ListSource;
        [Serialize]
        public List<Tag> Button4List;
        [Serialize]
        public List<string> Button4ListSource;
        [Serialize]
        public List<Tag> Button5List;
        [Serialize]
        public List<string> Button5ListSource;
        [Serialize]
        public bool Button1OverRide = false;
        [Serialize]
        public bool Button2OverRide = false;
        [Serialize]
        public bool Button3OverRide = false;
        [Serialize]
        public bool Button4OverRide = false;
        [Serialize]
        public bool Button5OverRide = false;
        [Serialize]
        public bool init = false;
        //public static Dictionary<Tag, GameObject> DazRows = new Dictionary<Tag, GameObject>();
        //public static bool DazShowSortArrows = false;
        [Serialize]
        public int WorldID;
        public Component CritInvCritterInventory;
        public FieldInfo CritInvPinned;
        public IDictionary CritInvPinnedDict;
        public HashSet<Tag> CritInvThisWildDict;
        public HashSet<Tag> CritInvThisTameDict;
        public HashSet<Tag> CritInvThisArtDict;


        //public static void SyncRows(Dictionary<Tag, GameObject> KlieRows) //called on PinnedResourcePanel.OnSpawn, this is the list of objects displayed in Pinned Resource Panel, use this reference to manipulated it, careful of race conditions
        //{
        //    DazRows = KlieRows; 
        //}

        public void Initialise(int worldID) //runs everytime a world is switched to, both on load and game play
        {
            Debug.Log("Daz init check? " + init + " " + worldID);
            if (!init)
            {
                //PinnedResourcesPanel inst = PinnedResourcesPanel.Instance;// PinnedResourcesPanel.Instance.PointerEnterActions += OnMouseEnter();
                //inst.pointerEnterActions += new KScreen.PointerEnterActions(RscOnMouseEnter);
                //inst.pointerExitActions += new KScreen.PointerExitActions(RscOnMouseExit);
                //Source is which mod is a line in pinned res window from?
                //"base" = base game
                //"ciwild" = Critter Inventory, WIld
                WorldID = this.GetComponent<WorldContainer>().id;
                CurrentHdrButton = 1;
                Button1List = new List<Tag>();
                Button1ListSource = new List<string>(); 
                Button2List = new List<Tag>();
                Button3ListSource = new List<string>();
                Button3List = new List<Tag>();
                Button4ListSource = new List<string>();
                Button4List = new List<Tag>();
                Button4ListSource = new List<string>();
                Button5List = new List<Tag>();
                Button5ListSource = new List<string>();
                if (WorldID == 0)
                {
                    Button1ListOverRide = new List<Tag>();
                    Button1ListOverRideSource = new List<string>();
                    Button2ListOverRide = new List<Tag>();
                    Button2ListOverRideSource = new List<string>();
                    Button3ListOverRide = new List<Tag>();
                    Button3ListOverRideSource = new List<string>();
                    Button4ListOverRide = new List<Tag>();
                    Button4ListOverRideSource = new List<string>();
                    Button5ListOverRide = new List<Tag>();
                    Button5ListOverRideSource = new List<string>();
                }
                init = true;
            }
            //sync Critter Inventory data
            if(DazStatics.CritterInventoryInstalled)
            {
                DazStatics.GameStartDataLoad();
                CritInvCritterInventory = ClusterManager.Instance.GetWorld(worldID).worldInventory.GetComponent("CritterInventory");
                CritInvPinned = CritInvCritterInventory.GetType().GetField("pinned", BindingFlags.Instance | BindingFlags.NonPublic);
                CritInvPinnedDict = (IDictionary)CritInvPinned.GetValue(CritInvCritterInventory);
                CritInvThisTameDict = (HashSet<Tag>)CritInvPinnedDict[DazStatics.CritInvCritterEnumValueTame];
                CritInvThisWildDict = (HashSet<Tag>)CritInvPinnedDict[DazStatics.CritInvCritterEnumValueWild];
                CritInvThisArtDict = (HashSet<Tag>)CritInvPinnedDict[DazStatics.CritInvCritterEnumValueArtificial];
            }

            //upgrade from ver 1.1 to 1.2 for adding Source field, remove in version 1.3
            DazStatics.GameStartDataLoad();
            if(Button1ListSource==null)
            {
                Debug.Log("Pinned Resource Window Extended: Old data format found, upgrading......");
                Button1ListSource = new List<string>();
                if (Button1List.Count > 0)
                {
                    for (int i = 0; i < Button1List.Count; i++)
                    {
                        Button1ListSource.Add("base");
                    }
                }
            }
            if (Button2ListSource == null)
            {
                Button2ListSource = new List<string>();
                if (Button2List.Count > 0)
                {
                    for (int i = 0; i < Button2List.Count; i++)
                    {
                        Button2ListSource.Add("base");
                    }
                }
            }
            if (Button3ListSource == null)
            {
                Button3ListSource = new List<string>();
                if (Button3List.Count > 0)
                {
                    for (int i = 0; i < Button3List.Count; i++)
                    {
                        Button3ListSource.Add("base");
                    }
                }
            }
            if (Button4ListSource == null)
            {
                Button4ListSource = new List<string>();
                if (Button4List.Count > 0)
                {
                    for (int i = 0; i < Button4List.Count; i++)
                    {
                        Button4ListSource.Add("base");
                    }
                }
            }
            if (Button5ListSource == null)
            {
                Button5ListSource = new List<string>();
                if (Button5List.Count > 0)
                {
                    for (int i = 0; i < Button5List.Count; i++)
                    {
                        Button5ListSource.Add("base");
                    }
                }
            }
            if (DazStatics.baseDstore.Button1ListOverRideSource == null)
            {
                DazStatics.baseDstore.Button1ListOverRideSource = new List<string>();
                if (DazStatics.baseDstore.Button1ListOverRide.Count > 0)
                {
                    for (int i = 0; i < DazStatics.baseDstore.Button1ListOverRide.Count; i++)
                    {
                        DazStatics.baseDstore.Button1ListOverRideSource.Add("base");
                    }
                }
            }
            if (DazStatics.baseDstore.Button2ListOverRideSource == null)
            {
                DazStatics.baseDstore.Button2ListOverRideSource = new List<string>();
                if(DazStatics.baseDstore.Button2ListOverRide.Count > 0)
                {
                    for (int i = 0; i < DazStatics.baseDstore.Button2ListOverRide.Count; i++)
                    {
                        DazStatics.baseDstore.Button2ListOverRideSource.Add("base");
                    }   
                }
            }
            if (DazStatics.baseDstore.Button3ListOverRideSource == null)
            {
                DazStatics.baseDstore.Button3ListOverRideSource = new List<string>();
                if (DazStatics.baseDstore.Button3ListOverRide.Count > 0)
                {
                    for (int i = 0; i < DazStatics.baseDstore.Button3ListOverRide.Count; i++)
                    {
                        DazStatics.baseDstore.Button3ListOverRideSource.Add("base");
                    }
                }
            }
            if (DazStatics.baseDstore.Button4ListOverRideSource == null)
            {
                DazStatics.baseDstore.Button4ListOverRideSource = new List<string>();
                if (DazStatics.baseDstore.Button4ListOverRide.Count > 0)
                {
                    for (int i = 0; i < DazStatics.baseDstore.Button4ListOverRide.Count; i++)
                    {
                        DazStatics.baseDstore.Button4ListOverRideSource.Add("base");
                    }
                }
            }
            if (DazStatics.baseDstore.Button5ListOverRideSource == null)
            {
                DazStatics.baseDstore.Button5ListOverRideSource = new List<string>();
                if (DazStatics.baseDstore.Button5ListOverRide.Count > 0)
                {
                    for (int i = 0; i < DazStatics.baseDstore.Button5ListOverRide.Count; i++)
                    {
                        DazStatics.baseDstore.Button5ListOverRideSource.Add("base");
                    }
                }
            }
            Debug.Log("Daz init check" + Button1List.Count + "?" + Button1ListSource.Count+"?"+ Button2List.Count + "?" + Button2ListSource.Count);
        }

        public void OnSpawn()
        {
            base.OnSpawn();
            Debug.Log("Daz rsc sort module on spawn");
        }
        public void SanitizeList() //clean up visible list for page switch
        {
            ClusterManager.Instance.GetWorld(WorldID).worldInventory.pinnedResources.Clear(); //base game
            if(DazStatics.CritterInventoryInstalled) //remove critter inventory items
            {
                CritInvThisWildDict.Clear();
                CritInvThisTameDict.Clear();
                CritInvThisArtDict.Clear();

                //old method to purge by mouseclick on red X, new method works, nuke this before release
                //foreach(HierarchyReferences href in DazStatics.CritInvCurrentListWild.Values)
                //{
                //    href.GetReference<MultiToggle>("PinToggle").onClick.Invoke();
                //}
                //foreach (HierarchyReferences href in DazStatics.CritInvCurrentListTame.Values)
                //{
                //    href.GetReference<MultiToggle>("PinToggle").onClick.Invoke();
                //}
                //foreach (HierarchyReferences href in DazStatics.CritInvCurrentListArtificial.Values)
                //{
                //    href.GetReference<MultiToggle>("PinToggle").onClick.Invoke();
                //}
            }
        }

        public void UpdatePinnedResources()
        {
            foreach (TagSourceItem tagSrc in GetCurrentList())
            {
                if (tagSrc.source == "base")
                {
                    ClusterManager.Instance.GetWorld(WorldID).worldInventory.pinnedResources.Add(tagSrc.tag);
                }
                else if (DazStatics.CritterInventoryInstalled && (tagSrc.source == "ciwild"))
                {
                    CritInvThisWildDict.Add(tagSrc.tag);
                }
                else if (DazStatics.CritterInventoryInstalled && (tagSrc.source == "citame"))
                {
                    CritInvThisTameDict.Add(tagSrc.tag);
                }
                else if (DazStatics.CritterInventoryInstalled && (tagSrc.source == "ciart"))
                {
                    CritInvThisArtDict.Add(tagSrc.tag);
                }
            }
        }
        public void OnDestroy()
        {
            base.OnDestroy();
        }

        public List<TagSourceItem> MakeList (List<Tag> tagList, List<string> sourceList)
        {
            if(tagList.Count != sourceList.Count)
            {
                Debug.Log("Daz list make count mismatch! " + tagList.Count + "?" + sourceList.Count);
                if(tagList.Count > sourceList.Count)
                {
                    tagList.RemoveRange(sourceList.Count, tagList.Count - sourceList.Count);
                }
                else
                {
                    sourceList.RemoveRange(tagList.Count, sourceList.Count - tagList.Count);
                }
            }
            int itemCount = Math.Min(tagList.Count, sourceList.Count);
            List<TagSourceItem> retList = new List<TagSourceItem>();
            for(int i=0;i<itemCount;i++)
            {
                retList.Add(new TagSourceItem(tagList[i], sourceList[i]));
            }
            return retList;
        }

        public List<TagSourceItem> GetList(int num)
        {
            if(num == 1)
            {
                return GetList(1, DazStatics.baseDstore.Button1OverRide);
            }
            else if (num == 2)
            {
                return GetList(2, DazStatics.baseDstore.Button2OverRide);
            }
            else if (num == 3)
            {
                return GetList(3, DazStatics.baseDstore.Button3OverRide);
            }
            else if (num == 4)
            {
                return GetList(4, DazStatics.baseDstore.Button4OverRide);
            }
            else if (num == 5)
            {
                return GetList(5, DazStatics.baseDstore.Button5OverRide);
            }
            else
            {
                Debug.Log("Daz getlist int failed");
                return new List<TagSourceItem>();
            }
        }
        public List<TagSourceItem> GetList(int num, bool overRide) //get List<Tag> associated with a button, specified by passed int.
        {
            if (num == 0)
            {
                Debug.Log("Daz Pinned Resource List looking for list 0, reset to list 1");
                CurrentHdrButton = 1;
                num = 1;
            }
            if (num == 1)
            {
                if (overRide)
                {
                    if (DazStatics.baseDstore.Button1ListOverRide == null)
                    {
                        DazStatics.baseDstore.Button1ListOverRide = new List<Tag>();
                    }
                    return MakeList(DazStatics.baseDstore.Button1ListOverRide, DazStatics.baseDstore.Button1ListOverRideSource);
                }
                else
                {
                    if (Button1List == null)
                    {
                        Button1List = new List<Tag>();
                    }
                    return MakeList(Button1List, Button1ListSource);
                }
            }
            else if (num == 2)
            {
                if (overRide)
                {
                    if (DazStatics.baseDstore.Button2ListOverRide == null)
                    {
                        DazStatics.baseDstore.Button2ListOverRide = new List<Tag>();
                    }
                    return MakeList(DazStatics.baseDstore.Button2ListOverRide, DazStatics.baseDstore.Button2ListOverRideSource);
                }
                else
                {
                    if (Button2List == null)
                    {
                        Button2List = new List<Tag>();
                    }
                    return MakeList(Button2List, Button2ListSource);
                }
            }
            else if (num == 3)
            {
                if (overRide)
                {
                    if (DazStatics.baseDstore.Button3ListOverRide == null)
                    {
                        DazStatics.baseDstore.Button3ListOverRide = new List<Tag>();
                    }
                    return MakeList(DazStatics.baseDstore.Button3ListOverRide, DazStatics.baseDstore.Button3ListOverRideSource);
                }
                else
                {
                    if (Button3List == null)
                    {
                        Button3List = new List<Tag>();
                    }
                    return MakeList(Button3List, Button3ListSource);
                }
            }
            else if (num == 4)
            {
                if (overRide)
                {
                    if (DazStatics.baseDstore.Button4ListOverRide == null)
                    {
                        DazStatics.baseDstore.Button4ListOverRide = new List<Tag>();
                    }
                    return MakeList(DazStatics.baseDstore.Button4ListOverRide, DazStatics.baseDstore.Button4ListOverRideSource);
                }
                else
                {
                    if (Button4List == null)
                    {
                        Button4List = new List<Tag>();
                    }
                    return MakeList(Button4List, Button4ListSource);
                }
            }
            else if (num == 5)
            {
                if (overRide)
                {
                    if (DazStatics.baseDstore.Button5ListOverRide == null)
                    {
                        DazStatics.baseDstore.Button5ListOverRide = new List<Tag>();
                    }
                    return MakeList(DazStatics.baseDstore.Button5ListOverRide, DazStatics.baseDstore.Button5ListOverRideSource);
                }
                else
                {
                    if (Button5List == null)
                    {
                        Button5List = new List<Tag>();
                    }
                    return MakeList(Button5List, Button5ListSource);
                }
            }
            else
            {
                Debug.Log("Daz Pinned Resource DataStore missing List<Tag> for " + num);
                return new List<TagSourceItem>();
            }
        }

        public void AddItemToCurList(TagSourceItem tgItem)
        {
            {
                AddItemToList(tgItem, CurrentHdrButton);
            }
        }
        public void AddItemToList(TagSourceItem tgItem, int pageNum)
        {
            if(pageNum==1)
            {
                AddItemToList(tgItem, pageNum, DazStatics.baseDstore.Button1OverRide);
            }
            else if (pageNum == 2)
            {
                AddItemToList(tgItem, pageNum, DazStatics.baseDstore.Button2OverRide);
            }
            else if (pageNum == 3)
            {
                AddItemToList(tgItem, pageNum, DazStatics.baseDstore.Button3OverRide);
            }
            else if (pageNum == 4)
            {
                AddItemToList(tgItem, pageNum, DazStatics.baseDstore.Button4OverRide);
            }
            else if (pageNum == 5)
            {
                AddItemToList(tgItem, pageNum, DazStatics.baseDstore.Button5OverRide);
            }
        }
        public void AddItemToList(TagSourceItem tgItem, int pageNum, bool overRide)
        {
            if(pageNum == 1)
            {
                if(overRide)
                {
                    DazStatics.baseDstore.Button1ListOverRide.Add(tgItem.tag);
                    DazStatics.baseDstore.Button1ListOverRideSource.Add(tgItem.source);
                }
                else
                {
                    Button1List.Add(tgItem.tag);
                    Button1ListSource.Add(tgItem.source);
                }
            }
            else if (pageNum == 2)
            {
                if (overRide)
                {
                    DazStatics.baseDstore.Button2ListOverRide.Add(tgItem.tag);
                    DazStatics.baseDstore.Button2ListOverRideSource.Add(tgItem.source);
                }
                else
                {
                    Button2List.Add(tgItem.tag);
                    Button2ListSource.Add(tgItem.source);
                }
            }

            else if (pageNum == 3)
            {
                if (overRide)
                {
                    DazStatics.baseDstore.Button3ListOverRide.Add(tgItem.tag);
                    DazStatics.baseDstore.Button3ListOverRideSource.Add(tgItem.source);
                }
                else
                {
                    Button3List.Add(tgItem.tag);
                    Button3ListSource.Add(tgItem.source);
                }
            }

            else if (pageNum == 4)
            {
                if (overRide)
                {
                    DazStatics.baseDstore.Button4ListOverRide.Add(tgItem.tag);
                    DazStatics.baseDstore.Button4ListOverRideSource.Add(tgItem.source);
                }
                else
                {
                    Button4List.Add(tgItem.tag);
                    Button4ListSource.Add(tgItem.source);
                }
            }

            else if (pageNum == 5)
            {
                if (overRide)
                {
                    DazStatics.baseDstore.Button5ListOverRide.Add(tgItem.tag);
                    DazStatics.baseDstore.Button5ListOverRideSource.Add(tgItem.source);
                }
                else
                {
                    Button5List.Add(tgItem.tag);
                    Button5ListSource.Add(tgItem.source);
                }
            }
        }

        public void AddItemCurrentlist(TagSourceItem addItem, int idx) //add item to specific location in list at idx number
        {
            if(CurrentHdrButton==1)
            {
                if(DazStatics.baseDstore.Button1OverRide)
                {
                    DazStatics.baseDstore.Button1ListOverRide.Insert(idx, addItem.tag);
                    DazStatics.baseDstore.Button1ListOverRideSource.Insert(idx, addItem.source);
                }
                else
                {
                    Button1List.Insert(idx, addItem.tag);
                    Button1ListSource.Insert(idx, addItem.source);
                }
            }
            else if (CurrentHdrButton == 2)
            {
                if (DazStatics.baseDstore.Button2OverRide)
                {
                    DazStatics.baseDstore.Button2ListOverRide.Insert(idx, addItem.tag);
                    DazStatics.baseDstore.Button2ListOverRideSource.Insert(idx, addItem.source);
                }
                else
                {
                    Button2List.Insert(idx, addItem.tag);
                    Button2ListSource.Insert(idx, addItem.source);
                }
            }
            else if (CurrentHdrButton == 3)
            {
                if (DazStatics.baseDstore.Button3OverRide)
                {
                    DazStatics.baseDstore.Button3ListOverRide.Insert(idx, addItem.tag);
                    DazStatics.baseDstore.Button3ListOverRideSource.Insert(idx, addItem.source);
                }
                else
                {
                    Button3List.Insert(idx, addItem.tag);
                    Button3ListSource.Insert(idx, addItem.source);
                }
            }
            else if (CurrentHdrButton == 4)
            {
                if (DazStatics.baseDstore.Button4OverRide)
                {
                    DazStatics.baseDstore.Button4ListOverRide.Insert(idx, addItem.tag);
                    DazStatics.baseDstore.Button4ListOverRideSource.Insert(idx, addItem.source);
                }
                else
                {
                    Button4List.Insert(idx, addItem.tag);
                    Button4ListSource.Insert(idx, addItem.source);
                }
            }
            else if (CurrentHdrButton == 5)
            {
                if (DazStatics.baseDstore.Button5OverRide)
                {
                    DazStatics.baseDstore.Button5ListOverRide.Insert(idx, addItem.tag);
                    DazStatics.baseDstore.Button5ListOverRideSource.Insert(idx, addItem.source);
                }
                else
                {
                    Button5List.Insert(idx, addItem.tag);
                    Button5ListSource.Insert(idx, addItem.source);
                }
            }
        }
        public void RemoveItem(TagSourceItem rmvItem)
        {
            if(CurrentHdrButton == 1)
            {
                RemoveItem(rmvItem, 1);
            }
            else if (CurrentHdrButton == 2)
            {
                RemoveItem(rmvItem, 2);
            }
            else if (CurrentHdrButton == 3)
            {
                RemoveItem(rmvItem, 3);
            }
            else if (CurrentHdrButton == 4)
            {
                RemoveItem(rmvItem, 4);
            }
            else if (CurrentHdrButton == 5)
            {
                RemoveItem(rmvItem, 5);
            }
        }
        public void RemoveItem(TagSourceItem rmvItem, int pageNum)
        {
            if(pageNum ==1)
            {
                RemoveItem(rmvItem, pageNum, DazStatics.baseDstore.Button1OverRide);
            }
            else if (pageNum == 2)
            {
                RemoveItem(rmvItem, pageNum, DazStatics.baseDstore.Button2OverRide);
            }
            else if (pageNum == 3)
            {
                RemoveItem(rmvItem, pageNum, DazStatics.baseDstore.Button3OverRide);
            }
            else if (pageNum == 4)
            {
                RemoveItem(rmvItem, pageNum, DazStatics.baseDstore.Button4OverRide);
            }
            else if (pageNum == 5)
            {
                RemoveItem(rmvItem, pageNum, DazStatics.baseDstore.Button5OverRide);
            }
        }
        public void RemoveItem(TagSourceItem rmvItem, int pageNum, bool overRide)
        {
            Debug.Log("Daz remove trace" + rmvItem.tag + "?" + rmvItem.source+"?"+pageNum+"?"+overRide);
            if (pageNum == 1)
            {
                if (overRide)
                {
                    int rmvIdx = GetList(1, overRide).IndexOf(rmvItem);
                    DazStatics.baseDstore.Button1ListOverRide.RemoveAt(rmvIdx);
                    DazStatics.baseDstore.Button1ListOverRideSource.RemoveAt(rmvIdx);
                }
                else
                {
                    int rmvIdx = GetList(1, overRide).IndexOf(rmvItem);
                    Button1List.RemoveAt(rmvIdx);
                    Button1ListSource.RemoveAt(rmvIdx);
                }
            }
            else if (pageNum == 2)
            {
                if (overRide)
                {
                    int rmvIdx = GetList(2, overRide).IndexOf(rmvItem);
                    DazStatics.baseDstore.Button2ListOverRide.RemoveAt(rmvIdx);
                    DazStatics.baseDstore.Button2ListOverRideSource.RemoveAt(rmvIdx);
                }
                else
                {
                    int rmvIdx = GetList(2, overRide).IndexOf(rmvItem);
                   Button2List.RemoveAt(rmvIdx);
                    Button2ListSource.RemoveAt(rmvIdx);
                }
            }
            else if (pageNum == 3)
            {
                if (overRide)
                {
                    int rmvIdx = GetList(3, overRide).IndexOf(rmvItem);
                    DazStatics.baseDstore.Button3ListOverRide.RemoveAt(rmvIdx);
                    DazStatics.baseDstore.Button3ListOverRideSource.RemoveAt(rmvIdx);
                }
                else
                {
                    
                    int rmvIdx = GetList(3, overRide).IndexOf(rmvItem);
                    Button3List.RemoveAt(rmvIdx);
                    Button3ListSource.RemoveAt(rmvIdx);
                }
            }
            else if (pageNum == 4)
            {
                if (overRide)
                {
                    int rmvIdx = GetList(4, overRide).IndexOf(rmvItem);
                    DazStatics.baseDstore.Button4ListOverRide.RemoveAt(rmvIdx);
                    DazStatics.baseDstore.Button4ListOverRideSource.RemoveAt(rmvIdx);
                }
                else
                {
                    int rmvIdx = GetList(4, overRide).IndexOf(rmvItem);
                    Button4List.RemoveAt(rmvIdx);
                    Button4ListSource.RemoveAt(rmvIdx);
                }
            }
            else if (pageNum == 5)
            {
                if (overRide)
                {
                    int rmvIdx = GetList(5, overRide).IndexOf(rmvItem);
                    DazStatics.baseDstore.Button5ListOverRide.RemoveAt(rmvIdx);
                    DazStatics.baseDstore.Button5ListOverRideSource.RemoveAt(rmvIdx);
                }
                else
                {
                    int rmvIdx = GetList(5, overRide).IndexOf(rmvItem);
                    Button5List.RemoveAt(rmvIdx);
                    Button5ListSource.RemoveAt(rmvIdx);
                }
            }
        }
        public void ResetData(List<TagSourceItem> newList)
        {
            if (CurrentHdrButton == 1)
            {
                if (DazStatics.baseDstore.Button1OverRide)
                {
                    DazStatics.baseDstore.Button1ListOverRide.Clear();
                    DazStatics.baseDstore.Button1ListOverRideSource.Clear();
                    foreach(TagSourceItem tgItem in newList)
                    {
                        DazStatics.baseDstore.Button1ListOverRide.Add(tgItem.tag);
                        DazStatics.baseDstore.Button1ListOverRideSource.Add(tgItem.source);
                    }
                }
                else
                {
                    Button1List.Clear();
                    Button1ListSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        Button1List.Add(tgItem.tag);
                        Button1ListSource.Add(tgItem.source);
                    }
                }
            }
            else if (CurrentHdrButton == 2)
            {
                if (DazStatics.baseDstore.Button2OverRide)
                {
                    DazStatics.baseDstore.Button2ListOverRide.Clear();
                    DazStatics.baseDstore.Button2ListOverRideSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        DazStatics.baseDstore.Button2ListOverRide.Add(tgItem.tag);
                        DazStatics.baseDstore.Button2ListOverRideSource.Add(tgItem.source);
                    }
                }
                else
                {
                    Button2List.Clear();
                    Button2ListSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        Button2List.Add(tgItem.tag);
                        Button2ListSource.Add(tgItem.source);
                    }
                }
            }
            else if (CurrentHdrButton == 3)
            {
                if (DazStatics.baseDstore.Button3OverRide)
                {
                    DazStatics.baseDstore.Button3ListOverRide.Clear();
                    DazStatics.baseDstore.Button3ListOverRideSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        DazStatics.baseDstore.Button3ListOverRide.Add(tgItem.tag);
                        DazStatics.baseDstore.Button3ListOverRideSource.Add(tgItem.source);
                    }
                }
                else
                {
                    Button3List.Clear();
                    Button3ListSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        Button3List.Add(tgItem.tag);
                        Button3ListSource.Add(tgItem.source);
                    }
                }
            }
            else if (CurrentHdrButton == 4)
            {
                if (DazStatics.baseDstore.Button4OverRide)
                {
                    DazStatics.baseDstore.Button4ListOverRide.Clear();
                    DazStatics.baseDstore.Button4ListOverRideSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        DazStatics.baseDstore.Button4ListOverRide.Add(tgItem.tag);
                        DazStatics.baseDstore.Button4ListOverRideSource.Add(tgItem.source);
                    }
                }
                else
                {
                    Button4List.Clear();
                    Button4ListSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        Button4List.Add(tgItem.tag);
                        Button4ListSource.Add(tgItem.source);
                    }
                }
            }
            else if (CurrentHdrButton == 5)
            {
                if (DazStatics.baseDstore.Button5OverRide)
                {
                    DazStatics.baseDstore.Button5ListOverRide.Clear();
                    DazStatics.baseDstore.Button5ListOverRideSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        DazStatics.baseDstore.Button5ListOverRide.Add(tgItem.tag);
                        DazStatics.baseDstore.Button5ListOverRideSource.Add(tgItem.source);
                    }
                }
                else
                {
                    Button5List.Clear();
                    Button5ListSource.Clear();
                    foreach (TagSourceItem tgItem in newList)
                    {
                        Button5List.Add(tgItem.tag);
                        Button5ListSource.Add(tgItem.source);
                    }
                }
            }
        }
        public List<TagSourceItem> GetCurrentList() //get List<Tag> of currently selected button
        {
            return GetList(CurrentHdrButton);
        }
        public void ReOrdertag(Tag tg, string source, bool Up) //rework!
        {
            Debug.Log("Daz sort button clicks start");
            if(source.Length<1)//error catch
            {
                Debug.Log("Daz sort reorder error catch hit, to base:from:" + source+"?");
                source = "base";
            }
            DazStatics.hdrButtonChangeInProgress = true; //race condition exists, lock out PinnedResoucePanel.SortRows from running on true.
            List<TagSourceItem> workingList = GetCurrentList(); //get List<Tag> of current Header button
            int OldTagIndex = 0; //save Index location of Tag being moved
            TagSourceItem compare = new TagSourceItem(tg, source);
            for (int i = 0; i < workingList.Count; i++)
            {
                if (workingList[i] == compare)
                {
                    OldTagIndex = i;
                    break;
                }
            }
            bool breakout = false;
            if (DiscoveredResources.Instance.newDiscoveries.ContainsKey(tg) && source == "base") //if resrouce was in new discoveries list and not pinned resources list, move it over, but don't change location in list, somehow Critter Inventory was adding to new discoveries list? (or the base game never clears Critters from New Discoveries?)
            {
                Debug.Log("Daz new discovery " + tg);
                DiscoveredResources.Instance.newDiscoveries.Remove(tg);
                if (!ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tg))
                {
                    ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Add(tg);
                }
                breakout = true;

            }
            if (!breakout)
            {
                if (!(OldTagIndex == 0) && Up || !((OldTagIndex + 1) == workingList.Count) && !Up) //can't move top level item up, or bottom level item down, check here for that
                {
                    workingList.RemoveAt(OldTagIndex);//row needs moving, remove it from the list
                    RemoveItem(new TagSourceItem(tg, source)); //remove item from data storage
                    if (Up) //reinsert tag at new location moving up
                    {
                        workingList.Insert(OldTagIndex - 1, compare);
                        AddItemCurrentlist(new TagSourceItem(tg, source), OldTagIndex - 1);

                    }
                    else //reinsert tag at new location, moving down
                    {
                        workingList.Insert(OldTagIndex + 1, compare);
                        AddItemCurrentlist(new TagSourceItem(tg, source), OldTagIndex + 1);
                    }
                }
                ResetData(workingList); //update base saved data lists
            }
            
            DazStatics.hdrButtonChangeInProgress = false; //reenabled PinnedResourcePanel.SortRows
            Debug.Log("Daz sort button clicks end");
            DazStatics.ListRefreshRequired = true;
            PinnedResourcesPanel.Instance.Refresh(); //Refresh PinnedResourcePanel for new tag sort

            
        }
    }
    
    

    
}
