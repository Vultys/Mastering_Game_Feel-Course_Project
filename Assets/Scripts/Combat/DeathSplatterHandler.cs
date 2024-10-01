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
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        if(colorChanger != null)
        {
            splatterColorChanger?.SetColor(sender.GetComponent<ColorChanger>().DefaultColor);
        }
        newSplatterPrefab.transform.SetParent(transform);
    }

    private void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFX = Instantiate(sender.DeathVFXPrefab, sender.transform.position, transform.rotation);
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        if(colorChanger != null)
        {
            ps.startColor = colorChanger.DefaultColor;  
        }      
        deathVFX.transform.SetParent(transform);
    }
}
