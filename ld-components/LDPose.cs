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
        public Vector3 Position;

        [IsLD]
        public Quaternion Orientation;

        public void Update()
        {
            Position = gameObject.transform.position;
            Orientation = gameObject.transform.rotation;
        }
    }
}
