using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    // Harry and Ciara 12/02/2018
    //diceix??
    public GameObject diceFace;

    public static Dice Create(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GameObject diceObject = Instantiate(Resources.Load("Prefabs/Dice") as GameObject);
        Dice dice = diceObject.AddComponent<Dice>();
        diceObject.transform.position = position;
        diceObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        diceObject.transform.localScale = scale;
        return dice;
    }

    public void Roll()
    {
        Vector3 cameraPosition = Camera.main.gameObject.transform.position;
        this.gameObject.transform.position = cameraPosition + new Vector3(0, 20, 0);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(new System.Random().Next(-180, 180), new System.Random().Next(-180, 180), new System.Random().Next(-180, 180)));
    }

    public int NumberFaceUp()
    {
        // TODO: Finish this by not checking whole values and checking between ranges.
        Vector3 up = this.gameObject.transform.up;
        Vector3 right = this.gameObject.transform.right;
        Vector3 forward = this.gameObject.transform.forward;
        if (Math.Round(up.y) > 0.75)
            return 3;
        if (Math.Round(up.y) < -0.75)
            return 4;
        if (Math.Round(right.y) < -0.75)
            return 5;
        if (Math.Round(right.y) > 0.75)
            return 2;
        if (Math.Round(forward.y) < -0.75)
            return 6;
        if (Math.Round(forward.y) > 0.75)
            return 1;
        else
            return 0;
    }
}