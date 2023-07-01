using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;
    
	Vector3 _destPos;
	private PlayerState _state = PlayerState.Idle;
	
	public enum PlayerState
	{
		Die,
		Moving,
		Idle
	}

    void Start()
    {
	    GameManager.InputMng.MouseAction -= OnMouseClicked;
	    GameManager.InputMng.MouseAction += OnMouseClicked;
		//GameManager.InputMng.KeyAction -= OnKeyboard;
		//GameManager.InputMng.KeyAction += OnKeyboard;
    }

    void UpdateDie()
    {
	    
    }
    
    void UpdateMoving()
    {
	    Vector3 dir = _destPos - transform.position;
	    if (dir.magnitude < 0.0001f)
	    {
		    _state = PlayerState.Idle;
	    }
	    else
	    {
		    float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
		    transform.position += dir.normalized * moveDist;
		    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
	    }
	    
	    // 애니메이션
	    Animator anim = GetComponent<Animator>();
	    anim.SetFloat("speed", _speed);
    }
    
    void UpdateIdle()
    {
	    // 애니메이션
	    Animator anim = GetComponent<Animator>();
	    anim.SetFloat("speed", 0);
    }
    
    void Update()
    {
	    switch (_state)
	    {
		    case PlayerState.Die:
			    UpdateDie();
			    break;
		    case PlayerState.Moving:
			    UpdateMoving();
			    break;
		    case PlayerState.Idle:
			    UpdateIdle();
			    break;
	    }
    }
	
    void OnMouseClicked(Define.MouseEvent evt)
    {
	    if (_state == PlayerState.Die)
		    return;
	    
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
	    {
		    _destPos = hit.point;
		    _state = PlayerState.Moving;
	    }
    }
    
    /*
    void OnKeyboard()
    {
		if (Input.GetKey(KeyCode.W))
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
			transform.position += Vector3.forward * Time.deltaTime * _speed;
		}

		if (Input.GetKey(KeyCode.S))
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
			transform.position += Vector3.back * Time.deltaTime * _speed;
		}

		if (Input.GetKey(KeyCode.A))
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
			transform.position += Vector3.left * Time.deltaTime * _speed;
		}

		if (Input.GetKey(KeyCode.D))
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
			transform.position += Vector3.right * Time.deltaTime * _speed;
		}
	}
	*/
}
