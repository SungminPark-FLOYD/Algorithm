using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Player
    {
        //Player 죄표는 Player만 수정 가능함
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        Random _random = new Random();
       

        Board _board;
        public void Initialize(int posY, int posX, int destY, int destX, Board board)
        {
            PosY = posY;
            PosX = posX;
            
            _board = board;
        }

        const int MOVE_TICK = 100;
        int _sumTick = 0;
        //프레임 관리
        public void Update(int deltaTick)
        {
            _sumTick += deltaTick;
            if(_sumTick >= MOVE_TICK)
            {
                _sumTick = 0;

                //0.1초마다 실행될 로직
                int ranValue = _random.Next(0, 5);
                switch(ranValue)
                {
                    //벽이 있다는 가정하에 로직을 완성했기 때문에 맵 범위를 넘지 않게 범위설정을 해주어야 한다
                    case 0: //상
                        if (PosY - 1 >= 0 && _board.Tile[PosY - 1, PosX] == Board.TileType.Empty)
                            PosY = PosY - 1;
                        break;
                    case 1: //하
                        if (PosY + 1 < _board.Size && _board.Tile[PosY + 1, PosX] == Board.TileType.Empty)
                            PosY = PosY + 1;
                        break;
                    case 2: //좌
                        if (PosX - 1 >= 0 && _board.Tile[PosY, PosX -1] == Board.TileType.Empty)
                            PosX = PosX - 1;
                        break;
                    case 3: //우
                        if (PosX + 1 > _board.Size && _board.Tile[PosY, PosX + 1] == Board.TileType.Empty)
                            PosX = PosX + 1;
                        break;
                        
                }
            }
        }
    }
}
