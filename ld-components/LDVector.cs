using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.eca2ld_unity.ld_components
{
    public class LDVector
    {
        public double x;
        public double y;
        public double z;

        public void Set(UnityEngine.Vector3 from)
        {
            x = from.x;
            y = from.y;
            z = from.z;
        }
    }
}
