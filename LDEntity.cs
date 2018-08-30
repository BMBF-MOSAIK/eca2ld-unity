using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDEntity : MonoBehaviour
{

    LDPEntity entity;
    public string EntityName;
    public string HostAddress = "localhost";
    public int HostPort = 12345;

    // Use this for initialization
    void Start()
    {
        entity = new LDPEntity(gameObject, HostAddress, HostPort);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
