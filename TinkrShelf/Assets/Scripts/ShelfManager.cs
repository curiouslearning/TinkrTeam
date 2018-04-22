using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShelfManager : MonoBehaviour, IPointerClickHandler
{
    private GameObject Image;
    private GameObject Title;
	private string manifestFileName;
	public List<BookObject> bookInfos;
    private string[] allBookJsons;
    public static int i = 0;
    public static int j = 0;
    public GameObject bookwheel;
    bool check = false;
    int count = 0;
    string name1 = "";
    int degree = 0;
    public static bool arrowright = false;
    public static bool arrowright60 = false;
    public static bool arrowleft = false;
    public static bool arrowleft60 = false;
    public void Update()
    {
        if (check == true)
        {
            if (name == "left")
            {
                bookwheel.transform.Rotate(0, 0, 1);
            }
            else
            {
                bookwheel.transform.Rotate(0, 0, -1);
            }
            count++;
            if (count == degree)
            {
                check = false;
               
            }
        }
    }


    public void left()
    {
        count = 0;
        check = true;
        name = "left";
        degree = 30;
        arrowleft60 = false;
        arrowleft = true;
        arrowright = false;
        arrowright60 = false;
    }
    public void right()
    {
        Debug.Log("rightin");
        count = 0;
        check = true;
        name = "right";
        degree = 30;
        arrowleft60 = false;
        arrowleft = false;
        arrowright = true;
        arrowright60 = false;
    }
    public void right60()
    {
        count = 0;
        check = true;
        name = "right";
        degree = 60;
        arrowleft60 = false;
        arrowleft = false;
        arrowright = false;
        arrowright60 = true;
    }
    public void left60()
    {  
            count = 0;
            check = true;
            name = "left";
            degree = 60;
            arrowleft60 = true;
        arrowleft = false;
        arrowright = false;
        arrowright60 = false;
    }
    // load the shelf with data before game starts!
    void Awake () {
        Image = GameObject.Find("Image");
        Title = GameObject.Find("Title");
       
        manifestFileName = "Manifests/manifest";  //set to passed file name1
		LoadShelfData();
        //loading appropriate center image 
        Image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bookInfos[2].book.pathToThumbnail);

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        //Debug.Log(go);
        if (go.name == "Cover")
        {
            if (go.GetComponentInParent<BookObject>() != null)
            {
                

                if (go.GetComponentInParent<BookObject>().position==1)
                {
                   
                    right60();
                }
                else if(go.GetComponentInParent<BookObject>().position==2)
                {
                    right();
                }
                else if(go.GetComponentInParent<BookObject>().position==4)
                {
                    left();
                }
                else if(go.GetComponentInParent<BookObject>().position==5)
                {
                    left60();
                }
            }
        }
       
    }
    private void LoadShelfData()
	{
        TextAsset file = Resources.Load(manifestFileName) as TextAsset;
        if (file!=null)
		{
            // Read the json from the file into a string
            string dataAsJson = file.ToString();
            //gets array of json string objects
            allBookJsons = JsonHelper.GetJsonObjectArray(dataAsJson, "books");
            j = allBookJsons.Length-1;
			foreach (string jsonObj in allBookJsons)
			{
                bookInfos[i].book = JsonUtility.FromJson<Book>(jsonObj);  //add string object as JSONObject to array of books
               
                bookInfos[i].SetCoverThumbnail();
                i++;
                if(i==5)
                {
                    break;
                }
            }
		}
		else
		{
			Debug.LogError("Cannot load shelf data!");
		}

	}
   
     
    public void BackToShelf()
    {
        SceneManager.LoadScene("Shelf");

    }
    public void LoadBookLeftArrow(GameObject entry,GameObject leaving)
    {
        if (i == allBookJsons.Length)
        {
            i = 0;
        }
        entry.GetComponent<BookObject>().book = JsonUtility.FromJson<Book>(allBookJsons[i]);
        entry.GetComponent<BookObject>().SetCoverThumbnail();
        leaving.GetComponent<BookObject>().RemoveThumbnail();
        leaving.GetComponent<BookObject>().book = null;
        i++;
        
        
    }
    public void LoadBookRightArrow(GameObject entry, GameObject leaving)
    {
        if (j<0)
        {
            j = allBookJsons.Length - 1;
        }
        entry.GetComponent<BookObject>().book = JsonUtility.FromJson<Book>(allBookJsons[j]);
        entry.GetComponent<BookObject>().SetCoverThumbnail();
        leaving.GetComponent<BookObject>().RemoveThumbnail();
        leaving.GetComponent<BookObject>().book = null;
        j--;


    }
    /*public void loadbookdata(GameObject entry, GameObject leaving)
    {
        
       Debug.Log(entry+ "  "+leaving);

        Debug.Log(i + " " + allBookJsons.Length);
        
        if (i == allBookJsons.Length)
        {
            Debug.Log("loadbookenter_ifenter");
            entry.GetComponent<BookObject>().book = leaving.GetComponent<BookObject>().book;
            entry.GetComponent<BookObject>().SetCoverThumbnail();
            leaving.GetComponent<BookObject>().RemoveThumbnail();
            leaving.GetComponent<BookObject>().book = null;
            
        }
        else if(i< allBookJsons.Length)
        {
            Debug.Log("new 1");
            entry.GetComponent<BookObject>().book = JsonUtility.FromJson<Book>(allBookJsons[i]);
            entry.GetComponent<BookObject>().SetCoverThumbnail();
            Debug.Log(entry.GetComponent<BookObject>().book.title);
            i++;
        }
    }*/

    public void loadImageandText(BookObject bo) {
        
        // change center text with title
       Title.GetComponent<Text>().text = bo.book.title;
        //change center image with character
        Image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bo.book.pathToThumbnail);

    }
}
