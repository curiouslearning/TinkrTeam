using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ShelfManager : MonoBehaviour, IPointerClickHandler
{

	private string manifestFileName;
	public List<BookObject> bookInfos;

	// load the shelf with data before game starts!
	void Awake () {
		manifestFileName = "Manifests/manifest.json";  //set to passed file name
		LoadShelfData();
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
                    ShiftBookToCenter(go.GetComponent<BookObject>());
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
		// Path.Combine combines strings into a file path
		//data path is read only
		string filePath = Path.Combine(Application.dataPath, manifestFileName);
		if(File.Exists(filePath))
		{
			// Read the json from the file into a string
			string dataAsJson = File.ReadAllText(filePath); 

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
    {
        //swap book with this position with book at center.
        Book temp = new Book();
        temp = bo.book;
        bo.book = bookInfos[2].book;
        bookInfos[2].book = temp;

    }
    public void BackToShelf()
    {
        Debug.Log("hello");
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
                bookName += "/";
            }
        }
        string filePath = Path.Combine("Books/", bookName);
        string finalPath = Path.Combine(filePath, "Scenes/Scene01");
        SceneManager.LoadScene(finalPath);
    }
}
