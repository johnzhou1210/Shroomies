using System.Collections.Generic;
using UnityEngine;

public class ChangePalette : MonoBehaviour
{

    [SerializeField]
    private Material paletteShader;

    [SerializeField]
    private List<Palette> palettes = new List<Palette>();
    public bool firstRun = true;
    public static Palette holder;

    private static int count = 0;

    public static int Count { get => count; set => count = value; }

    private void Awake()
    {
        ChangeColor(Count, firstRun);
    }


    public void ChangeColor()
    {        
        Count++;

        if(Count > palettes.Count - 1) {
            Count = 0;
        }

        paletteShader.SetColor("_Color1", palettes[Count].color1);
        paletteShader.SetColor("_Color2", palettes[Count].color2);
        paletteShader.SetColor("_Color3", palettes[Count].color3);
        paletteShader.SetColor("_Color4", palettes[Count].color4);

        holder = palettes[Count];

        EventBroker.CallPaletteChange();
    }

    public void ChangeColor(int paletteSet, bool firstTime)
    {
        Count = paletteSet;

        if (Count > palettes.Count - 1)
        {
            Count = 0;
        }
        paletteShader.SetColor("_Color1", palettes[Count].color1);
        paletteShader.SetColor("_Color2", palettes[Count].color2);
        paletteShader.SetColor("_Color3", palettes[Count].color3);
        paletteShader.SetColor("_Color4", palettes[Count].color4);

        holder = palettes[Count];

        EventBroker.CallPaletteChange();
    }
}
