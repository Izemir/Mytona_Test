using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
	[SerializeField] private GameObject Bar;
	[SerializeField] private SpriteRenderer BarImg;
	[SerializeField] private TMP_Text Text;
	[SerializeField] private TMP_Text DamageText;
    [SerializeField] private TMP_Text ChangesText;
    private float maxHP;
	private Player player;
	private void Awake()
	{
		player = GetComponent<Player>();
		player.AddListenerOnHPChange(OnHPChange);
	}

	public void OnDeath()
	{
		Bar.SetActive(false);
	}
    
	private void LateUpdate()
	{
		Bar.transform.rotation = Camera.main.transform.rotation;
	}

	private IEnumerator ShowChanges(float diff)
	{
		if (diff > 0) ChangesText.color = Color.green;
		else ChangesText.color = Color.red;
		ChangesText.text = diff.ToString();
        yield return new WaitForSeconds(1);
        ChangesText.text = "";
    }

	private void OnHPChange(float health, float diff)
	{
		if(diff!=0) StartCoroutine(ShowChanges(diff));
		var frac = health / player.GetMaxHealth();
		Text.text = $"{health:####}/{player.GetMaxHealth():####}";
		BarImg.size = new Vector2(frac, BarImg.size.y);
		var pos = BarImg.transform.localPosition;
		pos.x = -(1 - frac) / 2;
		BarImg.transform.localPosition = pos;
		if (health <= 0)
		{
			Bar.SetActive(false);
		}
	}

	private void OnUpgrade()
	{
		DamageText.text = $"{player.GetDamage()}";
	}
}