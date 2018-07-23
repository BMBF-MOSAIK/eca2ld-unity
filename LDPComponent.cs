using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ECA2LD
{
    class LDPComponent : LinkedDataPoint
    {
        private Component c;

        public LDPComponent(GameObject gameObject, Component c) : base(gameObject)
        {
            this.c = c;
            Name = c.GetType().ToString();
            string uri = "http://localhost:12345/test/" + gameObject.GetComponent<LDEntity>().EntityName + "/" + c.GetType() + "/";
            LDPGraph = new ComponentLDPGraph(new Uri(uri), c);
            initializeHttpListener(uri);
        }

    }
}
