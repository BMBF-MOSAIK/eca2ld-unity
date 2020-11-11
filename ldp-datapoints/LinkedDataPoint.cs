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
    protected bool isActive = true;

    public string Name;
    public string Uri { get; protected set; }

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
        if (serializer == null)
        {
            Debug.LogError("No TTL Serializer attached to entity " + Name + "! Attach TTL serializer to enable publishing as Linked Entity");
        }
    }

    protected void initializeHttpListener(string uri)
    {
        Uri = uri;
        Endpoint = new HttpListener();
        Endpoint.Prefixes.Add(uri);
        Endpoint.Start();
        Task.Factory.StartNew(listen);
    }

    protected void listen()
    {
        while (!Endpoint.IsListening)
            Thread.Sleep(10);

        while (Endpoint.IsListening && isActive)
        {
            if (serializer != null)
            {
                var c = Endpoint.GetContext();
                if (c.Request.HttpMethod == "GET")
                {
                    try
                    {
                        LDPGraph.BuildRDFGraph();
                    }
                    catch (Exception ex)
                    {
                        c.Response.StatusCode = 500;
                        byte[] responseBuffer = responseBuffer = System.Text.Encoding.UTF8.GetBytes("An error occurred while rebuilding LDP Graph: " + ex.Message);
                        c.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                        c.Response.OutputStream.Flush();
                        c.Response.OutputStream.Close();
                        return;
                    }
                    Debug.Log("Received request on " + c.Request.Url);
                    if (LDPGraph is ValueGraph)
                        serializer.SerializeJSONResponse(LDPGraph as ValueGraph, c);
                    else
                        serializer.SerializeTTLResponse(LDPGraph, c);
                }
                else if (c.Request.HttpMethod == "POST")
                {
                    OnPost(c);
                }
                else if (c.Request.HttpMethod == "PUT")
                {
                    OnPut(c);
                }
            }
        }
    }
    protected virtual void OnPost(HttpListenerContext c)
    {
        throw new NotImplementedException();
    }

    protected virtual void OnPut(HttpListenerContext c)
    {
        throw new NotImplementedException();
    }
}
