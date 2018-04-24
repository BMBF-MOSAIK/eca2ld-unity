using Assets.Scripts.ECA2LD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VDS.RDF;

public abstract class LinkedDataPoint : MonoBehaviour
{

    protected HttpListener Endpoint;
    protected Queue<Action> pendingActions;
    protected LDPGraph LDPGraph;
    protected string uri;
  
    public string Name;

    // Use this for initialization
    protected void Start()
    {
        BuildUri();
        pendingActions = new Queue<Action>();
        Endpoint = new HttpListener();
        Endpoint.Prefixes.Add(uri);
        Endpoint.Start();
        Task.Factory.StartNew(listen);
    }

    protected void Update()
    {
        lock (pendingActions)
        {
            if (pendingActions.Count > 0)
            {
                var action = pendingActions.Dequeue();
                action.Invoke();
            }
        }
    }

    protected virtual void BuildUri()
    {        
    }

    protected void listen()
    {
        while (!Endpoint.IsListening)
            Thread.Sleep(10);

        while (Endpoint.IsListening)
        {
            var c = Endpoint.GetContext();
            byte[] responseBuffer;

            lock (pendingActions)
            {
                pendingActions.Enqueue(() =>
                {
                    responseBuffer = System.Text.Encoding.UTF8.GetBytes(LDPGraph.GetTTL());
                    c.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                    c.Response.OutputStream.Flush();
                    c.Response.OutputStream.Close();
                });
            }
        }
    }
}
