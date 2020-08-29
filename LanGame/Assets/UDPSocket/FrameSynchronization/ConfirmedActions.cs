using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
public class ConfirmedActions {
	public List<NetworkPlayer> playersConfirmedCurrentAction;
	public List<NetworkPlayer> playersConfirmedPriorAction;

	private LockStepManager lsm;

	public ConfirmedActions (LockStepManager lsm) {
		this.lsm = lsm;
		playersConfirmedCurrentAction = new List<NetworkPlayer> (lsm.numberOfPlayers);
		playersConfirmedPriorAction = new List<NetworkPlayer> (lsm.numberOfPlayers);
	}

	public void NextTurn () {
		//clear prior actions
		playersConfirmedPriorAction.Clear ();

		List<NetworkPlayer> swap = playersConfirmedPriorAction;

		//last turns actions is now this turns prior actions
		playersConfirmedPriorAction = playersConfirmedCurrentAction;

		//set this turns confirmation actions to the empty list
		playersConfirmedCurrentAction = swap;
	}

	public bool ReadyForNextTurn () {
		//check that the action that is going to be processed has been confirmed
		//检查将要处理的操作是否已确认
		if (playersConfirmedPriorAction.Count == lsm.numberOfPlayers) {
			return true;
		}
		//if 2nd turn, check that the 1st turns action has been confirmed
		//如果是第2圈，检查第1圈动作已经确认
		if (lsm.LockStepTurnID == LockStepManager.FirstLockStepTurnID + 1) {
			return playersConfirmedCurrentAction.Count == lsm.numberOfPlayers;
		}
		//no action has been sent out prior to the first turn
		//在第一个转弯之前没有发出任何动作
		if (lsm.LockStepTurnID == LockStepManager.FirstLockStepTurnID) {
			return true;
		}
		//if none of the conditions have been met, return false
		return false;
	}
}}