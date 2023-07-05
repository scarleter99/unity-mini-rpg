using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	public enum PlayerState
	{
		Die,
		Idle,
		Moving,
		Skill
	}

	private int _layerMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
	
	private PlayerStat _stat;
	
	private Vector3 _destPos;
	private GameObject _lockTarget;
	private bool _stopSkill = false;
	
	[SerializeField]
	private PlayerState _state = PlayerState.Idle;

	public PlayerState State
	{
		get => _state;
		set
		{
			_state = value;

			Animator anim = GetComponent<Animator>();
			switch (_state)
			{
				case PlayerState.Die:
					break;
				case PlayerState.Idle:
					anim.CrossFade("WAIT", 0.1f);
					break;
				case PlayerState.Moving:
					anim.CrossFade("RUN", 0.1f);
					break;
				case PlayerState.Skill:
					anim.CrossFade("ATTACK", 0.1f, -1, 0);
					break;
			}
		}
	}
	
	void Start()
    {
	    _stat = gameObject.GetComponent<PlayerStat>();
	    
	    GameManager.InputMng.MouseAction -= OnMouseEvent;
	    GameManager.InputMng.MouseAction += OnMouseEvent;

	    GameManager.UIMng.MakeWorldSpaceUI<UI_HpBar>(transform);
    }

    void Update()
    {
		switch (State)
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
		    case PlayerState.Skill:
			    UpdateSkill();
			    break;
	    }
    }

    void UpdateDie()
    {
	    
    }
    
    void UpdateIdle()
    {
		
    }
    
    void UpdateMoving()
    {
	    if (_lockTarget != null)
	    {
		    _destPos = _lockTarget.transform.position;
		    float distance = (_destPos - transform.position).magnitude;
		    if (distance <= 1)
		    {
			    State = PlayerState.Skill;
		    }
	    }

	    Vector3 dir = _destPos - transform.position;
	    if (dir.magnitude < 0.1f)
	    {
		    State = PlayerState.Idle;
	    }
	    else
	    {
		    NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
		    
		    float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
		    nma.Move(dir.normalized * moveDist);
			
		    Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
		    if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
		    {
			    if(Input.GetMouseButton(0) == false)
				    State = PlayerState.Idle;
			    return;
		    }
		    
		    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
	    }
    }

    void UpdateSkill()
    {
	    if (_lockTarget != null)
	    {
		    Vector3 dir = _lockTarget.transform.position - transform.position;
		    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
	    }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
	    switch (State)
	    {
		    case PlayerState.Idle:
			    OnMouseEvent_IdleRun(evt);
			    break;
		    case PlayerState.Moving:
			    OnMouseEvent_IdleRun(evt);
			    break;
		    case PlayerState.Skill:
			    if (evt == Define.MouseEvent.PointerUp)
				    _stopSkill = true;
			    break;
	    }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
	    if (State == PlayerState.Die)
		    return;

	    RaycastHit hit;
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _layerMask);
	    //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

	    switch (evt)
	    {
		    case Define.MouseEvent.PointerDown:
			    if (raycastHit)
			    {
				    _destPos = hit.point;
				    State = PlayerState.Moving;

				    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
				    {
					    _stopSkill = false;
					    _lockTarget = hit.collider.gameObject;
				    }
				    else
					    _lockTarget = null;
			    }
			    break;
		    case Define.MouseEvent.Press:
			    if (_lockTarget == null && raycastHit)
				    _destPos = hit.point;
			    break;
		    case Define.MouseEvent.PointerUp:
			    _stopSkill = true;
			    break;
	    }
    }

    void OnHitEvent()
    {
	    if (_stopSkill)
			State = PlayerState.Idle;
	    else
		    State = PlayerState.Skill;
	    
	    GameManager.SoundMng.Play("UnityChan/univ0001");
    }
}
