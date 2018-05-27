using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class DataCollection : MonoBehaviour {

	private static string path;
	private string[] sectionsData;
	string[] wholeData;
	JSONNode dataNode;
	string appID;
	List<string> opt;
	WholeDataJSON wholeDataJSON;
	private List<SectionJSON> sections;

	public void Awake(){
		path = Application.persistentDataPath + "/JSONData.json";
		LoadLocalJSON();
		AddNewBook ("appName");
		AddNewSection ("appName", "0" );
		Debug.Log (dataNode.ToString());
		AddInSectionData (0, "some time", 12);
		AddInTouchData (0,"this time","graphic","duck");
		opt = new List<string> ();
		opt.Add("green frog");
		opt.Add("yellow frog");
		Debug.Log ("called");
		AddInResponseData (0,"red frog","red frog", opt,true, "12");
		SaveLocalJSON ();
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
		}

	}

	public void AddNewSection(string appID, string sectionID){
		if (dataNode ["tabletID"] [name] [sectionID] == null) {
		JSONNode node = new JSONObject ();
		dataNode ["tabletID"] [appID].Add (sectionID, node);
	    }
	}

	private void LoadLocalJSON()
	{
		string dataAsJSON;
		if (! File.Exists (Application.persistentDataPath + "/JSONData.json")) {
			AddFile ();
			Debug.Log (" no exists");
		} 
	    	dataAsJSON = File.ReadAllText (path);
			dataNode = JSON.Parse (dataAsJSON);
	}

	private void SaveLocalJSON()
	{
		File.WriteAllText (Application.persistentDataPath + "/JSONData.json", dataNode.ToString() );
		Debug.Log ("written");
	}

	public void AddInSectionData(int pageNo, string inTime, int timeSpent){
		JSONNode node = new JSONObject();
		node ["inTime"] = inTime;
		node["timeSpent"] = timeSpent;
		Debug.Log ("tablet: "+appID+pageNo);
		dataNode["tabletID"][appID][pageNo]["IN_APP_SECTION"].Add(node);


	}

	public void AddInTouchData(int pageNo, string time, string type, string name){
		//type will be button, text or image
		JSONNode node = new JSONObject();
		node ["time"] = time;
		node ["type"] = type;
		node["name"] = name;
		Debug.Log ("reached node:"+node.ToString());
		dataNode["tabletID"][appID][pageNo]["IN_APP_TOUCH"].Add(node);

	}

	public void AddInResponseData(int pageNo, string selection, string answer, List<string> options, bool correct, string timeElapsed){
		//type will be button, text or image
		JSONNode node = new JSONObject();
		node ["selection"] = selection;
		node ["answer"] = answer;

		if (correct)
			node ["correct"] = "yes";
		else
			node ["correct"] = "no";
		node ["timeTaken"] = timeElapsed;

		node ["options"] = new JSONArray ();
		Debug.Log ("reached node:"+node.ToString());
		for (int i = 0; i < options.Count; i++) {
			node ["options"].Add( options[i]);
			Debug.Log ("reached node:"+node.ToString());
		}

		Debug.Log ("tablet: "+node);

		dataNode["tabletID"][appID][pageNo]["IN_APP_RESPONSE"].Add(node);

		Debug.Log ("something:"+dataNode.ToString());

	}


	public static string GetPath(){
		return path;
	}
}