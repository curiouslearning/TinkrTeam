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

	public static string sceenscript; 


	Font font;
	Transform canvasTransform;

	private int i,j;
	float width=0.0f, startingX, startingY, startingXText, startingYText;
	float height = 32.94f;  //height of text:32.94
	private readonly float minWordSpace = 15.0f;
	private readonly float minLineSpace = 30.0f;
  
	private bool autoPlaying = false;
	private bool cancelAutoPlay = false;


    public void Awake()
    {

		startingX = -400.0f;
		startingY = 170.0f;   //abhi ke liye static
		startingXText = 0.0f;
		startingYText = 0.0f;
		font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		canvasTransform = this.transform;  //if this script attached to canvas; otherwise update this line to store canvas transform.

        if (!bundleloaded)
        {
            bundleloaded = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/5pageproxy"));
            if (bundleloaded == null)
            {
                Debug.Log("Failed to load AssetBundle!");

            }
        }
        LoadStoryData("5PageProxy.json");
    }
    void Start () {
       
    }
    private void LoadStoryData(string fileName)
    {
		pageNumber = 0;
        TextAsset charDataFile = bundleloaded.LoadAsset(fileName) as TextAsset;
        string json = charDataFile.ToString();
        storyBookJson = JsonUtility.FromJson<StoryBookJson>(json);
		LoadCompletePage ();

    }
	public void LoadNextPage()
	{
		Destroy(GameObject.Find("SceneManager"+(pageNumber)));
		pageNumber++;
		if (pageNumber > 4) {
			right.SetActive (false);
		}
		EmptyPage ();
		LoadCompletePage ();
	}
	public void LoadPreviousPage()
	{

		Destroy(GameObject.Find("SceneManager"+(pageNumber)));
		pageNumber--;
		if(pageNumber<0)
		{left.SetActive (false);
		}
		EmptyPage ();
		LoadCompletePage ();
	}
	public void EmptyPage()
	{for(int i=0;i<tinkerGraphicObjects.Count;i++)
		{Destroy (tinkerGraphicObjects [i]);
		}
		for (int j=0;j<stanzaObjects.Count;j++)
		{Destroy (stanzaObjects[j]);
		}



	
	}
	public void LoadCompletePage()
	{   
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
		sceenscript =storyBookJson.pages [pageNumber].script;
		Debug.Log ("values" + pageNumber + sceenscript);
		go.AddComponent(Type.GetType(sceenscript));
		GameObject.Find ("Canvas").GetComponent<GStanzaManager> ().sceneManager = GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager>();
		GameObject.Find("SceneManager"+pageNumber).GetComponent<GSManager>().stanzaManager=GameObject.Find("Canvas").GetComponent<GStanzaManager>();
		GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ().myCanvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();
		GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ().Lbutton = GameObject.FindWithTag ("left_arrow");
		GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ().Rbutton = GameObject.FindWithTag ("right_arrow");
		GameObject.Find ("GameManager").GetComponent<GGameManager> ().sceneManager = GameObject.Find ("SceneManager"+pageNumber).GetComponent<GSManager> ();
	
	}

    public void LoadStanzaAudio()
    {
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
	{   startingY = 170.0f;
		stanzaManager.stanzas.Clear ();
		j =0;
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
		GameObject UItextGO = new GameObject("Text2");
		UItextGO.transform.SetParent(parent.transform);
       // Debug.Log(anim.runtimeAnimatorController);
        Text text = UItextGO.AddComponent<Text>();
		text.text = textToPrint;
		text.fontSize = fontSize;
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
		trans.pivot = new Vector2 (0,1);

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
		Debug.Log(gameObjectData.draggable+""+this);
		go.GetComponent<GTinkerGraphic>().SetDraggable(gameObjectData.draggable);//go.AddComponent<Collider>();
		BoxCollider col = go.AddComponent<BoxCollider>();
		col.isTrigger = true;
		col.size = new Vector2(1, 1);
        if (gameObjectData.anim.Length >0)
        {

			Anim anim = gameObjectData.anim[0];
			LoadAssetImages(go.GetComponent<GTinkerGraphic>(), anim.animName, anim.numberOfImages);

			if (gameObjectData.anim[0].movable.speed!=0)
			{
				Movable movable = gameObjectData.anim[0].movable;
				go.GetComponent<GTinkerGraphic>().PlayAnimation(0, 0.25f, anim.isLooping, movable);
			}
			else
			{
				Debug.Log("helllooooooooo");
				go.GetComponent<GTinkerGraphic>().PlayAnimation(0, 0.25f, anim.isLooping, null);
			}
		}
		else
		{
			LoadAssetImage(gameObjectData.imageName, go.GetComponent<SpriteRenderer>());
		}
        
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

	public void LoadAssetImages(GTinkerGraphic tinkerGraphic,string startName,int length)
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

