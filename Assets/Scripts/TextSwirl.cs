using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextSwirl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private string text; //GetComponentInChildren<Text>().text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text = this.GetComponentInChildren<Text>().text;
        this.GetComponentInChildren<Text>().text = "⌘  " + text + "  ⌘";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponentInChildren<Text>().text = text;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("OfflineMenu");
    }
}