﻿using System.Collections.Generic;
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

	// load the shelf with data before game starts!
	void Awake () {
        Image = GameObject.Find("Image");
        Title = GameObject.Find("Title");
       
        manifestFileName = "Manifests/manifest";  //set to passed file name
		LoadShelfData();
        //loading appropriate center image 
        foreach (var bookVar in bookInfos)
        {
            if (bookVar.position == 3)
            {
                Image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bookVar.book.pathToThumbnail);
            }
        }
      
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log(go);

        if (go.name == "Shelf Test")
        {
            BackToShelf();

        }
        if (go.name == "Cover")
        {
            if (go.GetComponentInParent<BookObject>() != null)
            {
                if (go.GetComponentInParent<BookObject>().position == 3)
                {
                    LoadCenterBook();
                }
                else
                {
                    // shift book to center
                    ShiftBookToCenter(go.GetComponentInParent<BookObject>());
                }
            }
        }
        else if (go.GetComponent<BookObject>() != null)
        {
            if (go.GetComponent<BookObject>().position == 3)
            {
                LoadCenterBook();
            }
            else
            {
                // shift book to center
                ShiftBookToCenter(go.GetComponent<BookObject>());
            }
        }
        else if (go.name == "Image" || go.name == "Title")
        {
            LoadCenterBook();
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
            string[] allBookJsons = JsonHelper.GetJsonObjectArray(dataAsJson, "books");
			int i=0;
			foreach (string jsonObj in allBookJsons)
			{
                bookInfos[i].book = JsonUtility.FromJson<Book>(jsonObj);  //add string object as JSONObject to array of books
                bookInfos[i].position = i + 1;
                bookInfos[i].SetCoverThumbnail();
                i++;
            }
		}
		else
		{
			Debug.LogError("Cannot load shelf data!");
		}

	}
    private void ShiftBookToCenter(BookObject bo)
    {   //change center text with title
        Title.GetComponent<Text>().text = bo.book.title;
        //change center image with character
        Image.GetComponent<SpriteRenderer>().sprite= Resources.Load<Sprite>(bo.book.pathToThumbnail);

        //swap book with this position with book at center.
        Book temp = new Book();
        temp = bo.book;
        bo.book = bookInfos[2].book;
        bookInfos[2].book = temp;

        //set cover thumbnail again 
        bo.SetCoverThumbnail();
        bookInfos[2].SetCoverThumbnail();
    }
    public void BackToShelf()
    {
        SceneManager.LoadScene("Shelf");

    }
    private void LoadCenterBook()
    {
        string bookName = "";
        //finding bookname
        foreach (var bookVar in bookInfos)
        {
            if (bookVar.position == 3)
            {
                bookName = bookVar.book.fileName;
                Debug.Log(bookName);
                bookName += "/";
            }
        }
        string filePath = Path.Combine("Books/", bookName);
        string finalPath = Path.Combine(filePath, "Scenes/Scene01");
        SceneManager.LoadScene(finalPath);
    }
}
