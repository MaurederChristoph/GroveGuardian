using UnityEngine;
using UnityEngine.UI;

public class AbilityIcons : Singleton<AbilityIcons> {
    [field: SerializeField] public Image[] Icons { get; private set; } = new Image[3];
    [field: SerializeField] public CooldownHandler[] Cooldowns{ get; private set; } = new CooldownHandler[3];
    [SerializeField] private Sprite _noAbilitySprite;
    [SerializeField] private Sprite _ultSprite;

    public void ChangeIcon(int index, Sprite icon) {
        Icons[index].sprite = icon;
    }

    public void ChangeIcon(int index) {
        Sprite sprite = _noAbilitySprite;
        if(index == 3) {
            sprite = _ultSprite;
        }
        Icons[index].sprite = sprite;
    }

    public void StartCooldown(int i, float cooldown) {
        Cooldowns[i].StartCooldown(cooldown);
    }
}
