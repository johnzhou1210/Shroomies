using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

public class ParticlesPaletteChange : MonoBehaviour
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
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color1.r, ChangePalette.holder.color1.g, ChangePalette.holder.color1.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color2.r, ChangePalette.holder.color2.g, ChangePalette.holder.color2.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color3.r, ChangePalette.holder.color3.g, ChangePalette.holder.color3.b, 
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color4.r, ChangePalette.holder.color4.g, ChangePalette.holder.color4.b, 
                    gameObject.GetComponent<Image>().color.a);
        }
        else if(gameObject.CompareTag("Text") || gameObject.CompareTag("Score"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.holder.color1.r, ChangePalette.holder.color1.g, ChangePalette.holder.color1.b, 
                    gameObject.GetComponent<TMP_Text>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.holder.color2.r, ChangePalette.holder.color2.g, ChangePalette.holder.color2.b,
                    gameObject.GetComponent<TMP_Text>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.holder.color3.r, ChangePalette.holder.color3.g, ChangePalette.holder.color3.b, 
                    gameObject.GetComponent<TMP_Text>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<TMP_Text>().color = new Color(ChangePalette.holder.color4.r, ChangePalette.holder.color4.g, ChangePalette.holder.color4.b, 
                    gameObject.GetComponent<TMP_Text>().color.a);
        }

        else if (gameObject.CompareTag("Overlay"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.holder.color1.r, ChangePalette.holder.color1.g, ChangePalette.holder.color1.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.holder.color2.r, ChangePalette.holder.color2.g, ChangePalette.holder.color2.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.holder.color3.r, ChangePalette.holder.color3.g, ChangePalette.holder.color3.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<SpriteRenderer>().color = new Color(ChangePalette.holder.color4.r, ChangePalette.holder.color4.g, ChangePalette.holder.color4.b, 
                    gameObject.GetComponent<SpriteRenderer>().color.a);
        }
        else if (gameObject.CompareTag("UpgradeFrame"))
        {
            if (PaletteColor == 1)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color1.r, ChangePalette.holder.color1.g, ChangePalette.holder.color1.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 2)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color2.r, ChangePalette.holder.color2.g, ChangePalette.holder.color2.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 3)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color3.r, ChangePalette.holder.color3.g, ChangePalette.holder.color3.b,
                    gameObject.GetComponent<Image>().color.a);
            else if (PaletteColor == 4)
                gameObject.GetComponent<Image>().color = new Color(ChangePalette.holder.color4.r, ChangePalette.holder.color4.g, ChangePalette.holder.color4.b,
                    gameObject.GetComponent<Image>().color.a);
        }
    }
}
