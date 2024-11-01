using System;
using UnityEngine;

namespace SGGames.Scripts.Dialogue
{
    public enum Speaker
    {
        PLAYER,
        NPC_TEST,
        //NPC NAME INSERT BELOW
    }
    
    [CreateAssetMenu(fileName = "DialogueData", menuName = "SGGames/DialogueData", order = 1)]
    public class DialogueData : ScriptableObject
    {
        public DialogueLine[] DialogueLines;
    }

    [Serializable]
    public class DialogueLine
    {
        public Speaker Speaker;
        public string Dialogues;
        public DialogueChoice[] Choices;
    }

    [Serializable]
    public class DialogueChoice
    {
        public string ChoiceDialogue;
        public DialogueData ChoiceData;
    }
}

