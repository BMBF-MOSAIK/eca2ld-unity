using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDEntity : MonoBehaviour
{

    LDPEntity entity;
    public string EntityName;
    // Use this for initialization
    void Start()
    {
        entity = new LDPEntity(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
