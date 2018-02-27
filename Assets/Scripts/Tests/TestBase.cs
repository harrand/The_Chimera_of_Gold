using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * This sets up the base of the testing - which is then used to test the rest of the program
 * @author Harry Hollands
 */
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