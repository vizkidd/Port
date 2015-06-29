using UnityEngine;
using System.Collections;
using TagFrenzy;

public class GunFire : MonoBehaviour {

	public ParticleSystem muzzle_flash;
	public GameObject barrel;

	public GameObject impactPrefab;
	GameObject[] impacts;
	int currentImpact = 0;
	int maxImpacts = 5;
	int tempcount;
	struct Gun
	{
		public float range,piercing,damage,bullets;
		public Gun(float r,float p,float d,float b)
		{
			range=r;piercing=p;damage=d;bullets=b;
		}
	}
	Animator anim;
	bool fire;
	RaycastHit[] rays;
	bool previousState,currentState,shooting;
	Gun gun,pistol;
	// Use this for initialization
	void Start () {
	
		muzzle_flash=GetComponentInChildren<ParticleSystem>();
		anim=GetComponentInChildren<Animator>();
		Debug.Log(muzzle_flash.transform.position);
		rays=new RaycastHit[5];
		gun=new Gun();
		pistol=new Gun(25,2,10,1);
		gun=pistol;

		impacts = new GameObject[maxImpacts];
		for(int i = 0; i < maxImpacts; i++)
		{
			impacts[i] = (GameObject)Instantiate(impactPrefab);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
		gun=(Gun)pistol;
		previousState=currentState;
		currentState=Input.GetButtonDown("Fire1");
		if(currentState!=previousState && previousState!=true && !Input.GetKey(KeyCode.LeftShift))
		{
			anim.SetTrigger("Fire");
			shooting=true;
			muzzle_flash.Play();
		}
        
	}

	void FixedUpdate(){
		if(shooting)
		{
			Bullets(rays,barrel.transform,gun.piercing,gun.range,gun.bullets);
			shooting=false;
		}

	}
	void Bullets(RaycastHit[] rays,Transform origin,float piercingFactor,float range,float bullets)
	{
		if(shooting)
		{
			for(int i=0;i<bullets;i++)
			{
				Physics.Raycast(origin.position,origin.forward,out rays[i],range*piercingFactor);
			}
			for(int i=0;i<bullets;i++)
			{
			
				Debug.Log("Rays["+i+"] transfprm:"+rays[i].transform.tag+" count:"+tempcount++);
				if(piercingFactor>0)
				{
				switch (rays[i].transform.tag) {
					default:

						piercingFactor=0;
						impacts[currentImpact].transform.position = rays[i].point;
						impacts[currentImpact].GetComponent<ParticleSystem>().Play();
						shooting=false;
						break;
					case "Enemy":
						//reduce enem's health


						//subtract piercing factor using hardness of object
						//piercingFactor-=rays[i].gameObject.hardness;
						piercingFactor-=1;

						Bullet(rays[i],rays[i].transform,piercingFactor,gun.range,gun.bullets);
						break;
					case "Passable":
						//subtract piercing factor using hardness of object
						//piercingFactor-=rays[i].gameObject.hardness;
						piercingFactor-=1;
						
						impacts[currentImpact].transform.position = rays[i].point;
						impacts[currentImpact].GetComponent<ParticleSystem>().Play();

						Bullet(rays[i],rays[i].transform,piercingFactor,gun.range,gun.bullets);
						break;
					case "Impenetrable":
						piercingFactor=0;

						impacts[currentImpact].transform.position = rays[i].point;
						impacts[currentImpact].GetComponent<ParticleSystem>().Play();
						shooting=false;
						break;
					}

					if(++currentImpact >= maxImpacts)
						currentImpact = 0;
					if(!shooting || piercingFactor==0)
						break;
				}
				else
					shooting=false;
			}
		}

	}

	void Bullet(RaycastHit ray,Transform origin,float piercingFactor,float range,float bullets)
	{
		if(shooting)
		{

			Physics.Raycast(origin.position,origin.forward,out ray,range*piercingFactor);
			Debug.Log("Ray transfprm:"+ray.transform.tag);
				if(piercingFactor>0)
				{
					switch (ray.transform.tag) {
				default:
					
					piercingFactor=0;
					impacts[currentImpact].transform.position = ray.point;
					impacts[currentImpact].GetComponent<ParticleSystem>().Play();
					shooting=false;
					break;
				
				case "Enemy":
						//reduce enem's health
						
						
						//subtract piercing factor using hardness of object
						//piercingFactor-=rays[i].gameObject.hardness;
						piercingFactor-=1;

					impacts[currentImpact].transform.position = ray.point;
					impacts[currentImpact].GetComponent<ParticleSystem>().Play();
					
						Bullet(ray,ray.transform,piercingFactor,gun.range,gun.bullets);
						break;
					case "Passable":
						//subtract piercing factor using hardness of object
						//piercingFactor-=rays[i].gameObject.hardness;
						piercingFactor-=1;

					impacts[currentImpact].transform.position = ray.point;
					impacts[currentImpact].GetComponent<ParticleSystem>().Play();

					Bullet(ray,ray.transform,piercingFactor,gun.range,gun.bullets);
						break;
					case "Impenetrable":
						piercingFactor=0;

					impacts[currentImpact].transform.position = ray.point;
					impacts[currentImpact].GetComponent<ParticleSystem>().Play();
					shooting=false;
						break;
					}
				if(++currentImpact >= maxImpacts)
					currentImpact = 0;
			}
				else
					shooting=false;
			}
		}
		

}
