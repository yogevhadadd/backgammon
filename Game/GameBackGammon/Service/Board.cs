using Game.model;

namespace GameBackGammon.Service
{
    public class Board
    {
        public Location[] board = new Location[24];
        public Board()
        {
            for (int i = 0; i < 24; i++)
            {
                board[i] = new Location();
            }
            board[0].Color = "black";
            board[1].Color = "";
            board[2].Color = "";
            board[3].Color = "";
            board[4].Color = "";
            board[5].Color = "white";
            board[6].Color = "";
            board[7].Color = "white";
            board[8].Color = "";
            board[9].Color = "";
            board[10].Color = "";
            board[11].Color = "black";
            board[12].Color = "white";
            board[13].Color = "";
            board[14].Color = "";
            board[15].Color = "";
            board[16].Color = "black";
            board[17].Color = "";
            board[18].Color = "black";
            board[19].Color = "";
            board[20].Color = "";
            board[21].Color = "";
            board[22].Color = "";
            board[23].Color = "white";
            board[0].Amount = 2;
            board[1].Amount = 0;
            board[2].Amount = 0;
            board[3].Amount = 0;
            board[4].Amount = 0;
            board[5].Amount = 5;
            board[6].Amount = 0;
            board[7].Amount = 3;
            board[8].Amount = 0;
            board[9].Amount = 0;
            board[10].Amount = 0;
            board[11].Amount = 5;
            board[12].Amount = 5;
            board[13].Amount = 0;
            board[14].Amount = 0;
            board[15].Amount = 0;
            board[16].Amount = 3;
            board[17].Amount = 0;
            board[18].Amount = 5;
            board[19].Amount = 0;
            board[20].Amount = 0;
            board[21].Amount = 0;
            board[22].Amount = 0;
            board[23].Amount = 2;
        }
    }
}
