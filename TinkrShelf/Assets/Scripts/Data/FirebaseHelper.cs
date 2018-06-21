using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEngine.UI;

public class FirebaseHelper  : MonoBehaviour{

	JSONNode dataToLog;
	string dataAsJSON, path;
	string[] apps;
	public static int appID, secID;
	static int tabID = 1041;
	public Text text;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		StartCoroutine(checkInternetConnection((isConnected)=>{
			// handle connection status here
			if(isConnected){
			    Debug.Log("internet connection found");
				text.text = "connected";

				//sending data directly to firebase using "72 hours rule"! (removed local data storage)
			    //LogEvent ();
			
			}
			else{
				Debug.Log("not connected");
				text.text = "not connected";
			}
		}));

	}

	IEnumerator checkInternetConnection(System.Action<bool> action){
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error != null) {
			action (false);
		} else {
			action (true);
		}
	} 

	public static void AddBook(int id){
		appID = id;
	
	}
		
	public static void AddSection(int no){
		secID = no;
	}

	//sending data directly to firebase using "72 hours rule"! (removed local data storage)
	public void LogEvent(){

		string label, time, timeEnter;
		string answer, selection, foilList;
		double timeSpent;
		long count;

		path = DataCollection.GetPath ();
		if (File.Exists(path)) {
			dataAsJSON = File.ReadAllText (path);
			dataToLog = JSON.Parse (dataAsJSON);

			if (dataToLog.Tag == JSONNodeType.Object)
			{
				foreach (KeyValuePair<string, JSONNode> app in (JSONObject)dataToLog[tabID])
				{
					appID = int.Parse(app.Key);
					Debug.Log (app+"");
					foreach (KeyValuePair<string, JSONNode> section in (JSONObject)app.Value) {
						
						secID =  int.Parse(section.Key);
						Debug.Log (string.Format ("{0}: {1} {2}", 
							dataToLog [tabID] [app.Key], dataToLog [tabID] [app.Key] [section.Key], dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_SECTION"]));
						Debug.Log ("" + app.Key + section.Key);
						Debug.Log (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"].ToString () + "");

						if (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_SECTION"] != null) {
							count = (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_SECTION"]).Count;
							for (int i = 0; i < count; i++) {
								timeEnter = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_SECTION"] [0] ["inTime"];
								timeSpent = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_SECTION"] [0] ["timeSpent"];
								LogInAppSection (timeEnter, timeSpent);
								dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_SECTION"].Remove (0);
							}

						}


						if (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"] != null) {
							count = (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"]).Count;
							for (int i = 0; i < count; i++) {
								label = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"] [0] ["label"];
								time = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"] [0] ["time"];
								LogInAppTouch ( label,time);
								dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"].Remove (0);
							}
						}


						if (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] != null) {
							count = (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"]).Count;
							for (int i = 0; i < count; i++) {
								answer = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["answer"];
								selection = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["selection"];
								time = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["timeTaken"];
								foilList = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["foil"].ToString();
								LogInAppResponse (selection,answer,foilList,time);
								dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"].Remove (0);
							}
						}


					}


				}
				    

			}
		}

		else {
			Debug.Log ("no file to log data!");		
 		}

		text.text = dataToLog.ToString ();
		DataCollection.SaveLocalJSON (dataToLog);
	}

	public static void LogInAppSection( string timeEnter, double timeSpent){

		if (timeEnter != null ) {
			Firebase.Analytics.FirebaseAnalytics.LogEvent (
				"IN_APP_SECTION",
				new Firebase.Analytics.Parameter[] {
					new Firebase.Analytics.Parameter (
						"TABLET_ID", tabID),
					new Firebase.Analytics.Parameter (
						"APP_ID", appID),
					new Firebase.Analytics.Parameter (
						"SECTION_ID", secID),
					new Firebase.Analytics.Parameter (
						"TIME_ENTER", timeEnter),
					new Firebase.Analytics.Parameter (
						"TIME_SPENT", timeSpent)
				}
			);
		}
	}

	public static void LogInShelfSection( string timeEnter, double timeSpent){

		if (timeEnter != null) {
			Firebase.Analytics.FirebaseAnalytics.LogEvent (
				"IN_APP_SECTION",
				new Firebase.Analytics.Parameter[] {
					new Firebase.Analytics.Parameter (
						"TABLET_ID", tabID),
					new Firebase.Analytics.Parameter (
						"APP_ID", 0),
					new Firebase.Analytics.Parameter (
						"TIME_ENTER", timeEnter),
					new Firebase.Analytics.Parameter (
						"TIME_SPENT", timeSpent)
				}
			);
		}
	}

	public static void LogInAppTouch( string label, string timestamp){

		if (label != null ) {
			Firebase.Analytics.FirebaseAnalytics.LogEvent (
				"IN_APP_TOUCH",
				new Firebase.Analytics.Parameter[] {
					new Firebase.Analytics.Parameter (
						"TABLET_ID", tabID),
					new Firebase.Analytics.Parameter (
						"APP_ID", appID),
					new Firebase.Analytics.Parameter (
						"SECTION_ID", secID),
					new Firebase.Analytics.Parameter (
						"LABEL", label),
					new Firebase.Analytics.Parameter (
						"TIME_OF_TOUCH", timestamp)
				}
			);
		}
	}


	public static void LogInShelfTouch( string label, string timestamp){
		if (label != null ) {
			Firebase.Analytics.FirebaseAnalytics.LogEvent (
				"IN_APP_TOUCH",
				new Firebase.Analytics.Parameter[] {
					new Firebase.Analytics.Parameter (
						"TABLET_ID", tabID),
					new Firebase.Analytics.Parameter (
						"APP_ID", "Shelf"),
					new Firebase.Analytics.Parameter (
						"LABEL", label),
					new Firebase.Analytics.Parameter (
						"TIME_OF_TOUCH", timestamp)
				}
			);
		}
	}


	public static void LogInAppResponse(string selection, string answer,string options, string timeElapsed){

		if (answer != null ) {
			Firebase.Analytics.FirebaseAnalytics.LogEvent (
				"IN_APP_RESPONSE",
				new Firebase.Analytics.Parameter[] {
					new Firebase.Analytics.Parameter (
						"TABLET_ID", tabID),
					new Firebase.Analytics.Parameter (
						"APP_ID", appID),
					new Firebase.Analytics.Parameter (
						"SECTION_ID", secID),
					new Firebase.Analytics.Parameter (
						"SELECTION", selection),
					new Firebase.Analytics.Parameter (
						"CORRECT_ANSWER", answer),
					new Firebase.Analytics.Parameter (
						"FOIL",options),
					new Firebase.Analytics.Parameter (
						"TIME_OF_RESPONSE", timeElapsed)
				}
			);
		}
	}
}
