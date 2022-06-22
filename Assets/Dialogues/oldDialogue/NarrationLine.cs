using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Line")]
public class NarrationLine : ScriptableObject
{
    [SerializeField]
    NarrationCharacter speaker;
    [SerializeField]
    string text;

    public NarrationCharacter Speaker => speaker;
    public string Text => text;
    
}
