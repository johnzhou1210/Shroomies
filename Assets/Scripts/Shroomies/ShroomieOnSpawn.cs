using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomieOnSpawn : MonoBehaviour
{
    public ParticleSystem ParticlesBits;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //particles
        ParticleSystem.MainModule psBitsMAIN = ParticlesBits.main;
        psBitsMAIN.startColor = ChangePalette.holder.color1;
        psBitsMAIN.maxParticles = 2;
        Vector3 psBitsOffset = GetComponent<Transform>().position;
        psBitsOffset.y += 1f;
        ParticleSystem.TextureSheetAnimationModule psBitsTSA = ParticlesBits.textureSheetAnimation;
        psBitsTSA.rowIndex = 2;
        Instantiate(ParticlesBits, transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        
    }
}
