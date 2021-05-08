using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public Animator anim;
    public Transform target;
    public Transform camTransform;
    public GameObject explosion;
    public ParticleSystem Flare;

    public Text currentAmmoCountText, slotsText;

    //shooting effects
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private float nextTimeToFire = 0f;
    [SerializeField] private float reloadTime = 1.4f;
    public float weaponDamage = 20f;

    //ammo
    public int availableSlots;
    public float totalAmmo = 70f;
    public float maxCountOfCurrentAmmo = 25f;
    public float currentAmmo = 25f;

    private void Start() {

        anim = GetComponent<Animator>();
        totalAmmo -= currentAmmo;
        availableSlots = (int)Mathf.Ceil(totalAmmo / maxCountOfCurrentAmmo);
        SetUI();

    }

    private void Update () {

        if (Input.GetKeyDown(KeyCode.R) && totalAmmo > 0) {
            StartCoroutine(Reload());
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
		
	}

    public IEnumerator Reload() {

        anim.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = totalAmmo >= maxCountOfCurrentAmmo ? maxCountOfCurrentAmmo : totalAmmo;
        totalAmmo -= currentAmmo;
        availableSlots -= 1;
        SetUI();

    }

    public void Shoot() {
        
        if (currentAmmo > 0) {

            Flare.Play();
            currentAmmo--;

            RaycastHit hit;
            if (Physics.Raycast(camTransform.transform.position, camTransform.transform.forward, out hit, range)) {
                /*
                 * Do Damage...
                 * Enemy enemy = hit.transform.GetComponent<Enemy>();
                 * enemy.takeDamage(weaponDamage);
                 */

                //Chain Destroying...
                GameObject hitedObject = hit.transform.gameObject;
                if (hitedObject.tag.Equals("Chain"))
                    Destroy(hitedObject);

            }

            GameObject generatedExplosion = Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(generatedExplosion, 0.2f);
            SetUI();
        }

    }

    public void AddBullets(int bulletPickUpCount) {
        totalAmmo += bulletPickUpCount;
        availableSlots = (int)Mathf.Ceil(totalAmmo / maxCountOfCurrentAmmo);
        SetUI();
    }

    public void SetUI() {
        currentAmmoCountText.text = currentAmmo.ToString();
        slotsText.text = availableSlots.ToString();
    }

}
