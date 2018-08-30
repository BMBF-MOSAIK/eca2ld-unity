using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VDS.RDF;

namespace Assets.Scripts.ECA2LD
{
    public class EntityLDPGraph : LDPGraph
    {
        private ILiteralNode n_e;
        private GameObject gameObject;

        public EntityLDPGraph(Uri u, GameObject o) : base(u)
        {
            this.gameObject = o;
            n_e = RDFGraph.CreateLiteralNode(o.GetComponent<LDEntity>().EntityName.ToString(), "xsd:string");
            BuildRDFGraph();
        }

        protected override void BuildRDFGraph()
        {
            RDFGraph.Assert(new Triple(un, RDF_TYPE, LDP_DIRECT_CONTAINER));
            RDFGraph.Assert(new Triple(un, DCT_IDENTIFIER, n_e));
            RDFGraph.Assert(new Triple(un, LDP_HASMEMBERRELATION, DCT_HAS_PART));
            addComponentNodes();
        }

        private void addComponentNodes()
        {
            var components = gameObject.GetComponents(typeof(Component));
            foreach (Component c in components)
            {
                var componentUri = new Uri(dp_uri.TrimEnd('/') + "/" + c.GetType() + "/");
                RDFGraph.Assert(new Triple(un, DCT_HAS_PART, RDFGraph.CreateUriNode(componentUri)));
            }
        }
    }
}
