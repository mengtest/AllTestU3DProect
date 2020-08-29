public enum PokerHouse {
	/// <summary>
	/// none
	/// </summary>
	none = -1,
	/// <summary>
	/// 黑桃
	/// </summary>
	black = 0,
	/// <summary>
	/// 红桃
	/// </summary>
	peach = 1,
	/// <summary>
	/// 梅花
	/// </summary>
	plum = 2,
	/// <summary>
	/// 方片
	/// </summary>
	piece = 3,
}
public enum PokerValueType {
	_0 = 55,
	_3 = 1,
	_4 = 2,
	_5 = 3,
	_6 = 4,
	_7 = 5,
	_8 = 6,
	_9 = 7,
	_10 = 8,
	_J = 9,
	_Q = 10,
	_K = 11,
	_A = 12,
	_2 = 13,
	_minJoker = 53,
	_maxJoker = 54,
}
public enum RoleType {
	none,
	/// <summary>
	/// 农民
	/// </summary>
	peasant,
	/// <summary>
	/// 地主
	/// </summary>
	landlord,
}
public enum PokerPosType {
	Center = 1,
	Left = 2,
	Right = 3,
}
public enum GameState {
	/// <summary>
	/// 空状态
	/// </summary>
	None,
	/// <summary>
	/// 游戏开始
	/// </summary>
	GameStart,
	/// <summary>
	/// 抢地主
	/// </summary>
	GrabLandLording,
	/// <summary>
	/// 出牌
	/// </summary>
	OutPoker,
	/// <summary>
	/// 游戏结束
	/// </summary>
	GameOver
}
public enum AnimeType {
	None,
	Scale,
	OffSet,
	Both,
}