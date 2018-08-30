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

    public LDPEntity(GameObject gameObject, string hostAddress, int hostPort) : base(gameObject)
    {
        uri = "http://" + hostAddress + ":" + hostPort + "/" + Name + "/";
        LDPGraph = new EntityLDPGraph(new System.Uri(uri), gameObject);
        initializeHttpListener(uri);
        CreateComponentEndpoints();
    }

    public override void Shutdown()
    {
        foreach (LDPComponent c in ComponentEndpoints)
        {
            c.Shutdown();
        }

        base.Shutdown();
    }

    private void CreateComponentEndpoints()
    {
        ComponentEndpoints = new List<LDPComponent>();
        foreach (Component c in gameObject.GetComponents(typeof(Component)))
        {
            ComponentEndpoints.Add(new LDPComponent(gameObject, c));
        }
    }

}
