using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities.Generic
{
    //! \name generic Binary Tree
    //! @{
    public class TTree<TItem> 
    {
        TTree<TItem> m_LeftTree = null;
        TTree<TItem> m_RightTree = null;

        //! property for node data
        public TItem NodeData 
        { 
            get; 
            set; 
        }

        //! property for left sub-tree
        public TTree<TItem> LeftTree
        {
            get { return m_LeftTree; }
            set
            {
                if (null == value)
                {
                    if (null != m_LeftTree)
                    {
                        m_LeftTree.Parent = null;
                    }
                }
                m_LeftTree = value;
                if (null != value)
                {
                    value.Parent = this;
                }
            }
        }

        //! property for right sub-tree
        public TTree<TItem> RightTree
        {
            get {return m_RightTree;}
            set
            {
                if (null == value)
                {
                    if (null != m_RightTree)
                    {
                        m_RightTree.Parent = null;
                    }
                }
                m_RightTree = value;
                if (null != value)
                {
                    value.Parent = this;
                }
            }
        }

        public TTree<TItem> Parent
        {
            get;
            set;
        }


        //! \brief default constructor
        public TTree(TItem nodeValue)
        {
            this.NodeData = nodeValue;
            this.LeftTree = null;
            this.RightTree = null;
        }

        //! \brief constructor with sub-tree
        public TTree(TItem nodeValue, TTree<TItem> LeftTree, TTree<TItem> RightTree)
        {
            this.NodeData = nodeValue;
            this.LeftTree = LeftTree;
            this.RightTree = RightTree;
        }

    }
    //! @}
}
