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
    protected bool isActive = true;

    public string Name;

    public virtual void Shutdown()
    {
        isActive = false;
    }

    // Use this for initialization
    protected LinkedDataPoint(GameObject gameObject)
    {
        this.gameObject = gameObject;
        Name = gameObject.GetComponent<LDEntity>().EntityName;
        serializer = gameObject.GetComponent<TTLSerializer>();
    }

    protected void initializeHttpListener(string uri)
    {
        Debug.Log("Initializing Http Listener");
        this.uri = uri;
        Endpoint = new HttpListener();
        Endpoint.Prefixes.Add(uri);
        Endpoint.Start();
        Task.Factory.StartNew(listen);
    }

    protected void listen()
    {
        Debug.Log("Listening");
        while (!Endpoint.IsListening)
            Thread.Sleep(10);

        while (Endpoint.IsListening && isActive)
        {
            if (serializer != null)
            {
                var c = Endpoint.GetContext();
                Debug.Log("Received request on " + c.Request.Url);
                serializer.SerializeTTL(LDPGraph, c);
            }
        }
        Debug.Log("Shutting down datapoint " + uri);
    }
}
