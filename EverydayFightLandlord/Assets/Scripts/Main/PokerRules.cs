using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Main
{
    /// <summary>
    /// 出牌规则
    /// </summary>
    public static class PokerRules
    {

        public delegate List<Poker> SelectPokerACT(List<Poker> _playerCards, int _maxPokerValue, List<Poker> _lastPokerList);
        //委托,出牌时记录当前牌型函数
        public static SelectPokerACT SelectPokerAct = null;

        /// <summary>是否是单张
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsSingle(List<Poker> cards,ref int _value)
        {
            if (cards.Count == 1)
            {
                _value = cards[0].info.value;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>是否是对子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDouble(List<Poker> cards,ref int _value)
        {
            if (cards.Count == 2)
            {
                if (cards[0].info.value == cards[1].info.value && cards[0].info.value != (int)PokerValueType.小王 && cards[0].info.value != (int)PokerValueType.大王)
                {
                    _value = cards[0].info.value;
                    return true;
                }
            }
            return false;
        }

        /// <summary>是否是顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsStraight(List<Poker> cards,ref int _value)
        {
            if (cards.Count < 5 || cards.Count > 12) return false;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].info.value != cards[i + 1].info.value + 1)
                {
                    return false;
                }
            }
            _value = cards[cards.Count - 1].info.value;
            return true;
        }

        /// <summary>是否是双顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDoubleStraight(List<Poker> cards,ref int _value)
        {
            if (cards.Count < 6 || cards.Count % 2 != 0)
            {
                return false;
            }

            for (int i = 0; i < cards.Count - 2; i += 2)
            {
                //对子不相等 对子之间不是顺子 没有小王 没有大王
                if (cards[i].info.value != cards[i + 1].info.value || cards[i + 1].info.value != cards[i + 2].info.value + 1|| cards[i].info.value == (int)PokerValueType.小王 || cards[i].info.value == (int)PokerValueType.大王) 
                {
                    return false;
                }
            }
            _value = cards[0].info.value;
            return true;
        }

        /// <summary>飞机不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraight(List<Poker> cards, ref int _value)
        {
            if (cards.Count < 6 || cards.Count % 3 != 0)
                return false;

            for (int i = 0; i < cards.Count - 3; i += 3)
            {
                //不是三张相同 与第四张不是顺子,第四张不是2
                if (cards[i].info.value != cards[i + 1].info.value || cards[i].info.value != cards[i + 2].info.value || cards[i].info.value != cards[i + 3].info.value + 1 || cards[i].info.value == (int)PokerValueType.大王 || cards[i].info.value == (int)PokerValueType.小王 || cards[i].info.value == (int)PokerValueType._2)
                    return false;
            }
            _value = cards[0].info.value;
            return true;
        }

        /// <summary>是否是飞机带翅膀
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraightAnd(List<Poker> cards, ref int _value)
        {
            //不满足基本牌数
            if (cards.Count < 8)
                return false;

            int count = 0, index = -1;

            //牌组赋值给临时变量
            List<Poker> temp = new List<Poker>(cards);
            for (int i = 0; i < cards.Count - 2; )
            {
                //判断飞机的三张是否相等
                if (cards[i].info.value == cards[i + 1].info.value && cards[i].info.value == cards[i + 2].info.value)
                {
                    //删掉飞机留下翅膀
                    temp.Remove(cards[i]);
                    temp.Remove(cards[i + 1]);
                    temp.Remove(cards[i + 2]);
                    count++;
                    i += 3;
                    //记录第一对出现的飞机
                    if (index == -1)
                    {
                        index = i;
                    }
                }
                else
                {
                    i++;
                } 
            }

            //飞机不满足个数 翅膀不满足个数
            if (count < 2 || count * 3 + (cards.Count - count * 3) != cards.Count)
            {
                return false;
            }

            for (int i = 0; i < cards.Count - 2; i++)
            {
                //找到飞机
                if (cards[i].info.value == cards[i + 1].info.value && cards[i].info.value == cards[i + 2].info.value)
                {
                    for (int j = 0; j < temp.Count; j++)
                    {
                        //遍历翅膀是否与飞机相同
                        if (cards[i].info.value == temp[j].info.value)
                        {
                            return false;
                        }
                    }
                }
            }

            _value = cards[index].info.value;
            return true;
        }

        /// <summary>是否是三不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsOnlyThree(List<Poker> cards,ref int _value)
        {
            if (cards.Count != 3)
                return false;
            //第一张与第二张不同  第二张与第三张不同
            if (cards[0].info.value != cards[1].info.value || cards[0].info.value != cards[2].info.value)
                return false;
            
            _value = cards[0].info.value;
            return true;
        }

        /// <summary>是否是三带一
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndOne(List<Poker> cards,ref int _value)
        {
            ///只有四张
            if (cards.Count != 4)
                return false;

            ///判断头三张是否相同
            if (cards[0].info.value == cards[1].info.value && cards[1].info.value == cards[2].info.value && cards[0].info.value != (int)PokerValueType.大王 && cards[0].info.value != (int)PokerValueType.小王)
            {
                _value = cards[0].info.value;
                return true;
            }
            ///判断尾三张是否相同
            else if (cards[1].info.value == cards[2].info.value && cards[2].info.value == cards[3].info.value && cards[1].info.value != (int)PokerValueType.大王 && cards[1].info.value != (int)PokerValueType.小王)
            {
                _value = cards[1].info.value;
                return true;
            }

            return false;
        }

        /// <summary>是否是三代二
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndTwo(List<Poker> cards,ref int _value)
        {
            if (cards.Count != 5)
                return false;

            ///判断头三张是否是相同
            if (cards[0].info.value == cards[1].info.value && cards[1].info.value == cards[2].info.value && cards[0].info.value != (int)PokerValueType.小王 && cards[0].info.value != (int)PokerValueType.大王)
            {
                //判断尾两张是否相同
                if (cards[3].info.value == cards[4].info.value)
                {
                    _value = cards[0].info.value;
                    return true;
                }
            }
            ///判断尾三张是否是相同
            else if (cards[2].info.value == cards[3].info.value && cards[3].info.value == cards[4].info.value && cards[0].info.value != (int)PokerValueType.小王 && cards[0].info.value != (int)PokerValueType.大王)
            {
                //判断前两张是否相同
                if (cards[0].info.value == cards[1].info.value)
                {
                    _value = cards[2].info.value;
                    return true;
                }
            }

            return false;
        }

        /// <summary>是否是炸弹
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoom(List<Poker> cards,ref int _value)
        {
            if (cards.Count != 4)
                return false;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                ///判断四张是否相同
                if (cards[i].info.value != cards[i + 1].info.value || cards[i].info.value == (int)PokerValueType.小王 || cards[i].info.value == (int)PokerValueType.大王)
                {
                    return false;
                }
            }
            _value = (cards[0].info.value + 1) * 100;
            return true;
        }

        /// <summary>是否是王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsJokerBoom(List<Poker> cards,ref int _value)
        {
            if (cards.Count != 2)
                return false;
            if (cards[0].info.value >= (int)PokerValueType.小王 && cards[1].info.value >= (int)PokerValueType.小王)
            {
                _value = 99999;
                return true;
            }
            return false;
        }

        ///<summary>判断是否符合出牌规则
        ///</summary>
        ///<param name="cards"></param>
        ///<param name="type"></param>
        ///<returns></returns>
        public static bool IsOutPokerRule(List<Poker> cards, ref int _value)
        {
            bool isRule = false;
            switch (cards.Count)
            {
                case 1:
                    if (IsSingle(cards,ref _value))
                    {
                        SelectPokerAct = SelectSingle;
                        isRule = true;
                    }
                    break;
                case 2:
                    if (IsDouble(cards,ref _value))
                    {
                        SelectPokerAct = SelectDouble;
                        isRule = true;
                    }
                    else if (IsJokerBoom(cards, ref _value))
                    {
                        SelectPokerAct = SelectJokerBoom;
                        isRule = true;
                    }
                    break;
                case 3:
                    if (IsOnlyThree(cards, ref _value))
                    {
                        SelectPokerAct = SelectOnlyThree;
                        isRule = true;
                    }
                    break;
                case 4:
                    if (IsBoom(cards, ref _value))
                    {
                        SelectPokerAct = SelectBoom;
                        isRule = true;
                    }
                    else if (IsThreeAndOne(cards, ref _value))
                    {
                        SelectPokerAct = SelectThreeAndOne;
                        isRule = true;
                    }
                    break;
                case 5:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    else if (IsThreeAndTwo(cards, ref _value))
                    {
                        SelectPokerAct = SelectThreeAndTwo;
                        isRule = true;
                    }
                    break;
                case 6:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraight;
                        isRule = true;
                    }
                    else if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    break;
                case 7:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    break;
                case 8:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    else if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                case 9:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    else if (IsOnlyThree(cards, ref _value))
                    {
                        SelectPokerAct = SelectOnlyThree;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                case 10:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    else if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards,ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;

                case 11:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    break;
                case 12:
                    if (IsStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectStraight;
                        isRule = true;
                    }
                    else if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    else if (IsOnlyThree(cards, ref _value))
                    {
                        SelectPokerAct = SelectOnlyThree;
                        isRule = true;
                    }
                    break;
                case 13:
                    break;
                case 14:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    break;
                case 15:
                    if (IsOnlyThree(cards, ref _value))
                    {
                        SelectPokerAct = SelectOnlyThree;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                case 16:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                case 17:
                    break;
                case 18:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsOnlyThree(cards, ref _value))
                    {
                        SelectPokerAct = SelectOnlyThree;
                        isRule = true;
                    }
                    break;
                case 19:
                    break;

                case 20:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                case 21:
                    if (IsTripleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraight;
                        isRule = true;
                    }
                    break;
                case 22:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    break;
                case 23:
                    break;
                case 24:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                case 25:
                    if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                case 26:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    break;
                case 27:
                    break;
                case 28:
                    if (IsDoubleStraight(cards, ref _value))
                    {
                        SelectPokerAct = SelectDoubleStraight;
                        isRule = true;
                    }
                    else if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
                default:
                    if (IsTripleStraightAnd(cards, ref _value))
                    {
                        SelectPokerAct = SelectTripleStraightAnd;
                        isRule = true;
                    }
                    break;
            }
            return isRule;
        }

        /// <summary>依照等待扑克,从手牌选中一手牌
        /// </summary>
        /// <param name="_playerCards">玩家自己的牌</param>
        /// <param name="_maxPokerValue">上一轮最大牌值</param>
        /// <param name="_lastPokerList">上一轮扑克组</param>
        /// <returns></returns>
        public static bool SelectPoker(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = null;
            //如果上一个值为王炸
            if (_maxPokerValue == 99999)
            {
                return false;
            }
            //如果上一个牌型不为空
            if (SelectPokerAct != null)
            {
                //先根据当前牌型找比它大的牌
                poker = SelectPokerAct(_playerCards, _maxPokerValue, _lastPokerList);
                //未找到合适牌再找炸弹
                if (poker == null ||poker.Count == 0)
                {
                    poker = SelectBoom(_playerCards, _maxPokerValue, _lastPokerList);
                    //没有炸弹再找王炸
                    if (poker == null || poker.Count == 0)
                    {
                        poker = SelectJokerBoom(_playerCards, _maxPokerValue, _lastPokerList);
                    }
                }
            }
            else
            {
                //上一个牌型为空,表示当前为最大
                ///找一手牌随便出
                if (poker == null || poker.Count == 0)
                {
                    //找一手飞机带翅膀
                    poker = SelectTripleStraightAnd(_playerCards, _maxPokerValue, _lastPokerList);
                    if (poker == null || poker.Count == 0)
                    {
                        //找双顺
                        poker = SelectDoubleStraight(_playerCards, _maxPokerValue, _lastPokerList);
                        if (poker == null || poker.Count == 0)
                        {
                            //找顺子
                            poker = SelectStraight(_playerCards, _maxPokerValue, _lastPokerList);
                            if (poker == null || poker.Count == 0)
                            {
                                //找三带二
                                poker = SelectThreeAndTwo(_playerCards, _maxPokerValue, _lastPokerList);
                                if (poker == null || poker.Count == 0)
                                {
                                    //找三带一
                                    poker = SelectThreeAndOne(_playerCards, _maxPokerValue, _lastPokerList);
                                    if (poker == null || poker.Count == 0)
                                    {
                                        //找三不带
                                        poker = SelectOnlyThree(_playerCards, _maxPokerValue, _lastPokerList);
                                        if (poker == null || poker.Count == 0)
                                        {
                                            //找一对
                                            poker = SelectDouble(_playerCards, _maxPokerValue, _lastPokerList);
                                            if (poker == null || poker.Count == 0)
                                            {
                                                //找单张
                                                poker = SelectSingle(_playerCards, _maxPokerValue, _lastPokerList);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //找不到任何牌型时
            if (poker == null || poker.Count == 0)
            {
                return false;
            }
            //选中找到的手牌
            for (int i = 0; i < poker.Count; i++)
            {
                PokerManage.waitPoker.Add(poker[i]);
            }
            //排序牌型
            PokerManage.PokerSort(PokerManage.waitPoker);

            return true;
        }

        /// <summary>找一手双顺
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectDoubleStraight(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 1; i--)
            {
                poker.Clear();
                //找一对比牌值大的底牌
                if (_playerCards[i].info.value == _playerCards[i - 1].info.value && _playerCards[i].info.value > _maxPokerValue)
                {
                    poker.Add(_playerCards[i]);
                    poker.Add(_playerCards[i - 1]);
                    int count = 2;
                    for (int j = i; j >= 1; j--)
                    {
                        //上一轮如果没有最大(自己最大)   上一轮最后是 A 结尾 
                        if (_lastPokerList.Count != 0 && _lastPokerList[0].info.value == (int)PokerValueType.A)
                        {
                            return null;
                        }

                        //当前找到的这个值不符合顺子,表示向后找到的牌已不符合顺子,跳过这个值,找下一个比牌值大的底牌
                        //在每一次找底牌时清空列表
                        if (_playerCards[j].info.value - _playerCards[i].info.value > count / 2)
                        {
                            break;
                        }

                        //符合顺子牌型时添加进列表
                        if (_playerCards[j].info.value - _playerCards[i].info.value == count / 2 && _playerCards[j - 1].info.value - _playerCards[i].info.value == count / 2)
                        {
                            poker.Add(_playerCards[j]);
                            poker.Add(_playerCards[j - 1]);
                            count++;
                            count++;
                        }

                        if (_lastPokerList.Count != 0)
                        {
                            //当与最大牌组数量一致时跳出
                            if (count == _lastPokerList.Count)
                            {
                                return poker;
                            }
                        }
                    }
                    //循环结束的时候找到的双顺对子符合基本数量时并且当前牌值自己最大的时候,返回牌组
                    if (count / 2 > 3 && _maxPokerValue == -1)
                    {
                        return poker;
                    }
                }
            }
            return null;
        }

        /// <summary>找一手飞机带翅膀
        /// </summary>
        /// <param name="_playerCards"></param>
        /// <param name="_maxPokerValue"></param>
        /// <param name="_lastPokerList"></param>
        /// <returns></returns>
        public static List<Poker> SelectTripleStraightAnd(List<Poker> _playerCards, int _maxPokerValue, List<Poker> _lastPokerList)
        {
            //先在牌型中找三不带
            List<Poker> poker = SelectTripleStraight(_playerCards, _maxPokerValue, _lastPokerList);
            if (poker==null || poker.Count==0)
            {
                return null;
            }
            //获得飞机个数
            int count = poker.Count / 3;
            //翅膀是单张还是对子
            bool isDouble = (_lastPokerList.Count - poker.Count) == count * 2;
            //已经找到符合翅膀的个数
            int isCount = 0;
            //当前自己是最大
            bool isMax = _maxPokerValue == -1;

            //当前自己是最大 或者 翅膀是对子时
            if (isMax || isDouble)
            {
                for (int i = _playerCards.Count - 1; i >= 1; i--)
                {
                    //手牌找到符合的对子
                    if (_playerCards[i].info.value == _playerCards[i - 1].info.value && _playerCards[i].info.value != (int)PokerValueType.小王 && _playerCards[i].info.value != (int)PokerValueType.大王)
                    {
                        //判断是否与翅膀有重复
                        bool isRepeat = false;
                        for (int j = 0; j < poker.Count;j++)
                        {
                            if (_playerCards[i].info.value == poker[j].info.value)
                            {
                                isRepeat = true;
                                break;
                            }
                        }
                        //无重复时添加进牌组
                        if (!isRepeat)
                        {
                            poker.Add(_playerCards[i]);
                            poker.Add(_playerCards[i - 1]);
                            isCount +=2 ;
                        }
                        //当飞机个数与找到的翅膀个数相等时
                        if (count * 2 == isCount)
                        {
                            return poker;
                        }
                    }
                }
            }
            poker = SelectTripleStraight(_playerCards, _maxPokerValue, _lastPokerList);
            isCount = 0;
            //当前自己是最大 或者 翅膀不是对子,是个子的时候
            if (isMax || !isDouble)
            {
                //遍历牌组
                for (int i = _playerCards.Count - 1; i >= 1; i--)
                {
                    bool isRepeat = false;
                    for (int j = 0; j < poker.Count; j++)
                    {
                        //当牌与某一个飞机值相等时重复
                        if (_playerCards[i].info.value == poker[j].info.value)
                        {
                            isRepeat = true;
                        }
                    }
                    //没有重复时添加进牌组
                    if (!isRepeat)
                    {
                        poker.Add(_playerCards[i]);
                        isCount++;
                    }

                    //当飞机个数与找到的翅膀个数相等时
                    if (count == isCount)
                    {
                        return poker;
                    }
                }
            }

            return null;
        }

        /// <summary>找一手飞机不带
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectTripleStraight(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            int lastCount = 0;
            //如果是上一把牌值最大是飞机带翅膀,得到飞机数
            if (SelectPokerAct == SelectTripleStraightAnd)
            {
                for (int i = 0; i < _lastPokerList.Count - 2; i++)
                {
                    if (_lastPokerList[i].info.value == _lastPokerList[i + 1].info.value && _lastPokerList[i + 1].info.value == _lastPokerList[i + 2].info.value)
                    {
                        lastCount += 3;
                    }
                }
            }
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 3; i--)
            {
                poker.Clear();
                if (_playerCards[i].info.value > _maxPokerValue && _playerCards[i - 1].info.value > _maxPokerValue && _playerCards[i - 2].info.value > _maxPokerValue && _playerCards[i].info.value != _playerCards[i - 3].info.value)
                {
                    poker.Add(_playerCards[i]);
                    poker.Add(_playerCards[i - 1]);
                    poker.Add(_playerCards[i - 2]);
                    int count = 3;
                    for (int j = i; j >= 3; j--)
                    {
                        if (_playerCards[j].info.value - _playerCards[i].info.value > count / 3)
                        {
                            break;
                        }
                        if (_playerCards[j].info.value - _playerCards[i].info.value == count / 3 && _playerCards[j - 1].info.value - _playerCards[i].info.value == count / 3 && _playerCards[j - 2].info.value - _playerCards[i].info.value == count / 3)
                        {
                            bool isRepeat = false;
                            for (int k = 0; k < poker.Count; k++)
                            {
                                if (_playerCards[j].info.value==poker[k].info.value)
                                {
                                    isRepeat = true;
                                }
                            }
                            if (!isRepeat)
                            {
                                poker.Add(_playerCards[j]);
                                poker.Add(_playerCards[j - 1]);
                                poker.Add(_playerCards[j - 2]);
                                count++;
                                count++;
                                count++;
                            }

                        }
                        if (_lastPokerList.Count != 0)
                        {
                            if (SelectPokerAct == SelectTripleStraightAnd)
                            {
                                //满足飞机数跳出
                                if (count == lastCount)
                                {
                                    return poker;
                                }
                            }
                            //满足三不带数量跳出
                            if (count == _lastPokerList.Count)
                            {
                                return poker;
                            }
                        }
                    }
                    if (count / 3 > 2 && _maxPokerValue == -1)
                    {
                        return poker;
                    }
                }
            }
            return null;
        }
        
        /// <summary>找一手三带二
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectThreeAndTwo(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 2; i--)
            {
                if (_playerCards[i].info.value == _playerCards[i - 1].info.value && _playerCards[i - 1].info.value == _playerCards[i - 2].info.value && _playerCards[i].info.value != (int)PokerValueType.小王 && _playerCards[i].info.value != (int)PokerValueType.大王)
                {
                    if (_playerCards[i].info.value > _maxPokerValue)
                    {
                        poker.Add(_playerCards[i]);
                        poker.Add(_playerCards[i - 1]);
                        poker.Add(_playerCards[i - 2]);
                        for (int j = _playerCards.Count - 1; j >= 1; j--)
                        {
                            if (_playerCards[j].info.value == _playerCards[j - 1].info.value && _playerCards[i].info.value != _playerCards[j].info.value && _playerCards[j].info.value != (int)PokerValueType.小王 && _playerCards[j].info.value != (int)PokerValueType.大王)
                            {
                                poker.Add(_playerCards[j]);
                                poker.Add(_playerCards[j - 1]);
                                return poker;
                            }
                        }
                    }
                }
            }
            return null;
        }
        
        /// <summary>找一手顺子
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectStraight(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 0; i--)
            {
                poker.Clear();
                //判断首张扑克大小
                if (_playerCards[i].info.value > _maxPokerValue)
                {
                    poker.Add(_playerCards[i]);
                    int count = 1;
                    //向后找
                    for (int j = i; j >= 0; j--)
                    {
                        //上一轮如果没有最大(自己最大)   上一轮最后是 A 结尾 
                        if (_lastPokerList.Count != 0 && _lastPokerList[0].info.value == (int)PokerValueType.A)
                        {
                            return null;
                        }
                        //超出大小为一
                        if (_playerCards[j].info.value - _playerCards[i].info.value > count)
                        {
                            break;
                        }
                        //判断是否符合  一张牌时 头减尾等于一  两张牌时 头减尾等于二
                        if (_playerCards[j].info.value - _playerCards[i].info.value  == count)
                        {
                            poker.Add(_playerCards[j]);
                            count++;
                        }
                        //上一轮有牌时
                        if (_lastPokerList.Count != 0)
                        {
                            //数目一致返回
                            if (count == _lastPokerList.Count)
                            {
                                return poker;
                            }
                        }
                    }
                    //满足基本数目 并且 上一轮没有牌时
                    if (count > 5 && _maxPokerValue == -1)
                    {
                        return poker;
                    }
                }
            }
            return null;
        }
        
        /// <summary>找一手三带一
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectThreeAndOne(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 3; i--)
            {
                if (_playerCards[i].info.value == _playerCards[i - 1].info.value && _playerCards[i - 1].info.value == _playerCards[i - 2].info.value && _playerCards[i].info.value != (int)PokerValueType.小王 && _playerCards[i].info.value != (int)PokerValueType.大王)
                {
                    if (_playerCards[i].info.value > _maxPokerValue)
                    {
                        poker.Add(_playerCards[i]);
                        poker.Add(_playerCards[i - 1]);
                        poker.Add(_playerCards[i - 2]);
                        for (int j = _playerCards.Count - 1; j >= 1; j--)
                        {
                            if (_playerCards[j].info.value != _playerCards[i].info.value)
                            {
                                poker.Add(_playerCards[j]);
                                return poker;
                            }
                        }
                    }
                }
            }
            return null;
        }
        
        /// <summary>找一手炸弹
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectBoom(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 3; i--)
            {
                if (_playerCards[i].info.value == _playerCards[i - 1].info.value && _playerCards[i - 1].info.value == _playerCards[i - 2].info.value && _playerCards[i - 2].info.value == _playerCards[i - 3].info.value && _playerCards[i].info.value != (int)PokerValueType.小王 && _playerCards[i].info.value != (int)PokerValueType.大王)
                {
                    if ((_playerCards[i].info.value + 1) * 100> _maxPokerValue)
                    {
                        poker.Add(_playerCards[i]);
                        poker.Add(_playerCards[i - 1]);
                        poker.Add(_playerCards[i - 2]);
                        poker.Add(_playerCards[i - 3]);
                        return poker;
                    }
                }
            }
            return null;
        }

        /// <summary>找一手王炸
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectJokerBoom(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 1; i--)
            {
                if (_playerCards[i].info.value >= (int)PokerValueType.小王 && _playerCards[i - 1].info.value >= (int)PokerValueType.小王)
                {
                    poker.Add(_playerCards[i]);
                    poker.Add(_playerCards[i - 1]);
                    return poker;
                }
            }
            return null;
        }

        /// <summary>找一手三张
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectOnlyThree(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 2; i--)
            {
                if (_playerCards[i].info.value == _playerCards[i - 1].info.value && _playerCards[i - 1].info.value == _playerCards[i - 2].info.value && _playerCards[i].info.value != (int)PokerValueType.小王 && _playerCards[i].info.value != (int)PokerValueType.大王)
                {
                    if (_playerCards[i].info.value > _maxPokerValue)
                    {
                        poker.Add(_playerCards[i]);
                        poker.Add(_playerCards[i - 1]);
                        poker.Add(_playerCards[i - 2]);
                        return poker;
                    }
                }
            }
            return null;
        }
        
        /// <summary>找一手对子
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectDouble(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 1; i--)
            {
                if (_playerCards[i].info.value == _playerCards[i - 1].info.value && _playerCards[i].info.value != (int)PokerValueType.小王 && _playerCards[i].info.value != (int)PokerValueType.大王)
                {
                    if (_playerCards[i].info.value > _maxPokerValue)
                    {
                        poker.Add(_playerCards[i]);
                        poker.Add(_playerCards[i - 1]);
                        return poker;
                    }
                }
            }
            return null;
        }

        /// <summary>找一手单张
        /// </summary>
        /// <returns></returns>
        public static List<Poker> SelectSingle(List<Poker> _playerCards,int _maxPokerValue,List<Poker> _lastPokerList)
        {
            List<Poker> poker = new List<Poker>();
            for (int i = _playerCards.Count - 1; i >= 0; i--)
            {
                if (_playerCards[i].info.value > _maxPokerValue)
                {
                    poker.Add(_playerCards[i]);
                    return poker;
                }
            }
            return null;
        }
    }
 }