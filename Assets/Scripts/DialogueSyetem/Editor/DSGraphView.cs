using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddGridBackground();
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            
            gridBackground.StretchToParentSize();
            
            Insert(0, gridBackground);
        }
        
    }
}

