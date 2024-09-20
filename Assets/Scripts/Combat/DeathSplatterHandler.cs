using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    private void OnEnable() 
    {
        Health.OnDeath += SpawnDeathSplatterPrefab;
        Health.OnDeath += SpawnDeathVFX;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatterPrefab;
        Health.OnDeath -= SpawnDeathVFX;
    }


    private void SpawnDeathSplatterPrefab(Health sender)
    {
        GameObject newSplatterPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, transform.rotation);
        ColorChanger splatterColorChanger = newSplatterPrefab.GetComponent<ColorChanger>();
        splatterColorChanger.SetColor(sender.GetComponent<ColorChanger>().DefaultColor);
        newSplatterPrefab.transform.SetParent(transform);
    }

    private void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFX = Instantiate(sender.DeathVFXPrefab, sender.transform.position, transform.rotation);
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
        ps.startColor = sender.GetComponent<ColorChanger>().DefaultColor;        
        deathVFX.transform.SetParent(transform);
    }
}
