using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItens : MonoBehaviour
{
    // use this class to identify the character body pieces
    // and your actual itens sprites
   [System.Serializable]
    public class ChangeItens
    {
        public ActualItens hatSpritesActual;
        public ActualItens bodySpritesActual;
        public ActualItens armSpritesActual;
        public ActualItens legSpritesActual;

        [Space(15)]
        [Header("Character Pieces Body:")]
        public SpriteRenderer hatPiece;
        public SpriteRenderer bodyPiece;
        public SpriteRenderer armLeftPiece;
        public SpriteRenderer armRightPiece;
        public SpriteRenderer legLeftPiece;
        public SpriteRenderer legRightPiece;

        // scriptable object who containigs all character itens information
        [Space(15)]
        [Header("Character Scriptable Object:")]
        public GameItens scriptableInfos;

        public void SetHat(string itemName)
        {
            // for each item on scriptable object, check if the received item exists to equip
            for (int i = 0; i < scriptableInfos.hats.Length; i++)
            {
                if (itemName == scriptableInfos.hats[i].nameItem)
                {
                    hatSpritesActual.imgBack = scriptableInfos.hats[i].imgSpriteBACK;
                    hatSpritesActual.imgFront = scriptableInfos.hats[i].imgSpriteFRONT;
                    hatSpritesActual.imgSide = scriptableInfos.hats[i].imgSpriteSIDE;
                }
            }

            // change imediately the selected sprite inside your body
            ChangeBodySprite("Front");
        }

        public void SetShirt(string itemName)
        {
            for (int i = 0; i < scriptableInfos.body.Length; i++)
            {
                if (itemName == scriptableInfos.body[i].nameItem)
                {
                    bodySpritesActual.imgBack = scriptableInfos.body[i].imgSpriteBACK;
                    bodySpritesActual.imgFront = scriptableInfos.body[i].imgSpriteFRONT;
                    bodySpritesActual.imgSide = scriptableInfos.body[i].imgSpriteSIDE;
                }
            }

            ChangeBodySprite("Front");
        }

        public void SetArm(string itemName)
        {
            for (int i = 0; i < scriptableInfos.arms.Length; i++)
            {
                if (itemName == scriptableInfos.arms[i].nameItem)
                {
                    armSpritesActual.imgBack = scriptableInfos.arms[i].imgSpriteBACK;
                    armSpritesActual.imgFront = scriptableInfos.arms[i].imgSpriteFRONT;
                    armSpritesActual.imgSide = scriptableInfos.arms[i].imgSpriteSIDE;
                }
            }

            ChangeBodySprite("Front");
        }

        public void SetLeg(string itemName)
        {
            for (int i = 0; i < scriptableInfos.legs.Length; i++)
            {
                if (itemName == scriptableInfos.legs[i].nameItem)
                {
                    legSpritesActual.imgBack = scriptableInfos.legs[i].imgSpriteBACK;
                    legSpritesActual.imgFront = scriptableInfos.legs[i].imgSpriteFRONT;
                    legSpritesActual.imgSide = scriptableInfos.legs[i].imgSpriteSIDE;
                }
            }

            ChangeBodySprite("Front");
        }

        public void ChangeBodySprite(string direction)
        {
            // compare what direction the player is facing to add the correct sprite to him
            switch (direction)
            {
                case "Front":
                    hatPiece.sprite = hatSpritesActual.imgFront;
                    bodyPiece.sprite = bodySpritesActual.imgFront;
                    armLeftPiece.sprite = armSpritesActual.imgFront;
                    armRightPiece.sprite = armSpritesActual.imgFront;
                    legLeftPiece.sprite = legSpritesActual.imgFront;
                    legRightPiece.sprite = legSpritesActual.imgFront;
                    break;
                case "Back":
                    hatPiece.sprite = hatSpritesActual.imgBack;
                    bodyPiece.sprite = bodySpritesActual.imgBack;
                    armLeftPiece.sprite = armSpritesActual.imgBack;
                    armRightPiece.sprite = armSpritesActual.imgBack;
                    legLeftPiece.sprite = legSpritesActual.imgBack;
                    legRightPiece.sprite = legSpritesActual.imgBack;
                    break;
                case "Side":
                    hatPiece.sprite = hatSpritesActual.imgSide;
                    bodyPiece.sprite = bodySpritesActual.imgSide;
                    armLeftPiece.sprite = armSpritesActual.imgSide;
                    armRightPiece.sprite = armSpritesActual.imgSide;
                    legLeftPiece.sprite = legSpritesActual.imgSide;
                    legRightPiece.sprite = legSpritesActual.imgSide;
                    break;
            }
        }
    }

    // use this class to specify each sprite direction of the body character 
    [System.Serializable]
    public class ActualItens
    {
        public Sprite imgFront;
        public Sprite imgBack;
        public Sprite imgSide;
    }

    public ChangeItens characterBody;
}
