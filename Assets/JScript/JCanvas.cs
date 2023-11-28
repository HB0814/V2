using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

/// <summary>
/// UI 관리를 위한 캔버스 스크립트이다
/// </summary>
public class JCanvas : MonoBehaviour
{
    public static JCanvas _instance;
    public static JCanvas Instance { get { return _instance; } }
    
    /// <summary> [게임 오브젝트] 메인 캔버스 </summary>
    [Header("메인화면 캔버스")]
    public GameObject MainCanvas;
    /// <summary> [게임 오브젝트] 로고 오브젝트 </summary>
    [Header("메인화면 로고")]
    public GameObject MainLogo;
    /// <summary> [게임 오브젝트] 시작 버튼 오브젝트 </summary>
    [Header("메인화면 시작 버튼")]
    public GameObject StartBtn;
    /// <summary> [게임 오브젝트] 설정 버튼 오브젝트 </summary>
    [Header("메인화면 설정 버튼")]
    public GameObject MainSettingBtn;
    /// <summary> [게임 오브젝트] 엔드 버튼 오브젝트 </summary>
    [Header("메인화면 끝 버튼")]
    public GameObject EndBtn;
    /// <summary> [게임 오브젝트] 엔드 버튼 오브젝트 </summary>
    [Header("페이드 인 아웃")]
    public GameObject FadeIn;

    [Header("==============================================")] [Space(1f)]
    public string s;
    
    /// <summary> [게임 오브젝트] 주금 캔버스 </summary>
    [Header("죽음 캔버스")]
    public GameObject DeadCanvas;
    
    /// <summary> [게임 오브젝트] 인 게임 캔버스 </summary>
    [Header("인 게임 캔버스")]
    public GameObject InGameCanvas;
    /// <summary> [게임 오브젝트] 로고 오브젝트 </summary>
    [Header("인 게임 인벤토리 버튼")]
    public GameObject InvenBtn;
    /// <summary> [게임 오브젝트] 시작 버튼 오브젝트 </summary>
    [Header("인 게임 설정 버튼")]
    public GameObject InGameSettingBtn;
    /// <summary> [게임 오브젝트] 엔드 버튼 오브젝트 </summary>
    [Header("인 게임 끝 버튼")]
    public GameObject InGameEndBtn;

    /// <summary> [게임 오브젝트] HPbar 오브젝트 </summary>
    [Header("인 게임 체력 바")] 
    public GameObject InGameHPBar;
    /// <summary> [게임 오브젝트] HPbar 오브젝트 </summary>
    [Header("인 게임 체력 바 슬라이더")] 
    public Slider InGameHPBarSlider;
    
    /// <summary> [게임 오브젝트] Resolution 오브젝트 </summary>
    [Header("메인 게임 해상도 조절")] 
    public Dropdown MainResolution;
    
    /// <summary> [게임 오브젝트] Resolution 오브젝트 </summary>
    [Header("인 게임 해상도 조절")] 
    public Dropdown InGameResolution;

    private List<Resolution> resolutions = new List<Resolution>();
    private int nResolutionNum;
    
    [Header("==============================================")] [Space(1f)]
    private bool isOpen;
    
    /// <summary> [게임 오브젝트] 엔드 버튼 오브젝트 </summary>
    [Header("창 인벤토리")]
    public GameObject InventoryWindow;
    
    /// <summary> [게임 오브젝트] 엔드 버튼 오브젝트 </summary>
    [Header("창 세팅")]
    public GameObject SettingWindow;

    /// <summary> [게임 오브젝트] 플레이어 </summary>
    [Header("[게임 오브젝트] 플레이어")]
    public GameObject Player;

    /// <summary> [사운드 오브젝트] 버튼 사운드 </summary>
    [Header("[사운드 오브젝트]버튼 올리면 사운드")] public AudioSource PointEnter;
    
    /// <summary> [사운드 오브젝트] 버튼 사운드 </summary>
    [Header("[사운드 오브젝트]메인 사운드")] public AudioSource MainBGM;
    
    /// <summary> [사운드 오브젝트] 메인 설정 사운드 </summary>
    [Header("[사운드 오브젝트]메인 설정 슬라이더")] public Slider MainSlider;
    
    /// <summary> [사운드 오브젝트] 인게임 설정 사운드 </summary>
    [Header("[사운드 오브젝트]인게임 설정 슬라이더")] public Slider InGameSlider;

    /// <summary> [사운드 변수] 사운드 크기 값 </summary>
    [Header("[사운드 변수]사운드 크기 값")]
    public float SoundValue;
    


    void Awake()
    {
        // 인스턴스가 이미 존재하는지 확인하고, 존재하지 않는 경우에만 인스턴스 생성
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        resolutions.AddRange(Screen.resolutions);
        InGameResolution.options.Clear();
        MainResolution.options.Clear();

        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " x " + item.height + " x " + item.refreshRateRatio + "hz";
            InGameResolution.options.Add(option);
            MainResolution.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                InGameResolution.value = nResolutionNum;
                MainResolution.value = nResolutionNum;
            }

            nResolutionNum++;
        }
        InGameResolution.RefreshShownValue();
        MainResolution.RefreshShownValue();
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& Player.GetComponent<Player>().isStart)
        {
            if (!isOpen)
            {
                if(!InGameCanvas.activeSelf)
                    InGameCanvas.SetActive(true);
                InGameCanvas.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
                isOpen = true;
            }
            else
            {
                InGameCanvas.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
                InGameCanvas.SetActive(false);
                isOpen = false;
            }
        }
    } 
    
//==================================================================================
//==================================================================================
//==================================================================================
//               < 사용자 지정 함수 >
//==================================================================================
//==================================================================================
//==================================================================================

    public void OnActiveSetting()
    {
        
    }
    

    public void HPBarSet(float Max, float Cur)
    {
        InGameHPBarSlider.value = Cur / Max;
    }

    public void DeadLogo()
    {
        float score = Time.time;
    }

    public void SelectResolution(int x)
    {
        nResolutionNum = x;
    }

    public void ChangeResoultion()
    {
        Screen.SetResolution(resolutions[nResolutionNum].width,resolutions[nResolutionNum].height,FullScreenMode.FullScreenWindow);
    }

    public void PlayerDead()
    {
        DeadCanvas.GetComponent<CanvasGroup>().DOFade(1f, 1f);
    }
    
    public void SoundSetting(float x)
    {
        SoundValue = x;
        MainSlider.value = SoundValue;
        InGameSlider.value = SoundValue;
        MainBGM.volume = SoundValue;
        PointEnter.volume = SoundValue;
    }

    public void GameEnd()
    {
        Application.Quit();
    }
    
    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene (gameObject.scene.name);
    }
    
//==================================================================================
//==================================================================================
//==================================================================================
//               < 이벤트 함수 >
//==================================================================================
//==================================================================================
//==================================================================================




    /// <summary> 시작 버튼 이벤트 </summary>
    public void OnStartBtnEvent()
    {
        DG.Tweening.Sequence StartSeq = DOTween.Sequence()
            .Append(MainCanvas.GetComponent<CanvasGroup>().DOFade(0, 1f))
            .AppendCallback(MainCanvasActiveFalse)
            .Append(FadeIn.GetComponent<CanvasGroup>().DOFade(1f, 1f))
            .AppendInterval(3f)
            .Append(FadeIn.GetComponent<CanvasGroup>().DOFade(0f, 1f))
            .Append(InGameHPBar.GetComponent<CanvasGroup>().DOFade(1f,1f));
    }
    
    /// <summary> 메인 화면 캔버스 끄기 </summary>
    private void MainCanvasActiveFalse()
    {
        MainCanvas.SetActive(false);
        //InGameCanvas.SetActive(true);
    }
    /// <summary> 버튼 올리면 켜지기 </summary>
    public void ESCButtonEnterEvent(GameObject Btn)
    {
        PointEnter.Play();
        Btn.GetComponent<Image>().DOFade(0.2f, 0.3f);
    }
    /// <summary> 버튼 벗어나면 끄기 </summary>
    public void ESCButtonExitEvent(GameObject Btn)
    {
        PointEnter.Play();
        Btn.GetComponent<Image>().DOFade(0f, 0.3f);
    }
    
    /// <summary> 메인 버튼 올리면 켜지기 </summary>
    public void MainButtonEnterEvent(GameObject Btn)
    {
        PointEnter.Play();
        Btn.GetComponent<Image>().DOFade(1f, 0.5f);
    }
    /// <summary> 메인 버튼 벗어나면 끄기 </summary>
    public void MainButtonExitEvent(GameObject Btn)
    {
        Btn.GetComponent<Image>().DOFade(0f, 0.5f);
    }

    public void WindowOpenEvent(GameObject Canvas)
    {
        InventoryWindow.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
        SettingWindow.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
        Canvas.SetActive(true);
        Canvas.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
    }
    
    public void WindowCloseEvent(GameObject Canvas)
    {
        PointEnter.Play();
        Canvas.GetComponent<CanvasGroup>().DOFade(0f, 0.4f);
        Canvas.SetActive(false);
    }

    public void PlayerDeadEvent()
    {
        Invoke("PlayerDead", 2f);
    }
    
    
    
}
