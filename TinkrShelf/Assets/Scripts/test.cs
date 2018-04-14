using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class test : MonoBehaviour, IPointerClickHandler
{
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    GameObject go = eventData.pointerCurrentRaycast.gameObject;
    //    Debug.Log(go);

    //    if (go.name == "Shelf Test")
    //    {
    //        BackToShelf();
    //    }
    //}
    public void BackToShelf()
    {
        
        SceneManager.LoadScene("Shelf");

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log(go);

        if (go.name == "Title")
        {
            BackToShelf();
        }
    }
}
