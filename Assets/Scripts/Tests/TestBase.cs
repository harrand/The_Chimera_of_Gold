using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestBase
{
    protected bool success;

    public TestBase()
    {
        Debug.Log("Constructing " + this.GetType().Name);
        success = false;
    }

    ~TestBase()
    {
        Debug.Log("Destructing " + this.GetType().Name + " (" + (success ? "success)" : "fail)"));
    }

    public bool IsSuccessful()
    {
        return this.success;
    }
}