using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject infoGameCanvas;

    public void OpenInfoCanvas()
    {
        Debug.Log("OpenInfoCanvas");
        infoGameCanvas.SetActive(true);
    }
    
    public void CloseInfoCanvas()
    {
        infoGameCanvas.SetActive(false);
    }
}
