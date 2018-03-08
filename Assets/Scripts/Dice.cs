using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections;

/**
 * Dice is created once at the beginning of the game and then rolled whenever a player plays.
 * @author Ciara O'Brien, Harry Hollands
 */
public class Dice : MonoBehaviour
{
    /**
     * This makes the initial dice
     * @author Harry Hollands
     * @param position The position of the dice in the world	
     * @param rotation The rotation of the dice in Euler angles
     * @param scale The scale of the dice
     * @return the dice created
     */
	public static Dice Create(Vector3 position, Vector3 rotation, Vector3 scale)
    {
<<<<<<< HEAD
        //ClientScene.RegisterPrefab(this.gameObject);

        if (isServer)
        {
            Debug.Log("Server");
            //NetworkServer.Spawn(this.gameObject);
        }
        else
        {
            Debug.Log("NOT");
            //NetworkServer.Spawn(this.gameObject);
        }
    }

    public static Dice Create(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GameObject diceObject = GameObject.FindGameObjectWithTag("Dice");
        Dice dice = diceObject.AddComponent<Dice>();
        //diceObject.tag = "Dice";
        diceObject.transform.position = position;
        diceObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        diceObject.transform.localScale = scale;

=======
        GameObject diceObject = Instantiate(Resources.Load("Prefabs/Dice") as GameObject);
        Dice dice = diceObject.AddComponent<Dice>();
        diceObject.transform.position = position;
        diceObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        diceObject.transform.localScale = scale;
>>>>>>> parent of 2017a61... Networking work session 2
        return dice;
    }

    /**
     * Teleports the dice object to the main camera position and applies a random rotation, essentially simulating a literal throw of the die.
     * Velocity of the dice object is also reset incase it was going super fast beforehand.
     * @author Harry Hollands
     */
	public void Roll(Vector3 desiredPosition)
    {
		this.gameObject.SetActive(true);
        Vector3 cameraPosition = Camera.main.gameObject.transform.position;
        this.gameObject.transform.position = desiredPosition;
		this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(new System.Random().Next(-180, 180), new System.Random().Next(-180, 180), new System.Random().Next(-180, 180)));
    }

    /**
    * Returns the face which is geometrically deemed to be face up on the dice 3D model.
    * @author Harry Hollands, Ciara O'Brien
    * @return a uint holding the current face up - checked when rolled
    */
    public uint NumberFaceUp()
    {
		if(!this.gameObject.activeSelf)
			return 0;
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