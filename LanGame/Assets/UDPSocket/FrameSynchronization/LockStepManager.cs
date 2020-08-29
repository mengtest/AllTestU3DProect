using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class LockStepManager : MonoBehaviour {
		//第一回合
		public static readonly int FirstLockStepTurnID = 0;
		private PendingActions pendingActions;
		private ConfirmedActions confirmedActions;
		private Queue<IAction> actionsToSend;
		bool initialized = false;
		public int numberOfPlayers;
		//当前回合id
		public int LockStepTurnID = FirstLockStepTurnID;
		//游戏每4帧下一次同步消息	
		private int GameFramesPerLocksetpTurn = 4;
		//每秒游戏帧数
		private int GameFramesPerSecond = 20;
		//当前回合帧的游戏帧数
		private int GameFrame = 0;
		//累计时间
		private float AccumilatedTime = 0f;
		//50 miliseconds
		private float FrameLength = 0.05f;
		//called once per unity frame
		public void Update () {
			AccumilatedTime = AccumilatedTime + Time.deltaTime;
			//每50ms更新游戏帧，一帧中多次更新游戏
			while (AccumilatedTime > FrameLength) {
				GameFrameTurn ();
				AccumilatedTime = AccumilatedTime - FrameLength;
			}
		}
		private void GameFrameTurn () {
			//first frame is used to process actions
			//第一帧的时候处理回合
			if (GameFrame == 0) {
				//同步回合(一个回合可以包含多个动作)
				if (LockStepTurn ()) {
					GameFrame++;
				}
			} else {
				//update game
				//TODO: Add custom physics
				//SceneManager.Manager.TwoDPhysics.Update (GameFramesPerSecond);
				//更新游戏逻辑

				// List<IHasGameFrame> finished = new List<IHasGameFrame>();
				// foreach(IHasGameFrame obj in SceneManager.Manager.GameFrameObjects) {
				// 	obj.GameFrameTurn(GameFramesPerSecond);
				// 	if(obj.Finished) {
				// 		finished.Add (obj);
				// 	}
				// }

				// foreach(IHasGameFrame obj in finished) {
				// 	SceneManager.Manager.GameFrameObjects.Remove (obj);
				// }

				//游戏帧结束的时候重新检测下一个同步操作
				GameFrame++;
				if (GameFrame == GameFramesPerLocksetpTurn) {
					GameFrame = 0;
				}
			}
		}
		public void AddAction (IAction action) {
			// log.Debug ("Action Added");
			if (!initialized) {
				// log.Debug("Game has not started, action will be ignored.");
				return;
			}
			actionsToSend.Enqueue (action);
		}
		private bool LockStepTurn () {
			Debug.Log ("LockStepTurnID: " + LockStepTurnID);
			//Check if we can proceed with the next turn
			//检查是否有下一个回合
			bool nextTurn = NextTurn ();
			if (nextTurn) {
				//向其它玩家发送等待的动作
				SendPendingAction ();
				//the first and second lockstep turn will not be ready to process yet
				//第三帧的时候开始处理当前动作
				if (LockStepTurnID >= FirstLockStepTurnID + 3) {
					ProcessActions ();
				}
			}
			//otherwise wait another turn to recieve all input from all players
			//否则等待另一个回合来接受所有玩家的输入

			return nextTurn;
		}

		/// <summary>
		/// it will return false.
		/// 检查是否满足下一个回合
		/// </summary>
		private bool NextTurn () {
			if (confirmedActions.ReadyForNextTurn () && pendingActions.ReadyForNextTurn ()) {
				//增加回合id
				LockStepTurnID++;
				//move the confirmed actions to next turn
				//将已确认的动作移动到下一个回合
				confirmedActions.NextTurn ();
				//move the pending actions to this turn
				pendingActions.NextTurn ();

				return true;
			}

			return false;
		}
		
		private void SendPendingAction () {
			IAction action = null;
			//动作队列
			if (actionsToSend.Count > 0) {
				action = actionsToSend.Dequeue ();
			}

			//if no action for this turn, send the NoAction action
			//没有任何动作
			if (action == null) {
				action = new NoAction ();
			}
			//add action to our own list of actions to process
			//增加到待操作动作列表中
			pendingActions.AddAction (action, Convert.ToInt32 (Network.player.ToString ()), LockStepTurnID, LockStepTurnID);
			//confirm our own action
			//确认玩家当前自己的动作
			confirmedActions.playersConfirmedCurrentAction.Add (Network.player);
			//send action to all other players
			//向其它所有玩家发消息
			// nv.RPC("RecieveAction", RPCMode.Others, LockStepTurnID, Network.player.ToString(), BinarySerialization.SerializeObjectToByteArray(action));

			// log.Debug("Sent " + (action.GetType().Name) + " action for turn " + LockStepTurnID);
		}

		//执行当前回合
		private void ProcessActions () {
			foreach (IAction action in pendingActions.CurrentActions) {
				action.ProcessAction ();
			}
		}
	}
}