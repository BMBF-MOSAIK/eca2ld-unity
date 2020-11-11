using Assets.Scripts.eca2ld_unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VDS.RDF;

namespace Assets.Scripts.ECA2LD
{
    class LDPAttribute : LinkedDataPoint
    {
        private LDAttribute a;
        private VDS.RDF.Parsing.TurtleParser turtleParser = new VDS.RDF.Parsing.TurtleParser();

        public LDPAttribute(GameObject gameObject, LDAttribute a) : base(gameObject)
        {
            this.a = a;
            Name = a.Name;
            Uri = gameObject.GetComponent<LDEntity>().Uri + a.ParentComponent.GetType() + "/" + a.Name + "/";
            LDPGraph = new AttributeLDPGraph(new Uri(Uri), a);
            initializeHttpListener(Uri);
        }

        public override void Shutdown()
        {
            a.Shutdown();
            base.Shutdown();
        }

        protected override void OnPut(System.Net.HttpListenerContext c)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(c.Request.InputStream);
            VDS.RDF.Graph receivedGraph = new VDS.RDF.Graph();
            try
            {
                turtleParser.Load(receivedGraph, sr);
            }
            catch (Exception e)
            {
                c.Response.StatusCode = 500;
                byte[] responseBuffer = Encoding.UTF8.GetBytes("An error occurred while parsing received RDF payload: " + e.Message);
                c.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Count());
                c.Response.OutputStream.Flush();
                c.Response.OutputStream.Close();
                return;
            }

            var valueTriples = receivedGraph.GetTriplesWithPredicate(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#value"));
            var subjectTriples = receivedGraph.GetTriplesWithSubject(new Uri(Uri));
            if (valueTriples == null || subjectTriples == null || valueTriples.Count() != 1 || subjectTriples.Count() != 1)
            {
                c.Response.StatusCode = 400;
                byte[] responseBuffer = Encoding.UTF8.GetBytes("Attribute Resources only accept single rdf:value triple as Update with the Attribute Resource as subject.");
                c.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Count());
            }
            else
            {
                VDS.RDF.LiteralNode valueLiteral = valueTriples.ElementAt(0).Object as VDS.RDF.LiteralNode;
                try
                {
                    var newValue = Newtonsoft.Json.JsonConvert.DeserializeObject(valueLiteral.Value, a.Type, new Newtonsoft.Json.Converters.StringEnumConverter());
                    a.Value = newValue;
                }
                catch (Exception e)
                {
                    c.Response.StatusCode = 500;
                    byte[] responseBuffer = Encoding.UTF8.GetBytes("An error occurred while deserializing received value update: " + e.Message);
                    c.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Count());
                    c.Response.OutputStream.Flush();
                    c.Response.OutputStream.Close();
                    return;
                }
                c.Response.StatusCode = 202;
            }
            c.Response.OutputStream.Flush();
            c.Response.OutputStream.Close();
        }

    }
}
