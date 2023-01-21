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

    public static class LayerNumber
    {

        public const int player = 6;
        public const int character = 7;
        public const int interactableObject = 8;
        public const int building = 9;
        public const int map = 10;
        public const int floor = 11;

    }

    //출력할 대사 오브젝트 
    public static Dictionary<int, Dictionary<int, string[]>> BasicDialogue;
    public static Dictionary<int, Dictionary<int, string[]>> MainDialogue;
}
