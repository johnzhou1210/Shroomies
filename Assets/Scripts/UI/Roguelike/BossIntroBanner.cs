using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossIntroBanner : MonoBehaviour
{
    [SerializeField] GameObject bossIntro, bannerObj, bossTitle, bossImage;

    private void OnEnable()
    {
        StageLogic.OnCueBossBanner += OnBossBannerInvoke;
    }

    private void OnDisable()
    {
        StageLogic.OnCueBossBanner -= OnBossBannerInvoke;
    }

    public void OnBossBannerInvoke(EntityDisplayInfo displayInfo)
    {
        bannerObj.GetComponent<Image>().color = displayInfo.Aesthetic;
        bossTitle.GetComponent<TextMeshProUGUI>().text = displayInfo.DisplayName;
        bossImage.GetComponent<Image>().sprite = displayInfo.DisplayImage;
        bossIntro.SetActive(true);
        Invoke(nameof(Disable), 5f);
    }

    void Disable()
    {
        bossIntro.SetActive(false);
    }
}