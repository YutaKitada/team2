using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishStarScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 5, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Contains("Enemy"))
        {
            if(collision.transform.tag == "Enemy")
            {
                collision.transform.GetComponent<Enemy>().Damage();
            }
            
            SoundManager.PlaySE(5);
            UIManager.hpGageFillAmount += 10;
            GameManager.combo++;
            ComboUI.comboTimer = 0;
        }
    }
}
