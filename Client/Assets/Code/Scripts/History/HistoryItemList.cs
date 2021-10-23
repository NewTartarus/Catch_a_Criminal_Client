namespace ScotlandYard.Scripts.History
{
    using UnityEngine;

    public class HistoryItemList : MonoBehaviour
    {
        [SerializeField] protected Transform content;
        [SerializeField] protected GameObject historyItemPrefab;

        public void AddVisibleItem(HistoryItem historyItem, bool displayPosition)
        {
            if(historyItem.Data.PlayerType == Enums.EPlayerType.MISTERX)
            {
                GameObject go = Instantiate(historyItemPrefab);
                go.GetComponent<HistoryItemView>()?.Init(historyItem, displayPosition);
                go.transform.SetParent(content, false);
            }
        }
    }
}
