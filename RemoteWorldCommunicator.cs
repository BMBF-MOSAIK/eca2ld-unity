using Assets.Scripts.ECA2LD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using VDS.RDF;

public class RemoteWorldCommunicator : MonoBehaviour
{
    /// <summary>
    /// Uri to which Communicator sends World info
    /// </summary>
    public string RemoteUri;

    /// <summary>
    /// Baseuri under which this world is hosted
    /// </summary>
    public string BaseUri;

    /// <summary>
    /// List of Entities to be sent to remote world
    /// </summary>
    public List<GameObject> Entities = new List<GameObject>();

    bool sent = false;

    // Use this for initialization
    void Update()
    {
        if (!sent)
        {
            bool entitiesInitialized = true;
            foreach (GameObject o in Entities)
            {
                LDEntity e = o.GetComponent<LDEntity>();
                if (e != null)
                    entitiesInitialized = entitiesInitialized && e.Uri != null;
                else
                {
                    entitiesInitialized = false;
                    break;
                }
            }
            if (entitiesInitialized)
            {
                serialize();
                sent = true;
            }
        }
    }

    void serialize()
    {
        Graph ttlGraph = new Graph();
        ttlGraph.NamespaceMap.AddNamespace("ldp", new Uri("http://www.w3.org/ns/ldp#"));
        foreach (GameObject o in Entities)
        {
            LDEntity e = o.GetComponent<LDEntity>();

            if (e != null)
            {
                ttlGraph.Assert(new Triple(
                    ttlGraph.CreateUriNode(new Uri(BaseUri)),
                    ttlGraph.CreateUriNode("ldp:contains"),
                    ttlGraph.CreateUriNode(new Uri(e.Uri))
                ));
            }
        }
        GetComponent<TTLSerializer>().SerializeTTLRequest(ttlGraph, sendRequest);
    }

    void sendRequest(string serializedTTL)
    {
        UnityWebRequest request = new UnityWebRequest(RemoteUri, "POST");
        request.SetRequestHeader("Content-Type", "text/turtle");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(serializedTTL));
        var r = request.SendWebRequest();
    }
}
