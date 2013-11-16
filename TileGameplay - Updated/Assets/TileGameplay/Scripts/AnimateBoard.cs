using UnityEngine;
using System.Collections;

public class AnimateBoard : MonoBehaviour {

	private GameObject camera;
	
	//RotateAroundCamera
	private float rotateSpeed  = 45.0f;
	public Transform cameraPivotPoint;
	private bool rotating = false;
	private int rotateCount = 0;		//60 = 1 second of rotation
	private int rotatePosition = 0;		//0, 1, 2, 3 for the 4 different positions the camera can be in
		
	//MoveTowards
	private float moveSpeed = 6.5f;
	public GameObject pivotObject;
	private bool movingTowardsTarget = false;
	private bool movingAwayTarget = false;
	private int moveCount = 0;
	
	//rotate camera Up and Down
	private float rotateYSpeed = 49.0f;
	private bool rotatingY = false;
	private bool rotatedUp = false;
	private int rotateYCount = 0;
	
	private int dummyInt = 0;
	
	// Use this for initialization
	void Start () 
	{
		camera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void FixedUpdate () 					//0.02 fixed timestep
	{
		//RotateAround
		if(Input.GetKeyDown("r"))
		{
			if(rotating == true || rotatingY == true)
				//rotating = false;
				dummyInt += 1;
			else
			{
				rotating = true;
				rotatePosition = (rotatePosition+1)%4;
				if(rotatePosition == 1)
					movingAwayTarget = true;
				if(rotatePosition == 2)
					movingTowardsTarget = true;
				if(rotatePosition == 3)
					movingAwayTarget = true;
				if(rotatePosition == 0)
					movingTowardsTarget = true;
			}
		}
		
		if(Input.GetKeyDown("t"))
		{
			if(rotatingY == true || rotating == true)
				//rotatingY = false;
				dummyInt += 1;
			else
				rotatingY = true;
		}
		
		if(rotatingY)
		{
			if(!rotatedUp)
			{
				if(rotateYCount < 50)
				{
					if(rotatePosition == 0)
					{
						RotateUpPos0();
						rotateYCount++;
					}
					else if(rotatePosition == 1)
					{
						RotateUpPos1();
						rotateYCount++;
					}
					else if(rotatePosition == 2)
					{
						RotateUpPos2();
						rotateYCount++;
					}
					else if(rotatePosition == 3)
					{
						RotateUpPos3();
						rotateYCount++;
					}
				}
				else
				{
					rotatingY = false;
					rotatedUp = true;
					rotateYCount = 0;
				}
			}
			else
			{
				if(rotateYCount < 50)
				{
					if(rotatePosition == 0)
					{
						RotateDownPos0();
						rotateYCount++;
					}
					else if(rotatePosition == 1)
					{
						RotateDownPos1();
						rotateYCount++;
					}
					else if(rotatePosition == 2)
					{
						RotateDownPos2();
						rotateYCount++;
					}
					else if(rotatePosition == 3)
					{
						RotateDownPos3();
						rotateYCount++;
					}
				}
				else
				{
					rotatingY = false;
					rotatedUp = false;
					rotateYCount = 0;
				}
			}
		}
		
		if(rotating)
		{
			if(rotateCount < 100)
			{
				RotateAroundPivot();
				rotateCount++;
			}
			else
			{
				rotating = false;
				rotateCount = 0;
			}
		}
		
		if(movingTowardsTarget)
		{
			if(moveCount < 100)
			{
				MoveTowardsBoard();
				moveCount++;
			}
			else
			{
				movingTowardsTarget = false;
				moveCount = 0;
			}
		}
		if(movingAwayTarget)
		{
			if(moveCount < 100)
			{
				MoveAwayBoard();
				moveCount++;
			}
			else
			{
				movingAwayTarget = false;
				moveCount = 0;
			}
		}
	}
	
	//Rotate Up from different side angles
	public void RotateUpPos0()
	{
		transform.RotateAround(cameraPivotPoint.position, Vector3.right, rotateSpeed * Time.deltaTime);
	}
	
	public void RotateUpPos1()
	{
		transform.RotateAround(cameraPivotPoint.position, Vector3.back, rotateSpeed * Time.deltaTime);
	}
	
	public void RotateUpPos2()
	{
		transform.RotateAround(cameraPivotPoint.position, -Vector3.right, rotateSpeed * Time.deltaTime);
	}
	
	public void RotateUpPos3()
	{
		transform.RotateAround(cameraPivotPoint.position, -Vector3.back, rotateSpeed * Time.deltaTime);
	}
	
	//Rotate Down from different side angles
	public void RotateDownPos0()
	{
		transform.RotateAround(cameraPivotPoint.position, -Vector3.right, rotateSpeed * Time.deltaTime);
	}
	
	public void RotateDownPos1()
	{
		transform.RotateAround(cameraPivotPoint.position, -Vector3.back, rotateSpeed * Time.deltaTime);
	}
	
	public void RotateDownPos2()
	{
		transform.RotateAround(cameraPivotPoint.position, Vector3.right, rotateSpeed * Time.deltaTime);
	}
	
	public void RotateDownPos3()
	{
		transform.RotateAround(cameraPivotPoint.position, Vector3.back, rotateSpeed * Time.deltaTime);
	}
	
	
	//Rotation around board
	public void RotateAroundPivot()
	{
		transform.RotateAround(cameraPivotPoint.position, Vector3.up, rotateSpeed * Time.deltaTime);
	}
	
	
	//Zooming In/Out to fit to board
	public void MoveTowardsBoard()
	{
		transform.position = Vector3.MoveTowards(transform.position, pivotObject.transform.position, moveSpeed*Time.deltaTime);
		if(Vector3.Distance(transform.position, pivotObject.transform.position) < 0.1)
		{
			transform.position = pivotObject.transform.position;
			movingTowardsTarget = false;
		} 
	}
	
	public void MoveAwayBoard()
	{
		transform.position = Vector3.MoveTowards(transform.position, pivotObject.transform.position, -moveSpeed*Time.deltaTime);
		if(Vector3.Distance(transform.position, pivotObject.transform.position) < 0.1)
		{
			transform.position = pivotObject.transform.position;
			movingAwayTarget = false;
		} 
	}
}