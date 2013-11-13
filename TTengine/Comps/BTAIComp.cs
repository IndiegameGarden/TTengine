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

        internal HashSet<Comp> CompsToEnable = new HashSet<Comp>();
        internal HashSet<Comp> CompsToDisable = new HashSet<Comp>();

        /// <summary>The root of the Behavior Tree, defined using a slightly modified TreeSharp framework. See 'TreeSharp'
        /// namespace classes, and online documentation http://bit.ly/18ihNDz </summary>
        public TreeNode rootNode;

        /// <summary>To let an AI (BT node) indicate a required Comp to enable in this Entity</summary>
        /// <param name="comp">The component that should be enabled, as a result of a BTAI decision</param>
        public void EnableComp(Comp comp)
        {
            CompsToEnable.Add(comp);
        }

        /// <summary>To let an AI (BT node) indicate a non-required Comp for this Entity</summary>
        /// <param name="comp">The component that should be disabled, as a result of a BTAI decision, BUT ONLY
        /// if there are no other AI states active that would require this Component.</param>
        public void DisableComp(Comp comp)
        {
            CompsToDisable.Add(comp);
        }

    }
}
