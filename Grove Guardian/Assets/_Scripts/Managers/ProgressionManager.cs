using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : Singleton<ProgressionManager> {
    public Action OnPointCompletion { get; private set; }

    [Header("Balance")]
    [SerializeField] private int _timeToComplete;
    [SerializeField] private int _pointsPerSecond;

    [Header("System")]
    [SerializeField] private Slider _slider;

    public bool IsFinished { get; private set; } = false;
    private float _currentProgression = 0;
    private float _maxProgression = 0;
    private bool _hasInvoked = false;
    
    private void Start() {
        _maxProgression = _timeToComplete * _pointsPerSecond;
        _slider.maxValue = _maxProgression;
        GameManager.Instance.OnNewWave += ResetProgression;
    }
    private void FixedUpdate() {
        if(_maxProgression >= _currentProgression) {
            if(_currentProgression < 0) {
                _currentProgression = 0;
                return;
            }
            _currentProgression += _pointsPerSecond * Time.fixedDeltaTime;
            _slider.value = _currentProgression;
        } else if(!_hasInvoked) {
            _hasInvoked = true;
            OnPointCompletion?.Invoke();
            IsFinished = true;
        }
    }

    public void ResetProgression() {
        _currentProgression = 0;
        _hasInvoked = false;
        IsFinished = false;
    }

    public void AddProgression(int value) {
        _currentProgression += value;
    }

    public void RemoveProgression(int value) {
        _currentProgression -= value;
    }

    public void RemoveConstantProgression(int value) {
        _pointsPerSecond -= value;
    }

    public void AddConstantProgression(int value) {
        _pointsPerSecond += value;
    }

    public void AddPointCompletionListener(Action newListener) {
        OnPointCompletion += newListener;
    }
    public void RemovePointCompletionListener(Action listenerToRemove) {
        OnPointCompletion -= listenerToRemove;
    }
}
