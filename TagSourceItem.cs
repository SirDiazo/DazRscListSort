using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazRscListSort
{
    public class TagSourceItem
    {
        public Tag tag;
        public string source;
        //"base" is base game
        //"ciwild" is Critter Inventory, Wild
        //"citame" is Critter Inventory, Tame
        //"ciart" is Critter Inventory, Artificial

        public TagSourceItem(Tag tg, string src)
        {
            tag = tg;
            source = src;
        }

        public static bool operator ==(TagSourceItem lhs, TagSourceItem rhs)
        {
            if (lhs is null || rhs is null)
            {
                return false;
            }
            else if (lhs.tag == rhs.tag && lhs.source == rhs.source)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(TagSourceItem lhs, TagSourceItem rhs)
        {
            if (lhs is null || rhs is null)
            {
                return true;
            }
            if (lhs.tag == rhs.tag && lhs.source == rhs.source)
            {
                return false;
            }
            return true;
        }
        public bool Equals(TagSourceItem other)
        {
            if (other is null)
            {
                return false;
            }
            else if (tag == other.tag && source == other.source)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Equals(Tag tg) //no null check, visual studio chokes on it
        {
            if (tag == tg)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object o)
        {
            TagSourceItem obj = o as TagSourceItem;
            return Equals(obj);
        }
        public override int GetHashCode()
        {
            return (tag.GetHashCode() << 2) ^ source.GetHashCode();
        }
    }
}
