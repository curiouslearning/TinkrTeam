using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollection : MonoBehaviour {

	private string localDataFile;
	private string[] appJSONS;
	private List<AppJSON> appJSONObjects;
	public void Awake(){
		localDataFile = "Manifests/JSONData";  //set to the static file name!
		LoadLocalJSON();
	}

	// Use this for initialization
	void Start () {
	}

	private void LoadLocalJSON()
	{
		TextAsset file = Resources.Load(localDataFile) as TextAsset;
		if (file!=null)
		{
			// Read the json from the file into a string
			string dataAsJson = file.ToString();
			//gets array of json string objects
			appJSONS = JsonHelper.GetJsonObjectArray(dataAsJson, "tabletID");
			foreach (string jsonObj in appJSONS)
			{  
				appJSONObjects.Add(JsonUtility.FromJson<AppJSON> (jsonObj));  //add string object as JSONObject to array of books
			}

		}
		else
		{
			Debug.LogError("Cannot load shelf data!");
		}

	}


	public void AddInSectionData(int pageNo, string inTime, int timeSpent){
	   
	}


}