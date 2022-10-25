namespace OkayDinos.Nimble.ProgressSystem
{

    public enum QuestType { Interaction, Item }
    public enum ProgressState { NOT_COMPLETE, CURRENT, COMPLETE }

    [System.Serializable]
    public class QuestData
    {
        public int mi_id;                    //ID number for the quest
        public QuestType QuestType;
        public ItemData ItemData;
        public int ItemAmount;
        public ProgressState mc_Progress;    //state of the current quest (enum)
        public string ms_questObjective;     //quest objective
        public int mi_nextQuest;             //the next quest if there is any
    }
}