using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadAssetExample : MonoBehaviour {
    static AssetBundle bundleloaded;
    private string[] allPagesJsons;
    public static StoryBookJson storyBookJson;
    public static int pageNumber;
	public GStanzaManager stanzaManager;

	private string[] allStanzaJsons;
	private string page;

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
        LoadPageData(pageNumber);
	
		LoadStanzaData();
		TokenizeStanza();
    }
    public void LoadPageData(int pageNo)
    {
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
   

	public void LoadStanzaData()
	{
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
		return go.GetComponent<StanzaObject>();
	}

	GTinkerText CreateText( StanzaObject parent, float x, float y, string textToPrint, int fontSize, Color textColor)
	{
		GameObject UItextGO = new GameObject("Text2");
		UItextGO.transform.SetParent(parent.transform);

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

		UItextGO.AddComponent<GTinkerText> ();
		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate (trans);
		trans.pivot = new Vector2 (0,1);

		width = width + trans.rect.width + minWordSpace;
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
        
        if (gameObjectData.anim.Length >0)
        {
            go.AddComponent<PngToAnim>();
            Anim anim = gameObjectData.anim[0];
            LoadAssetImages(anim.animName, anim.numberOfImages);
            go.GetComponent<PngToAnim>().PlayAnimation(0, 0.25f, anim.isLooping);
        }
        else
        {
            LoadAssetImage(gameObjectData.imageName, go.GetComponent<SpriteRenderer>());
        }
        go.AddComponent<GTinkerGraphic>();
        go.GetComponent<GTinkerGraphic>().dataTinkerGraphic = gameObjectData;
        go.GetComponent<GTinkerGraphic>().sceneManager = GameObject.Find("SceneManager").GetComponent<GSManager>();
        go.GetComponent<GTinkerGraphic>().myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Debug.Log(gameObjectData.draggable+""+this);
        go.GetComponent<GTinkerGraphic>().SetDraggable(gameObjectData.draggable);//go.AddComponent<Collider>();
        BoxCollider col = go.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector2(1, 1);


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

    public void LoadAssetImages(string startName,int length)
    {
        
        PngToAnim.sprites= new Sprite[length];
        for (int i = 0; i < length; i++)
        {
            var sprite = bundleloaded.LoadAsset<Sprite>(startName+"-"+(i+1));
            PngToAnim.sprites[i] = sprite;
        
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

