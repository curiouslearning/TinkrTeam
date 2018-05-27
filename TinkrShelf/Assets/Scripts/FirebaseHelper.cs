using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class FirebaseHelper  : MonoBehaviour{

	JSONNode dataToLog;
	string dataAsJSON, path;
	string[] apps;
	string timeEnter;
	int timeSpent;
	string tabletID ="tabletID";

	// Use this for initialization
	void Start () {
		StartCoroutine(checkInternetConnection((isConnected)=>{
			// handle connection status here
			LogEvent ();
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

	public void LogEvent(){
		path = DataCollection.GetPath ();
		if (File.Exists(path)) {
			dataAsJSON = File.ReadAllText (path);
			dataToLog = JSON.Parse (dataAsJSON);
			dataToLog = dataToLog[tabletID];
			if (dataToLog.Tag == JSONNodeType.Object)
			{
				foreach (KeyValuePair<string, JSONNode> apps in (JSONObject)dataToLog)
				{
					foreach (KeyValuePair<string, JSONNode> sections in (JSONObject)apps.Value) {

						Debug.Log(string.Format("{0}: {1} {2}", 
							dataToLog [apps.Key],  dataToLog [apps.Key] [sections.Key] , dataToLog  [apps.Key] [sections.Key] ["IN_APP_SECTION"] ));
						Debug.Log ("" +apps.Key + sections.Key );
						JSONNode node = dataToLog [apps.Key] [sections.Key] ["IN_APP_SECTION"];

						if (dataToLog [apps.Key] [sections.Key] ["IN_APP_SECTION"] != null) {
							for (int i=0; i< (dataToLog [apps.Key] [sections.Key] ["IN_APP_SECTION"]).Count; i++) {
								timeEnter = dataToLog [apps.Key] [sections.Key] ["IN_APP_SECTION"][i]["inTime"];
								timeSpent =  dataToLog [apps.Key] [sections.Key] ["IN_APP_SECTION"][i]["timeSpent"];
								LogInAppSection (tabletID, dataToLog [apps.Key], dataToLog [apps.Key] [sections.Key] ,timeEnter, timeSpent);
								dataToLog [apps.Key] [sections.Key] ["IN_APP_SECTION"].Remove (i);
							}

							if (dataToLog [apps.Key] [sections.Key] ["IN_APP_TOUCH"] != null) {
								for (int i = 0; i < (dataToLog [apps.Key] [sections.Key] ["IN_APP_TOUCH"]).Count; i++) {
									timeEnter = dataToLog [apps.Key] [sections.Key] ["IN_APP_TOUCH"] [i] ["inTime"];
									timeSpent = dataToLog [apps.Key] [sections.Key] ["IN_APP_TOUCH"] [i] ["timeSpent"];
									LogInAppSection (tabletID, dataToLog [apps.Key], dataToLog [apps.Key] [sections.Key], timeEnter, timeSpent);
									dataToLog [apps.Key] [sections.Key] ["IN_APP_SECTION"].Remove (i);
								}
							}





							Debug.Log ("here"+dataToLog);
						}


					}
				    

				}
			}
		//	var keys = dataToLog.Values;
		//	Debug.Log ("" +keys);



		} else {
			Debug.Log ("no file to log data!");		
 		}
	
	}

	public void LogInAppSection(string tabID, string appID, string secID, string timeEnter, int timeSpent){
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

	public void LogInAppTouch(string tabID, string appID, string secID, string label,string type, string timestamp){
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



	public void LogInAppResponse(string tabID, string appID, string secID, string selection, string answer,string correct, string[] options, string timeElapsed){
		

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
						"TIME", timeElapsed),
					new Firebase.Analytics.Parameter (
						"options",options.ToString())
				}
			);
		}
	}



}
