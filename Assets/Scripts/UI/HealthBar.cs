using System.Collections;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour,IMobComponent
{
    [SerializeField] private GameObject Bar;
    [SerializeField] private SpriteRenderer BarImg;
    [SerializeField] private TMP_Text Text;
    [SerializeField] private TMP_Text ChangesText;
    private float maxHP;
    private void Awake()
    {
        var mob = GetComponent<Mob>();
        maxHP = mob.GetMaxHealth();
        mob.AddListenerOnHPChange(OnHPChange);
    }

    public void OnDeath()
    {
        Bar.SetActive(false);
    }
    
    private void LateUpdate()
    {
        Bar.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnHPChange(float health, float diff)
    {
        if (diff != 0) StartCoroutine(ShowChanges(diff));
        var frac = health / maxHP;
        Text.text = $"{health:####}/{maxHP:####}";
        BarImg.size = new Vector2(frac, BarImg.size.y);
        var pos = BarImg.transform.localPosition;
        pos.x = -(1 - frac) / 2;
        BarImg.transform.localPosition = pos;
    }

    private IEnumerator ShowChanges(float diff)
    {
        if (diff > 0) ChangesText.color = Color.green;
        else ChangesText.color = Color.red;
        ChangesText.text = diff.ToString();
        yield return new WaitForSeconds(1);
        ChangesText.text = "";
    }
}