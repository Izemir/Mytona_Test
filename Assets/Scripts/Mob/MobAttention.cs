using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Playables;

public class MobAttention : MonoBehaviour
{
    [SerializeField] private TMP_Text attentionText;
    private Mob mob;
    private MeleeAttack meleeAttack;
    private RangeAttack rangeAttack;
    private void Start()
    {
        mob = GetComponent<Mob>();
        if (GetComponent<RangeAttack>())
        {
            rangeAttack = GetComponent<RangeAttack>();
            rangeAttack.AddListenerOnAttack(ShowAttack);
        }
        if (GetComponent<MeleeAttack>())
        {
            meleeAttack = GetComponent<MeleeAttack>();
            meleeAttack.AddListenerOnAttack(ShowAttack);
        }
        
    }
    public void ShowAttack()
    {
        StartCoroutine(ChangeText());        
    }

    private IEnumerator ChangeText()
    {
        attentionText.text = "!";
        yield return new WaitForSeconds(1);
        attentionText.text = "";
    }
}
