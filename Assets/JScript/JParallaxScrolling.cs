using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 페러렐릭 스크롤링 기능
/// </summary>
public class JParallaxScrolling : MonoBehaviour
{
    /// <summary> 메인 카메라 오브젝트 </summary>
    [Header("메인 카메라")][Space(2f)]
    public GameObject camera_object = null;

    
    /// <summary> 움직이는 배경의 양 끝 부분 </summary>
    [Header("움직여야 할 내 배경 부분")][Space(2f)]
    public Transform background_leftPoint = null;
    public Transform background_rightPoint = null;

    
    /// <summary> 맵의 총 크기, 양 끝 부분 </summary>
    [Header("맵 크기, 양 끝 부분")] [Space(2f)] [SerializeField]
    public Transform ground_leftPoint = null;
    public Transform ground_rightPoint = null;

    
    /// <summary> 카메라 뷰의 양 끝 부분  </summary>
    [Header("카메라의 너비 부분")] [Space(2f)] 
    public Transform camera_leftPoint = null;
    public Transform camera_rightPoint = null;

    float ground_sideSpace = 0f, background_sideSpace = 0f;

    void Start() {
        float camera_width = camera_leftPoint.position.x - camera_rightPoint.position.x;
        ground_sideSpace = ground_rightPoint.position.x - ground_leftPoint.position.x;
        background_sideSpace = background_leftPoint.position.x - background_rightPoint.position.x - camera_width * 0.5f;
    }

    void Update() {
        SetPosition();
    }
    
    
    /// <summary> 카메라의 움직임과 카메라와 바닥의 상대적인 위치를 기반으로 배경의 새로운 x-위치를 계산한다 </summary>
    void SetPosition() {
        float background_xPos = camera_object.transform.position.x + ((camera_object.transform.position.x - ground_leftPoint.position.x) / ground_sideSpace - 0.5f) * background_sideSpace;

        transform.position = new Vector2(background_xPos, transform.position.y);
    }
}

