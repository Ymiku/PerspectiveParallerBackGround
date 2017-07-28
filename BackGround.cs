using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {
	private Vector2 _size = Vector2.zero;
	private float _width;
	public float interval;
	private int _arrayStartPos;
	private float _lastX;
	private Transform[] backs;
	private Transform cameraTrans;
	private Camera mainCamera;
	private float width;
	private float halfWidth;
	// Use this for initialization
	void Start () {
		mainCamera = CameraController.Instance.GetComponent<Camera> ();
		width =  (mainCamera.ViewportToWorldPoint (new Vector3(1f,0f,transform.position.z-mainCamera.transform.position.z)) - mainCamera.ViewportToWorldPoint (new Vector3(0f,0f,transform.position.z-mainCamera.transform.position.z))).x;
		halfWidth = width / 2f;
		cameraTrans = mainCamera.transform;
		GetSize ();
		Clone ();
	}
	void GetSize()
	{
		float sizeTemp;
		SpriteRenderer[] rendererArray =  GetComponentsInChildren<SpriteRenderer>(); 
		if (rendererArray.Length == 0) 
			return;
		for (int i = 0; i < rendererArray.Length; i++) {
			sizeTemp = -rendererArray [i].bounds.size.x/2f + rendererArray [i].transform.position.x;
			if (sizeTemp < _size.x||i==0) {
				_size = new Vector2 (sizeTemp,_size.y);
			}
			sizeTemp += rendererArray [i].bounds.size.x;
			if (sizeTemp > _size.y||i==0) {
				_size = new Vector2 (_size.x,sizeTemp);
			}
		}
		_size -= new Vector2 (transform.position.x,transform.position.x);
		_width = _size.y - _size.x;
	}
	void Clone()
	{
		int num = (int)(width/(_width+interval));
		num++;
		backs = new Transform[num+1]; 
		backs [0] = transform;
		for (int i = 0; i < num; i++) {
			GameObject t = BackGroundManager.Instance.CloneBackGround (gameObject);
			backs [i + 1] = t.transform;
		}
		SetOriPos ();
	}
	void SetOriPos()
	{
		int eachNum = (int)(backs.Length/2f);
		float intX;
		int backCount = 0;
		if (backs.Length % 2f == 0) {
			intX = cameraTrans.position.x + _width/2f - (interval + _width)*eachNum;
			for (int i = 0; i < backs.Length; i++) {
				backs [backCount].position = new Vector3 (intX,backs [backCount].position.y,backs [backCount].position.z);
				intX += (interval + _width);
				backCount++;
			}
		} else {
			intX = cameraTrans.position.x - interval/2f - (interval + _width)*eachNum;
			backCount = 0;
			for (int i = 0; i < backs.Length; i++) {
				backs [backCount].position = new Vector3 (intX,backs [backCount].position.y,backs [backCount].position.z);
				intX += (interval + _width);
				backCount++;
			}
		}
		_arrayStartPos = 0;
		_lastX = cameraTrans.position.x;
	}
	// Update is called once per frame
	void Update () {
		SerialUpdate ();
	}
	void SerialUpdate()
	{
		FitScreen ();
	}
	void FitScreen()
	{
		while (true) {
			if (backs [_arrayStartPos].position.x + _width / 2f + interval < cameraTrans.position.x - halfWidth) {
				backs [_arrayStartPos].position += new Vector3 (backs.Length*(_width+interval),0f,0f);
				StartPosAddOne ();
			} else {
				break;
			}
		}
		while (true) {
			if (backs [GetEndPos()].position.x - _width / 2f > cameraTrans.position.x + halfWidth) {
				backs [GetEndPos()].position -= new Vector3 (backs.Length*(_width+interval),0f,0f);
				StartPosReduceOne ();
			} else {
				break;
			}
		}
	}
	void StartPosAddOne()
	{
		_arrayStartPos++;
		if (_arrayStartPos >= backs.Length) {
			_arrayStartPos = 0;
		}
	}
	void StartPosReduceOne()
	{
		_arrayStartPos--;
		if (_arrayStartPos <= -1) {
			_arrayStartPos = backs.Length-1;
		}
	}
	int GetEndPos()
	{
		if (_arrayStartPos == 0)
			return backs.Length - 1;
		return _arrayStartPos - 1;
	}
}
