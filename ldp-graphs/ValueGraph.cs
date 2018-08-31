using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ECA2LD
{
    public class ValueGraph : LDPGraph
    {
        public LDAttribute a { get; private set; }
        public ValueGraph(Uri u, LDAttribute a) : base(u)
        {
            this.a = a;
        }

        protected override void BuildRDFGraph()
        {
            // nothing to do here, as Graph will just work as accessor to attribute, which is admittedly not very beautiful,
            // but helps to cope with Unity Threading behaviour, as by this, we can just re-use the HttpListener that we wrote
            // before.
        }
    }
}
