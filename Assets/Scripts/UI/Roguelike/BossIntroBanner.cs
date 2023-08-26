using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossIntroBanner : MonoBehaviour
{
    [SerializeField] GameObject _bossIntro, _bannerObj, _bossTitle, _bossImage;
    public void OnBossBannerInvoke(EntityDisplayInfo displayInfo) {
        _bannerObj.GetComponent<Image>().color = displayInfo.Aesthetic;
        _bossTitle.GetComponent<TextMeshProUGUI>().text = displayInfo.DisplayName;
        _bossImage.GetComponent<Image>().sprite = displayInfo.DisplayImage;
        _bossIntro.SetActive(true);
        Invoke("disable", 5f);
    }

    void disable() {
        _bossIntro.SetActive(false);
    }

}
