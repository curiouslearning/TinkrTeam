using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ShelfManager : MonoBehaviour, IPointerClickHandler
{  
	public static string bookscenePath="";
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
    //string name1 = "";
    int degree = 0;
    public static bool arrowright = false;
    public static bool arrowright60 = false;
    public static bool arrowleft = false;
    public static bool arrowleft60 = false;
	public Button Left;
	public Button Right;
	public static bool rotation=false;
	//assetbundle
	private AssetBundle myLoadedAssetBundle;
	private string[] scenePaths;

	void Start()
	{    Image = GameObject.Find("Image");
		Title = GameObject.Find("Title");


		manifestFileName = "Manifests/manifest";  //set to passed file name1
		LoadShelfData();
		//loading appropriate center image 
		Image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bookInfos[2].book.pathToThumbnail);

		myLoadedAssetBundle = AssetBundle.LoadFromFile (bookscenePath);

		scenePaths = myLoadedAssetBundle.GetAllScenePaths ();
		Debug.Log (bookscenePath[0]);
		Debug.Log (scenePaths[0]);
	}


    public void Update()
    {
        if (check == true)
        {
            if (name == "left")
			{  if (bookwheel.transform.position.z % 30 == 0) 
					bookwheel.transform.Rotate (0, 0, 1);
            }
            else
			{ if (bookwheel.transform.position.z % 30 == 0) 
					bookwheel.transform.Rotate (0, 0, -1);

            }
            count++;
            if (count == degree)
            {
                check = false;
				Left.interactable = true;
				Right.interactable = true;
				rotation = true;
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
		Left.interactable = false;

    }
    public void right()
    {
        count = 0;
        check = true;
        name = "right";
        degree = 30;
        arrowleft60 = false;
        arrowleft = false;
        arrowright = true;
        arrowright60 = false;
		Right.interactable = false;
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

    public void OnPointerClick(PointerEventData eventData)
	{    
		GameObject go = eventData.pointerCurrentRaycast.gameObject;
		Debug.Log (go.name);
		if (go.name == "LeftArrow")
			left ();
		if (go.name == "RightArrow")
			right ();

		if (go.name == "Cover" )
		 {
			if (go.GetComponentInParent<BookObject> () != null) {
                

				if (go.GetComponentInParent<BookObject> ().position == 1) {
					if (rotation)
						right60 ();
				} else if (go.GetComponentInParent<BookObject> ().position == 2) {
					if (rotation)
						right ();
				} 
					
				else if (go.GetComponentInParent<BookObject> ().position == 4) {
					if (rotation)
						left ();
				} else if (go.GetComponentInParent<BookObject> ().position == 5) {
					if (rotation)
						left60 ();
				}
			} else if (go.GetComponents<BookObject> () != null) 
			{if (go.GetComponent<BookObject> ().position == 1) {
					if (rotation)
						right60 ();
				} else if (go.GetComponent<BookObject> ().position == 2) {
					if (rotation)
						right ();
				} 
			else if (go.GetComponent<BookObject> ().position == 4) {
				if (rotation)
					left ();
			} else if (go.GetComponent<BookObject> ().position == 5) {
				if (rotation)
					left60 ();
			}
			}
			
	}
		else if (go.name == "Image" || go.name == "Title") {
			Debug.Log(bookscenePath);
			LoadCentreBook ();
       
    }
	}
	public void LoadCentreBook()
	{
		SceneManager.LoadScene (bookscenePath+"/Scene00");
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
        
        entry.GetComponent<BookObject>().book = JsonUtility.FromJson<Book>(allBookJsons[i]);
        entry.GetComponent<BookObject>().SetCoverThumbnail();
        leaving.GetComponent<BookObject>().RemoveThumbnail();
        leaving.GetComponent<BookObject>().book = null;
        i++;
        j++;
        if (i > allBookJsons.Length - 1)
        {
            i = 0;
        }
        if (j > allBookJsons.Length - 1)
        {
            j = 0;
        }

    }
    public void LoadBookRightArrow(GameObject entry, GameObject leaving)
    {
        
        entry.GetComponent<BookObject>().book = JsonUtility.FromJson<Book>(allBookJsons[j]);
        entry.GetComponent<BookObject>().SetCoverThumbnail();
        leaving.GetComponent<BookObject>().RemoveThumbnail();
        leaving.GetComponent<BookObject>().book = null;
        j--;
        i--;
        if (j < 0)
        {
            j = allBookJsons.Length - 1;
        }
        if (i < 0)
        {
            i = allBookJsons.Length - 1;
        }
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
		Debug.Log(bo.book.pathToThumbnail);
        Image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bo.book.pathToThumbnail);

    }
}
