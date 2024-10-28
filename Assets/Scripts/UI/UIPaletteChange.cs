using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class UIPaletteChange : MonoBehaviour
{
    [Range(1, 4)]
    public int PaletteColor = 1;



    private void OnEnable()
    {
        EventBroker.OnPaletteChange += ChangeColor;


    }

    private void OnDisable()
    {
        EventBroker.OnPaletteChange -= ChangeColor;
    }

    private void Start() {
        ChangeColor();
    }


    public void ChangeColor()
    {

        if (gameObject.CompareTag("Banner"))
        {
            // gameObject.GetComponent<Image>().color = nColor + paletteColor;
            if (PaletteColor == 1)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color1.r, ChangePalette.Holder.color1.g, ChangePalette.Holder.color1.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color2.r, ChangePalette.Holder.color2.g, ChangePalette.Holder.color2.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color3.r, ChangePalette.Holder.color3.g, ChangePalette.Holder.color3.b, 
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color4.r, ChangePalette.Holder.color4.g, ChangePalette.Holder.color4.b, 
                    gameObject.GetComponent<Image>().color.a);
        }
        else if(gameObject.CompareTag("Text") || gameObject.CompareTag("Score"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.Holder.color1.r, ChangePalette.Holder.color1.g, ChangePalette.Holder.color1.b, 
                    gameObject.GetComponent<TMP_Text>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.Holder.color2.r, ChangePalette.Holder.color2.g, ChangePalette.Holder.color2.b,
                    gameObject.GetComponent<TMP_Text>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.Holder.color3.r, ChangePalette.Holder.color3.g, ChangePalette.Holder.color3.b, 
                    gameObject.GetComponent<TMP_Text>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.Holder.color4.r, ChangePalette.Holder.color4.g, ChangePalette.Holder.color4.b, 
                    gameObject.GetComponent<TMP_Text>().color.a);
        }

        else if (gameObject.CompareTag("Overlay"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.Holder.color1.r, ChangePalette.Holder.color1.g, ChangePalette.Holder.color1.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.Holder.color2.r, ChangePalette.Holder.color2.g, ChangePalette.Holder.color2.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.Holder.color3.r, ChangePalette.Holder.color3.g, ChangePalette.Holder.color3.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.Holder.color4.r, ChangePalette.Holder.color4.g, ChangePalette.Holder.color4.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
        }
        else if (gameObject.CompareTag("UpgradeFrame"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color1.r, ChangePalette.Holder.color1.g, ChangePalette.Holder.color1.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color2.r, ChangePalette.Holder.color2.g, ChangePalette.Holder.color2.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color3.r, ChangePalette.Holder.color3.g, ChangePalette.Holder.color3.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.Holder.color4.r, ChangePalette.Holder.color4.g, ChangePalette.Holder.color4.b,
                    gameObject.GetComponent<Image>().color.a);
        }
    }
}
