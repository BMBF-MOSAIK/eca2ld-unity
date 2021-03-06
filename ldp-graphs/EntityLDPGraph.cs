﻿using Assets.Scripts.eca2ld_unity;
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
        private Component[] components;

        public EntityLDPGraph(Uri u, GameObject o) : base(u)
        {
            this.gameObject = o;
            n_e = RDFGraph.CreateLiteralNode(o.GetComponent<LDEntity>().EntityName.ToString(), new Uri("xsd:string"));
            components = gameObject.GetComponents(typeof(Component));
            BuildRDFGraph();
        }

        public override void BuildRDFGraph()
        {
            RDFGraph.Clear();
            RDFGraph.Assert(new Triple(un, RDF_TYPE, LDP_DIRECT_CONTAINER));
            RDFGraph.Assert(new Triple(un, DCT_IDENTIFIER, n_e));
            RDFGraph.Assert(new Triple(un, LDP_HASMEMBERRELATION, DCT_HAS_PART));
            addComponentNodes();
        }

        private void addComponentNodes()
        {
            foreach (Component c in components)
            {
                if (c is LDComponent)
                {
                    var componentUri = new Uri(dp_uri.TrimEnd('/') + "/" + c.GetType() + "/");
                    RDFGraph.Assert(new Triple(un, DCT_HAS_PART, RDFGraph.CreateUriNode(componentUri)));
                }
            }
        }
    }
}
