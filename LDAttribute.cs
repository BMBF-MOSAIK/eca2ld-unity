using Assets.Scripts.eca2ld_unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ECA2LD
{
    public class LDAttribute
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public object Value
        {
            get
            {
                bool isProperty = ParentComponent.GetType().GetProperty(Name) != null;
                return isProperty ? ParentComponent.GetType().GetProperty(Name).GetValue(ParentComponent)
                    : ParentComponent.GetType().GetField(Name).GetValue(ParentComponent);
            }
            set
            {
                try
                {
                    bool isProperty = ParentComponent.GetType().GetProperty(Name) != null;
                    if(isProperty)
                        ParentComponent.GetType().GetProperty(Name).SetValue(ParentComponent, value);
                    else
                        ParentComponent.GetType().GetField(Name).SetValue(ParentComponent, value);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public LDComponent ParentComponent { get; private set; }

        private LDPValue valueDatapoint;

        public LDAttribute(LDComponent parentComponent, string name, Type type, object value)
        {
            ParentComponent = parentComponent;
            Name = name;
            Type = type;
            Value = value;
            valueDatapoint = new LDPValue(parentComponent.gameObject, this);
        }

        public void Shutdown()
        {
            valueDatapoint.Shutdown();
        }
    }
}
