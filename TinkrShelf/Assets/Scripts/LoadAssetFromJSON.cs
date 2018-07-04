using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Script to load the scene based on JSON describing the book.
/// </summary>
public class LoadAssetFromJSON : MonoBehaviour {
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
    //public GameObject endPageHome;
    //public GameObject endPageReadAgain;
	//public GameObject dropdown;
    //static float previousTextWidth;
    public static string sceneScript;
    Font font;
    Transform canvasTransform;
    
    private int noOfPages, i, j;
    float width = 0.0f, startingX, startingY, startingXText, startingYText;
    float height = 32.94f;  //height of text:32.94
    private readonly float minWordSpace = 30.0f;
    private readonly float minLineSpace = 30.0f;

    //variables for logging data
    public DateTime inTime;
    int timeSpent;

    //sending data directly to firebase using "72 hours rule"! (removed local data storage)
    //public DataCollection dataCollector;

    public void Awake()
    {

        //font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        font = Resources.Load<Font>("Font/OpenDyslexic-Regular");

        canvasTransform = this.transform;  //if this script is attached to canvas; otherwise update this line to store canvas transform.

        if (!bundleloaded)
        {
            bundleloaded = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/catstory"));  //ShelfManager.selectedBook.ToLower())
            if (bundleloaded == null)
            {
                Debug.Log("Failed to load AssetBundle!");

            }
        }
        //sending data directly to firebase using "72 hours rule"! (removed local data storage)
        //dataCollector.LoadLocalJSON ();

        //FirebaseHelper.AddBook(ShelfManager.selectedBook);

        LoadStoryData();
    }

    void Start() {
		
        startingX = storyBookJson.textStartPositionX;
        startingY = storyBookJson.textStartPositionY;

    }



	/// <summary>
	/// Loads the story data.
	/// </summary>
    public void LoadStoryData()
    {
   string fileName = ShelfManager.selectedBook.ToLower() + ".json";
       
            pageNumber = 0;
            TextAsset charDataFile = bundleloaded.LoadAsset(fileName) as TextAsset;
        if (charDataFile !=null)
        {
            string json = charDataFile.ToString();
            storyBookJson = JsonUtility.FromJson<StoryBookJson>(json);
            noOfPages = storyBookJson.pages.Length;

            //sending data directly to firebase using "72 hours rule"! (removed local data storage)
            //dataCollector.AddNewBook (storyBookJson.id.ToString());

            FirebaseHelper.AddBook(storyBookJson.id);
            left.SetActive(false);
            right.SetActive(true);
            //dropdown.SetActive(true);
            //endPageHome.SetActive(false);
            //endPageReadAgain.SetActive(false);
            LoadCompletePage();

        }
    }

	/// <summary>
	/// Loads the next page on "next" arrow/button click.
	/// </summary>
    public void LoadNextPage()
    {
        stanzaManager.RequestCancelAutoPlay();
        left.SetActive(true);
        DateTime time = DateTime.Now;

        TimeSpan span = (time - inTime);
        FirebaseHelper.LogInAppTouch("Button_Page_Right_Arrow", time.ToString());

        //sending data directly to firebase using "72 hours rule"! (removed local data storage)
        //DataCollection.AddInSectionData (inTime.ToString(), span.ToString());

        FirebaseHelper.LogInAppSection(inTime.ToString(), span.TotalSeconds);

        Destroy(GameObject.Find("SceneManager" + (pageNumber)));

        pageNumber++;

        if (pageNumber == (noOfPages - 1)) {
            right.SetActive(false);
            //endPageHome.SetActive(true);
            //endPageReadAgain.SetActive(true);
			//dropdown.SetActive (false);

        }

        EmptyPage();
        LoadCompletePage();
    }

        
	/// <summary>
	/// Loads the previous page on "previous" arrow/button click.
	/// </summary>
    public void LoadPreviousPage()
    {
        stanzaManager.RequestCancelAutoPlay();

        //previousTextWidth = 0;
        DateTime time = DateTime.Now;
        TimeSpan span = ( time- inTime);
        FirebaseHelper.LogInAppTouch("Button_Page_Left_Arrow", time.ToString());

        //sending data directly to firebase using "72 hours rule"! (removed local data storage)
        //DataCollection.AddInSectionData (inTime.ToString(), span.ToString());
        FirebaseHelper.LogInAppSection(inTime.ToString(), span.TotalSeconds);


        Destroy(GameObject.Find("SceneManager" + (pageNumber)));
        pageNumber--;
        if (pageNumber == 0) {
            left.SetActive(false);
        }
        else if (pageNumber == (noOfPages-2))
        {
            //dropdown.SetActive(true);
            right.SetActive(true);
            //endPageHome.SetActive(false);
            //endPageReadAgain.SetActive(false);
        }
        EmptyPage();
        LoadCompletePage();

    }

    /// <summary>
    /// Destrroys all the scene objects before loading another page.
    /// </summary>
	public void EmptyPage()
	{   if (tinkerGraphicObjects != null) {
			for (int i = 0; i < tinkerGraphicObjects.Count; i++) {
				Destroy (tinkerGraphicObjects [i]);
			}
		}
		if (stanzaObjects != null) {
			for (int j = 0; j < stanzaObjects.Count; j++) {
				Destroy (stanzaObjects [j]);
			}
		}
		stanzaObjects = null;
        //stanzaManager.RequestCancelAutoPlay();
    }

	/// <summary>
	/// Loads all the page asssets.
	/// </summary>
	public void LoadCompletePage()
	{   
		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//dataCollector.AddNewSection ("5PageProxy", pageNumber.ToString() );
		Debug.Log(pageNumber);
		FirebaseHelper.AddSection(pageNumber);
		inTime = DateTime.Now;
		LoadSceneSpecificScript ();
		LoadPageData(pageNumber);
		LoadStanzaData();
		TokenizeStanza();
		LoadStanzaAudio();
		LoadTriggers();
		LoadAudios();

    }
   



    public void LoadSceneSpecificScript()
    {

        GameObject go = new GameObject();
        go.transform.SetParent(canvasTransform);
        go.name = "SceneManager" + pageNumber;
        //if (pageNumber != 18)
        //{
            sceneScript = storyBookJson.pages[pageNumber].script;
            go.AddComponent(Type.GetType(sceneScript));
            GameObject.Find("Canvas").GetComponent<GStanzaManager>().sceneManager = GameObject.Find("SceneManager" + pageNumber).GetComponent<GSManager>();
            GameObject.Find("SceneManager" + pageNumber).GetComponent<GSManager>().gameManager = GameObject.Find("GameManager").GetComponent<GGameManager>();
            GameObject.Find("SceneManager" + pageNumber).GetComponent<GSManager>().stanzaManager = GameObject.Find("Canvas").GetComponent<GStanzaManager>();
            GameObject.Find("SceneManager" + pageNumber).GetComponent<GSManager>().myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameObject.Find("SceneManager" + pageNumber).GetComponent<GSManager>().Lbutton = GameObject.FindWithTag("left_arrow");
            GameObject.Find("SceneManager" + pageNumber).GetComponent<GSManager>().Rbutton = GameObject.FindWithTag("right_arrow");
            GameObject.Find("GameManager").GetComponent<GGameManager>().sceneManager = GameObject.Find("SceneManager" + pageNumber).GetComponent<GSManager>();
        //}
        //else {
        //    sceneScript = storyBookJson.pages[pageNumber].script;
        //    go.AddComponent(Type.GetType(sceneScript));

        //}


    }

	/// <summary>
	/// Loads the audio for stanza auto narration to the canvas.
	/// </summary>
    public void LoadStanzaAudio()
    {
        Destroy(GameObject.Find("Canvas").GetComponent<AudioSource>());
        GameObject.Find("Canvas").AddComponent<AudioSource>().clip = LoadAudioAsset(storyBookJson.pages[pageNumber].audioFile);
    }

	/// <summary>
	/// Loads the audio for each word.
	/// </summary>
    public void LoadAudios()
    {
        TimeStampClass[] timeStamps = storyBookJson.pages[pageNumber].timestamps;
        for (int i = 0; i < timeStamps.Length; i++)
        {
            tinkerTextObjects[i].AddComponent<AudioSource>().clip = LoadAudioAsset(timeStamps[i].audio);

        }
    }

	/// <summary>
	/// Loads the audio asset from asset bundle.
	/// </summary>
	/// <returns>The audio asset.</returns>
	/// <param name="name">Name of the audio.</param>
    public AudioClip LoadAudioAsset(string name)
    {if (name != "")
            return bundleloaded.LoadAsset<AudioClip>(name);
        else
            return null; 
    }

	/// <summary>
	/// Loads the game objects related to any page number.
	/// </summary>
	/// <param name="pageNo">Page no.</param>
    public void LoadPageData(int pageNo)
    { tinkerGraphicObjects.Clear();
        if (storyBookJson != null)
        {
            if (storyBookJson.pages[pageNo] != null)
            {
                PageClass page = storyBookJson.pages[pageNo];
                GameObjectClass[] gameObjects = page.gameObjects;
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    CreateGameObject(gameObjects[i]);
                }

            }
        }

    }

	/// <summary>
	/// Links/Pairs TinkerTexts and TinkerGraphics.
	/// </summary>
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


	/// <summary>
	/// Loads all the stanzas on the page.
	/// </summary>
    public void LoadStanzaData()
    {
        startingX = storyBookJson.textStartPositionX;
        startingY = storyBookJson.textStartPositionY;

        stanzaManager.stanzas.Clear();
        j = 0;
        stanzaObjects = new List<GameObject>();
        TextClass[] texts = storyBookJson.pages[pageNumber].texts;
		int length = storyBookJson.pages [pageNumber].timestamps.Length;
		if(length==1)
			startingX=-95.0f ;
		else if(length==2)
			startingX=-175.0f ;
		else if(length==3)
			startingX=-212.0f ;
		else if(length==4)
			startingX=-280.0f ;
		else if(length==5)
			startingX=-335.0f ;
		else if(length==6)
			startingX=-340.0f ;
		else if(length==7)
			startingX=-380.0f ;


        foreach (TextClass text in texts)
        {
            stanzaManager.stanzas.Add(CreateStanza(startingX, startingY));
            stanzaManager.stanzas[j].transform.SetParent(canvasTransform);
            stanzaManager.stanzas[j].stanzaValue = text;//add string object as JSONObject to array of books
            startingY = startingY - height - minLineSpace;
            j++;
        }

    }

    /// <summary>
    /// Tokenizes the stanza into TinkerTexts.
    /// </summary>
	public void TokenizeStanza ()
   {
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

	/// <summary>
	/// Creates the stanza at any given position.
	/// </summary>
	/// <returns>The StanzaObject object.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	StanzaObject CreateStanza( float x, float y)
	{
		GameObject go = Instantiate (Resources.Load ("Prefabs/StanzaObject")) as GameObject;
		go.transform.SetParent(canvasTransform);
		go.transform.localScale = new Vector3(1,1,1);
		RectTransform trans = go.GetComponent<RectTransform>();
		//trans.position=new Vector3(0,0,0);
		trans.position = new Vector3((x+26.59184f),92.0f,0);

		trans.anchoredPosition = new Vector3(x, y,0);
    
        go.GetComponent<StanzaObject>().stanzaManager = GameObject.Find("Canvas").GetComponent<GStanzaManager>();
		stanzaObjects.Add (go);
		return go.GetComponent<StanzaObject>();
	}

	/// <summary>
	/// Creates TinkerText for a word at a position.
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="parent">Parent stanza of the TinkerText.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="textToPrint">Text to print (word).</param>
	/// <param name="fontSize">Font size.</param>
	/// <param name="textColor">Text color.</param>
	GTinkerText CreateText( StanzaObject parent, float x, float y, string textToPrint, int fontSize, Color textColor)
	{
		GameObject UItextGO = new GameObject("Text_"+textToPrint);
		UItextGO.transform.SetParent(parent.transform);
       // Debug.Log(anim.runtimeAnimatorController);
        Text text = UItextGO.AddComponent<Text>();

		text.text = textToPrint;
		text.fontSize = storyBookJson.textFontSize;
		text.color = textColor;
		text.font = font;
		text.transform.localScale = new Vector3(1,1,1);

		//used for fitting the text box to the size of text.
		ContentSizeFitter csf= UItextGO.AddComponent<ContentSizeFitter> ();
		csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

		VerticalLayoutGroup vlg = UItextGO.AddComponent<VerticalLayoutGroup> ();
		vlg.childControlHeight = true;
		vlg.childControlWidth = true;


		RectTransform trans = UItextGO.GetComponent<RectTransform>();
		text.alignment = TextAnchor.UpperLeft;
		trans.anchoredPosition = new Vector3(x, y,0);
		UItextGO.GetComponent<RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate (trans);

		trans.anchoredPosition = new Vector3(x+trans.rect.width/2, y,0);
		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate (trans);

		width = width+trans.rect.width+minWordSpace;

		//add the animator and script to the word.
        UItextGO.AddComponent<Animator>().runtimeAnimatorController = Resources.Load("TextAnimations/textzoomcontroller") as RuntimeAnimatorController;
        GTinkerText tinkerText= UItextGO.AddComponent<GTinkerText>();
        tinkerText.stanza =UItextGO.GetComponentInParent<StanzaObject>();
        tinkerTextObjects.Add(UItextGO);
        return UItextGO.GetComponent<GTinkerText>();
	}

	/// <summary>
	/// Creates the game object.
	/// </summary>
	/// <param name="gameObjectData">Game object data.</param>
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

		if(gameObjectData.destroyOnCollision != "NIL"){
			var rigidbody = go.AddComponent<Rigidbody> ();
			rigidbody.isKinematic = true;
		}
		//add BoxCollider after adding the sprite for proper size!
		BoxCollider col = go.AddComponent<BoxCollider>();
		col.isTrigger = true;
		tinkerGraphicObjects.Add(go);

	}

    public void LoadAssetFromBundle(string name)
    {
      
       var prefab = bundleloaded.LoadAsset<GameObject>(name);
        Instantiate(prefab);     
    }

	/// <summary>
	/// Loads the asset image.
	/// </summary>
	/// <param name="name">Namevof the asset image.</param>
	/// <param name="sr">Sprite Renderer.</param>
    public void LoadAssetImage(string name,SpriteRenderer sr)
    {
        var sprite = bundleloaded.LoadAsset<Sprite>(name);
        sr.sprite = sprite;
    }

	/// <summary>
	/// Loads the animation images.
	/// </summary>
	/// <param name="tinkerGraphic">Tinker graphic.</param>
	/// <param name="startName">Start name.</param>
	/// <param name="length">Number of the animation frames.</param>
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
