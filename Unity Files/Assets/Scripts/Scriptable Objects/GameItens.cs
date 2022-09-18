using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Itens", menuName = "TBS Assets/Game Itens Object")]
public class GameItens : ScriptableObject
{
    // specifies all itens properties to the game
    [System.Serializable]
    public class itemSpecs
    {
        public string nameItem;
        public Sprite imgSpriteFRONT;
        public Sprite imgSpriteSIDE;
        public Sprite imgSpriteBACK;
        public float value;
    }

    public itemSpecs[] hats;
    public itemSpecs[] body;
    public itemSpecs[] legs;
    public itemSpecs[] arms;
}
