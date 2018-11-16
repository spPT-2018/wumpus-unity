using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

    Vector3 target;
    float spent, dur;

    private void Start()
    {
        SetLerpPos(transform.position, 0);
    }

    // Update is called once per frame
    void Update ()
    {
        spent += Time.deltaTime;
        if (Vector3.Distance(transform.position, target) < Mathf.Epsilon)
        {
            transform.position = target;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, target, spent / dur);
        }
    }

    public void SetLerpPos(Vector3 pos, float duration = 1f)
    {
        spent = 0;
        dur = duration;
        target = pos;
    }
}
