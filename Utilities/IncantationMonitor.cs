using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities.Generic;

namespace ESnail.Utilities
{
    public delegate void IncantationReport(IIncantation tInc);

    public interface IIncantation
    {
        event IncantationReport IncantationCantillatedReport;

        Boolean Available
        {
            get;
        }

        String ToString();
    }

    partial class TIncantationMonitor<TDataType>
    {
        public class Incantation : IIncantation, ISafeID
        {
            private SafeID m_tID = null;
            private Boolean m_bAvailable = false;
            TIncantationMonitor<TDataType> m_Parent = null;
            private Int32 m_PeekIndex = 0;

            protected Boolean GetNext(ref TDataType tItem)
            {
                if ((null == m_Parent) || (!m_bAvailable))
                {
                    return false;
                }

                return m_Parent.Peek(ref tItem,m_PeekIndex++);
            }

            protected void BeginGetData()
            {
                m_PeekIndex = 0;
            }

            protected virtual Boolean Initialized
            {
                get { return true; }
            }

            internal Int32 PeekCount
            {
                get { return m_PeekIndex; }
            }

            public SafeID ID
            {
                get
                {
                    return m_tID;
                }
                set { }
            }

            //! \brief available
            public Boolean Available
            {
                get { return m_bAvailable && Initialized; }
            }


            public Incantation(TIncantationMonitor<TDataType> tParent)
            {
                if (null == tParent)
                {
                    return;
                }

                m_Parent = tParent;

                m_bAvailable = true;
            }

            public TIncantationMonitor<TDataType> Parent
            {
                get { return m_Parent; }
            }

            public event IncantationReport IncantationCantillatedReport;

            //! \brief raising event
            internal void OnIncantationCantillated(TIncantationMonitor<TDataType>.Incantation tInc)
            {
                if (null != IncantationCantillatedReport)
                {
                    try
                    {
                        IncantationCantillatedReport.Invoke(tInc);
                    }
                    catch (Exception) { }
                }
            }

            public virtual Int32 Parse()
            {
                return -1;
            }

        }
    }

    //! \name incantation monitor
    //! @{
    public abstract partial class TIncantationMonitor<TDataType> : ESDisposableClass
    {
        List<TDataType> m_queueWords = new List<TDataType>();
        TSet<TIncantationMonitor<TDataType>.Incantation> m_IncantationSet = new TSet<TIncantationMonitor<TDataType>.Incantation>();

        private Boolean Peek(ref TDataType tItem, Int32 tIndex)
        {
            lock (((ICollection)m_queueWords).SyncRoot)
            {
                if (m_queueWords.Count == 0)
                {
                    return false;
                }
                else if (m_queueWords.Count <= tIndex)
                {
                    return false;
                }

                tItem = m_queueWords[tIndex];
            }

            return true;
        }

        //! \brief add item to queue
        public Boolean Append(TDataType tItem)
        {
            if (null == tItem)
            {
                return false;
            }

            lock (((ICollection)m_queueWords).SyncRoot)
            {
                m_queueWords.Add(tItem);
            }

            //! refresh
            Refresh();

            return true;
        }


        public Boolean AppendRange(TDataType[] tItems)
        {
            if (null == tItems)
            {
                return false;
            }
            if (0 == tItems.Length)
            {
                return true;
            }

            lock (((ICollection)m_queueWords).SyncRoot)
            {
                m_queueWords.AddRange(tItems);
            }

            //! refresh
            Refresh();

            return true;
        }

        public delegate void IncantationWordHandler(TDataType tItem);

        public event IncantationWordHandler WordDroppedEvent;

        private void OnItemDropped(TDataType tItem)
        {
            if (null != WordDroppedEvent)
            {
                try
                {
                    WordDroppedEvent.Invoke(tItem);
                }
                catch (Exception) { }
            }
        }

        public virtual IIncantation NewIncantation()
        {
            return new TIncantationMonitor<TDataType>.Incantation(this);
        }

        public Boolean AddIncantation(IIncantation tInc)
        {
            TIncantationMonitor<TDataType>.Incantation tIncantationItem = tInc as TIncantationMonitor<TDataType>.Incantation;
            if (null == tIncantationItem)
            {
                return false;
            }

            switch (m_IncantationSet.Add(tIncantationItem))
            { 
                case SET_ADD_RESULT.SET_FAILED:
                    return false;
                case SET_ADD_RESULT.SET_OBJECT_EXIST:
                case SET_ADD_RESULT.SET_OK:
                    break;
            }

            return true;
        }

        

        public TIncantationMonitor<TDataType>.Incantation[] Incantations
        {
            get { return m_IncantationSet.Items; }
        }

        //! \brief clear all words
        public void Clear()
        {
            if (null != WordDroppedEvent)
            {
                lock (((ICollection)m_queueWords).SyncRoot)
                {
                    while (m_queueWords.Count > 0)
                    {
                        TDataType tItem = m_queueWords[0];
                        m_queueWords.RemoveAt(0);

                        OnItemDropped(tItem);
                    }
                }
            }
            else
            {
                lock (((ICollection)m_queueWords).SyncRoot)
                {
                    m_queueWords.Clear();
                }
            }
        }

        //! \brief reset monitor
        public void Reset()
        {
            Clear();

            //! unregister all event handler
            WordDroppedEvent = null;

            m_IncantationSet.Clear();
        }

        //! \brief refresh monitor
        public void Refresh()
        {

            do
            {
                Boolean bAllRequestDrop = true;
                Int32 nMaxDrop = 0;

                lock (((ICollection)m_queueWords).SyncRoot)
                {
                    if (m_queueWords.Count <= 0)
                    {
                        break;
                    }
                }

                foreach (TIncantationMonitor<TDataType>.Incantation tInc in m_IncantationSet.Items)
                {
                    if (!tInc.Available)
                    {
                        continue;
                    }

                    Int32 tResult = tInc.Parse();
                    if (tResult > 0)
                    {
                        nMaxDrop = tInc.PeekCount > nMaxDrop ? tInc.PeekCount : nMaxDrop;
                        bAllRequestDrop = false;

                        //! raising event
                        tInc.OnIncantationCantillated(tInc);
                    }
                    else if (0 == tResult)
                    {
                        bAllRequestDrop = false;
                    }
                }

                if (nMaxDrop > 0)
                {
                    lock (((ICollection)m_queueWords).SyncRoot)
                    {
                        if (m_queueWords.Count >= nMaxDrop)
                        {
                            m_queueWords.RemoveRange(0, nMaxDrop);
                        }
                    }
                }
                else if (bAllRequestDrop)
                {
                    lock (((ICollection)m_queueWords).SyncRoot)
                    {
                        if (m_queueWords.Count > 0)
                        {
                            OnItemDropped(m_queueWords[0]);
                            m_queueWords.RemoveAt(0);
                        }
                    }

                }
            } while (true);
        }

    }
    //! @}

    public class KeyboardIncantationMonitor : TIncantationMonitor<Keys>
    {

        public class KeysIncantation : TIncantationMonitor<Keys>.Incantation
        {
            private List<Keys> m_KeysList = new List<Keys>();
            private Boolean m_bInitialized = false;

            public KeysIncantation(KeyboardIncantationMonitor tParent)
                : base(tParent)
            { 
                
            }

            public void AddKey(Keys tKey)
            {
                lock (((ICollection)m_KeysList).SyncRoot)
                {
                    m_KeysList.Add(tKey);

                    m_bInitialized = true;
                }
            }

            public void AddRange(Keys[] tKeys)
            {
                lock (((ICollection)m_KeysList).SyncRoot)
                {
                    m_KeysList.AddRange(tKeys);

                    m_bInitialized = true;
                }
            }

            public void AddRange(List<Keys> tKeys)
            {
                lock (((ICollection)m_KeysList).SyncRoot)
                {
                    m_KeysList.AddRange(tKeys);

                    m_bInitialized = true;
                }
            }

            protected override Boolean Initialized
            {
                get
                {
                    return m_bInitialized;
                }
            }

            public override int Parse()
            {
                BeginGetData();

                lock (((ICollection)m_KeysList).SyncRoot)
                {
                    if (0 == m_KeysList.Count)
                    {
                        m_bInitialized = false;
                        return 0;
                    }

                    foreach (Keys tKey in m_KeysList)
                    {
                        Keys tKeyItem = Keys.None;

                        if (GetNext(ref tKeyItem))
                        {
                            if (tKey != tKeyItem)
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }

                    //! success
                    return m_KeysList.Count;
                }
            }

            public Keys[] KeyStream
            {
                get { return m_KeysList.ToArray(); }
            }

            public override String ToString()
            {
                StringBuilder sbKeys = new StringBuilder();
                lock (((ICollection)m_KeysList).SyncRoot)
                {
                    if (0 == m_KeysList.Count)
                    {
                        return "<NO KEY>";
                    }
                

                    foreach (Keys tKey in m_KeysList)
                    {
                        sbKeys.Append('<');
                        sbKeys.Append(tKey.ToString());
                        sbKeys.Append('>');
                    }
                }

                return sbKeys.ToString();
            }
        }

        public override IIncantation NewIncantation()
        {
            return new KeysIncantation(this);
        }

        protected override void _Dispose()
        {
            
        }
    }


    
}
