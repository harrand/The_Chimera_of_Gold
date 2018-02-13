using UnityEngine;

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

    public int NumberFaceUp()
    {
        // TODO: Finish this by not checking whole values and checking between ranges.
        Vector3 up = this.gameObject.transform.up;
        const float e = Vector3.kEpsilon;
        Debug.Log("up = " + up);
        if (up == new Vector3(0, 1, 0))
            return 3;
        else if (up == new Vector3(0, -1, 0))
            return 4;
        else if (up == new Vector3(0, 0, 1))
            return 6;
        else if (up == new Vector3(0, 0, -1))
            return 1;
        else if (up == new Vector3(1, 0, 0))
            return 5;
        else if (up == new Vector3(-1, 0, 0))
            return 2;
        return 0;
        /*
        if (rot.eulerAngles.z < 45 && rot.eulerAngles.x < 45)
            return 3;
       else if()
        {

        }
        */
    }
}