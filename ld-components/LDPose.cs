using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.eca2ld_unity.ld_components
{
    public class LDPose : LDComponent
    {
        [IsLD]
        public LDVector Position = new LDVector();

        [IsLD]
        public LDQuat Orientation = new LDQuat();

        public void Update()
        {
            Position.Set(gameObject.transform.position);
            Orientation.Set(gameObject.transform.rotation);
        }
    }
}
