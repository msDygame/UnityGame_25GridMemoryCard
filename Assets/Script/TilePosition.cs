using UnityEngine;
using System.Collections;
enum EnumTileState : int
{
    HIDE = 0,
    FLIP = 1,//翻正面    
    SHOW = 2,
	TURN_OVER = 3,//翻回背面
    MAX = 4
};

public class TilePosition : MonoBehaviour 
{
    protected int m_iTileState = 0 ;
    public int TileState
    {
		set { m_iTileState = value; }
		get { return m_iTileState ; }
    }
	protected int m_iTileSymbol = 0 ;
    public int TileSymbol
	{
		set { m_iTileSymbol = value; }
		get { return m_iTileSymbol ; }
	}
	//
	void Awake()
	{
		m_iTileSymbol = 0 ;
		m_iTileState = (int)EnumTileState.HIDE;
	}
	// Use this for initialization
	void Start () 
	{

	}	
	// Update is called once per frame
	void Update () 
	{	
	}
	//
	void OnGUI()
	{
	}
}
