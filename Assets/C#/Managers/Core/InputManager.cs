using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 게임 내 모든 입력 처리
public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    private bool _pressed = false;
    private float _pressedTime = 0;

    // 입력이 없다면 바로 리턴, 입력이 있다면 KeyAction/MouseAction을 Invoke
    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (KeyAction != null && Input.anyKey)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }

                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if (Time.time < _pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);

                }
                
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }
    
    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}

/* 사용예제
    void Start()
    {
        Managers.InputMng.KeyAction += OnKeyboard;
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * Time.deltaTime * mSpeed;
        }
    }
*/
