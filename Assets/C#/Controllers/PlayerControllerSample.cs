using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerSample : MonoBehaviour
{
	private PlayerStat _stat;
	
	private Vector3 _destPos;
	private PlayerState _state = PlayerState.Idle;

	public enum PlayerState
	{
		Die,
		Moving,
		Idle,
		Skill
	}

    void Start()
    {
	    _stat = gameObject.GetComponent<PlayerStat>();
	    
	    Managers.InputMng.MouseAction -= OnMouseClicked;
	    Managers.InputMng.MouseAction += OnMouseClicked;
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
		    transform.position += dir.normalized * moveDist;
			
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

    private int _layerMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    
    void OnMouseClicked(Define.MouseEvent evt)
    {
	    if (_state == PlayerState.Die)
		    return;
	    
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

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
			Managers.SoundMng.Play("UnityChan/univ0001");
    }
}
