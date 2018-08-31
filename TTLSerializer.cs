using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using VDS.RDF;
using VDS.RDF.Writing;

namespace Assets.Scripts.ECA2LD
{
    public class TTLSerializer : MonoBehaviour
    {

        protected Queue<Action> pendingActions;
        private CompressingTurtleWriter writer = new CompressingTurtleWriter();

        // Use this for initialization
        void Start()
        {
            pendingActions = new Queue<Action>();
        }

        // Update is called once per frame
        void Update()
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

        public void SerializeTTLResponse(LDPGraph graph, HttpListenerContext c)
        {
            byte[] responseBuffer;

            lock (pendingActions)
            {
                pendingActions.Enqueue(() =>
                {
                    responseBuffer = System.Text.Encoding.UTF8.GetBytes(graph.GetTTL());
                    c.Response.ContentType = "text/turtle";
                    c.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                    c.Response.OutputStream.Flush();
                    c.Response.OutputStream.Close();
                });
            }
        }

        public void SerializeJSONResponse(ValueGraph graph, HttpListenerContext c)
        {
            byte[] responseBuffer;

            lock (pendingActions)
            {
                pendingActions.Enqueue(() =>
                {
                    responseBuffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(graph.a.Value));
                    c.Response.ContentType = "application/json";
                    c.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                    c.Response.OutputStream.Flush();
                    c.Response.OutputStream.Close();
                });
            }
        }

        public void DeserializeJSONRequest(HttpListenerContext c, Type targetType, Action<object> continuation)
        {
            c.Response.StatusCode = 200;
            try
            {
                using (Stream input = c.Request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(input, Encoding.UTF8))
                    {
                        var deserialized = JsonConvert.DeserializeObject(reader.ReadToEnd(),targetType);
                        continuation(deserialized);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                c.Response.OutputStream.Write(Encoding.UTF8.GetBytes(e.Message), 0, e.Message.Length);
                c.Response.StatusCode = 500;
            }
            c.Response.OutputStream.Close();
        }

        public void SerializeTTLRequest(Graph g, Action<string> continuation)
        {
            lock (pendingActions)
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();
                writer.Save(g, sw);
                continuation(sw.ToString());
            }
        }
    }
}