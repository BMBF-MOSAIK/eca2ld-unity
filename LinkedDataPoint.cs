using Assets.Scripts.ECA2LD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VDS.RDF;

public abstract class LinkedDataPoint
{
    protected GameObject gameObject;
    protected HttpListener Endpoint;
    protected TTLSerializer serializer;
    protected LDPGraph LDPGraph;
    protected string uri;

    public string Name;

    // Use this for initialization
    protected LinkedDataPoint(GameObject gameObject)
    {
        this.gameObject = gameObject;
        Name = gameObject.GetComponent<LDEntity>().EntityName;
        serializer = gameObject.GetComponent<TTLSerializer>();
    }

    protected void initializeHttpListener(string uri)
    {
        this.uri = uri;
        Endpoint = new HttpListener();
        Endpoint.Prefixes.Add(uri);
        Endpoint.Start();
        Task.Factory.StartNew(listen);
    }

    protected void listen()
    {
        while (!Endpoint.IsListening)
            Thread.Sleep(10);

        while (Endpoint.IsListening)
        {
            if (serializer != null)
            {
                var c = Endpoint.GetContext();
                serializer.SerializeTTL(LDPGraph, c);
            }
        }
    }
}
