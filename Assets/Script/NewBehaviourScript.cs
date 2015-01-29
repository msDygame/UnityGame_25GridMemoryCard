using UnityEngine;
//using UnityEditor;//只能放在 .\editor\ !
using System.Collections;
enum EnumPosition : int
{
	ROW1_1 = 0,
    ROW1_2 = 1,
    ROW1_3 = 2,
    ROW1_4 = 3,
    ROW1_5 = 4,
    ROW2_1 = 5,
    ROW2_2 = 6,
    ROW2_3 = 7,
    ROW2_4 = 8,
    ROW2_5 = 9,
    ROW3_1 = 10,
    ROW3_2 = 11,
    ROW3_3 = 12,
    ROW3_4 = 13,
    ROW3_5 = 14,
    ROW4_1 = 15,
    ROW4_2 = 16,
    ROW4_3 = 17,
    ROW4_4 = 18,
    ROW4_5 = 19,
    ROW5_1 = 20,
    ROW5_2 = 21,
    ROW5_3 = 22,
    ROW5_4 = 23,
    ROW5_5 = 24,
	MAX = 25,//max = row x column = 25
    ROW = 5,//橫排 或 列
    COLUMN = 5//直排 或 行
};

public class NewBehaviourScript : MonoBehaviour 
{
	//對付Home鍵用
	protected bool isPause = false ;
	protected bool isFocus = false ;
	//從30個相異物中取不重覆12個的組合
	protected static int iTotalSlot = (int)EnumPosition.MAX;
	protected static int iTotalSymbol = 30 ;
	protected static int iSelectedSymbol = 12 ;
	protected int[] iSymbolArray ; //selected symbol
	public Sprite spritesBack;//背景
	//public gameobject of perfab
	public GameObject[] mPublicGameObject = new GameObject[25] ;
	//current gameObject,for move,scale,changeTexture
    protected GameObject[] mCurrentGameObject ;
	//default position
    protected static Vector3[] vPosition ;
	//change texture by spriteRender 
	protected SpriteRenderer spriteRenderer;
	//symbol sprite Array for change texture 
	public Sprite[] SpritesArray = new Sprite[iTotalSymbol];//total symbol,public for unity set
	//screen scale
	private Vector2 scale ;
	private float baseW = 1024.0f;
	private float baseH =  768.0f;
	//display gameResult(結算畫面)
	protected string sCount = "" ;//temp
	protected int m_iCount = 0 ;//次數
	protected float fTimerPressed= 0.05f;//temp
	protected float fTimerPassed = 0.0f ;//使用時間
	protected float fRemainTimer = 0.0f ;//剩餘時間=可用時間-使用時間
	protected float fTotalGameTime = 180.0f ;//可用時間
	protected int  miScore = 0 ;//分數
	//flag
	protected bool bGameOver = false ;//剩餘時間為0時
	protected bool bGameComplete = false ;//完成
	protected bool bClick1Pressed = false ;
	protected bool bClick2Pressed = false ;
	protected int  iClick1Index = 0 ;
	protected int  iClick2Index = 0 ;
	protected bool bDialogPressed = false ;//true=OK,flase=cancel
	//touch input by android/ios(觸控螢幕,android手機用以取代pc的上下左右)
	protected string sTouch = "" ;//Touched GameObject name
	protected string sDebug1= "" ;
	protected string sDebug2= "" ;
	protected string sDebug3= "" ;
	protected string sDebug4= "" ;
	protected string sDebug5= "" ;
	void Awake()
	{
        mCurrentGameObject = new GameObject[25];
		vPosition = new Vector3[25];
		vPosition[0] = new Vector3(-2.7f , 4.0f , 0.0f) ;
		vPosition[1] = new Vector3(-2.7f , 2.0f , 0.0f) ;
		vPosition[2] = new Vector3(-2.7f , 0.0f , 0.0f) ;
		vPosition[3] = new Vector3(-2.7f ,-2.0f , 0.0f) ;
		vPosition[4] = new Vector3(-2.7f ,-4.0f , 0.0f) ;
		vPosition[5] = new Vector3(-1.35f, 4.0f , 0.0f) ;
		vPosition[6] = new Vector3(-1.35f, 2.0f , 0.0f) ;
		vPosition[7] = new Vector3(-1.35f, 0.0f , 0.0f) ;
		vPosition[8] = new Vector3(-1.35f,-2.0f , 0.0f) ;
		vPosition[9] = new Vector3(-1.35f,-4.0f , 0.0f) ;
		vPosition[10]= new Vector3( 0.0f , 4.0f , 0.0f) ;
		vPosition[11]= new Vector3( 0.0f , 2.0f , 0.0f) ;
		vPosition[12]= new Vector3( 0.0f , 0.0f , 0.0f) ;
		vPosition[13]= new Vector3( 0.0f ,-2.0f , 0.0f) ;
		vPosition[14]= new Vector3( 0.0f ,-4.0f , 0.0f) ;
		vPosition[15]= new Vector3( 1.35f, 4.0f , 0.0f) ;
		vPosition[16]= new Vector3( 1.35f, 2.0f , 0.0f) ;
		vPosition[17]= new Vector3( 1.35f, 0.0f , 0.0f) ;
		vPosition[18]= new Vector3( 1.35f,-2.0f , 0.0f) ;
		vPosition[19]= new Vector3( 1.35f,-4.0f , 0.0f) ;
		vPosition[20]= new Vector3( 2.7f , 4.0f , 0.0f) ;
		vPosition[21]= new Vector3( 2.7f , 2.0f , 0.0f) ;
		vPosition[22]= new Vector3( 2.7f , 0.0f , 0.0f) ;
		vPosition[23]= new Vector3( 2.7f ,-2.0f , 0.0f) ;
		vPosition[24]= new Vector3( 2.7f ,-4.0f , 0.0f) ;        
        iSymbolArray = new int[iSelectedSymbol];
	}
	// Use this for initialization
	void Start () 
	{

        for (int i = 0; i < (int)EnumPosition.MAX; i++)
        {
            int iIndex = (int)EnumPosition.ROW1_1 + i ;
            //default Instantiate(clone gameobject)
			mCurrentGameObject[iIndex] = GameObject.Instantiate(mPublicGameObject[iIndex], vPosition[iIndex], transform.rotation) as GameObject;
			//default scale of gameobject
			mCurrentGameObject[iIndex].transform.localScale = new Vector3(0.45f , 0.45f , 1.0f);
        }		
		//game start
		ResetGame() ;
		isPause = false ;
		isFocus = false ;
	}
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			//user pressed back key
			Application.Quit() ;
		}		
		if (Input.GetKeyDown(KeyCode.Menu)) 
		{
			//user pressed menu key
		}
		//觸控輸入
		if ((bGameOver == false) && (bGameComplete == false)) 
		{
			TouchUpdate() ;		
			// key down delay
			// if (Time.time < fNextPressed) return ;
			fTimerPassed = Time.time - fTimerPressed;
			fRemainTimer = fTotalGameTime - fTimerPassed ;
			if (fRemainTimer <= 0.0f)
			{ 
				bGameOver = true ; 
				miScore = 0 ;
			}
			//
			if (Input.GetAxis("Vertical") > 0.2)
			{				
			}
			else if (Input.GetAxis ("Horizontal") > 0.2)
			{				
			}
			else if (Input.GetButtonDown("left"))
			{
			}
			else if (Input.GetButtonDown("down"))
			{
			}
			else if (Input.GetButtonDown("right"))
			{
			}
			else if (Input.GetButtonDown("up"))
			{
			}
			//check if flip delayed
			Delayed() ;
		}
		else 
		{
			//if (bGameOver == true) 
			//if (bGameComplete == true)
			//	ResetGame() ;
			Application.Quit() ;
		}
	}
	//
	void OnGUI()
	{
		scale = new Vector2(Screen.width/baseW,Screen.height/baseH);		
		GUIUtility.ScaleAroundPivot(scale , Vector2.zero);
		//
		GUIStyle gsLabel = new GUIStyle();
		//check if game complete
		if (bGameComplete)
		{
			sCount = "Congratulation!!" ;
			gsLabel.normal.textColor = Color.magenta ;
			gsLabel.fontSize = 36 ;
			//
			GUI.Label(new Rect(280,160,220,80),sCount,gsLabel) ;
			sCount = "結算畫面:" ;
			GUI.Label(new Rect(280,210,220,80),sCount,gsLabel) ;
			string stringstr = (fTimerPassed).ToString("##0.0");
			sCount = "使用秒數:" + stringstr + "秒."  ;
			GUI.Label(new Rect(280,260,220,80),sCount,gsLabel) ;
			sCount = "翻轉次數:" + m_iCount + "次."  ;
			GUI.Label(new Rect(280,310,220,80),sCount,gsLabel) ;
			sCount = "得分:" + miScore + "分."  ;
			GUI.Label(new Rect(280,360,220,80),sCount,gsLabel) ;
		}
		else
		{
			//check if game over
			if (bGameOver == true)
			{
				sCount = "Timeout!" ;
				gsLabel.fontSize = 36 ;
				gsLabel.normal.textColor = Color.red ;
				GUI.Label(new Rect(280,160,220,80),sCount,gsLabel) ;
				sCount = "結算畫面:" ;
				GUI.Label(new Rect(280,210,220,80),sCount,gsLabel) ;
				string stringstr = (fTimerPassed).ToString("##0.0");
				sCount = "使用秒數:" + stringstr + "秒."  ;
				GUI.Label(new Rect(280,260,220,80),sCount,gsLabel) ;
				sCount = "翻轉次數:" + m_iCount + "次."  ;
				GUI.Label(new Rect(280,310,220,80),sCount,gsLabel) ;
				sCount = "得分:" + miScore + "分."  ;
				GUI.Label(new Rect(280,360,220,80),sCount,gsLabel) ;
			}
			else
			{
				string stringstr = (fRemainTimer).ToString("##0.0"); //result: 567.8
				sCount = "" + stringstr + "秒" ;
				gsLabel.fontSize = 24 ;
				gsLabel.normal.textColor = Color.black ;
				GUI.Label(new Rect(3, 10,220,80),sCount,gsLabel) ;
				//deubg
				GUI.Label(new Rect(3, 60,220,80),sDebug1,gsLabel) ;
				GUI.Label(new Rect(3,110,220,80),sDebug2,gsLabel) ;
				GUI.Label(new Rect(3,160,220,80),sDebug3,gsLabel) ;
				GUI.Label(new Rect(3,210,220,80),sDebug4,gsLabel) ;
				GUI.Label(new Rect(3,260,220,80),sDebug5,gsLabel) ;
			}
		}
	}
	//
	void FixedUpdate() 
	{
		//有效!
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitObject = Physics2D.Raycast(tapPoint, -Vector2.up);
            if (hitObject)
            {
                sTouch = hitObject.collider.name + "TEST2";
            }
        }
	}
	//Home鍵押下強制暫停時，先 OnApplicationPause，後 OnApplicationFocus；
	//重新「啟動」手機時，先OnApplicationFocus，後 OnApplicationPause；
	void OnApplicationPause()
	{
		// 強制暫停時，事件
		if (isPause == false)
		{		  
			Application.Quit() ;//不用回復了..QUIT!
		}		
		else//回復
		{			
			isFocus = true;			
		}
	}
	//
	void OnApplicationFocus()
	{
		// 「啟動」手機時，事件
		if(isFocus)			
		{			
			isPause=false;			
			isFocus=false;
		}
		//暫停
		if(isPause)			
		{
			isFocus=true;
		}
	}
	//觸碰螢幕 Android/iOS,PC會觸發MouseDown
	//目標物件必需要有Box2DCollider!!
	public void TouchUpdate()
	{
		//手機時不處理..
#if (!UNITY_ANDROID)
		//有效!
		RaycastHit2D hitII = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);	
		if(hitII.collider != null)			
		{
			//呃..是手機時,會一直觸發...
			sTouch = hitII.collider.name ;
			OnClick(sTouch) ;
		}
#endif
		//以下script加在Camera下面 
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)//觸控按鈕 - 移出	
		{
		}
		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)//觸控按鈕 - 按住
		{
			sTouch = "" ;
			Vector3 mousePosition ;
			mousePosition.x = Input.GetTouch(0).position.x ;
			mousePosition.y = Input.GetTouch(0).position.y ;
			mousePosition.z = camera.nearClipPlane ; //Mathf.Infinity;
			RaycastHit2D hitIII = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);//-Vector2.up);
			if(hitIII.collider != null)			
			{
				//Ray has been Cast and hit an Object
				sTouch = hitIII.collider.name;
				OnClick(sTouch) ;
			}
		}
		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)//觸控按鈕 - 滑動
		{

		}
	}   
	//Reset
	public void ResetGame()
	{
		//random game
		CheckReset() ;
		//reset counter
		m_iCount = 0 ;//need after RandomTile()
		//reset timer
		fTimerPressed = Time.time ;
		fTimerPassed = 0.0f ;
		fRemainTimer = 0.0f ;
		//reset flag
		bGameOver = false ;
		bGameComplete = false ;
		bClick1Pressed = false ;
		bClick2Pressed = false ;
		iClick1Index = -1 ;
		iClick2Index = -1 ;
		bDialogPressed = false ;
	}
	//
	public void QuitGame()
	{
		Application.Quit() ;
	}
	// Update is called once per frame
	public void OnClick(string sObjectName) 
	{
		int iIndex = -1 ;
			 if (string.Compare(sObjectName, "NewSprite11(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW1_1;
		else if (string.Compare(sObjectName, "NewSprite12(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW1_2;
		else if (string.Compare(sObjectName, "NewSprite13(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW1_3;
		else if (string.Compare(sObjectName, "NewSprite14(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW1_4;
		else if (string.Compare(sObjectName, "NewSprite15(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW1_5;
		else if (string.Compare(sObjectName, "NewSprite21(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW2_1;
		else if (string.Compare(sObjectName, "NewSprite22(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW2_2;
		else if (string.Compare(sObjectName, "NewSprite23(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW2_3;
		else if (string.Compare(sObjectName, "NewSprite24(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW2_4;
		else if (string.Compare(sObjectName, "NewSprite25(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW2_5;
		else if (string.Compare(sObjectName, "NewSprite31(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW3_1;
		else if (string.Compare(sObjectName, "NewSprite32(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW3_2;
		else if (string.Compare(sObjectName, "NewSprite33(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW3_3;
		else if (string.Compare(sObjectName, "NewSprite34(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW3_4;
		else if (string.Compare(sObjectName, "NewSprite35(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW3_5;
		else if (string.Compare(sObjectName, "NewSprite41(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW4_1;
		else if (string.Compare(sObjectName, "NewSprite42(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW4_2;
		else if (string.Compare(sObjectName, "NewSprite43(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW4_3;
		else if (string.Compare(sObjectName, "NewSprite44(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW4_4;
		else if (string.Compare(sObjectName, "NewSprite45(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW4_5;
		else if (string.Compare(sObjectName, "NewSprite51(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW5_1;
		else if (string.Compare(sObjectName, "NewSprite52(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW5_2;
		else if (string.Compare(sObjectName, "NewSprite53(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW5_3;
		else if (string.Compare(sObjectName, "NewSprite54(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW5_4;
		else if (string.Compare(sObjectName, "NewSprite55(Clone)" , true) == 0) iIndex = (int)EnumPosition.ROW5_5;
		//click outside or click unknown(?) gameobject
		else return ;
		//	
		OnClick(iIndex) ;
	}
	public void OnClick(int iIndex) 
	{
		if (iIndex < 0) return ;
		int iState = getTileState(iIndex) ;
		//click 1st tile
		if (bClick1Pressed == false)
		{
			if (iState == (int)EnumTileState.HIDE)
			{
				bClick1Pressed = true ;
				iClick1Index = iIndex ;
				setTileState(iIndex ,(int)EnumTileState.FLIP) ;
				m_iCount++ ;
			}
			else
			{
				//pressed tile already turn over
				return ;//nothing happen..
			}
		}		
		else
		{
			//bClick1Pressed == true,click 2nd tile
			if (bClick2Pressed == false)
			{
				if (iState == (int)EnumTileState.HIDE)
				{
					bClick2Pressed = true ;
					iClick2Index = iIndex ;
					setTileState(iIndex ,(int)EnumTileState.FLIP) ;
					m_iCount++ ;
				}
				else
				{
					//pressed tile already turn over
					return ;//nothing happen..
				}
			}
			else
			{
				//already pressed 2 tiles
				return ;//nothing happen..
			}
		}		
	}
	//
	public bool checkWin()
	{
		bool bFinish = true ;
		for (int i = 0 ; i < 25 ; i++)
		{		
			int iState = getTileState(i) ;
			if (iState != (int)EnumTileState.SHOW) { bFinish = false ; break ; }
		}
		return bFinish ;
	}
	//
	public void Delayed() 
	{
		for (int i = 0 ; i < 25 ; i++)
		{		
			int iState = getTileState(i) ;
			if (iState == (int)EnumTileState.HIDE) continue ;//nothing happen..
			//flip tile
			if (iState == (int)EnumTileState.FLIP)
			{
				if (mCurrentGameObject[i].transform.eulerAngles.y < 180)
				{	// Rotate picture 1 until 180 deg.
					mCurrentGameObject[i].transform.Rotate(0, 90 * Time.deltaTime, 0);// Rotate picture 1 on Y axis 
					if ((mCurrentGameObject[i].transform.eulerAngles.y > 90) && (mCurrentGameObject[i].transform.eulerAngles.y < 120))
					{
						//change real symbol
						int iSymbol = getTileSymbol(i) ;
						setTileSprite(i , iSymbol) ;
					}
				}
				else
				{
					Vector3 v = new Vector3(0,180,0);
					mCurrentGameObject[i].transform.eulerAngles = v ;// Prevent picture to rotate more then 180 deg.
					// You can't open first picture again
					setTileState(i ,(int)EnumTileState.SHOW );
					//check if game complete
					bGameComplete = checkWin() ;
					if (bGameComplete)
					{
						miScore = (int)(fRemainTimer * 10) - m_iCount ;
						if (miScore<0) miScore = 0 ;
					}
				}
			}
			//flip back tile(turn over)
			else if (iState == (int)EnumTileState.TURN_OVER)
			{
				if (mCurrentGameObject[i].transform.eulerAngles.y < 355 && mCurrentGameObject[i].transform.eulerAngles.y > 10)
				{
					mCurrentGameObject[i].transform.Rotate(0, 300 * Time.deltaTime, 0);
					if ((mCurrentGameObject[i].transform.eulerAngles.y > 270) && (mCurrentGameObject[i].transform.eulerAngles.y < 300))
					{		
						SetDefaultSprite(i) ;
					}
				}
				else
				{
					Vector3 v = new Vector3(0,0,0);
					mCurrentGameObject[i].transform.eulerAngles = v ;
					setTileState(i ,(int)EnumTileState.HIDE );
					//reset
					if (i == iClick1Index)
					{					
						iClick1Index = -1 ;
						bClick1Pressed = false ;
					}
					else if (i == iClick2Index)
					{
						iClick2Index = -1 ;
						bClick2Pressed = false ;
					}
				}
			}
			else if(iState == (int)EnumTileState.SHOW)
			{
				//pressed 2 tile already.
				if ((bClick1Pressed == true) && (bClick2Pressed == true))
				{
					if ((i != iClick1Index) && (i != iClick2Index)) continue ;//it is NOT your business!
					//
					int iState1 = getTileState(iClick1Index) ;
					int iState2 = getTileState(iClick2Index) ;
					//check 2 tile fliped already..
					if ((iState1 == (int)EnumTileState.FLIP) || (iState2 == (int)EnumTileState.FLIP))
					{
						//already 2 tile pressed,ad 1 is SHOW,but andother still flip..
						continue ;//wait it!nothing happen..
					}
					else//both tiles are SHOW
					{
						//check if selected is pair
						bool bCheck = CompareTile(iClick1Index, iClick2Index) ; 
						if (bCheck == false)
						{
							setTileState(iClick1Index , (int)EnumTileState.TURN_OVER ) ;
							setTileState(iClick2Index , (int)EnumTileState.TURN_OVER ) ;
						}
						else
						{					
							setTileState(iClick1Index , (int)EnumTileState.SHOW ) ;
							setTileState(iClick2Index , (int)EnumTileState.SHOW ) ;
							//reset
							iClick2Index = -1 ;
							iClick1Index = -1 ;
							bClick2Pressed = false ;
							bClick1Pressed = false ;
						}
					}
				}
				else
				{
					//pressed 1 tile only,wait another!
					continue ;//nothing happen..
				}
			}
		}
	}
	//Compare
	public bool CompareTile(int iSource,int iTarget)
	{
		if (iSource < 0) return false ;
		if (iTarget < 0) return false ;
		int iSymbolSrc = getTileSymbol(iSource);
		int iSymbolDes = getTileSymbol(iTarget);
		bool bCompare  = (iSymbolSrc == iSymbolDes) ? true : false ;
		return bCompare ;
	}	
	//UI
	public void setTileSprite(int iSourceTile , int iSymbolIndex)
	{
		if (iSourceTile < 0) return ;
		spriteRenderer = mCurrentGameObject[iSourceTile].GetComponent<SpriteRenderer>() ;
		spriteRenderer.sprite = SpritesArray[iSymbolIndex] ;		
	}
	//UI
	public void SetDefaultSprite(int iSourceTile)
	{
		if (iSourceTile < 0) return ;
		spriteRenderer = mCurrentGameObject[iSourceTile].GetComponent<SpriteRenderer>() ;
		spriteRenderer.sprite = spritesBack ;
	}
	//UI
	public int getTileSymbol(int iSourceTile)
	{
		if (iSourceTile < 0) return -1 ;
		TilePosition pTilePosition ;
		pTilePosition = mCurrentGameObject[iSourceTile].GetComponent<TilePosition>() ;
		//get
		return (pTilePosition.TileSymbol) ;
	}
	//UI
	public void setTileSymbol(int iSourceTile , int iTargetSymbol)
	{
		if (iSourceTile < 0) return ;
		TilePosition pTilePosition ;
		pTilePosition = mCurrentGameObject[iSourceTile].GetComponent<TilePosition>() ;
		//set
		pTilePosition.TileSymbol = iTargetSymbol ;
	}
	//UI
	public int getTileState(int iSourceTile)
	{
		if (iSourceTile < 0) return -1 ;
		TilePosition pTilePosition ;
		pTilePosition = mCurrentGameObject[iSourceTile].GetComponent<TilePosition>() ;
		//get
		return (pTilePosition.TileState) ;
	}
	//UI
	public void setTileState(int iSourceTile , int iTargetState)
	{
		if (iSourceTile < 0) return ;
		TilePosition pTilePosition ;
		pTilePosition = mCurrentGameObject[iSourceTile].GetComponent<TilePosition>() ;
		//set
		pTilePosition.TileState = iTargetState ;
	}
	//
	public void CheckReset()
	{
		int iDrawableCount = 15 ;
		int iRandomCount = 12 ;
		int iPositionCount = 25 ;
		int[] iSource = new int[iRandomCount];
		int[] iPosition = new int[iPositionCount] ;
		Random.seed = System.Guid.NewGuid().GetHashCode();
		//30取12
		for (int i = 0 ; i < iRandomCount ; /**/)
		{
			bool bFound = false ;//reset
			int iRandom = Random.Range(0,iDrawableCount) ;//產生1個0~29間的值
			//random symbol
			for(int j = 0 ; j < i ; j++) 
			{
				if (iSource[j] == iRandom) bFound = true ;
			}
			//symbolArray already have symbol..find again!
			if (bFound == true) continue ;
			//
			iSource[i] = iRandom ;
			i++ ;
		}
		//clear
		for (int i = 0 ; i < iPositionCount ; i++) iPosition[i] = 0 ;//defualt 0
		//set position,每組2個..會多1個,故從0~23
		for (int i = 0 ; i < (iPositionCount-1) ; /**/) 
		{
			bool bFound = false ;
			int iRandom = Random.Range(0,iPositionCount) ;
			//random position
			if (iPosition[iRandom] == 0) bFound = true ;
			//the position already have symbol , find again!
			if (bFound == false) continue ;
			//symbol從0~11
			int iSymbol = i / 2 ;//每組2個,即兩個position會用同一個symbol
			iPosition[iRandom] = iSource[iSymbol] ;
			i++ ;
		}
		//set default
		for (int i = 0 ; i < iPositionCount ; i++)
		{
			setTileSymbol(i , iPosition[i]) ;
			setTileState(i , (int)EnumTileState.HIDE) ;		
			SetDefaultSprite(i);
		}
	}
}