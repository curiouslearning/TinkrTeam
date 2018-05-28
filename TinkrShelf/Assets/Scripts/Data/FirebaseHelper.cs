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
	public static string appID, secID;
	static string tabID ="tabletID";
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

	public static void AddBook(string name){
		appID = name;
	
	}
		
	public static void AddSection(string no){
		secID = no;
	}

	public void LogEvent(){

		string label, type, time, timeEnter;
		string answer, selection, correct, options, timeSpent;
		long count;

		path = DataCollection.GetPath ();
		if (File.Exists(path)) {
			dataAsJSON = File.ReadAllText (path);
			dataToLog = JSON.Parse (dataAsJSON);

			if (dataToLog.Tag == JSONNodeType.Object)
			{
				foreach (KeyValuePair<string, JSONNode> app in (JSONObject)dataToLog[tabID])
				{
					appID = app.Key;
					Debug.Log (app+"");
					foreach (KeyValuePair<string, JSONNode> section in (JSONObject)app.Value) {
						
						secID = section.Key;
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
								type = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"] [0] ["type"];
								time = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"] [0] ["time"];
								LogInAppTouch ( label,type,time);
								dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_TOUCH"].Remove (0);
							}
						}


						if (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] != null) {
							count = (dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"]).Count;
							for (int i = 0; i < count; i++) {
								answer = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["answer"];
								correct = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["correct"];
								selection = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["selection"];
								time = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["timeTaken"];
								options = dataToLog [tabID] [app.Key] [section.Key] ["IN_APP_RESPONSE"] [0] ["options"].ToString();
								LogInAppResponse (selection,answer,options,correct,time);
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

		Debug.Log ("here"+dataToLog);
		text.text = dataToLog.ToString ();
		DataCollection.SaveLocalJSON (dataToLog);
	}

	public static void LogInAppSection( string timeEnter, string timeSpent){

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

	public static void LogInShelfSection( string timeEnter, string timeSpent){

		Debug.Log ("fireOn"+ timeSpent);
		if (timeEnter != null ) {
			Firebase.Analytics.FirebaseAnalytics.LogEvent (
				"IN_APP_SECTION",
				new Firebase.Analytics.Parameter[] {
					new Firebase.Analytics.Parameter (
						"TABLET_ID", tabID),
					new Firebase.Analytics.Parameter (
						"APP_ID", "Shelf"),
					new Firebase.Analytics.Parameter (
						"TIME_ENTER", timeEnter),
					new Firebase.Analytics.Parameter (
						"TIME_SPENT", timeSpent)
				}
			);
		}
	}

	public static void LogInAppTouch( string label,string type, string timestamp){

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
						"TYPE", type),
					new Firebase.Analytics.Parameter (
						"TIMESTAMP", timestamp)
				}
			);
		}
	}


	public static void LogInShelfTouch( string label,string type, string timestamp){
		Debug.Log ("fire"+ label);
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
						"TYPE", type),
					new Firebase.Analytics.Parameter (
						"TIMESTAMP", timestamp)
				}
			);
		}
	}


	public static void LogInAppResponse(string selection, string answer,string options, string correct, string timeElapsed){

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
						"ANSWER", answer),
					new Firebase.Analytics.Parameter (
						"CORRECT", correct),
					new Firebase.Analytics.Parameter (
						"OPTIONS",options),
					new Firebase.Analytics.Parameter (
						"TIME", timeElapsed)
				}
			);
		}
	}
}
