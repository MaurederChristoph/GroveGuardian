using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Unit : MonoBehaviour {
    private const float TICK_RATE = 0.5f;

    [SerializeField] protected int _maxHealth;
    [SerializeField] protected HealthBar _healthBar;

    [Header("Sound")]
    [SerializeField] protected SoundClips _OnHit;
    [SerializeField] protected SoundClips _OnDeath;
    [SerializeField] protected SoundClips _OnSpawn;

    protected Action<GameObject> OnDeath { get; set; }
    protected int _currentHealth;

    private readonly List<DamageInstance> _continuousDamageValues = new();
    private GameManager _gameManager;
    private bool _canInterrupt = true;

    virtual public void Start() {
        _healthBar.SetMaxHealth(_maxHealth);
        _currentHealth = _maxHealth;
        _gameManager = GameManager.Instance;
        PlaySound(_OnSpawn);
        _gameManager.OnNewWave += UpdateHealth;
    }

    public void UpdateHealth() {
        if(this is PlayerController) {
            _healthBar.SetMaxHealth(_maxHealth + _gameManager.PlayerHealthBonus);
            _currentHealth += _gameManager.PlayerHealthBonus;
        }
        if(this is EnemyUnit) {
            _healthBar.SetMaxHealth(_maxHealth + _gameManager.EnemyHealthBonus);
        }
    }

    public void DamageOverTime(DamageInstance damageInstance, float time) {
        StartContinuousDamage(damageInstance);
        StartCoroutine(DelayedEndContinuousDamage(damageInstance, time));
    }

    public void StartContinuousDamage(DamageInstance damage) {
        _continuousDamageValues.Add(damage);
        HandleDot();
    }

    public void EndContinuousDamage(DamageInstance damage) {
        _continuousDamageValues.Remove(damage);
    }

    private void HandleDot() {
        if(_continuousDamageValues.Count != 0) {
            Invoke(nameof(HandleDot), TICK_RATE);
        }

        foreach(var damage in _continuousDamageValues) {
            HandleDamage(damage);
        }
    }

    public virtual void HandleDamage(DamageInstance damageInstance) {
        switch(damageInstance.DamageType) {
            case DamageType.normal:
                _currentHealth -= damageInstance.Damage;
                HitEffect();
                if(this is EnemyUnit unit) {
                    unit.InterruptMovement();
                    if(_canInterrupt) {
                        GetComponent<Blackboard>().JustWasDamaged = true;
                        GetComponent<EnemyBehaviorTree>().InterruptTree();
                        _canInterrupt = false;
                    }
                    CancelInvoke(nameof(ResetDamaged));
                    Invoke(nameof(ResetDamaged), 1.5f);
                }
                if(_currentHealth <= 0) {
                    OnDeath?.Invoke(gameObject);
                    PlaySound(_OnDeath);
                    Destroy(gameObject);
                }
                PlaySound(_OnHit);
                break;
            case DamageType.heal:
                _currentHealth += damageInstance.Damage;
                if(_currentHealth > _maxHealth) {
                    _currentHealth = _maxHealth;
                }
                break;
        }
        _healthBar.SetHealth(_currentHealth);
    }

    private void ResetDamaged() {
        _canInterrupt = true;
        GetComponent<Blackboard>().JustWasDamaged = false;
    }

    private void PlaySound(SoundClips clip) {
        if(clip == _OnDeath && _OnDeath != SoundClips._None) {
            SoundManager.Instance.PlaySoundAt(gameObject.transform.position, clip, 1f);
        } else if(clip != SoundClips._None) {
            SoundManager.Instance.PlaySoundAt(gameObject.transform.position, clip);
        }
    }

    private IEnumerator DelayedEndContinuousDamage(DamageInstance damageInstance, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        EndContinuousDamage(damageInstance);
    }

    public void AddDeathListener(Action<GameObject> action) {
        OnDeath += action;
    }

    public void RemoveDeathListener(Action<GameObject> action) {
        OnDeath -= action;
    }

    protected abstract void HitEffect();
}
