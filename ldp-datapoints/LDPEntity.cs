using Assets.Scripts.ECA2LD;
using Assets.Scripts.eca2ld_unity;
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
        Uri = "http://" + hostAddress + ":" + hostPort + "/" + Name + "/";
        LDPGraph = new EntityLDPGraph(new Uri(Uri), gameObject);
        initializeHttpListener(Uri);
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
            if(c is LDComponent)
                ComponentEndpoints.Add(new LDPComponent(gameObject, c));
        }
    }

}
