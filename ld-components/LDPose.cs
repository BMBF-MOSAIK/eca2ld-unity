using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.eca2ld_unity.ld_components
{
    public class LDPose : LDComponent
    {
        private LDVector _position = new LDVector();
        private Queue<Action> pendingPositionUpdates = new Queue<Action>();

        private LDQuat _orientation = new LDQuat();
        private Queue<Action> pendingOrientationUpdates = new Queue<Action>();

        [IsLD]
        public LDVector Position
        {
            get { return _position; }
            set
            {
                pendingPositionUpdates.Enqueue(() =>
                    gameObject.transform.position = new Vector3((float)value.x, (float)value.y, (float)value.z));
            }
        }

        [IsLD]
        public LDQuat Orientation
        {
            get { return _orientation; }
            set
            {
                pendingOrientationUpdates.Enqueue(() =>
                    gameObject.transform.rotation = new Quaternion((float)value.x, (float)value.y, (float)value.z, (float)value.w));
            }
        }

        public void Update()
        {
            if (pendingPositionUpdates.Count > 0)
                pendingPositionUpdates.Dequeue().Invoke();

            if (pendingOrientationUpdates.Count > 0)
                pendingOrientationUpdates.Dequeue().Invoke();

            _position.Set(gameObject.transform.position);
            _orientation.Set(gameObject.transform.rotation);
        }
    }
}
