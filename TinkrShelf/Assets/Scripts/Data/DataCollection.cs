using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;


//sending data directly to firebase using "72 hours rule"! (removed local data storage)
public class DataCollection : MonoBehaviour {

	private static string path;
	private string[] sectionsData;
	string[] wholeData;
	static JSONNode dataNode;
	public static string appID;
	public static string secID;

	public void Awake(){
		LoadLocalJSON();
		List<string> opt = new List<string> ();
		//SaveLocalJSON (dataNode);
		long val= CheckSize ();
	}

	public void AddFile(){
		File.CreateText (Application.persistentDataPath + "/JSONData.json").Dispose();
		File.WriteAllText (Application.persistentDataPath + "/JSONData.json", "{\"tabletID\": {} }");	
	}

	public void AddNewBook(string name){
		appID = name;
		if (dataNode ["tabletID"] [name] == null) {
			JSONNode node = new JSONObject ();
			dataNode ["tabletID"].Add (name, node);
			Debug.Log ("book added"+dataNode.ToString());
		}

	}

	public void AddNewSection(string appID,string sectionID){
		secID = sectionID;
		Debug.Log ("section added");
		if (dataNode ["tabletID"] [appID] [sectionID] == null) {
		JSONNode node = new JSONObject ();
			dataNode ["tabletID"] [appID].Add (sectionID.ToString(), node);
			Debug.Log ("sec added"+dataNode.ToString());
	    }
	}

	public static long CheckSize(){
		var fileInfo = new System.IO.FileInfo (Application.persistentDataPath + "/JSONData.json");
		Debug.Log(fileInfo.Length+"");
		return fileInfo.Length;
	}

	public void LoadLocalJSON()
	{
		string dataAsJSON;
		path = Application.persistentDataPath + "/JSONData.json";
		if (! File.Exists (Application.persistentDataPath + "/JSONData.json")) {
			AddFile ();
			Debug.Log (" no exists");
		} 
	    	dataAsJSON = File.ReadAllText (path);
			dataNode = JSON.Parse (dataAsJSON);
		Debug.Log (dataNode);
	}

	public static void SaveLocalJSON(JSONNode node)
	{
		File.WriteAllText (Application.persistentDataPath + "/JSONData.json", node.ToString() );
		Debug.Log ("saved: "+node.ToString());
	}

	//sending data directly to firebase using "72 hours rule"! (removed local data storage)
	public static void AddInSectionData( string inTime, string timeSpent){
		JSONNode node = new JSONObject();
		node ["inTime"] = inTime;
		node["timeSpent"] = timeSpent;
		Debug.Log ("tablet: "+appID+secID);
		dataNode["tabletID"][appID][secID]["IN_APP_SECTION"].Add(node);

		SaveLocalJSON (dataNode);
	}

	//sending data directly to firebase using "72 hours rule"! (removed local data storage)
	public static void AddInTouchData( string label, string type, string time){
		//type will be button, text or image
		JSONNode node = new JSONObject();
		node ["time"] = time;
		node ["type"] = type;
		node["label"] = label;
		Debug.Log ("reached node:"+node.ToString());
		dataNode["tabletID"][appID][secID]["IN_APP_TOUCH"].Add(node);
		SaveLocalJSON (dataNode);

	}

	//sending data directly to firebase using "72 hours rule"! (removed local data storage)
	public static void AddInResponseData( string selection, string answer, List<string> options, string correct, string timeElapsed){
		//type will be button, text or image
		JSONNode node = new JSONObject();
		node ["selection"] = selection;
		node ["answer"] = answer;
		node ["correct"] = correct;
		node ["timeTaken"] = timeElapsed;

		node ["options"] = new JSONArray ();
		for (int i = 0; i < options.Count; i++) {
			node ["options"].Add( options[i]);
		}

		dataNode["tabletID"][appID][secID]["IN_APP_RESPONSE"].Add(node);

	}


	public static string GetPath(){
		return path;
	}
}