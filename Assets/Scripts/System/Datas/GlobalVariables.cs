using System.Collections.Generic;

public static class GlobalVariables
{
    //프로젝트의 글로벌 변수들

    public const int WorldSpaceUISortingOrder = 1;
    public const int CharacterStarSortingOrder = 10;

    public static class LayerName
    {
        public static readonly string Default = "Default";
        public static readonly string UI = "UI";
    }

    //출력할 대사 오브젝트 
    public static Dictionary<int, Dictionary<int, string[]>> BasicDialogue;
    public static Dictionary<int, Dictionary<int, string[]>> MainDialogue;
}
