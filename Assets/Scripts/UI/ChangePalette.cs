using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangePalette : MonoBehaviour
{

    [SerializeField]
    private Material paletteShader;

    [SerializeField]
    private List<Palette> palettes = new List<Palette>();

    public int count = 0;

    public void ChangeColor()
    {
        if(count > palettes.Count - 1) {
            count = 0;
        }
        paletteShader.SetColor("_Color1", palettes[count].color1);
        paletteShader.SetColor("_Color2", palettes[count].color2);
        paletteShader.SetColor("_Color3", palettes[count].color3);
        paletteShader.SetColor("_Color4", palettes[count].color4);

        count++;
    }
}
