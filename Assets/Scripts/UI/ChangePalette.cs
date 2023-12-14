using System.Collections.Generic;
using UnityEngine;

public class ChangePalette : MonoBehaviour
{

    [SerializeField]
    private Material paletteShader;

    [SerializeField]
    private List<Palette> palettes = new List<Palette>();
    public static Palette holder;

    public static int count = 0;

    private void Awake()
    {
        ChangeColor(count);
    }

    public void ChangeColor()
    {
        if(count > palettes.Count - 1) {
            count = 0;
        }
        paletteShader.SetColor("_Color1", palettes[count].color1);
        paletteShader.SetColor("_Color2", palettes[count].color2);
        paletteShader.SetColor("_Color3", palettes[count].color3);
        paletteShader.SetColor("_Color4", palettes[count].color4);

        holder = palettes[count];

        EventBroker.CallPaletteChange();
        count++;
    }

    public void ChangeColor(int paletteSet)
    {
        count = paletteSet;

        if (count > palettes.Count - 1)
        {
            count = 0;
        }
        paletteShader.SetColor("_Color1", palettes[count].color1);
        paletteShader.SetColor("_Color2", palettes[count].color2);
        paletteShader.SetColor("_Color3", palettes[count].color3);
        paletteShader.SetColor("_Color4", palettes[count].color4);

        holder = palettes[count];

        EventBroker.CallPaletteChange();
        
        count++;
    }
}
