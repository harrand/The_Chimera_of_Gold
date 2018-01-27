using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase
{
    private bool success;

    TestBase()
    {
        Debug.Log("Constructing " + this.GetType().Name);
        success = false;
    }

    ~TestBase()
    {
        Debug.Log("Destructing " + this.GetType().Name + " (" + (success ? "success)" : "fail)"));
    }
}
