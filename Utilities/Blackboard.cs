using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using ESnail.Utilities.IO;
using ESnail.Utilities.Generic;

namespace ESnail.Utilities
{
    //! \name blackboard
    //! @{
    static public class Blackboard
    {
        public class Namespace : ISafeID , ICollection 
        {
            private List<Slip> m_SlipList = new List<Slip>();
            private SafeID m_Name = "Ltd";

            public Namespace(String tName)
            {
                if (null != tName)
                {
                    if ("" != tName.Trim())
                    {
                        m_Name = tName;
                    }
                }
            }

            public SafeID Name
            {
                get { return m_Name; }
            }

            public SafeID ID
            {
                get
                {
                    return m_Name;
                }
                set{}
            }

            public Boolean Add(Slip tSlip)
            {
                if (null == tSlip)
                {
                    return false;
                }
                lock (((ICollection)m_SlipList).SyncRoot)
                {
                    m_SlipList.Add(tSlip);
                }
                return true;
            }

            public Boolean Add(SafeID tTag, params Object[] tObjs)
            {
                lock (((ICollection)m_SlipList).SyncRoot)
                {
                    m_SlipList.Add(new Slip(tTag,tObjs));
                }
                return true;
            }

            public void Remove(Slip tSlip)
            {
                if (null == tSlip)
                {
                    return;
                }

                lock (((ICollection)m_SlipList).SyncRoot)
                {
                    Int32 tIndex = m_SlipList.IndexOf(tSlip);
                    if (-1 != tIndex)
                    {
                        m_SlipList.Remove(m_SlipList[tIndex]);
                    }
                }
            }

            public void Clear()
            {
                lock (((ICollection)m_SlipList).SyncRoot)
                {
                    m_SlipList.Clear();
                }
            }

            public void RemoveAll(SafeID tTag)
            {
                lock (((ICollection)m_SlipList).SyncRoot)
                {
                    foreach (Slip tItem in m_SlipList)
                    {
                        if (tItem.Tag == tTag)
                        {
                            m_SlipList.Remove(tItem);
                        }
                    }
                }
            }

            public Slip[] FindAll(SafeID tTag)
            {
                List<Slip> tResult = new List<Slip>();

                lock (((ICollection)m_SlipList).SyncRoot)
                {
                    foreach (Slip tItem in m_SlipList)
                    {
                        if (tItem.Tag == tTag)
                        {
                            tResult.Add(tItem);
                        }
                    }
                }

                return tResult.ToArray();
            }

            public Slip Find(SafeID tTag)
            {
                lock (((ICollection)m_SlipList).SyncRoot)
                {
                    foreach (Slip tItem in m_SlipList)
                    {
                        if (tItem.Tag == tTag)
                        {
                            return tItem;
                        }
                    }
                }
                return null;
            }

            public void CopyTo(Array array, Int32 index)
            {
                if (array is Slip[])
                {
                    lock (((ICollection)m_SlipList).SyncRoot)
                    {
                        m_SlipList.CopyTo((Slip[])array, index);
                    }
                }
            }

            public Int32 Count
            {
                get { return m_SlipList.Count; }
            }

            public Boolean IsSynchronized
            {
                get { return ((ICollection)m_SlipList).IsSynchronized; }
            }

            public object SyncRoot
            {
                get { return ((ICollection)m_SlipList).SyncRoot; }
            }


            public IEnumerator GetEnumerator()
            {
                return m_SlipList.GetEnumerator();
            }
        }

        public class Slip
        {
            private SafeID m_Tag;
            private List<Object> m_Objects = new List<Object>();

            public Slip(SafeID tTag, params Object[] tObjs)
            {
                m_Tag = tTag;
                if (null != tObjs)
                {
                    m_Objects.AddRange(tObjs);
                }
                
            }

            public SafeID Tag
            {
                get { return m_Tag; }
            }

            public Object[] Contents
            {
                get { return m_Objects.ToArray(); }
            }
        }

        static TSet<Namespace> s_NameSpaceSet = new TSet<Namespace>();

        public static void AddNameSpace(String tID)
        {
            s_NameSpaceSet.Add(new Namespace(tID));
        }

        public static Namespace NameSpaces(String tID)
        {
            return s_NameSpaceSet[tID];
        }

        public static void Remove(String tID)
        {
            s_NameSpaceSet.Remove(tID);
        }

        public static Namespace Find(String tID)
        {
            return s_NameSpaceSet.Find(tID);
        }

        public static Object GetSingleObject(String tPath)
        {
            String[] tPaths = PathEx.Separate(tPath);
            if (null == tPaths)
            {
                return null;
            }
            else if (0 == tPaths.Length)
            {
                return null;
            }
            else if (1 == tPaths.Length)
            {
                return Find(tPaths[0]);
            }

            Namespace tNamespace = Find(tPaths[0]);
            if (null == tNamespace)
            {
                return null;
            }

            Slip tSlip = tNamespace.Find(tPaths[1]);
            if (null == tSlip.Contents)
            {
                return null;
            }
            else if (0 == tSlip.Contents.Length)
            {
                return null;
            }

            return tSlip.Contents[0];
        }

    }
    //! @}
}
