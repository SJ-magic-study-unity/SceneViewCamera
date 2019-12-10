/************************************************************
GameビューにてSceneビューのようなカメラの動きをマウス操作によって実現する.
attach this script to Camera Object.

■参考URL
	https://gist.github.com/EsProgram/0fd35669c28fd13594c8
	http://esprog.hatenablog.com/entry/2016/03/20/033322
	https://www.fast-system.jp/unity%EF%BC%9A%E3%83%9E%E3%82%A6%E3%82%B9%E3%81%A7scene%E3%83%93%E3%83%A5%E3%83%BC%E3%81%AE%E3%82%88%E3%81%86%E3%81%AA%E3%82%AB%E3%83%A1%E3%83%A9%E6%93%8D%E4%BD%9C%E3%82%92%E3%81%99%E3%82%8B/
************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************************
************************************************************/
[RequireComponent(typeof(Camera))]
public class SceneViewCamera : MonoBehaviour
{
	/****************************************
	****************************************/
	KeyCode Key_Reset = KeyCode.R;
	
	Vector3 position_init;
	Quaternion rotation_init;
	
	[SerializeField, Range(0.1f, 10f)]
	private float wheelSpeed = 1f;
	
	[SerializeField, Range(0.1f, 10f)]
	private float moveSpeed = 0.3f;
	
	[SerializeField, Range(0.1f, 10f)]
	private float rotateSpeed = 0.3f;
	
	private Vector3 preMousePos;
	
	/****************************************
	****************************************/
	/******************************
	******************************/
    void Start()
    {
		position_init = transform.position;
		rotation_init = transform.rotation;
	}
	
	/******************************
	******************************/
	private void Update()
	{
		if(Input.GetKeyDown(Key_Reset)) ResetTransform();
		
		MouseUpdate();
		return;
	}
	
	/******************************
	******************************/
	void ResetTransform()
	{
		transform.position = position_init;
		transform.rotation = rotation_init;
	}
	
	/******************************
	******************************/
	private void MouseUpdate()
	{
		float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		if(scrollWheel != 0.0f) MouseWheel(scrollWheel);
		
		if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) preMousePos = Input.mousePosition;
		
		MouseDrag(Input.mousePosition);
	}
	
	/******************************
	******************************/
	private void MouseWheel(float delta)
	{
		transform.position += transform.forward * delta * wheelSpeed;
		return;
	}
	
	/******************************
	******************************/
	private void MouseDrag(Vector3 mousePos)
	{
		Vector3 diff = mousePos - preMousePos;
		
		if(diff.magnitude < Vector3.kEpsilon) return;
		
		if(GlobalParam.b_FlipCamera){
			diff.x = -diff.x;
		}
		if(Input.GetMouseButton(2))			transform.Translate(-diff * Time.deltaTime * moveSpeed);
		else if(Input.GetMouseButton(1))	CameraRotate(new Vector2(-diff.y, diff.x) * rotateSpeed);
		
		preMousePos = mousePos;
	}
	
	/******************************
	******************************/
	public void CameraRotate(Vector2 angle)
	{
		transform.RotateAround(transform.position, transform.right, angle.x);
		transform.RotateAround(transform.position, Vector3.up, angle.y);
	}
}