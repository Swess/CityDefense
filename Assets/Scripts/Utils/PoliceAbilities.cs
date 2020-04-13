﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UI;
using UnityEngine.UI;

public class PoliceAbilities : MonoBehaviour
{
    //change to actual class of it? even needed? 
    //GameObject policeSquadLeader;

    //rioters
    //specify class
    public TesterRioter rioterTarget;

    //Grenade prefab
    public GameObject smokeGrenade;
    public float launchForce;

    //bullets
    //specify class?
    public List<GameObject> bulletTypes;
    GameObject bulletSetType;
    float bulletDamage;

    //water cannon
    public GameObject waterCannon;

    //arresting images and info bubble
    public Sprite arrestImg;
    public Sprite failedArrestImg;
    public Sprite aggressiveArrestImg;
    public Sprite failedAggressiveArrestImg;
    ScreenSpaceTargetBubble infoBubble;




    // Start is called before the first frame update
    void Start()
    {
        infoBubble = FindObjectOfType<ScreenSpaceTargetBubble>();
        bulletSetType = bulletTypes[0];
        bulletDamage = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void Arrest()
    {
        //logic for less points deducted from PR
        //...

        //show appropriate arresting image based on success/fail
        if (rioterTarget && rioterTarget.health <= 25.0f)
        {
            infoBubble.SetFramingImage(arrestImg);
            infoBubble.Open();
            Destroy(rioterTarget);
            infoBubble.Close();
        }
        else
        {
            infoBubble.SetFramingImage(failedArrestImg);
            infoBubble.Open();
            infoBubble.Close();
        }
    }
    
    public void AggressiveArrest()
    {
        //logic for more points deducted from PR
        //...

        //show appropriate arresting image based on success/fail
        if (rioterTarget)
        {
            infoBubble.SetFramingImage(aggressiveArrestImg);
            infoBubble.Open();
            Destroy(rioterTarget);
            infoBubble.Close();
        }
        else
        {
            infoBubble.SetFramingImage(failedAggressiveArrestImg);
            infoBubble.Open();
            infoBubble.Close();
        }
    }

    public void FireBullets()
    {
        //logic for PR system cost
        //...

        //logic for firing bullets
        if (rioterTarget)
        {
            Vector3 start = transform.position;
            Vector3 direction = rioterTarget.transform.position - start;
            float distance = direction.magnitude;

            //get the layermask of the rioter
            int layerMask = 1 << 10;

            //invert this layermask to collide with everything, but the player
            layerMask = ~layerMask;

            //check if we hit something else besides the rioter
            //if no such collision, then check if the player is visible from the front of 
            //the zombie tank and if so, then fire (0.2f was tested to be front enough)
            RaycastHit hit;
            if (direction != Vector3.zero)
            {
                if (!Physics.Raycast(start, direction.normalized, out hit, distance, layerMask))
                {
                    GameObject bullet = Instantiate(bulletSetType, transform.position, bulletSetType.transform.rotation); //TODO fix so spawn point isn't animation spawn point/prefab spawn point
                    if(bulletSetType == bulletTypes[0])
                        bullet.GetComponent<Animator>().Play("RubberBulletsAnimation");     //TODO so that animation is from transform.position
                    else
                        bullet.GetComponent<Animator>().Play("LethalBulletsAnimation");     //TODO so that animation is from transform.position
                    rioterTarget.TakeDamage(bulletDamage);
                }
            }
        }
    }

    public void UseRubberBullets()
    {
        //logic for PR system cost
        //...

        bulletSetType = bulletTypes[0];
        bulletDamage = 10.0f;
    }

    public void UseSmokeGrenade()
    {
        //logic for throwing grenade at an arch
        GameObject sG = Instantiate(smokeGrenade, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
        sG.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }

    public void UseWaterCannon()
    {
        //logic for PR system cost
        //...

        //logic for using water cannon
        Instantiate(waterCannon, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
    }

    public void UseLethalBullets()
    {
        //logic for PR system cost
        //...

        bulletSetType = bulletTypes[1];
        bulletDamage = 30.0f;
    }

    public void ReinforceSquad()
    {
        //logic for PR system cost
        //...

        //logic for summoning a police squad member
        //Create a flock object
        //spawn point is at donut shop
        //make it join calling flock/leader in calling flock
        //make sure they're not an Agent 0...
        //make sure they avoid rioters on way to calling flock
    }
}