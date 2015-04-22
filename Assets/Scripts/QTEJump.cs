using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class QTEJump : QTE
{
	private Button buttonJump;
	private float timeLeft;
    private Image circleFillTimeLeft;
	
	void Awake()
	{
		TimeLimit = 1.5f;
		buttonJump = PanelUI.transform.FindChild("ButtonJump").GetComponent<Button>();
        circleFillTimeLeft = PanelUI.transform.FindChild("FillCircle").GetComponent<Image>();

        buttonJump.onClick.AddListener(OnClickButtonJump);

	}

    public override void OnEnable()
    {
        base.OnEnable();

        buttonJump.enabled = true;
        buttonJump.targetGraphic.enabled = true;

        circleFillTimeLeft.fillAmount = 1f;
    }
	
	void Update()
	{
		if(NoChoice && GameManager.Instance.state == GameState.Run)
			responseTime += Time.deltaTime;
		
		//timeLeft = TimeLimit - responseTime;


        float value = (TimeLimit - responseTime) * (1f / TimeLimit);
        circleFillTimeLeft.fillAmount = value;
	}
	
	void OnClickButtonJump()
	{
		responseTime = Time.time - startTime;
        player.SetReadyToJump(true);
		NoChoice = false;
	}
}