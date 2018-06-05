using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class LoadAssetExample : MonoBehaviour {
    static AssetBundle bundleloaded;
    private string[] allPagesJsons;
    public static StoryBookJson storyBookJson;
    public static int pageNumber;
	public GStanzaManager stanzaManager;
    public List<GameObject> tinkerGraphicObjects;
    public List<GameObject> tinkerTextObjects;
	public List<GameObject> stanzaObjects;
    
    private string[] allStanzaJsons;
	private string page;
	public GameObject right;
	public GameObject left;

	public static string sceneScript; 
	Font font;
	Transform canvasTransform;

	private int noOfPages, i,j;
	float width=0.0f, startingX, startingY, startingXText, startingYText;
	float height = 32.94f;  //height of text:32.94
	private readonly float minWordSpace = 15.0f;
	private readonly float minLineSpace = 30.0f;

	//variables for logging data
	DateTime inTime;
	int timeSpent;

	private bool autoPlaying = false;
	private bool cancelAutoPlay = false;

	//sending data directly to firebase using "72 hours rule"! (removed local data storage)
	//public DataCollection dataCollector;

    public void Awake()
    {

		startingX = -2.0f;
		startingY = 129.0f;   //abhi ke liye static
		startingXText = 0.0f;
		startingYText = 0.0f;
		//font = Resources.GetBuiltinResource<Font>("OpenDyslexic-Regular.ttf");
		font=Resources.Load<Font>("Font/OpenDyslexic-Regular");
		canvasTransform = this.transform;  //if this script attached to canvas; otherwise update this line to store canvas transform.

        if (!bundleloaded)
        {
			//bundleloaded = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/"+ShelfManager.selectedBook.ToLower()));
			bundleloaded = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/catstory"));
            if (bundleloaded == null)
            {
                Debug.Log("Failed to load AssetBundle!");

            }
		}
		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//dataCollector.LoadLocalJSON ();
		//dataCollector.AddNewBook ("5PageProxy");

		//FirebaseHelper.AddBook(ShelfManager.selectedBook);
		//LoadStoryData (ShelfManager.selectedBook+".json");
		FirebaseHelper.AddBook("CatStory");
		LoadStoryData ("CatStory.json");
    }

    void Start () {
       
    }
    private void LoadStoryData(string fileName)
    {
		pageNumber = 0;
        TextAsset charDataFile = bundleloaded.LoadAsset(fileName) as TextAsset;
        string json = charDataFile.ToString();
        storyBookJson = JsonUtility.FromJson<StoryBookJson>(json);
		noOfPages = storyBookJson.pages.Length;
		left.SetActive (false);
		LoadCompletePage ();
    }
	public void LoadNextPage()
	{   
		left.SetActive (true);
		TimeSpan span = ( DateTime.Now- inTime );

		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//DataCollection.AddInSectionData (inTime.ToString(), span.ToString());

		FirebaseHelper.LogInAppSection (inTime.ToString(), span.ToString());

		Destroy(GameObject.Find("SceneManager"+(pageNumber)));
		pageNumber++;
		if (pageNumber == (noOfPages - 1)) {
			right.SetActive (false);
		} 
		EmptyPage ();
		LoadCompletePage ();
	}

	public void LoadPreviousPage()
	{
		TimeSpan span = ( DateTime.Now- inTime );

		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//DataCollection.AddInSectionData (inTime.ToString(), span.ToString());

		FirebaseHelper.LogInAppSection (inTime.ToString(), span.ToString());

		Destroy(GameObject.Find("SceneManager"+(pageNumber)));
		pageNumber--;
		if(pageNumber==0){
			left.SetActive (false);
		}
		else if (pageNumber != (noOfPages - 1)) {
			right.SetActive (true);
		}
		EmptyPage ();
		LoadCompletePage ();
	}

	public void EmptyPage()
	{
		for(int i=0;i<tinkerGraphicObjects.Count;i++)
		{
			Destroy (tinkerGraphicObjects [i]);
		}
		for (int j=0;j<stanzaObjects.Count;j++)
		{
			Destroy (stanzaObjects[j]);
		}
		stanzaObjects = null;
		stanzaManager.RequestCancelAutoPlay ();

	
	}
	public void LoadCompletePage()
	{   
		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//dataCollector.AddNewSection ("5PageProxy", pageNumber.ToString() );
		FirebaseHelper.AddSection(pageNumber.ToString());
		inTime = DateTime.Now;
		LoadSceneSpecificScript ();
		LoadPageData(pageNumber);
		LoadStanzaData();
		TokenizeStanza();
		LoadStanzaAudio();
		LoadTriggers();
		LoadAudios();

	}
		

	public void LoadSceneSpecificScript ()
	{  
		
		GameObject go = new  GameObject();
		go.transform.SetParent(canvasTransform);
		go.name="SceneManager"+pageNumber;
		sceneScript =storyBookJson.pages [pageNumber].script;
		go.AddComponent(Type.GetType(sceneScript));
		GameObject.Find ("Canvas").GetComponent<GStanzaManager> ().sceneManager = GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager>();
		GameObject.Find ("SceneManager" + pageNumber).GetComponent<GSManager> ().gameManager = GameObject.Find ("GameManager").GetComponent<GGameManager> ();
		GameObject.Find("SceneManager"+pageNumber).GetComponent<GSManager>().stanzaManager=GameObject.Find("Canvas").GetComponent<GStanzaManager>();
		GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ().myCanvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();
		GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ().Lbutton = GameObject.FindWithTag ("left_arrow");
		GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ().Rbutton = GameObject.FindWithTag ("right_arrow");
		GameObject.Find ("GameManager").GetComponent<GGameManager> ().sceneManager = GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ();
	
	}

    public void LoadStanzaAudio()
	{
		Destroy (GameObject.Find ("Canvas").GetComponent<AudioSource> ());
		GameObject.Find("Canvas").AddComponent<AudioSource>().clip= LoadAudioAsset(storyBookJson.pages[pageNumber].audioFile);

    }

    public void LoadAudios()
    {
        TimeStampClass[] timeStamps= storyBookJson.pages[pageNumber].timestamps;
        for(int i = 0; i < timeStamps.Length; i++)
        {
            tinkerTextObjects[i].AddComponent<AudioSource>().clip = LoadAudioAsset(timeStamps[i].audio);

        }
    }

    public AudioClip LoadAudioAsset(string name)
    {
        
        return bundleloaded.LoadAsset<AudioClip>(name);
    }

    public void LoadPageData(int pageNo)
	{tinkerGraphicObjects.Clear ();
        if (storyBookJson != null)
        {
            if (storyBookJson.pages[pageNo]!=null)
            {
                PageClass page = storyBookJson.pages[pageNo];
                GameObjectClass[] gameObjects = page.gameObjects;
                for (int i = 0; i < gameObjects.Length;i++)
                {
                    CreateGameObject(gameObjects[i]);
                }
                
            }
         }

    }

    public void LoadTriggers()
    {
        TriggerClass[] triggers = storyBookJson.pages[pageNumber].triggers;
        for (int i = 0; i < triggers.Length; i++)
        {
            if (triggers[i].typeOfLinking == 1)
            {

            }
            if (triggers[i].typeOfLinking == 2)
            {

            }
            if (triggers[i].typeOfLinking == 3)//two way linking of tinker graphic and tinker texts.
            {
                GameObject text = tinkerTextObjects[triggers[i].textId];
                GameObject graphic = tinkerGraphicObjects[triggers[i].sceneObjectId];

                text.GetComponent<GTinkerText>().pairedGraphic = graphic.GetComponent<GTinkerGraphic>();
                graphic.GetComponent<GTinkerGraphic>().pairedText1 = text.GetComponent<GTinkerText>();
            }
        }
    }
   

	public void LoadStanzaData()
	{   
		startingY = 129.0f;
		stanzaManager.stanzas.Clear ();
		j =0;
		stanzaObjects = new List<GameObject> ();
		TextClass[] texts= LoadAssetExample.storyBookJson.pages[LoadAssetExample.pageNumber].texts;

		foreach (TextClass text in texts)          
		{
			stanzaManager.stanzas.Add(CreateStanza(startingX, startingY));
			stanzaManager.stanzas[j].transform.SetParent(canvasTransform);
			stanzaManager.stanzas[j].stanzaValue = text;//add string object as JSONObject to array of books
			startingY = startingY -height - minLineSpace;  
			j++;
		}

	}

	public void TokenizeStanza (){
		tinkerTextObjects.Clear ();
		string[] words;

		for (i = 0; i < stanzaManager.stanzas.Count; i++) {
			words = stanzaManager.stanzas [i].stanzaValue.text.Split (' ');

			for (j = 0; j < words.Length; j++) {
				stanzaManager.stanzas[i].tinkerTexts.Add( CreateText (stanzaManager.stanzas[i], startingXText+width, startingYText , words[j], 30, Color.black) );
			}

			UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate (stanzaManager.stanzas[i].GetComponent<RectTransform>());
			width = 0.0f;
		}


	}

	StanzaObject CreateStanza( float x, float y)
	{
		GameObject go = Instantiate (Resources.Load ("Prefabs/StanzaObject")) as GameObject;
		go.transform.SetParent(canvasTransform);
		go.transform.localScale = new Vector3(1,1,1);
		RectTransform trans = go.GetComponent<RectTransform>();
		trans.anchoredPosition = new Vector3(x, y,0);
        go.GetComponent<StanzaObject>().stanzaManager = GameObject.Find("Canvas").GetComponent<GStanzaManager>();
		stanzaObjects.Add (go);
		return go.GetComponent<StanzaObject>();
	}

	GTinkerText CreateText( StanzaObject parent, float x, float y, string textToPrint, int fontSize, Color textColor)
	{
		GameObject UItextGO = new GameObject("Text_"+textToPrint);
		UItextGO.transform.SetParent(parent.transform);
       // Debug.Log(anim.runtimeAnimatorController);
        Text text = UItextGO.AddComponent<Text>();

		text.text = textToPrint;
		text.fontSize = 80;
		text.color = textColor;
		text.font = font;
		text.transform.localScale = new Vector3(1,1,1);

		ContentSizeFitter csf= UItextGO.AddComponent<ContentSizeFitter> ();
		csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

		VerticalLayoutGroup vlg = UItextGO.AddComponent<VerticalLayoutGroup> ();
		vlg.childControlHeight = true;
		vlg.childControlWidth = true;


		RectTransform trans = UItextGO.GetComponent<RectTransform>();
		text.alignment = TextAnchor.UpperLeft;
		trans.anchoredPosition = new Vector3(x, y,0);

		
		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate (trans);
		//trans.pivot = new Vector2 (0,1);

		width = width + trans.rect.width + minWordSpace;
        UItextGO.AddComponent<Animator>().runtimeAnimatorController = Resources.Load("TextAnimations/textzoomcontroller") as RuntimeAnimatorController;
        GTinkerText tinkerText= UItextGO.AddComponent<GTinkerText>();
        tinkerText.stanza =UItextGO.GetComponentInParent<StanzaObject>();
        tinkerTextObjects.Add(UItextGO);
        return UItextGO.GetComponent<GTinkerText>();
	}


	public void CreateGameObject(GameObjectClass gameObjectData)
	{
		Vector3 position = new Vector3(gameObjectData.posX, gameObjectData.posY);
		Vector3 scale = new Vector3(gameObjectData.scaleX, gameObjectData.scaleY);
		GameObject go = new GameObject(gameObjectData.label);
		go.transform.position=position;
		go.transform.localScale = scale;
		go.AddComponent<SpriteRenderer>();
		go.GetComponent<SpriteRenderer>().sortingOrder = gameObjectData.orderInLayer;
		go.AddComponent<GTinkerGraphic>();
		go.GetComponent<GTinkerGraphic>().dataTinkerGraphic = gameObjectData;
		go.GetComponent<GTinkerGraphic>().sceneManager = GameObject.Find("SceneManager"+(pageNumber)).GetComponent<GSManager>();
		go.GetComponent<GTinkerGraphic>().myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		go.GetComponent<GTinkerGraphic>().SetDraggable(gameObjectData.draggable);

		if (gameObjectData.anim.Length >0)
		{
			LoadAssetImages(go.GetComponent<GTinkerGraphic>(), gameObjectData.anim[0].animName, gameObjectData.anim[0].numberOfImages);
			go.GetComponent<GTinkerGraphic> ().secPerFrame = gameObjectData.anim [0].secPerFrame;

			if ( gameObjectData.anim [0].onStart) {

				go.GetComponent<GTinkerGraphic>().secPerFrame = gameObjectData.anim [0].secPerFrame;
				go.GetComponent<GTinkerGraphic>().sequences = gameObjectData.anim [0].sequences;
				go.GetComponent<GTinkerGraphic> ().PlayAnimation ();
			} else {
				LoadAssetImage(gameObjectData.imageName, go.GetComponent<SpriteRenderer>());

			}
		}
		else
		{
			LoadAssetImage(gameObjectData.imageName, go.GetComponent<SpriteRenderer>());
		}
		BoxCollider col = go.AddComponent<BoxCollider>();
		col.isTrigger = true;
		tinkerGraphicObjects.Add(go);

	}
    public void LoadAsset(string name)
    {
      
       var prefab = bundleloaded.LoadAsset<GameObject>(name);
        Instantiate(prefab);     
    }
    public void LoadAssetImage(string name,SpriteRenderer sr)
    {
        var sprite = bundleloaded.LoadAsset<Sprite>(name);
        sr.sprite = sprite;
    }

	public static void LoadAssetImages(GTinkerGraphic tinkerGraphic,string startName,int length)
	{

		tinkerGraphic.sprites= new Sprite[length];
		for (int i = 0; i < length; i++)
		{
			var sprite = bundleloaded.LoadAsset<Sprite>(startName+"-"+(i+1));
			tinkerGraphic.sprites[i] = sprite;

		}     
	}

    public void LoadScene()
    {
        if (!bundleloaded)
        {
            bundleloaded = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/books"));
            if (bundleloaded == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }
        }
        else
        {

            string[] scenes = bundleloaded.GetAllScenePaths();
            SceneManager.LoadScene(scenes[0]);

        }
    }
    /*IEnumerator InstantiateObject()

    {
        string uri = "file:///" + Application.dataPath + "/AssetBundles/" + ;
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri, 0);
        yield return request.Send();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject cube = bundle.LoadAsset<GameObject>("Cube");
        GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        Instantiate(cube);
        Instantiate(sprite);
    }*/
}

