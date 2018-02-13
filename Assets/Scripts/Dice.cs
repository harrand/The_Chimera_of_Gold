using UnityEngine;

public class Dice : MonoBehaviour
{
    // Harry and Ciara 12/02/2018
    //diceix??
    public GameObject diceFace;

    public static Dice Create(Vector3 position, Vector3 scale)
    {
        GameObject diceObject = Instantiate(Resources.Load("Prefabs/Dice") as GameObject);
        Dice dice = diceObject.AddComponent<Dice>();
        diceObject.transform.position = position;
        diceObject.transform.rotation = Quaternion.identity;
        diceObject.transform.localScale = scale;
        return dice;
    }
}