using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloframeBehaviour : MonoBehaviour
{
    //Stores the information about the icon.
    private GameObject icon;
    private SpriteRenderer iconRend;
    private Sprite[] iconSprites;
    private Sprite chosenIcon;

    //Stores the information about the HoloframeBorder
    private GameObject border;

    //This is a trigger collider that boosts the player when touched.
    [SerializeField] private BoxCollider booster;
    [SerializeField] private Sprite boosterIcon;

    //The boost force given to the player!
    [SerializeField] private float boostForce = 1;


    private void Start()
    {
        //Instantiate the icon attributes.
        icon = transform.Find("Icon").gameObject;
        iconSprites = Resources.LoadAll<Sprite>("HoloframeIcons");
        iconRend = icon.GetComponent<SpriteRenderer>();
        iconRend.sprite = ChooseRandomSprite();

        //Instantiate the border attributes
        border = transform.Find("Holoframe Border").gameObject;
    }

    private void Update()
    {
        if (ColorDictionary.StringToColorConversion[border.tag] == ColorController.currentColor)
        {
            iconRend.sprite = boosterIcon;

            booster.enabled = true;

        }
        else
        {
            iconRend.sprite = chosenIcon;

            booster.enabled = false;
        }
    }

    private Sprite ChooseRandomSprite()
    {
        int index = Random.Range(0, iconSprites.Length);

        chosenIcon = iconSprites[index];

        return chosenIcon;
    }

    public void Boost()
    {
        StartCoroutine(Blink(10, transform.forward, boostForce));
        Vector3 yOffset = new Vector3(0, GameObject.Find("World").transform.position.y, 0);
        GameObject.Find("World").transform.position -= yOffset;
        GameObject.Find("Player").transform.position -= yOffset;
    }

    private IEnumerator Blink(int blinks, Vector3 dir, float force)
    {
        for (int i = 0; i < blinks; i++)
        {
            GameObject.Find("World").transform.position -= new Vector3(dir.x, 0, dir.z) * force;
            GameObject.Find("Player").transform.position += new Vector3(0, dir.y, 0) * force;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
