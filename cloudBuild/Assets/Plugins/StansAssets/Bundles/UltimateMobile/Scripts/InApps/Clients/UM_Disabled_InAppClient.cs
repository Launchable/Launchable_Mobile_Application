using UnityEngine;
using System;
using System.Collections;

public class UM_Disabled_InAppClient : UM_BaseInAppClient, UM_InAppClient {
	

	//--------------------------------------
	// Initialization
	//--------------------------------------

	public void Connect() {}

	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public override void Purchase(UM_InAppProduct product) {}

	public override void Subscribe(UM_InAppProduct product) {}

	public override void Consume(UM_InAppProduct product)  {}

	public override void FinishTransaction(UM_InAppProduct product) {}

	public void RestorePurchases() {}

}
