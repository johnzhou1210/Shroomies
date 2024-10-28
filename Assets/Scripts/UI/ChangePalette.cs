using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangePalette : MonoBehaviour
{

    [SerializeField] private Material paletteShader;

    [SerializeField] private List<Palette> palettes = new();

    public static Palette Holder;
    public GameSettings GameSettings;

    private static int count = 0;

    public static int Count { get => count; set => count = value; }

    private void Awake()
    {
        ChangeColor(GameSettings.currentPalette);
    }

    public void ChangeColor()
    {        
        GameSettings.currentPalette++;

        if(GameSettings.currentPalette > palettes.Count - 1) {
            GameSettings.currentPalette = 0;
        }

        paletteShader.SetColor("_Color1", palettes[GameSettings.currentPalette].color1);
        paletteShader.SetColor("_Color2", palettes[GameSettings.currentPalette].color2);
        paletteShader.SetColor("_Color3", palettes[GameSettings.currentPalette].color3);
        paletteShader.SetColor("_Color4", palettes[GameSettings.currentPalette].color4);

        Holder = palettes[GameSettings.currentPalette];

        EventBroker.CallPaletteChange();
    }

    public void ChangeColorRandom() {
        GameSettings.currentPalette = Random.Range(0, palettes.Count - 1);

        paletteShader.SetColor("_Color1", palettes[GameSettings.currentPalette].color1);
        paletteShader.SetColor("_Color2", palettes[GameSettings.currentPalette].color2);
        paletteShader.SetColor("_Color3", palettes[GameSettings.currentPalette].color3);
        paletteShader.SetColor("_Color4", palettes[GameSettings.currentPalette].color4);

        Holder = palettes[GameSettings.currentPalette];

        EventBroker.CallPaletteChange();
    }

    public void ChangeColor(int paletteSet)
    {
        GameSettings.currentPalette = paletteSet;

        if (GameSettings.currentPalette > palettes.Count - 1)
        {
            GameSettings.currentPalette = 0;
        }
        paletteShader.SetColor("_Color1", palettes[GameSettings.currentPalette].color1);
        paletteShader.SetColor("_Color2", palettes[GameSettings.currentPalette].color2);
        paletteShader.SetColor("_Color3", palettes[GameSettings.currentPalette].color3);
        paletteShader.SetColor("_Color4", palettes[GameSettings.currentPalette].color4);

        Holder = palettes[GameSettings.currentPalette];
        GameSettings.paletteName = palettes[GameSettings.currentPalette].name;

        EventBroker.CallPaletteChange();
    }

    public void ChangeColor(bool Right) {
        if(Right) GameSettings.currentPalette++;
        else if(!Right) GameSettings.currentPalette--;

        if (GameSettings.currentPalette > palettes.Count - 1) GameSettings.currentPalette = 0;
        else if(GameSettings.currentPalette < 0) GameSettings.currentPalette = palettes.Count -1 ;

        paletteShader.SetColor("_Color1", palettes[GameSettings.currentPalette].color1);
        paletteShader.SetColor("_Color2", palettes[GameSettings.currentPalette].color2);
        paletteShader.SetColor("_Color3", palettes[GameSettings.currentPalette].color3);
        paletteShader.SetColor("_Color4", palettes[GameSettings.currentPalette].color4);

        Holder = palettes[GameSettings.currentPalette];
        GameSettings.paletteName = palettes[GameSettings.currentPalette].name;

        EventBroker.CallPaletteChange();
    }

}
