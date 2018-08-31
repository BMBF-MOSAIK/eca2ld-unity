using Assets.Scripts.ECA2LD;
using Assets.Scripts.eca2ld_unity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VDS.RDF;

public class LDPValue : LinkedDataPoint
{

    private LDAttribute a;

    public LDPValue(GameObject gameObject, LDAttribute a) : base(gameObject)
    {
        this.a = a;
        var e = gameObject.GetComponent<LDEntity>();
        Uri = e.Uri +
            a.ParentComponent.GetType() + "/" +
            a.Name + "/value/";
        LDPGraph = new ValueGraph(new Uri(Uri), a);
        initializeHttpListener(Uri);
    }

    protected override void OnPost(HttpListenerContext c)
    {
        serializer.DeserializeJSONRequest(c, a.Type, d => { a.Value = d; });
    }
}
