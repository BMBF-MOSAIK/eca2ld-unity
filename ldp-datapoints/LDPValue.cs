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

public class LDPValue : LinkedDataPoint
{

    private LDAttribute a;

    public LDPValue(GameObject gameObject, LDAttribute a) : base(gameObject)
    {
        var e = gameObject.GetComponent<LDEntity>();
        Uri = "http://" +
            e.HostAddress + ":" +
            e.HostPort + "/" +
            e.EntityName + "/" +
            a.ParentComponent.GetType() + "/" +
            a.Name + "/value/";
        LDPGraph = new ValueGraph(new Uri(Uri), a);
        initializeHttpListener(Uri);
    }
}
