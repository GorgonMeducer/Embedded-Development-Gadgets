using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
//using System.Collections.Specialized;

namespace ESnail.Utilities.Generic
{

    //! \name the result of adding set
    //! @{
    public enum SET_ADD_RESULT
    {
        SET_OK,                     //!< ok
        SET_OBJECT_EXIST,           //!< target with the same ID already exist
        SET_FAILED                  //!< failed
    }
    //! @}

    //! \name generic set which is thread safe
    //! @{
    public class TSet<TObject> : /*INotifyCollectionChanged,*/ IEnumerable
        where TObject : ISafeID
    {
        SortedList<SafeID, TObject> m_ObjectList = new SortedList<SafeID, TObject>();

        public void AddRange(IEnumerable<TObject> tTargets)
        {
            if (null == tTargets)
            {
                return;
            }

            foreach (TObject tItem in tTargets)
            {
                Add(tItem);
            }
        }

        //! add object to set
        public virtual SET_ADD_RESULT Add(TObject tTarget)
        {
            if (null == tTarget)
            {
                return SET_ADD_RESULT.SET_FAILED;
            }

            if (null != Find(tTarget.ID))
            {
                return SET_ADD_RESULT.SET_OBJECT_EXIST;
            }
            lock (((ICollection)m_ObjectList).SyncRoot)
            {
                m_ObjectList.Add(tTarget.ID, tTarget);
                //OnNotifyCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,tTarget));
            }

            

            return SET_ADD_RESULT.SET_OK;
        }

        //! remove object from set
        public Boolean Remove(SafeID tID)
        {
#if false
            TObject tTarget = Find(tID);
            if (null == tTarget)
            {
                return false;
            }
#endif
            lock (((ICollection)m_ObjectList).SyncRoot)
            {
                m_ObjectList.Remove(tID);
                //OnNotifyCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,tTarget));
            }
            return true;
        }

        public void RemoveBefore(SafeID tID)
        {
            Int32 tEndIndex = m_ObjectList.Keys.IndexOf(tID);
            if (-1 == tEndIndex)
            {
                return;
            }

            for (Int32 tIndex = 0; tIndex < tEndIndex; tIndex++)
            {
                m_ObjectList.RemoveAt(0);
            }
        }

        public void RemoveAfter(SafeID tID)
        {
            Int32 tStartIndex = m_ObjectList.Keys.IndexOf(tID) + 1;
            Int32 tLength = m_ObjectList.Count - tStartIndex;
            if (0 == tStartIndex)
            {
                return;
            }

            for (Int32 n = 0; n < tLength; n++)
            {
                m_ObjectList.RemoveAt(tStartIndex);
            }
        }

        //! clear all objects from set
        public virtual void Clear()
        {
            lock (((ICollection)m_ObjectList).SyncRoot)
            {
                m_ObjectList.Clear();
                //OnNotifyCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        //! find object
        public TObject Find(SafeID tID)
        {
            lock (((ICollection)m_ObjectList).SyncRoot)
            {
                int tIndex = m_ObjectList.Keys.IndexOf(tID);
                if (tIndex >= 0)
                {
                    return m_ObjectList.Values[tIndex];
                }
            }

            return default(TObject);
        }

        //! get all items
        public TObject[] Items
        {
            get { return this.ToArray(); }
        }

        //! indexer
        public TObject this[SafeID tID]
        {
            get { return Find(tID); }
        }

        public virtual TObject this[Int32 tIndex]
        {
            get { return m_ObjectList.Values[tIndex]; }
        }

        //! implement IEnumerable interface
        public IEnumerator GetEnumerator()
        {
            return m_ObjectList.GetEnumerator();
        }

        public TObject[] ToArray()
        {
            List<TObject> tTempList = new List<TObject>();
            tTempList.AddRange(m_ObjectList.Values);

            return tTempList.ToArray();
        }

        public Int32 Count
        {
            get { return m_ObjectList.Count; }
        }
        /*
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnNotifyCollectionChangedEvent(NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (null != CollectionChanged)
                {
                    CollectionChanged.Invoke(this,e);
                }
            }
            catch (Exception)
            { 
            
            }
        }
        */
    }
    //! @}
}