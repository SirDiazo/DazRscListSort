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

namespace DazRscListSort
{
    public class HdrBtn : Button
    {
        //Button class for the 5 header buttons to pick current list
        public int BtnNum; //which button are we? 1 to 5 left to right on screen
        public GameObject go; //our parent GameObject
        //DazRscListSort.DazRscListSortData dataStore; //master dataStorage object, updated every time button clicked to account for world changes

        public void Setup(int num, GameObject attachedGO) //called on button creation by mod, not Unity built-it
        {
            //dataStore = ClusterManager.Instance.activeWorld.GetComponent<DazRscListSort.DazRscListSortData>(); //get our datastorage object, this changes per asteroid in spaced out, move to OnClick
            BtnNum = num; //save our button number
            go = attachedGO; //save our gameObject
        }

        public override void OnPointerClick(PointerEventData eventData) //on click event, save data and change page
        {
            if (eventData.button == PointerEventData.InputButton.Right && BtnNum == DazStatics.currentDstore.CurrentHdrButton)
            {
                DazStatics.hdrButtonChangeInProgress = true; //disable PinnedResourcePanel.SortRows, will corrupt data if it runs during this method.
                DazStatics.currentDstore.SanitizeList(); //must run before button number update next line! 
                DazStatics.SetOverRideButtonState(BtnNum);
                DazStatics.currentDstore.UpdatePinnedResources(); //must run after button number change line above
                DazStatics.RscListRefreshHeader(); //updated header icons for new current list
                DazStatics.hdrButtonChangeInProgress = false; //reenabled PinnedResourcePanel.SortRows
            }
            else
            {
                if (BtnNum != DazStatics.currentDstore.CurrentHdrButton)
                {
                    DazStatics.hdrButtonChangeInProgress = true; //disable PinnedResourcePanel.SortRows, will corrupt data if it runs during this method.
                    DazStatics.currentDstore.SanitizeList(); //must run before button number update next line! 
                    DazStatics.currentDstore.CurrentHdrButton = BtnNum; //change the current page
                    DazStatics.currentDstore.UpdatePinnedResources(); //must run after button number change line above
                    DazStatics.RscListRefreshHeader(); //updated header icons for new current list
                    DazStatics.hdrButtonChangeInProgress = false; //reenabled PinnedResourcePanel.SortRows
                }

            }
            DazStatics.ListRefreshRequired = true;
        }

    }
    public class UpBtn : Button
    {
        //UpArrow button on resource item in list
        public Tag btnTag; //Tag of our resource type
        public string source;

        public void Setup(Tag tg, string src)
        {
            btnTag = tg; //save Tag of resource type, manually called by mod, not Unity
            source = src;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Daz up arror click " + btnTag + "?" + source);
            DazStatics.currentDstore.ReOrdertag(btnTag, source, true); //call method to move this data row up on button click
        }
    }
    public class DownBtn : Button
    {
        //DownArrow button on resource item in list
        public Tag btnTag; //Tag of our resource type
        public string source;

        public void Setup(Tag tg,string src)
        {
            btnTag = tg;
            source = src;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            DazStatics.currentDstore.ReOrdertag(btnTag, source, false); //move this resource down a row
        }
    }

    public class SortBtn : Button
    {
        //Enable sort button
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!DazStatics.ShowSortArrows)
            {
                DazStatics.ShowSortArrowsMethod();
                DazStatics.ShowSortArrows = true;
            }
            else
            {
                DazStatics.HideSortArrowsMethod();
                DazStatics.ShowSortArrows = false;
            }
        }
    }
}
