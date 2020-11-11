using Assets.Scripts.eca2ld_unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ECA2LD
{
    class LDPAttribute : LinkedDataPoint
    {
        private LDAttribute a;

        public LDPAttribute(GameObject gameObject, LDAttribute a) : base(gameObject)
        {
            this.a = a;
            Name = a.Name;
            Uri = gameObject.GetComponent<LDEntity>().Uri + a.ParentComponent.GetType() + "/" + a.Name + "/";
            LDPGraph = new AttributeLDPGraph(new Uri(Uri), a);
            initializeHttpListener(Uri);
        }

        public override void Shutdown()
        {
            a.Shutdown();
            base.Shutdown();
        }

    }
}
