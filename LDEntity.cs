using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDEntity : MonoBehaviour
{

    public LDPEntity dataPoint { get; private set; }
    public string Uri { get; private set; }
    public string EntityName;
    public string HostAddress = "localhost";
    public int HostPort = 12345;

    // Use this for initialization
    void Start()
    {
        Uri = "http://" + HostAddress + ":" + HostPort + "/" + EntityName + "/";
        dataPoint = new LDPEntity(gameObject, HostAddress, HostPort);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        dataPoint.Shutdown();
    }
}
