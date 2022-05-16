using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.FPS.Game;
using UnityEngine;

public class WinView : MonoBehaviour
{
    [SerializeField] TMP_Text txtScore;

    private void Start()
    {
        txtScore.text = $"You WIN: {EventManager.s_Score} score";
    }
}
