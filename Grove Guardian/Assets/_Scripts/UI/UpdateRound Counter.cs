using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class UpdateRoundCounter : MonoBehaviour
{
    [SerializeField] TMP_Text _textMeshPro;

    private void Start() {
        GameManager.Instance.OnNewWave += UpdateCounter;
    }

    private void UpdateCounter() {
        _textMeshPro.text = "Round " + GameManager.Instance.CurrentWave;
    }
}
