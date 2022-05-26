using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMeter : MonoBehaviour
{
    public Image BulletHitImage;
    bool NewBulletHit;
    bool BulletHit;
    Color ImageColor;
    private void Start()
    {
        BulletHitImage = GetComponent<Image>();
    }
    public void HitDetection()
    {
        StopAllCoroutines();
        NewBulletHit = true;
        BulletHit = true;
        ImageColor = BulletHitImage.color;
        ImageColor.a += 10*Time.deltaTime;
        if (BulletHitImage.fillAmount < 0.04f)
            BulletHitImage.fillAmount += 0.01f*10*Time.deltaTime;
        BulletHitImage.color = ImageColor;
        StartCoroutine(ReduceAlpha());
    }
    public bool DetectionFull()
    {
        if(BulletHitImage.fillAmount > 0.03f)
            BulletHitImage.color = Color.yellow;
        return BulletHitImage.fillAmount > 0.03f;
    }
    IEnumerator ReduceAlpha()
    {
        BulletHit = false;
        NewBulletHit = false;
        yield return new WaitForSeconds(2f);
        while(BulletHitImage.color.a>0 && BulletHitImage.fillAmount > 0)
        {
            ImageColor = BulletHitImage.color;
            ImageColor.a -= 10*Time.deltaTime;
            BulletHitImage.color = ImageColor;
            BulletHitImage.fillAmount -= 0.01f*Time.deltaTime;
        }
        ImageColor.a = 0;
        BulletHitImage.color = ImageColor;
        BulletHitImage.fillAmount = 0;
    }
}
