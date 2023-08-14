using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject CheckPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        startPoint.transform.position = CheckPoint.transform.position;
    }
}
