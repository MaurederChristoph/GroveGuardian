using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuRepresenter : MonoBehaviour {
    [field: SerializeField] public AbilityBase AbilityBase { get; private set; }
    private Image _image;
    private void Awake() {
        _image = GetComponent<Image>();
    }
    public void SetUpAbility() {
        _image.sprite = AbilityBase.Icon;
    }

    public void MakeAsPlaceHolder() {
        _image.color = new Color32(150, 150, 150, 200);
    }

    public void SetBaseAbility(AbilityBase abilityBase) {
        AbilityBase = abilityBase;
    }
}
