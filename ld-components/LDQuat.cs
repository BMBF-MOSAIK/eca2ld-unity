using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.eca2ld_unity.ld_components
{
    public class LDQuat
    {
        public double x;
        public double y;
        public double z;
        public double w;

        public void Set(UnityEngine.Quaternion from)
        {
            x = from.x;
            y = from.y;
            z = from.z;
            w = from.w;
        }
    }
}
