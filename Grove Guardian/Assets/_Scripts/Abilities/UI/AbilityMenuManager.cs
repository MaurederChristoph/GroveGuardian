using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMenuManager : Singleton<AbilityMenuManager> {
    public Action RemoveRaycast;
    public Action AddRaycast;

    public AbilityMenuSlots[] slots;

    private readonly List<(GameObject, int)> _abilities = new();

    public void AddActiveAbility(GameObject ability, int i) {
        _abilities.Add((ability, i));
    }

    private void Start() {
        Invoke(nameof(LoadAbilities), 0.02f);
    }


    public void LoadAbilities() {
        for(var i = 0;i < _abilities.Count;i++) {
            _abilities[i].Item1.GetComponent<Dragger>().HandleDragBegin();
            _abilities[i].Item1.GetComponent<Dragger>().ChangeParent(slots[_abilities[i].Item2].transform);
            _abilities[i].Item1.transform.position = slots[_abilities[i].Item2].transform.position;
            _abilities[i].Item1.GetComponent<Dragger>().HandleDragEnd();
            slots[_abilities[i].Item2].HandleDrop(_abilities[i].Item1);
        }
    }

    public void SetAbilities() {
        var abilities = GameManager.Instance.PlayerAbilities;

        SetAbility(0, abilities.SetBasicAbility);
        SetAbility(1, abilities.SetRangedAbility);
        SetAbility(2, abilities.SetMeleeAbility);
        SetAbility(3, abilities.SetUtilAbility);

        void SetAbility(int slotIndex, Action<MoodAbility> setAbilityAction) {
            var ability = slots[slotIndex]._currentAbility;
            if(ability != null) {
                var abilityComponent = ability.GetComponent<AbilityMenuRepresenter>();
                if(abilityComponent != null && abilityComponent.AbilityBase != null) {
                    setAbilityAction.Invoke((MoodAbility)abilityComponent.AbilityBase);
                }
            }
        }
        abilities.Start();
    }
}
