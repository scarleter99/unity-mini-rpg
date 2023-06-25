using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 전역으로 게임 전체와 다른 Manager를 관리하는 Manager
 * @details Singleton 패턴 적용
 */
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { Init(); return _instance; } }

    private DataManager _dataMng = new DataManager();
    private InputManager _inputMng = new InputManager();
    private PoolManager _poolMng = new PoolManager();
    private ResourceManager _resourceMng = new ResourceManager();
    private SceneManagerEx _sceneMng = new SceneManagerEx();
    private SoundManager _soundMng = new SoundManager();
    private UiManager _uiMng = new UiManager();

    public static DataManager DataMng => Instance._dataMng;
    public static InputManager InputMng => Instance._inputMng;
    public static PoolManager PoolMng => Instance._poolMng;
    public static ResourceManager ResourceMng => Instance._resourceMng;
    public static SceneManagerEx SceneMng => Instance._sceneMng;
    public static SoundManager SoundMng => Instance._soundMng;
    public static UiManager UIMng => Instance._uiMng;


    private void Start()
    {
        Init();
    }
    
    private void Update()
    {
        _inputMng.OnUpdate();
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }
            
            DontDestroyOnLoad(go);
            _instance = go.GetComponent<GameManager>();

            _instance._dataMng.Init();
            _instance._soundMng.Init();
            _instance._poolMng.Init();
        }
    }
    
    public static void Clear()
    {
        InputMng.Clear();
        SoundMng.Clear();
        SceneMng.Clear();
        UIMng.Clear();
        PoolMng.Clear();
    }
}
