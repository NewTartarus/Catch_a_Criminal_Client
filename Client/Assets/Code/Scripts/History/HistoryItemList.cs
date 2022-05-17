namespace ScotlandYard.Scripts.History
{
    using UnityEngine;

    public class HistoryItemList : MonoBehaviour
    {
        [SerializeField] protected Transform content;
        [SerializeField] protected GameObject historyItemPrefab;

        public void AddVisibleItem(HistoryItem historyItem)
        {
            if(historyItem.Data.PlayerRole == Enums.EPlayerRole.MISTERX)
            {
                GameObject go = Instantiate(historyItemPrefab);
                go.GetComponent<HistoryItemView>()?.Init(historyItem);
                go.transform.SetParent(content, false);
            }
        }
    }
}
