﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocHold : ClimbingHold {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentState == States.Grabbed)
        {
            ClimbARHandhold.setHoldColor(gameObject, ClimbARHandhold.ENTERED_COLOR);
        }
        else if (currentState == States.Released)
        {
            ClimbARHandhold.setHoldColor(gameObject, ClimbARHandhold.RESET_COLOR);
        }
    }

}
