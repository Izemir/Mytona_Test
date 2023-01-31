using UnityEngine;

//Bad class
public class PowerupSpawner : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private float HealthUpgradeWeight = 10;
    [SerializeField][Range(0, 100)] private float DamageUpgradeWeight = 10;
    [SerializeField][Range(0, 100)] private float MoveSpeedUpgradeWeight = 5;
    [SerializeField][Range(0, 100)] private float HealWeight = 25;
    [SerializeField][Range(0, 100)] private float WeaponChangeWeight = 2;
    [SerializeField][Range(0, 100)] private float RifleWeight = 25;
    [SerializeField][Range(0, 100)] private float AutomaticRifleWeight = 15;
    [SerializeField][Range(0, 100)] private float ShotgunWeight = 20;
    [SerializeField][Range(0, 100)] private float SniperRifleWeight = 15;

    [SerializeField] private PowerUp HealthPrefab;
    [SerializeField] private PowerUp DamagePrefab;
    [SerializeField] private PowerUp MoveSpeedPrefab;
    [SerializeField] private HealthPack HealPrefab;
    [SerializeField] private WeaponPowerUp RiflePrefab;
    [SerializeField] private WeaponPowerUp AutomaticRifleWPrefab;
    [SerializeField] private WeaponPowerUp ShotgunPrefab;
    [SerializeField] private WeaponPowerUp SniperRiflePrefab;

    [SerializeField] private int ActualWeaponType = 0;

    private float[] weights;
	private float[] weaponWeights;
    
	private GameObject[] prefabs;
	private WeaponPowerUp[] weaponPrefabs;
    
	private void Awake()
	{
		weights = new float[5];
		weights[0] = HealthUpgradeWeight;
		weights[1] = weights[0] + DamageUpgradeWeight;
		weights[2] = weights[1] + MoveSpeedUpgradeWeight;
		weights[3] = weights[2] + HealWeight;
		weights[4] = weights[3] + WeaponChangeWeight;
        
		weaponWeights = new float[4];
		weaponWeights[0] = RifleWeight;
		weaponWeights[1] = weaponWeights[0] + ShotgunWeight;
		weaponWeights[2] = weaponWeights[1] + AutomaticRifleWeight;
        weaponWeights[3] = weaponWeights[2] + SniperRifleWeight;

        prefabs = new[]
		{
			HealthPrefab.gameObject, 
			DamagePrefab.gameObject, 
			MoveSpeedPrefab.gameObject, 
			HealPrefab.gameObject
		};
		weaponPrefabs = new[]
		{
			RiflePrefab,
			ShotgunPrefab,
            AutomaticRifleWPrefab,
			SniperRiflePrefab
        };
        Player.Instance.OnWeaponChange += SetWeaponType;
        EventBus.Sub(Handle, EventBus.MOB_KILLED);
	}

    private void SetWeaponType(int type)
    {
		ActualWeaponType = type;
    }

    private void Handle()
	{
		Spawn(PickRandomPosition());
	}
	
	private Vector3 PickRandomPosition()
	{
		var vector3 = new Vector3();
		vector3.x = Random.value * 11 - 6;
		vector3.z = Random.value * 11 - 6;
		return vector3;
	}


	private void Spawn(Vector3 position)
	{
		int i = GetRandomType(weights);	

		if (i < 4)
		{
			Instantiate(prefabs[i], position, Quaternion.identity);
		}
		else
		{
			int weaponType = ActualWeaponType;
			do
			{
				weaponType = GetRandomType(weaponWeights);
			}
			while (weaponType == ActualWeaponType);
			Instantiate(weaponPrefabs[weaponType], position, Quaternion.identity);
		}
	}

	private int GetRandomType(float[] floats)
	{
		var floatsEnd = floats.Length - 1;
        var rand = Random.Range(0, floats[floatsEnd]);

        int i = 0;
        if (rand > floats[0])
        {
            foreach (var type in floats)
            {
                if (rand > type) i++;
                else break;
            }
        }

		return i;
    }
}