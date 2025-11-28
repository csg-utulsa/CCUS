using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePanelManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI yearText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI peopleText;
    //[SerializeField] TextMeshProUGUI storageText;
    [SerializeField] TextMeshProUGUI carbonText;
    [SerializeField] CarbonRotate carbonDial;
    [SerializeField] ChangeOpacity carbonDialGreenImageGraphic;
    public GameObject NewMoneyUIFeedbackPrefab;
    private string spacing = ""; //The amount of spacing for text
    LevelManager dm;

    private int previousMoney = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        dm = LevelManager.LM;
        previousMoney = dm.GetMoney();
    }

    // Update is called once per frame
    void Update()
    {
        if(yearText != null)
        {
            yearText.text = "Year: " + dm.GetYear().ToString();
        }


        if(dm.GetMoney() > previousMoney){
            OnMoneyIncrease(dm.GetMoney() - previousMoney);
            previousMoney = dm.GetMoney();
            
        } else if(dm.GetMoney() != previousMoney){
            previousMoney = dm.GetMoney();
        }
        

        if(moneyText != null)
        {
            // //Shakes the money when it increases
            // if(dm.GetMoney() > previousMoney){
            //     moneyText.GetComponent<ShakeGraphic>().ShakeItUp();
            // }
            // previousMoney = dm.GetMoney();

            //Updates Cash Text
            for(int i = 0; i < 6 - dm.GetMoney().ToString().Length; i++)
            {
                spacing += "0";
            }
            moneyText.text = "$" + dm.GetMoney().ToString();
        }
        spacing = "";

        /*if(storageText != null)
        {
            storageText.text = "Carbon Storage: " + dm.GetStored();
        }
        */
        if(carbonDial != null)
        {
            carbonDial.UpdateCarbon(dm.GetCarbon()); //Updates the Dial
            float carbonPercentage = (dm.GetCarbon() / dm.getMaxCarbon()); //Transforms the carbon number into a percent
            carbonDialGreenImageGraphic.SetOpacity(1f - carbonPercentage);

            //carbonText.text = (carbonPercentage).ToString() + "%"; //Changes component to match current carbon percent
        }

        if(peopleText != null){
            peopleText.text = "" + TemporaryPeopleManager.TPM.numberOfPeople;
        }
    }

    public void OnMoneyIncrease(int amountMoneyIncreased){
        
        //Shakes the money when it increases
        if(moneyText != null){
           moneyText.GetComponent<ShakeGraphic>().ShakeItUp(); 
        }

        //Adds alert when money increases
        GameObject UIFeedbackObject = Instantiate(NewMoneyUIFeedbackPrefab);
        UIFeedbackObject.GetComponentInChildren<TextMeshProUGUI>().text = "$" + amountMoneyIncreased;
        
        
    }
}
