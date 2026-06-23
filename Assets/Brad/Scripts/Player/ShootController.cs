using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 8f;
    [SerializeField] private float bulletSpeed = 15f;

    [Header("Ammo")]
    [SerializeField] private int magazineSize = 12;
    [SerializeField] private int reserveAmmo = 60;
    [SerializeField] private float reloadTime = 1.5f;

    private int currentAmmo;
    private bool isReloading;
    private float nextFireTime;

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public bool IsReloading => isReloading;

    private void Start()
    {
        currentAmmo = magazineSize;
    }

    public void OnShoot(InputAction.CallbackContext context) //works but stretch goal to add hold button fire
    {
        if (!context.performed)
            return;

        if (Time.time < nextFireTime)
            return;

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            if (reserveAmmo > 0)
                StartCoroutine(Reload());

            return;
        }

        nextFireTime = Time.time + (1f / fireRate);

        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (isReloading)
            return;

        if (currentAmmo == magazineSize || reserveAmmo <= 0)
            return;

        StartCoroutine(Reload());
    }
    
    private IEnumerator Reload()
    {
        Debug.Log("Reloading...");
        
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = magazineSize - currentAmmo;
        int bulletsToLoad = Mathf.Min(bulletsNeeded, reserveAmmo);

        currentAmmo += bulletsToLoad;
        reserveAmmo -= bulletsToLoad;

        isReloading = false;
    }

    public void AddAmmo(int amount) //might need this not sure
    {
        reserveAmmo += amount;
    }
}
