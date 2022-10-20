using Game.model;
using GameBackGammon.Service;

namespace Game.Service
{
    public class GameService
    {
        public string colorPlayer = "black";
        public LinkedList<bool>? whiteListTemporary  = new LinkedList<bool>();
        public LinkedList<bool>? blackListTemporary = new LinkedList<bool>();
        public int[] cubesTemporary = new int[4];
        public Board boardTemporary = new();
        public LinkedList<bool>? whiteList = new LinkedList<bool>();
        public LinkedList<bool>? blackList = new LinkedList<bool>(); 
        public int[] cubes = new int[4];
        public Board board = new();
        public bool[] showOnMoveTemp = new bool[25];
        private bool canPlay = true;
        private int deleteCube = -1;
        public string displayNameFirst = "";
        public string displayNameSecond = "";
        public string displayNameTurn = "";
        private Turn turn = new Turn();
        public GameService()
        {
            //WhiteListUp();
            //BlackListUp();
            //WhiteListUp();
            //BlackListUp();
            //WhiteListUp();
            //BlackListUp();
            //blackList.AddLast(true);
            //blackList.AddLast(true);
            //blackList.AddLast(true);
            //whiteList.AddLast(true);
            //whiteList.AddLast(true);
            //whiteList.AddLast(true);
        }
        public bool GetTurn(string name)
        {
            if(name == displayNameTurn)
            {
                return true;
            }
            return false;
        }
        private void SaveDetails()
        {
            for (int i = 0; i < cubesTemporary.Length; i++)
            {
                cubesTemporary[i] = 0;
            }
            if (colorPlayer == "black")
                colorPlayer = "white";
            else
                colorPlayer = "black";
            ChangeListEndTurn();
            SaveBoard();
        }
        private void SaveBoard()
        {
            for (int i = 0; i < board.board.Length; i++)
            {
                board.board[i].Amount = boardTemporary.board[i].Amount;
                board.board[i].Color = boardTemporary.board[i].Color;
            }
        }
        public bool FinishTurn()
        {
            for(int i = 0;i < cubesTemporary.Length; i++)
            {
                if (cubesTemporary[i] != 0) return false;
            }
            if (displayNameTurn == displayNameFirst)
            {
                displayNameTurn = displayNameSecond;
            }
            else displayNameTurn = displayNameFirst;
            SaveDetails();
            return true;
        }
        public int[] RoleCube()
        {
            Random rnd = new Random();
            cubesTemporary[0] = rnd.Next(1,7);
            cubesTemporary[1] = rnd.Next(1,7);
            cubes[0] = cubesTemporary[0];
            cubes[1] = cubesTemporary[1];
            if (cubesTemporary[0] == cubesTemporary[1])
            {
                cubesTemporary[2] = cubesTemporary[0];
                cubesTemporary[3] = cubesTemporary[0];
                cubes[2] = cubesTemporary[0];
                cubes[3] = cubesTemporary[0];
            }
            else
            {
                cubes[2] = 0;
                cubes[3] = 0;
            }
            return cubesTemporary;
        }
        public bool CanMove()
        {
            OnMove(50);
            if(whiteListTemporary.Count > 0 && colorPlayer == "white")
            {
                if (canPlay)
                    return true;
                SaveDetails();
                return false;
            }
            if (blackListTemporary.Count > 0 && colorPlayer == "black")
            {
                if (canPlay)
                    return true;
                SaveDetails();
                return false;
            }
            for (int i = 0;i<boardTemporary.board.Length; i++)
            {
                OnMove(i);
                if (canPlay)
                    return true;
            }
            SaveDetails();
            return false;
        }
        private bool EndGameBlack()
        {
            if (colorPlayer == "black")
            {
                if (blackListTemporary.Count > 0)
                {
                    return false;
                }
                for (int i = 0; i < boardTemporary.board.Length; i++)
                {
                    if (GetColorFromBoard(i, "black"))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private bool EndGameWhite()
        {
            if (colorPlayer == "white")
            {
                if (whiteListTemporary.Count > 0)
                {
                    return false;
                }
                for (int i = 0; i < boardTemporary.board.Length; i++)
                    if (GetColorFromBoard(i, "white")) return false;
            }
            return true;
        }
        public bool EndGame()
        {
            if(!EndGameBlack()) return false;
            if (!EndGameWhite()) return false;
            return true;
        }
        public void Reset()
        {
            ChangeListReset();
            for (int i = 0; i < cubesTemporary.Length; i++)
            {
                cubesTemporary[i] = cubes[i];
            }
            ResetCubes();
        }
        public void OnMove(int number)
        {
            Clear();
            for(int i = 0; i < cubesTemporary.Length; i++)
            {
                if(cubesTemporary[i] != 0)
                {
                    if (CheckCanThrow(number, i))
                    {
                        if (colorPlayer == "black")
                        {
                            showOnMoveTemp[24] = true;
                            canPlay = true;
                            for (int j = 0;j < 18; j++)
                            {
                                if(GetColorFromBoard(j, "black"))
                                {
                                    showOnMoveTemp[24] = false;
                                    canPlay = false;
                                }
                            }
                        }
                        if (colorPlayer == "white")
                        {
                            showOnMoveTemp[24] = true;
                            canPlay = true;
                            for (int j = 0; j < 18; j++)
                            {
                                if (GetColorFromBoard(6 + j, "white"))
                                {
                                    showOnMoveTemp[24] = false;
                                    canPlay = false;
                                }
                            }
                        }
                    }
                    if (colorPlayer == "black") CheckOnMoveBlack(number, i);
                    if (colorPlayer == "white") CheckOnMoveWhite(number, i);
                }
            }
        }
        public void Clear()
        {
            canPlay = false;
            for (int i = 0;  i < showOnMoveTemp.Length; i++)
                showOnMoveTemp[i] = false;
        }
        private void ChangeListResetWhite()
        {
            if (whiteListTemporary.Count > whiteList.Count)
            {
                int num = whiteListTemporary.Count - whiteList.Count;

                for (int i = 0; i < num; i++)
                {
                    whiteListTemporary.RemoveLast();
                }
            }
            else
            {
                int num = whiteList.Count - whiteListTemporary.Count; 

                for (int i = 0; i < num; i++)
                {
                    whiteListTemporary.AddLast(true);
                }
            }

        }
        private void ChangeListResetBlack()
        {
             
            if (blackListTemporary.Count > blackList.Count)
            {
                int num = blackListTemporary.Count - blackList.Count;
                for (int i = 0; i < num; i++)
                {
                    blackListTemporary.RemoveLast();
                }
            }
            else
            {
                int num = blackList.Count - blackListTemporary.Count;
                for (int i = 0; i < num; i++)
                {
                    blackListTemporary.AddLast(true);
                }
            }
        }
        private void ChangeListReset()
        {
            ChangeListResetBlack();
            ChangeListResetWhite();
        }
        private void ChangeListEndTurnWhite()
        {
            if (whiteListTemporary.Count > whiteList.Count)
            {
                for (int i = 0; i < whiteListTemporary.Count - whiteList.Count; i++)
                {
                    whiteList.AddLast(true);
                }
            }
            else
            {
                for (int i = 0; i < whiteList.Count - whiteListTemporary.Count; i++)
                {
                    whiteList.RemoveLast();
                }
            }

        }
        private void ChangeListEndTurnBlack()
        {
            if (blackListTemporary.Count > blackList.Count)
            {
                for (int i = 0; i < blackListTemporary.Count - blackList.Count; i++)
                {
                    blackList.AddLast(true);
                }
            }
            else
            {
                for (int i = 0; i < blackList.Count - blackListTemporary.Count; i++)
                {
                    blackList.RemoveLast();
                }
            }
        }
        private void ChangeListEndTurn()
        {
            ChangeListEndTurnBlack();
            ChangeListEndTurnWhite();
        }
        private bool CheckCanThrow(int number, int i)
        {
            return number + cubesTemporary[i] > 23 && colorPlayer == "black" && blackListTemporary.Count < 1 ||whiteListTemporary.Count <1  && (colorPlayer == "white" && number-cubesTemporary[i] < 0);
        }
        private bool CheckCanThrowWhite(int number, int i)
        {
            return (cubesTemporary[i] != 0 && (number == 50 || GetColorFromBoard(number, "white")));
        }
        private bool CheckCanThrowBlack(int number, int i)
        {
            return (number == 50 ||  GetColorFromBoard(number, "black"));
        }
        private bool CheckHelpMove(int num, string color)
        {
            return !GetColorFromBoard(num, color)
                || GetAmountFromBoard(num, 0) || GetAmountFromBoard(num, 1);

        }
        private void CheckOnMoveWhite(int number, int i)
        {
            if (CheckCanThrowWhite(number, i))
            {
                if(number != 50 && number - cubesTemporary[i] > -1 && CheckHelpMove(number - cubesTemporary[i], "black"))
                {
                    canPlay = true;
                    showOnMoveTemp[number - cubesTemporary[i]] = true;
                }
                if (number == 50 && CheckHelpMove(24 - cubesTemporary[i], "black"))
                {
                    canPlay = true;
                    showOnMoveTemp[24 - cubesTemporary[i] ] = true;
                }
            }
        }
        private void CheckOnMoveBlack(int number, int i)
        {
            if (CheckCanThrowBlack(number, i))
            {
                if(number != 50 && number + cubesTemporary[i] <24 && CheckHelpMove(number + cubesTemporary[i], "white"))
                {
                    canPlay = true;
                    showOnMoveTemp[number + cubesTemporary[i]] = true;
                }
                if (number == 50 && CheckHelpMove(cubesTemporary[i] - 1, "white"))
                {
                    canPlay = true;
                    showOnMoveTemp[cubesTemporary[i] - 1] = true;
                }
            }
        }
        public void PlayTurn(Turn turn)
        {
            this.turn = turn;
            Clear();
            if(colorPlayer == "black") BlackTurn();
            else WhiteTurn();
        }
        private void WhiteTurn()
        {
            if (ReleaseWhite()) return;
            if (ThrowToEndWhite()) return;
            if (DeleteOneCubesWhite()) return;
            MoveWhite();
        }
        private bool ReleaseWhite()
        {
            if(whiteListTemporary.Count > 0)
            {
                if (ChackCanOutFromWhiteList())
                {
                    if (ChackCubesReleaseWhite()) return true;
                    if (ChackCanEatBlackPlayer()) BlackListUp();
                    else ChangeAmountFromBoard(turn.To, 1);
                    cubesTemporary[deleteCube] = 0;
                    ChangeColorFromBoard(turn.To, colorPlayer); ;
                    WhiteListDown();
                }
                return true;
            }
            return false;
        }
        private bool ThrowToEndWhite()
        {
            if (ChackThrowOutBlack())
            {
                for (int i = 0; i < boardTemporary.board.Length - 6; i++)
                    if (GetColorFromBoard(i + 6, "white"))
                    { 
                        return true;
                    }
                for (int i = 0; i < cubesTemporary.Length; i++)
                {
                    if (turn.From - cubesTemporary[i] < 0)
                    {
                        ChangeAmountFromBoard(turn.From,-1);
                        if (GetAmountFromBoard(turn.From, 0))
                            ChangeColorFromBoard(turn.From, "");
                        cubesTemporary[i] = 0;
                        return true;
                    }
                }
                return true;
            }
            return false;
        }
        private bool DeleteOneCubesWhite()
        {
            deleteCube = 0;
            for (int i = 0; i < cubesTemporary.Length; i++)
            {
                if (cubesTemporary[i] == turn.From - turn.To)
                {
                    deleteCube = i;
                    return false;
                }
            }
            return true;
        }
        private void MoveWhite()
        {
            if (ThrowOut()) return;

            if (ChackCanMoveWhite())
            {
                if (GetColorFromBoard(turn.To, "black"))
                {
                    BlackListUp();
                }
                else
                {
                    ChangeAmountFromBoard(turn.To, 1);
                }
                if (GetAmountFromBoard(turn.From, 1))
                {
                    ChangeColorFromBoard(turn.From, "");
                }
                ChangeColorFromBoard(turn.To, "white");
                cubesTemporary[deleteCube] = 0;
                ChangeAmountFromBoard(turn.From,-1);
            }
        }
        private void BlackTurn()
        {

            if (ReleaseBlack()) return;
            if (ThrowToEndBlack()) return;
            if (DeleteOneCubesBlack()) return;
            MoveBlack();
        }
        private bool ReleaseBlack()
        {
            if(blackListTemporary.Count > 0)
            {
                if (ChackCanOutFromBlackList())
                {
                    if (ChackCubesReleaseBlack())
                        return true;
                    if (GetAmountFromBoard(turn.To, 1) && GetColorFromBoard(turn.To, "white"))
                        WhiteListUp();
                    else
                        ChangeAmountFromBoard(turn.To, 1);
                    cubesTemporary[deleteCube] = 0;
                    ChangeColorFromBoard(turn.To, colorPlayer); ;
                    BlackListDown();
                }
                return true;
            }
            return false;
        }
        private bool ThrowToEndBlack()
        {
            if (ChackThrowOutWhite())
            {
                for (int i = 0; i < boardTemporary.board.Length - 6; i++)
                {
                    if (GetColorFromBoard(i, "black"))
                    {
                        return true;
                    }
                }
                for (int i = 0; i < cubesTemporary.Length; i++)
                {
                    if (turn.From + cubesTemporary[i] > 23)
                    {
                        ChangeAmountFromBoard(turn.From,-1);
                        if (GetAmountFromBoard(turn.From,0))
                        {
                            ChangeColorFromBoard(turn.From,"");
                        }
                        cubesTemporary[i] = 0;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool DeleteOneCubesBlack()
        {
            deleteCube = 0;
            for (int i = 0; i < cubesTemporary.Length; i++)
            {
                if (cubesTemporary[i] == turn.To - turn.From)
                {
                    deleteCube = i;
                    return false;
                }
            }
            return true;
        }
        private void MoveBlack()
        {
            if (ThrowOut()) return;
            if (ChackCanMoveBlack())
            {
                if (GetColorFromBoard(turn.To, "white"))
                { 
                    WhiteListUp(); 
                }
                else
                {
                    ChangeAmountFromBoard(turn.To, 1);
                }
                if (GetAmountFromBoard(turn.From, 1))
                {
                    ChangeColorFromBoard(turn.From, "");
                }
                ChangeColorFromBoard(turn.To, "black");
                ChangeAmountFromBoard(turn.From,-1);
                cubesTemporary[deleteCube] = 0;
            }
        }
        private bool ChackCubesReleaseWhite()
        {
            for (int i = 0; i < cubesTemporary.Length; i++)
            {
                if (turn.To == 24 - cubesTemporary[i])
                {
                    deleteCube = i;
                    return false;
                }
            }
            return true;
        }
        private bool ChackCubesReleaseBlack()
        {
            for (int i = 0; i < cubesTemporary.Length; i++)
            {
                if (turn.To + 1 == cubesTemporary[i])
                {
                    deleteCube = i;
                    return false;
                }
            }
            return true;
        }
        private bool ThrowOut()
        {
            if (turn.To == 200)
            {
                ChangeAmountFromBoard(turn.From,-1);
                if (GetAmountFromBoard(turn.From, 0))
                {
                    ChangeColorFromBoard(turn.From,"");
                }
                cubesTemporary[deleteCube] = 0;
                return true;
            }
            return false;
        }
        private bool ChackCanOutFromWhiteList()
        {
            return  turn.From == 50 && (GetColorFromBoard(turn.To, "white") 
                || boardTemporary.board[turn.To].Amount < 2);
        }
        private bool ChackCanEatBlackPlayer()
        {
            return boardTemporary.board[turn.To].Amount == 1 && GetColorFromBoard(turn.To, "black");
        }
        private bool ChackThrowOutBlack()
        {
            return turn.To == 200 && whiteListTemporary.Count == 0;
        }
        private bool ChackThrowOutWhite()
        {
            return turn.To == 200 && blackListTemporary.Count == 0;
        }
        private bool ChackCanMoveBlack()
        {
            return turn.From < turn.To && (GetColorFromBoard(turn.To,colorPlayer) ||
                GetColorFromBoard(turn.To,"") ||
                boardTemporary.board[turn.To].Amount == 1);
        }
        private bool ChackCanMoveWhite()
        {
            return turn.From > turn.To && (GetColorFromBoard(turn.To, colorPlayer) ||
                GetColorFromBoard(turn.To, "") ||
                boardTemporary.board[turn.To].Amount == 1);
        }
        private bool ChackCanOutFromBlackList()
        {
            return  turn.From == 50 && (GetColorFromBoard(turn.To, "black") 
                || boardTemporary.board[turn.To].Amount < 2);
        }
        private bool GetAmountFromBoard(int num,int equal)
        {
            return boardTemporary.board[num].Amount == equal;
        }
        private bool GetColorFromBoard(int num,string color)
        {
            return boardTemporary.board[num].Color == color;
        }
        private void ChangeColorFromBoard(int num,string color)
        {
            boardTemporary.board[num].Color = color;
        }
        private void ChangeAmountFromBoard(int num, int posOrNegtive)
        {
            boardTemporary.board[num].Amount += posOrNegtive;
        }
        private void BlackListUp()
        {
            blackListTemporary.AddFirst(true);
        }
        private void BlackListDown()
        {
            blackListTemporary.RemoveLast();
        }
        private void WhiteListUp()
        {
            whiteListTemporary.AddFirst(true);
        }
        private void WhiteListDown()
        {
            whiteListTemporary.RemoveLast();
        }
        private void ResetCubes()
        {
            for (int i = 0; i < board.board.Length; i++)
            {
                boardTemporary.board[i].Amount = board.board[i].Amount;
                boardTemporary.board[i].Color = board.board[i].Color;
            }
        }
    }
}
