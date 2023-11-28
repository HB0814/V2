using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 카메라 시퀀스 처리를 위한 스크립트이다.
/// </summary>
public class JStartCameraSequence : MonoBehaviour
{
    /// <summary> [카메라 오브젝트] 메인 카메라 </summary>
    [Header("[카메라 오브젝트] 메인 카메라")]
    public GameObject MainCamGam;
    /// <summary> [게임 오브젝트] 플레이어 </summary>
    [Header("[게임 오브젝트] 플레이어")]
    public GameObject Player;

    /// <summary> [이벤트 ] 카메라 이벤트 호출 </summary>
    public void OnCameraSequenceEvent()
    {
        Invoke("StartCamera",2f);
        Invoke("PlayerStartMove",7f);
    }

    /// <summary> [ 함수 ] 카메라 이벤트 처리 </summary>
    private void StartCamera()
    {
    }

    private void PlayerStartMove()
    {
        Player.GetComponent<Player>().isStart = true;
        Animator playerAnim = Player.GetComponent<Animator>();
        playerAnim.SetBool("isLieDown", false);
    }
    
    void Awake()
    {
        
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
