using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using ESnail.Utilities.IO;

namespace ESnail.Utilities.Generic
{
    public interface IGroupList
    {
        Boolean AddItem(Object tItem);

        void Sort();
    }

    public interface IGroup
    {
        //! for specifying the group
        String Group
        {
            get;
        }
    }

    public class GroupComparator : IComparer<IGroup>
    {
        private UInt32 m_tLevel = 3;

        public GroupComparator()
        { 
        }

        public GroupComparator(UInt32 tLevel)
        {
            if (0 != tLevel)
            {
                m_tLevel = tLevel;
            }
        }

        public int Compare(IGroup x, IGroup y)
        {
            int tResult = 0;

            String[] tGroupX = PathEx.Separate(x.Group, ':');
            String[] tGroupY = PathEx.Separate(y.Group, ':');

            do
            {
                if ((tGroupX.Length == tGroupY.Length) && (tGroupX.Length == 0))
                {
                    break;
                }

                for (Int32 tIndex = 0; tIndex < m_tLevel; tIndex++)
                {
                    Int32 tLevel = tIndex * 2;
                    if ((tGroupX.Length == tLevel) && (tGroupY.Length > tLevel))
                    {
                        tResult = -1;
                        break;
                    }
                    if ((tGroupX.Length > tLevel) && (tGroupY.Length == tLevel))
                    {
                        tResult = 1;
                        break;
                    }

                    //! compare first level
                    do
                    {
                        Int32 tX;
                        Int32 tY;

                        if (!Int32.TryParse(tGroupX[tLevel], out tX))
                        {
                            tX = 0;
                        }
                        if (!Int32.TryParse(tGroupY[tLevel], out tY))
                        {
                            tY = 0;
                        }

                        tResult = tX - tY;
                    } while (false);
                    if (tResult != 0)
                    {
                        break;
                    }
                }

            } while (false);

            return tResult;
        }
    }

    public abstract class GroupListNode<TList, TListItem, TItem> : IGroup, IGroupList
        where TList : List<TListItem>, new()
        where TListItem : class, IGroup
        where TItem : class, IGroup
    {
        protected IComparer<IGroup> m_Comparator = null;

        protected TList m_List = new TList();

        public GroupListNode()
        {
            m_Comparator = new GroupComparator();
        }

        public GroupListNode(IComparer<IGroup> tComparator)
        {
            m_Comparator = tComparator;
        }

        protected virtual Int32 Level
        {
            get { return 0; }
        }

        public void Sort()
        {
            m_List.Sort(new LocalComparator(m_Comparator));
        }

        protected String FetchGroupIndex(String tGroup)
        {
            return PathEx.Left(tGroup, (this.Level + 1) * 2);
        }

        protected TListItem Find(String tGroup)
        {
            tGroup = FetchGroupIndex(tGroup);

            foreach (TListItem tNode in m_List)
            {
                if (FetchGroupIndex(tNode.Group) == tGroup)
                {
                    return tNode;
                }
            }

            return null;
        }

        public TListItem[] Items
        {
            get { return m_List.ToArray() as TListItem[]; }
        }

        public String Group
        {
            get
            {
                if (m_List.Count == 0)
                {
                    return "";
                }
                return FetchGroupIndex(m_List[0].Group);
            }
        }

        public Boolean AddItem(Object tItem)
        {
            return this.AddItem(tItem as TItem);
        }

        public virtual Boolean AddItem(TItem tItem)
        {
            //! terminal level
            if (typeof(TListItem) == typeof(TList))
            {
                m_List.Add(tItem as TListItem);
                this.Sort();
                return true;
            }

            return false;
        }

        private class LocalComparator : IComparer<TListItem>
        {
            private IComparer<IGroup> m_Comparator = null;

            public LocalComparator(IComparer<IGroup> tComparator)
            {
                m_Comparator = tComparator;
            }

            public int Compare(TListItem x, TListItem y)
            {
                return m_Comparator.Compare(x, y);
            }
        }
    }

    public abstract class GroupList<TList, TListItem, TItem> : GroupListNode<TList, TListItem, TItem>
        where TList : List<TListItem>, new()
        where TListItem : class, IGroupList, IGroup
        where TItem : class, IGroup
    {
        public GroupList(IComparer<IGroup> tComparator)
            : base(tComparator)
        {

        }

        public GroupList()
            : base()
        {

        }

        protected abstract TListItem CreateItem();

        public override Boolean AddItem(TItem tItem)
        {
            if (base.AddItem(tItem))
            {
                return true;
            }

            TListItem tList = this.Find(tItem.Group);
            if (null != tList)
            {
                if (tList.AddItem(tItem))
                {
                    tList.Sort();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            do
            {
                tList = CreateItem();
                if (!tList.AddItem(tItem))
                {
                    return false;
                }
                m_List.Add(tList);
                this.Sort();

                return true;
            } while (false);

            return false;
        }
    }

}