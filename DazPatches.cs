using HarmonyLib;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using KMod;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Reflection.Emit;
using System.Linq;

namespace DazRscListSort
{
    public class DazPatches : UserMod2 //mod loading class
    {
        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods) //runs after all mods have loaded, called automatically by base game
            {
                foreach (Mod mod in mods)
                {
                    if ((mod.staticID == "PeterHan.CritterInventory") && mod.IsActive()) //is Critter Inventory mod loaded?
                    {
                        Debug.Log("Pinned Resource List Extended: Critter Inventory Mod found!");
                        DazStatics.CritterInventoryInstalled = true;
                    }
                    if (mod.staticID == "DazRscListSort") //list my mod and version for troubleshooting
                    {
                        Debug.Log("Pinned Resource List Extended Version " + mod.packagedModInfo.version + " (By Diazo)");
                    }
                }
                if(DazStatics.CritterInventoryInstalled) //stop Critter Inventory from calling SetAsLastSibling (3 places) and do in this mod instead for those objects
                {
                Assembly CritInvAssem = null;
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if(asm.GetName().ToString().Substring(0,16)== "CritterInventory")
                    {
                        CritInvAssem = asm;
                    }
                }
                Type CritInvPinMng = Type.GetType("PeterHan.CritterInventory.NewResourceScreen.PinnedCritterManager, CritterInventory", false);

                MethodInfo CritInvPopPinRow = null;
                foreach(MethodInfo mthd in CritInvPinMng.GetMethods(BindingFlags.NonPublic | BindingFlags.Public |BindingFlags.Instance|BindingFlags.Static))
                {
                    //Debug.Log("Daz Method check" + mthd.Name);
                    if(mthd.Name == "PopulatePinnedRows")
                    {
                        CritInvPopPinRow = mthd;
                    }
                }
                Type typ = typeof(ManualPatches);
                MethodInfo transpilerMth = typ.GetMethod("CritterInvPopulatePinnedRowsTranspiler");
                harmony.Patch(CritInvPopPinRow, null, null, new HarmonyMethod(transpilerMth), null);

                //base game has a check to not update pinned resource window if nothing is pinned, but can only see base game resources, override this in case list only contains Critter Inventory Objects

                //typepatch
                Debug.Log("Daz method 0 check ");
                MethodInfo SyncRowsMethod = typeof(PinnedResourcesPanel).GetMethod("SyncRows",BindingFlags.NonPublic|BindingFlags.Instance);
                //Type PinnedRes = typeof(PinnedResourcesPanel);
                Debug.Log("Daz method 0a check " + SyncRowsMethod.Name);
                //MethodInfo SyncRowsMethod = PinnedRes.GetMethod("SyncRows");
                Debug.Log("Daz method 1 check " + SyncRowsMethod.Name);
                MethodInfo SyncRowsPatch = typeof(ManualPatches).GetMethod("BaseGamePinnedResourceWindow");
                Debug.Log("Daz patch2 called" + SyncRowsPatch.Name);
                harmony.Patch(SyncRowsMethod, null, null, new HarmonyMethod(SyncRowsPatch), null);
                Debug.Log("Daz patch2 complete");
            }


            }

        public static class ManualPatches
        {
            public static IEnumerable<CodeInstruction> BaseGamePinnedResourceWindow(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                Debug.Log("Daz patch2 start");
                int i = 0;
                foreach (CodeInstruction ci in instructions)
                {
                    Debug.Log("Daz syncrows instructsion " + i + "?" + ci.ToString());
                    i++;

                 }
                Debug.Log("Daz patch2 end");
                return instructions;
            }
            public static IEnumerable<CodeInstruction> CritterInvPopulatePinnedRowsTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                List<CodeInstruction> retCodes = new List<CodeInstruction>(); //remember [HarmonyDebug] on the method or class to log this
                int i = 0;
                foreach(CodeInstruction ci in instructions)
                {
                    if (i == 92)
                    {

                        System.Reflection.Emit.Label Label9 = ci.labels.FirstOrDefault(); //generating label by name did not work, so pull label from jump start location
                        CodeInstruction ci2 = new CodeInstruction(OpCodes.Nop);
                        ci2.labels.Add(Label9);
                        retCodes.Add(ci2);
                    }
                    else if (i == 93 || i == 94 || i == 117 || i == 118 || i == 119 || i == 120 || i == 121 || i == 122 || i == 123 || i == 124 || i == 125 || i == 126) //NoOp the 3 SetAslastSibling lines on the Pinned Resource window, leave the one(s) for the AllResourcesWindow active
                    {
                        retCodes.Add(new CodeInstruction(OpCodes.Nop));
                    }
                    else
                    {
                        retCodes.Add(ci);
                    }

                    i++;
                }
                //int i2 = 0;
                //foreach (CodeInstruction ci in retCodes) //enable this for which lines need updating
                //{
                //    Debug.Log("Daz instructions RET" + i2 + ":?:" + ci.ToString());

                    

                //    i2++;
                //}

                return retCodes.AsEnumerable();
            }
        }
            [HarmonyPatch(typeof(Game), "OnSpawn")]
        public static class Game_OnSpawn_Patch
        {
            public static void Postfix()
            {
                DazStatics.OnGameSpawn();

            }
        }
        [HarmonyPatch(typeof(Game), "OnDestroy")]
        public static class Game_OnDestroy_Patch
        {
            public static void Prefix()
            {
                DazStatics.OnGameDeSpawn();
            }
        }
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
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 1, 55); //setup the 5 Page buttons on the header
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 2, 71);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 3, 87);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 4, 103);
                DazStatics.SetupButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 5, 119);
                DazStatics.SetupSortButton(PinnedResourcesPanel.Instance.headerButton.gameObject, 35);
                if (ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton == 0) //this will only be hit the first time the mod is loaded into a game, so set to first page, otherwise last display page will populate this field on game load, use that if present.
                {
                    ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().CurrentHdrButton = 1;
                }
                ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>().Initialise(ClusterManager.Instance.activeWorldId); //call primary data object for setup
                DazStatics.GameStartDataLoad();
                DazStatics.RscListRefreshHeader(); //now have loaded, refresh button colors

                DazStatics.SyncRows(___rows); //link rows object to primary data store
                DazStatics.PinnedResourceWindowOnSpawnComplete = true;
                Debug.Log("Daz pinnedreswin onspawn");
            }
        }
        //[HarmonyPatch(typeof(PinnedResourcesPanel))] //was used for testing, not needed
        //[HarmonyPatch("SyncRows")]
        //public class PinnedRscPanelPatchSyncRows
        //{


        //    //primary worker method of the mod, this is called every 1000ms by Klei, note total replacement means any other mod touching this method will need compatibility probably
        //    public static bool Prefix(PinnedResourcesPanel __instance, Dictionary<Tag, GameObject> ___rows)
        //    {
        //        Debug.Log("Daz new discs sync rows" + DiscoveredResources.Instance.newDiscoveries.Count);
        //        foreach (KeyValuePair<Tag, float> kvp in DiscoveredResources.Instance.newDiscoveries)
        //        {
        //            Debug.Log("Daz new discoveries wync rows check " + kvp.Key);
        //        }
        //        //WorldInventory worldInv = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
        //        //bool flag1 = false;
        //        //foreach (Tag pinnedResource in worldInv.pinnedResources)
        //        //{
        //        //    if (!___rows.ContainsKey(pinnedResource))
        //        //    {
        //        //        flag1 = true;
        //        //        break;
        //        //    }
        //        //}
        //        //Debug.Log("Daz SyncRows Logging 1 " + flag1);
        //        //flag1 = false;
        //        //foreach (KeyValuePair<Tag, float> newDiscovery in DiscoveredResources.Instance.newDiscoveries)
        //        //{
        //        //    if (!___rows.ContainsKey(newDiscovery.Key) && __instance.IsDisplayedTag(newDiscovery.Key))
        //        //    {
        //        //        flag = true;
        //        //        break;
        //        //    }
        //        //}
        //        return true;
        //    }
        //}
        [HarmonyPatch(typeof(PinnedResourcesPanel))]
        [HarmonyPatch("SortRows")]
        public class PinnedRscPanelPatchSortRows
        {
            //primary worker method of the mod, this is called every 1000ms by Klei, note total replacement means any other mod touching this method will need compatibility probably
            public static bool Prefix(PinnedResourcesPanel __instance, Dictionary<Tag, GameObject> ___rows)
            {
                //as of version 1.2, clobber this entire method as I need to move all .SetLastSibling calls to my mod 
                //note that all other mods MUST have their .SetlastSibling calls Harmony.Unpatched!!!
                DazStatics.SortRscList(___rows); //still use method as trigger, passing base game items for processing, entries in list from mods are not passed here
                Debug.Log("Daz sortrows " + ClusterManager.Instance.activeWorldId);

                //in theory a transpiler replace the one line needed should be used, i lack the skills to do so, copy-paste entire method as a workaround
                //start Klei code
                //List<Tag> tagList = new List<Tag>(); //all items in ___rows
                //foreach (KeyValuePair<Tag, GameObject> row in ___rows)
                //{
                //    //Debug.Log("daz sortrows tag list start " + row.Key.Name);
                //    tagList.Add(row.Key);
                //}
                //end Klei code
                //below line is all i need to replace
                //tagList.Sort((Comparison<Tag>)((a, b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink())));
                //replace with my code, want to make this a transpiler?
                //List<Tag> RetList = DazStatics.SortRscList(___rows); //need to modify order of tagList obejct without changing object name so below code doesn't need modification.
                //RetList; //can't pass a list by ref or out? workaround with this
                //resume Klei code, using RetList instead of taglist
                //foreach (Tag key in RetList)
                //{
                //    ___rows[key].transform.SetAsLastSibling();
                //}

                //new code below so manual reordering function works
                //if (DazStatics.ListRefreshRequired && !DazStatics.ListRefreshComplete) //i can not get LayerRebuilder.MarkLayoutForRebuild to work, so add and then remove a dummy game object to force list refresh
                //                                                                       //this IF statement runs over 2 consecutive list updates
                //{
                //    Transform placeHolder = PinnedResourcesPanel.Instance.seeAllButton.transform.parent.Find("DazPlaceHolder"); //does our dummy gameobject already exist?
                //    GameObject placeHoldergo;
                //    if (placeHolder == null)
                //    {
                //        //make dummy game object
                //        placeHoldergo = new GameObject("DazPlaceHolder", typeof(RectTransform), typeof(KLayoutElement));
                //        placeHoldergo.transform.SetParent(PinnedResourcesPanel.Instance.seeAllButton.transform.parent, false);
                //    }
                //    else
                //    {
                //        //dummy object exists, assign
                //        placeHoldergo = placeHolder.gameObject;
                //    }
                //    placeHoldergo.SetActive(true); //first step, make dummy object active so it's "in" the list, despite the player not seeing it
                //    placeHoldergo.transform.SetAsLastSibling(); //add to group, this forces list refresh
                //    DazStatics.ListRefreshComplete = true; //done, set true so else if below runs next List update
                //}
                //else if (DazStatics.ListRefreshComplete)
                //{
                //    //dummy obejct has done it's job, disable it until next sort action
                //    PinnedResourcesPanel.Instance.seeAllButton.transform.parent.Find("DazPlaceHolder").gameObject.SetActive(false); //have to find dummy object again, but we know it exists for sure
                //    DazStatics.ListRefreshComplete = false; //reset refresh state so it can be called again
                //    DazStatics.ListRefreshRequired = false;
                //}
                //end of method, bypass original method with next line
                return false;
            }
        }
    }
}
