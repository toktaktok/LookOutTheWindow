using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Character")]
public class NarrationCharacter : ScriptableObject
{
    [SerializeField]
    private string characterName;

    public string CharacterName => characterName;
}
