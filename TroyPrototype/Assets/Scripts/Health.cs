using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int HP;
    public int NumOHeart;

    public Image[] hearts;
    public Sprite fullHearts;
    public Sprite emptyHearts;

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (HP > NumOHeart) //Keep HP to the maximum they have displayed
            {
                HP = NumOHeart;
            }

            if (i < HP) //Displays full hearts or emptyHearts
            {
                hearts[i].sprite = fullHearts;
            }
            else
            {
                hearts[i].sprite = emptyHearts;
            }

            if (i < NumOHeart)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        //need to code collision hit dmg to lose hp and then when it reaches 0 display death screen



    }
    //void FixedUpdate() {
    //    public void OnTriggerEnter2D(Collider2D collision)
    //    {
    //        if (collision.tag == "ProjectileEnemy")
    //        {
    //            hp -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
    //            Destroy(collision.gameObject);
    //        }
    //    }
    //}
}
