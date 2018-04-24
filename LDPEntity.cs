using Assets.Scripts.ECA2LD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VDS.RDF;

public class LDPEntity : LinkedDataPoint
{

    private List<LDPComponent> ComponentEndpoints;

    new void Start()
    {
        base.Start();
        LDPGraph = new EntityLDPGraph(new System.Uri(uri), gameObject);
        CreateComponentEndpoints();
    }

    protected override void BuildUri()
    {
        uri = "http://localhost:12345/test/" + Name + "/";
    }

    private void CreateComponentEndpoints()
    {
        foreach (Component c in gameObject.GetComponents(typeof(Component)))
        {
        }
    }

}
