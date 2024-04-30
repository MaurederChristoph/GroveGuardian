using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulateAbilitySelection : MonoBehaviour {
    const string FOLDER_PATH = "Abilities";

    [SerializeField] private GameObject _abilityContainerPrefab;
    [SerializeField] private GameObject _abilityMenuDisplayPrefab;
    [SerializeField] private Transform _containerContainer;

    private List<MoodAbility> _abilities = new();
    private void Start() {
        _abilities = new(LoadScriptableObjects());
        PopulateView();
    }

    private void PopulateView() {
        List<Mood> currentMoods = new(PersistentVariables.Instance.UnlockedMoods);
        foreach(var mood in currentMoods) {
            var container = Instantiate(_abilityContainerPrefab, _containerContainer);
            List<MoodAbility> abilities = GetAbilitiesByMood(mood);
            foreach(var ability in abilities) {
                var index = GetIndex(ability);
                if(index == -1) {
                    continue;
                }
                var abilityGo = Instantiate(_abilityMenuDisplayPrefab);
                SetSibling(abilityGo, container, index);
                var AMR = abilityGo.GetComponent<AbilityMenuRepresenter>();
                AMR.SetBaseAbility(ability);
                AMR.SetUpAbility();
                var i = CheckAbility(AMR);
                if(i != -1) {
                    AbilityMenuManager.Instance.AddActiveAbility(abilityGo, i);
                }
            }
        }
    }

    private int CheckAbility(AbilityMenuRepresenter aMR) {
        var abilitiesList = GameManager.Instance.PlayerAbilities.Abilities;
        var ability = abilitiesList.FirstOrDefault(a => a.AbilityBase == aMR.AbilityBase);
        return ability != null ? Array.IndexOf(abilitiesList, ability) : -1;
    }

    private int GetIndex(MoodAbility ability) {
        var slotToIndex = new Dictionary<AbilitySlot, int>{
            { AbilitySlot.Ranged, 0 },
            { AbilitySlot.Melee, 1 },
            { AbilitySlot.Utils, 2 },
            { AbilitySlot.Basic, 3 }
        };
        return slotToIndex.TryGetValue(ability.Slot, out var index) ? index : -1;
    }

    private void SetSibling(GameObject child, GameObject parent, int AbilityIndex) {
        var childIndex = 1;
        if(AbilityIndex == (int)AbilitySlot.Basic) {
            childIndex = 0;
        }
        child.transform.SetParent(parent.transform.GetChild(childIndex));
    }

    private List<MoodAbility> GetAbilitiesByMood(Mood mood) {
        return _abilities.Where(a => a.Mood == mood).ToList();
    }

    public List<MoodAbility> LoadScriptableObjects() {
        var scriptableObjects = new List<MoodAbility>();
        var loadedObjects = Resources.LoadAll(FOLDER_PATH, typeof(MoodAbility));
        foreach(var obj in loadedObjects) {
            if(obj is MoodAbility) {
                scriptableObjects.Add(obj as MoodAbility);
            }
        }
        return scriptableObjects;
    }
}
