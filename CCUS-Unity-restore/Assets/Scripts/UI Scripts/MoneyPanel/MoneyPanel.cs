using UnityEngine;
using TMPro;

public class MoneyPanel : MonoBehaviour
{
    
    public GameObject totalCashText;
    private TextMeshProUGUI totalCashTextTMPro;
    public GameObject NewMoneyUIFeedbackPrefab;
    public TextMeshProUGUI netMoneyText;

    private int previousMoney;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalCashTextTMPro = totalCashText.GetComponent<TextMeshProUGUI>();
        previousMoney = LevelManager.LM.GetStartingMoney();
        if(totalCashTextTMPro != null){
            totalCashTextTMPro.text = "$" + previousMoney;
        }
        GameEventManager.current.MoneyAmountUpdated.AddListener(MoneyAmountUpdated);
        GameEventManager.current.NetMoneyUpdated.AddListener(NetMoneyUpdated);
    }

    private void NetMoneyUpdated(){
        if(netMoneyText != null){
            netMoneyText.text = "$" + LevelManager.LM.NetMoney + " PER HOUR";
        }
    }

    private void MoneyAmountUpdated(){
        int currentMoney = LevelManager.LM.GetMoney();
        totalCashTextTMPro.text = "$" + currentMoney;

        if(currentMoney > previousMoney){
            OnMoneyIncrease(currentMoney - previousMoney);
            
        }
        previousMoney = currentMoney;
    }

    private void OnMoneyIncrease(int amountMoneyIncreased){
        
        //Shakes the money when it increases
        if(totalCashText != null){
            totalCashText.GetComponent<ShakeGraphic>().ShakeItUp(); 
        }

        //Adds alert when money increases
        GameObject UIFeedbackObject = Instantiate(NewMoneyUIFeedbackPrefab);
        UIFeedbackObject.GetComponentInChildren<TextMeshProUGUI>().text = "$" + amountMoneyIncreased;
        
        
    }
}
