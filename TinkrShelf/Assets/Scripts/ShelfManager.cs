﻿using System.Collections.Generic;
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
    private string localManifestFileName;
	public List<BookObject> bookInfos;
    private string[] allBookJsons;
    public static int i = 0;
    public static int j = 0;
    public GameObject bookwheel;
    bool check = false;
    int count = 0;
    int degree = 0;
    public static bool arrowright = false;
    public static bool arrowright60 = false;
    public static bool arrowleft = false;
    public static bool arrowleft60 = false;
	public Button Left;
	public Button Right;
	public static bool rotation=false;

	//asset bundle
	private AssetBundle myLoadedAssetBundle;
	private string[] scenePaths;

    //for data collection
    private System.DateTime inTime;	
	private const string url = "https://s3.ap-south-1.amazonaws.com/tinkr/Manifests/manifest.json";
    private string responseJson;
    private bool isServerJson = false;

	public static string selectedBook;


	void Awake()
	{    Image = GameObject.Find("Image");
	   	 Title = GameObject.Find("Title");
         
      	//call json file from server
    	WWW request = new WWW(url);
        StartCoroutine(OnResponse(request));

		localManifestFileName = "Manifests/manifest";  //set to passed file name
        inTime = System.DateTime.Now;   
	}

	private IEnumerator OnResponse(WWW req)
	{

		yield return req;
		if (req.error == null) {
			responseJson = req.text;
			Debug.Log ("data" + responseJson);
			// if internet-> ok
			if (responseJson != "")
				isServerJson = true;
		}
		LoadShelfData();

		// first store the name to reference while loading the assets of book!
		selectedBook = bookInfos[2].GetComponent<BookObject>().book.fileName;
		string filePath = Path.Combine ("Books/", selectedBook+"/");
		bookscenePath = Path.Combine(filePath,"Scenes");
		//loading inital center book on first time loading of shelf
		LoadImageandText(bookInfos[2]);

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
        FirebaseHelper.LogInShelfTouch ("Left Arrow","Button", System.DateTime.Now.ToString());
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
        FirebaseHelper.LogInShelfTouch ("Right Arrow","Button", System.DateTime.Now.ToString());
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

		if (go.name == "Cover" )
		 {

			if (go.GetComponentInParent<BookObject> () != null) {

		        FirebaseHelper.LogInShelfTouch ("Book "+go.GetComponentInParent<BookObject> ().position,"Book",System.DateTime.Now.ToString());		                

				if (go.GetComponentInParent<BookObject> ().position == 1) {
						right60 ();
               	} else if (go.GetComponentInParent<BookObject> ().position == 2){
						right ();
				} else if(go.GetComponentInParent<BookObject> ().position == 3){
		            	LoadCentreBook ();
		        } else if (go.GetComponentInParent<BookObject> ().position == 4) {
						left ();
				} else if (go.GetComponentInParent<BookObject> ().position == 5) {
						left60 ();
				}
			}
			
	    }
		else if (go.name == "Image" || go.name=="Title" ) {
			i = 0; j = 0;
        	FirebaseHelper.LogInShelfTouch (go.name,go.name,System.DateTime.Now.ToString());
			LoadCentreBook ();
         }
	}

	public void LoadCentreBook()
    {   
        System.TimeSpan span = System.DateTime.Now - inTime;
		FirebaseHelper.LogInShelfSection (inTime.ToString (), span.ToString ());
		SceneManager.LoadScene (bookscenePath+"/Scene01");
	}

    private void LoadShelfData()
	{
        TextAsset file = Resources.Load(localManifestFileName) as TextAsset;
        if (file!=null)
		{
            // Read the json from the file into a string
            string dataAsJson = file.ToString();

			if (isServerJson == true)
			{
				if (responseJson.Equals(dataAsJson))
				{

					Debug.Log("server manifest same as local manifest");
				}
				else
				{
					//use server manifest

					Debug.Log("using server json");
					dataAsJson = responseJson;

					// overwrite local manifest with server manifest

					string path =  Application.dataPath + "/Resources/Manifests/manifest.json";
					System.IO.File.WriteAllText(path, dataAsJson);
					Debug.Log("Write complete  "+dataAsJson);
				}
			}


            //gets array of json string objects
            allBookJsons = JsonHelper.GetJsonObjectArray(dataAsJson, "books");
            j = allBookJsons.Length-1;
			foreach (string jsonObj in allBookJsons)
			{  if (i < 5) {
					bookInfos.Add (new BookObject());
					bookInfos [i].book = JsonUtility.FromJson<Book> (jsonObj);  //add string object as JSONObject to array of books
               
					bookInfos [i].SetCoverThumbnail ();
					i++;
				}
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
   
    public void LoadImageandText(BookObject bo) {

        // change center text with title
       Title.GetComponent<Text>().text = bo.book.title;
        //change center image with character
	
	Image.GetComponent<Image>().sprite=Resources.Load<Sprite> (bo.book.pathToThumbnail);

    }




}
