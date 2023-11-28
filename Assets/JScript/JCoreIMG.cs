using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JCoreIMG : MonoBehaviour
{
    public Sprite[] SkillCoreIMG;
    public Text info;
    private Image img;

    public Player players;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (players.isCrouchCore)
        {
            img.sprite = SkillCoreIMG[0];
            info.text = "스킬 코어" + System.Environment.NewLine + "웅크리기";
        }
        else if (players.isRollCore)
        {            
            img.sprite = SkillCoreIMG[1];
            info.text = "스킬 코어" + System.Environment.NewLine + "구르기";
        }
        else if (players.isDropCore)
        {
            img.sprite = SkillCoreIMG[2];
            info.text = "스킬 코어" + System.Environment.NewLine + "내려찍기";
        }
        else if (players.isSummonCore)
        {
            img.sprite = SkillCoreIMG[3];
            info.text = "스킬 코어" + System.Environment.NewLine + "소환";
        }
    }
}
