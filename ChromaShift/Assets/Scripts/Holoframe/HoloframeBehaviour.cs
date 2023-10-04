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
    [SerializeField] private float boostForce = 1f;

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
        if (!border.activeInHierarchy)
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
        Vector3 rotation = transform.localEulerAngles;
        Vector3 launchDir = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * rotation.y), 0, -Mathf.Cos(Mathf.Deg2Rad * rotation.y));

        GameObject.Find("World").GetComponent<Rigidbody>().velocity = launchDir * boostForce;
    }
}
