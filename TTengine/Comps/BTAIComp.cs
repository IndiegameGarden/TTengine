using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis.Interface;
using TTengine.Core;
using TreeSharp;

namespace TTengine.Comps
{
    /// <summary>
    /// Behavior Tree (BT) AI component that specifies which BT AI behaviors are enabled for the Entity
    /// </summary>
    public class BTAIComp: IComponent
    {
        public BTAIComp()
        {
        }

        /// <summary>The root of the Behavior Tree, defined using a slightly modified TreeSharp framework. See 'TreeSharp'
        /// namespace classes, and online documentation http://bit.ly/18ihNDz </summary>
        public TreeNode rootNode;
    }
}
