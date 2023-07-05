using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	private Texture2D _attackIcon;
	private Texture2D _handIcon;
	
	private PlayerStat _stat;
	private Vector3 _destPos;
	
	public enum PlayerState
	{
		Die,
		Moving,
		Idle,
		Skill
	}
	
	private PlayerState _state = PlayerState.Idle;
	
	private enum CursorType
	{
		None,
		Attack,
		Hand,
	}

	private CursorType _cursorType = CursorType.None;

	void Start()
    {
	    _attackIcon = GameManager.ResourceMng.Load<Texture2D>("Textures/Cursors/Attack");
	    _handIcon = GameManager.ResourceMng.Load<Texture2D>("Textures/Cursors/Hand");
	    
	    _stat = gameObject.GetComponent<PlayerStat>();
	    
	    GameManager.InputMng.MouseAction -= OnMouseClicked;
	    GameManager.InputMng.MouseAction += OnMouseClicked;
    }

    void Update()
    {
	    UpdateMouseCursor();
		    
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

    void UpdateDie()
    {
	    
    }
    
    void UpdateMoving()
    {
	    Vector3 dir = _destPos - transform.position;
	    if (dir.magnitude < 0.1f)
	    {
		    _state = PlayerState.Idle;
	    }
	    else
	    {
		    NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
		    
		    float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
		    nma.Move(dir.normalized * moveDist);
			
		    Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
		    if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
		    {
			    _state = PlayerState.Idle;
			    return;
		    }
		    
		    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
	    }
	    
	    // 애니메이션
	    Animator anim = GetComponent<Animator>();
	    anim.SetFloat("speed", _stat.MoveSpeed);
    }
    
    void UpdateIdle()
    {
	    // 애니메이션
	    Animator anim = GetComponent<Animator>();
	    anim.SetFloat("speed", 0);
    }

    void UpdateMouseCursor()
    {
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit, 100.0f, _layerMask))
	    {
		    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
		    {
			    if (_cursorType == CursorType.Attack)
				    return;
			    
			    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
			    _cursorType = CursorType.Attack;
		    }
		    else
		    {
			    if (_cursorType == CursorType.Hand)
				    return;
			    
			    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
			    _cursorType = CursorType.Hand;
		    }
	    }
    }
    
    private int _layerMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    
    void OnMouseClicked(Define.MouseEvent evt)
    {
	    if (_state == PlayerState.Die)
		    return;
	    
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit, 100.0f, _layerMask))
	    {
		    _destPos = hit.point;
		    _state = PlayerState.Moving;

		    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
		    {
			    Debug.Log("Monster");
		    }
		    else
		    {
			    Debug.Log("Ground");
		    }
	    }
	    
	    if (evt == Define.MouseEvent.Click)
			GameManager.SoundMng.Play("UnityChan/univ0001");
    }
}
