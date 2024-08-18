using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishPopFeed : MonoBehaviour
{
    public Image fishImage;
    public Image rarityBG;

    public Sprite yellowBG, purpleBG, blueBG, greenBG;

    private float lifeTime = 4f;

    private RectTransform rt;
    
    public void Setup(Fish f)
    {
        rt = GetComponent<RectTransform>();

        rt.localScale = Vector3.zero;

        switch (f.rarity)
        {
            case (Fish.Rarity.Common):
                rarityBG.sprite = greenBG;
                break;
            case (Fish.Rarity.Uncommon):
                rarityBG.sprite = blueBG;
                break;
            case (Fish.Rarity.Rare):
                rarityBG.sprite = purpleBG;
                break;
            case (Fish.Rarity.Legendary):
                rarityBG.sprite = yellowBG;
                break;
        }

        fishImage.sprite = f.fishPortrait;

        LeanTween.scale(rt, Vector3.one, 0.3f).setEaseOutElastic();

        StartCoroutine(TimedDestroy());
    }

    private IEnumerator TimedDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        LeanTween.scale(rt, Vector3.zero, 0.2f).setEaseInOutBack().setOnComplete(() => Destroy(this.gameObject));
    }
    

}
