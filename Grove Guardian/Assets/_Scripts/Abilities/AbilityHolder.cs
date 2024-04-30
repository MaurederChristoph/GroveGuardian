using System;
using UnityEngine;

public class AbilityHolder : MonoBehaviour {
    private const int MAX_ABILITIES = 5;
    [SerializeField] private Animator _animator;

    [SerializeField]
    private KeyCode[] _keys = {
        KeyCode.Mouse0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3
    };

    public AbilityBundle[] Abilities { get; private set; } = new AbilityBundle[MAX_ABILITIES];
    [SerializeField] private MoodAbility _basicAbility;
    [SerializeField] private MoodAbility _rangedAbility;
    [SerializeField] private MoodAbility _meleeAbility;
    [SerializeField] private MoodAbility _utilAbility;
    [SerializeField] private MoodAbility _dash;
    private bool _currentlyCasting { get; set; } = false;

    public void Start() {
        SetUpAbilities();
        InitializeAbilities();
        UpdateIcons();
    }

    private void SetUpAbilities() {
        SetAbility(_basicAbility, 0);
        SetAbility(_rangedAbility, 1);
        SetAbility(_meleeAbility, 2);
        SetAbility(_utilAbility, 3);
        SetAbility(_dash, 4);
    }

    private void Update() {
        HandleInput();
    }
    private void FixedUpdate() {
        HandleCooldown();
    }

    public void SetBasicAbility(MoodAbility ability) {
        _basicAbility = ability;
    }
    public void SetRangedAbility(MoodAbility ability) {
        _rangedAbility = ability;
    }
    public void SetMeleeAbility(MoodAbility ability) {
        _meleeAbility = ability;
    }
    public void SetUtilAbility(MoodAbility ability) {
        _utilAbility = ability;
    }

    private void HandleInput() {
        if(_currentlyCasting) {
            return;
        }
        var abilityToCast = -1;
        for(var i = 0;i < _keys.Length;i++) {
            if(Input.GetKey(_keys[i])) {
                abilityToCast = i;
                break;
            }
        }
        if(abilityToCast == -1) {
            return;
        }
        CastAbility(abilityToCast);
    }

    private void HandleCooldown() {
        foreach(var ability in Abilities) {
            if(ability == null || ability.State != AbilityState.Cooldown) {
                continue;
            }
            if(ability.FirstCall) {
                _currentlyCasting = false;
                var index = GetIndex(ability);
                if(index > 0) {
                    AbilityIcons.Instance.StartCooldown(index - 1, ability.Cooldown);
                }
                ability.FirstCall = false;
            }
            if(ability.Cooldown > 0) {
                ability.Cooldown -= Time.fixedDeltaTime;
            } else {
                ability.State = AbilityState.Ready;
                ability.FirstCall = true;
            }
        }
    }

    private int GetIndex(AbilityBundle ability) {
        return Array.IndexOf(Abilities, ability);
    }

    private void CastAbility(int index) {
        if(Abilities[index] == null || Abilities[index].State != AbilityState.Ready) {
            return;
        }
        if(Abilities[index].AbilityBase.Slot != AbilitySlot.Utils) {
            _currentlyCasting = true;
        }
        AbilityBase ability = Abilities[index].AbilityBase;
        Abilities[index].State = AbilityState.Active;

        ability.Activate();
        if(ability.AnimationName.Trim() != "") {
            _animator.SetBool(ability.AnimationName, true);
            Invoke(nameof(DisableAnimations), 0.5f);
        }
    }
    
    private void DisableAnimations() {
        var parameters = _animator.parameters;
        foreach(var item in parameters) {
            _animator.SetBool(item.name, false);
        }
    }

    private void UpdateIcons() {
        for(var i = 1;i < Abilities.Length;i++) {
            UpdateIcons(i - 1);
        }
    }
    private void UpdateIcons(int i) {
        if(Abilities[i + 1] != null) {
            try {
                AbilityIcons.Instance.ChangeIcon(i, Abilities[i + 1].AbilityBase.Icon);
            } catch(Exception) {
                Debug.Log(i);
            }
        } else {
            AbilityIcons.Instance.ChangeIcon(i);
        }
    }

    public void SetAbility(AbilityBase ability, int i) {
        var bundle = new AbilityBundle(ability);
        Abilities[i] = bundle;
        if(i != 0) {
            UpdateIcons(i - 1);
        }
    }

    public void RemoveAbility(int i) {
        Abilities[i] = null;
        UpdateIcons(i);
    }

    private void InitializeAbilities() {
        foreach(var abilities in Abilities) {
            abilities?.AbilityBase.InitializeAbility(gameObject);
        }
    }
}
public enum AbilityState {
    Ready,
    Active,
    Cooldown,
}
public class AbilityBundle {
    public bool FirstCall { get; set; } = true;
    public AbilityBase AbilityBase { get; set; }
    public float Cooldown { get; set; }
    public AbilityState State { get; set; }

    public AbilityBundle(AbilityBase abilityBase) {
        AbilityBase = abilityBase;
        Cooldown = abilityBase.Cooldown;
        State = AbilityState.Ready;
        abilityBase.OnAbilityStop += () => {
            Cooldown = abilityBase.Cooldown;
            State = AbilityState.Cooldown;
        };
    }
}
