using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

public class UIPaletteChange : MonoBehaviour
{
    [Range(1, 4)]
    public int PaletteColor = 1;



    private void OnEnable()
    {
        EventBroker.onPaletteChange += changeColor;

        changeColor();
    }

    private void OnDisable()
    {
        EventBroker.onPaletteChange -= changeColor;
    }

    public void changeColor()
    {

        if (gameObject.CompareTag("Banner"))
        {
            // gameObject.GetComponent<Image>().color = nColor + paletteColor;
            if (PaletteColor == 1)
                gameObject.GetComponent<Image>().color = ChangePalette.holder.color1;
            else if (PaletteColor == 2)
                gameObject.GetComponent<Image>().color = ChangePalette.holder.color2;
            else if (PaletteColor == 3)
                gameObject.GetComponent<Image>().color = ChangePalette.holder.color3;
            else if (PaletteColor == 4)
                gameObject.GetComponent<Image>().color = ChangePalette.holder.color4;
        }
        else if(gameObject.CompareTag("Text") || gameObject.CompareTag("Score"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<TMP_Text>().color = ChangePalette.holder.color1;
            else if (PaletteColor == 2)
                gameObject.GetComponent<TMP_Text>().color = ChangePalette.holder.color2;
            else if (PaletteColor == 3)
                gameObject.GetComponent<TMP_Text>().color = ChangePalette.holder.color3;
            else if (PaletteColor == 4)
                gameObject.GetComponent<TMP_Text>().color = ChangePalette.holder.color4;
        }

        else if (gameObject.CompareTag("Overlay"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<SpriteRenderer>().color = ChangePalette.holder.color1;
            else if (PaletteColor == 2)
                gameObject.GetComponent<SpriteRenderer>().color = ChangePalette.holder.color2;
            else if (PaletteColor == 3)
                gameObject.GetComponent<SpriteRenderer>().color = ChangePalette.holder.color3;
            else if (PaletteColor == 4)
                gameObject.GetComponent<SpriteRenderer>().color = ChangePalette.holder.color4;
        }
    }
}
