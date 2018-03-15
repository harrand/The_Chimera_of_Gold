using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {
    
    public static bool visible = false;
    public GameObject popUp;
    public void ShowToolTip()
    {
        //Debug.Log("enter");
        if(!visible)
        {
            popUp.SetActive(true);
            visible = true;
              
        }
    }
    public void HideToolTip()
    {

        //Debug.Log("exit");
        if (visible)
        {
            popUp.SetActive(false);
            visible = false;
        }
    }
}
