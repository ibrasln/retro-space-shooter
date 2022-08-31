using System.Collections;
using UnityEngine;

public class SupLaserController : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Transform laserPos;

    private void Start()
    {
        StartCoroutine(CreateLaser());
    }

    IEnumerator CreateLaser()
    {
        yield return new WaitForSeconds(2f);
        laserPos.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject laser = Instantiate(laserPrefab, laserPos.position, laserPos.rotation);      
        laserPos.gameObject.SetActive(false);
        Destroy(laser, 2);
        Destroy(gameObject, 3.5f);
    }

}
