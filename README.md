# TinkrTeam
Repository for the Tinkr Player and Tinkr Books in collaboration with DISQ

# Player Overview
(Ben will write).
## Shelf

 Including a list of what scripts and files are used in creating the Shelf and how those interact.

#### Files Used:
1. Shelf Scripts <br>
Path: TinkrTeam\TinkrShelf\Assets\Scripts <br>

Script Name | Description | Attached to which game object 
--- | --- | ---
ShelfManager.cs | Main contoller script of shelf and contains all functions performed in shelf  | Canvas
BookObject.cs | Its a book object class for book.cs | Every child of books_wheel game object
BookCollision.cs | It extends shelf manager and uses colliders to detect position of books in shelf | Every child of books_wheel game object

2. Shelf Images <br>
Path: TinkrTeam\TinkrShelf\Assets\Resources\Thumbnail

#### Interaction:
```javascript 
ShelfManager.cs -> BookObject.cs -> BookCollision.cs

 1. Load initial/local asset bundle on shelf scene
 2. Call server manifest file with timeout of 1.0sec.

 3. if (server manifest is newer than local manifest) 
         use server manifest         
    else
         use local manifest
     
 4.  LoadShelfData();
     LoadInitialCenterBook();
       
 5. if( clicked on Image || Title || center book)
        it ll load scene asynchronusly from "Books/Decodable/CatTale/Common/Scenes/Scene01"
 6. if (clicked on any other book other than center)
        rotate the books clockwise/anticlockwise

 ```

## Player
Section for the "Player" including what scripts parse the Tinkr Book JSON, display the book, and listing all the interactive function scripts.

Json related Scripts:
Script Name | Description 
--- | --- 
JsonHelper.cs|all functions needed to parse json
Anim.cs| Parse anim related info from respective book json to this class
AudioClass.cs|Parse audio related info from respective book json to this class
GameObjectClass.cs|Parse game objects related info from respective book json to this class
Movable.cs|Parse movable gameobject related info from respective book json to this class
PageClass.cs|Parse page related info from respective book json to this class
Sequence.cs|Parse animation sequence related info from respective book json to this class
StoryBookJson.cs|Parse storybook related info from respective book json to this class
TextClass.cs|Parse text related info from respective book json to this class
TimeStampClass.cs|Parse time stamp of audio related info from respective book json to this class
TriggerClass.cs|Parse triggers related info from respective book json to this class

Other Major Scripts:

Script Name | Description | Attached to which game object 
--- | --- | ---
GGameManager.cs|Script responsible for controling scenes,touch events and home menu |GameManager
GGameManagerInitializer.cs| Initialize game manager instance|GameManagerInitializer
LoadAssetFromJSON.cs|Script to load the scene based on JSON describing the book.|Canvas
GStanzaManager.cs|Script responsible for all stanza related functions|Canvas
GSManager.cs|Script responsible for controling the scenes.|SceneManager0
StanzaObject.cs|its a stanza object class for GStanzaManager|StanzaObject(Clone)
GTinkerText.cs|Script responsible for every function of the text.|every child of stanzaobject
GTinkerGraphic.cs|Script responsible for every function of the graphic.|graphic_game_object





## Creating a New Tinkr Book
Section for "Creating a New Tinkr Book" to detail what needs to be done in creating a new Tinkr Book
### Recipe to make Tinkrbook 


##### 1. Setting the folder hierarchy right. 
StepNo | Description | How
--- | --- |---
1.1|Create a folder at this path with title of the book example "xyzTale" | Path:TinkrTeam\TinkrShelf\Assets\Books\Decodable
1.2|Create 6 folders at this path with level-wise-name of the book example "xyzTaleLevel0"..and soon.|Path:TinkrTeam\TinkrShelf\Assets\Books\Decodable\xyzTale\
1.3|Create a folder with name "Common" at the following path(it refers to common assets and ll contain 5 subfolders)|Common/AnimPNGs; Common/Audio; Common/Images; Common/Scenes 

##### 2. Creating book specific json and other scripts
Step No | Description | How
--- | --- |---
1.|Creating Book specific JSON| Refer any book json at this path "TinkrTeam\TinkrShelf\Assets\Books\Decodable\CatTale\catstorylevel2\Resources" and modify values at R.H.S according to proper value,positioning ,timing, scaling of game object and add corresponding script to it as needed.
2.|Updating manifest|Fill in new book details in shelf manifest at this path: TinkrTeam\TinkrShelf\Assets\Resources\Manifests  
3.|books assets path|TinkrTeam\TinkrShelf\Assets\Books\Decodable\xyzTale\xyzTalelevelx
4.|shelf assets path|TinkrTeam\TinkrShelf\Assets\Resources\Thumbnail

#### what led to specific scripts creation
Also detailing what functionality to date has required the creation of separate scripts rather than what is included in the collection of interactive functions.

## Data Collection
#### Files Used:
Script Name | Description | Path
--- | --- |---
FirebaseHelper.cs| Script responsible for logging events to firebase. | TinkrTeam\TinkrShelf\Assets\Scripts\Data\

#### Functions Used:
Functions Name | Description 
--- | --- |
AddBook |Assigns the value of book id to appID.
AddSection | Assigns the value of page number to secID.
LogInAppSection | Logs IN_APP_SECTION event to firebase. IN_APP_SECTION depicts time spent on a particular page.
LogInShelfSection | Logs IN_APP_SECTION event for shelf to firebase(special functions because shelf doesnot have section id!!!).
LogInAppTouch | IN_APP_TOUCH event depicts the touch on any graphic, text, button.
LogInShelfTouch |Logs IN_APP_TOUCH event for shelf to firebase(special functions because shelf doesnot have section id!!!) IN_APP_TOUCH event depicts the touch on any graphic, text, button.
LogInAppResponse | Logs the IN_APP_RESPONSE event to firebase. IN_APP_RESPONSE event depicts response given to any question asked in the Tinkbook.
	

##### Resources
1. [User defined firebase parameters](https://docs.google.com/document/d/1JYCuHP-1GYdxnnDzkVdkgTE318nCjmsSk5XnNTxM5BE/)
2. (Ben to include documentation on decisions made in collecting data in this section).

<a href="https://zenhub.com"><img src="https://raw.githubusercontent.com/ZenHubIO/support/master/zenhub-badge.png"></a>
