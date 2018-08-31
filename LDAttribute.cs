using Assets.Scripts.eca2ld_unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ECA2LD
{
    public class LDAttribute
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public LDComponent ParentComponent { get; private set; }

        private LDPValue valueDatapoint;

        public LDAttribute(LDComponent parentComponent, string name, object value)
        {
            ParentComponent = parentComponent;
            Name = name;
            Value = value;
            valueDatapoint = new LDPValue(parentComponent.gameObject, this);
        }

        public void Shutdown()
        {
            valueDatapoint.Shutdown();
        }
    }
}
