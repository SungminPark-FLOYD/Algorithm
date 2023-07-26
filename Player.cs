﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Pos
    {
        public Pos(int y, int x) { Y = y; X = x; }
        public int Y;
        public int X;
    }
    class Player
    {
        //Player 죄표는 Player만 수정 가능함
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        Random _random = new Random();

        Board _board;

        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        int _dir = (int)Dir.Up;
        List<Pos> _points = new List<Pos>();
        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;
            
            _board = board;

            AStar();

        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F) return 0;
                return F < other.F ? 1 : -1;
            }
        }
        void AStar()
        {
            //대각선 이동 추가 (UL, DL, DR, UR)
            int[] deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };
            int[] deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            //비용이 변할 수 있기때문에 확장성 있게 따로 관리
            int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };

            //점수 매기기
            //F = G + H
            //F = 최종 점수 (작을 수록 좋음, 경로에 따라 달리짐
            //G = 시작점에서 해당 좌표까지 이동하는데 드는 비용(작을 수록 좋음, 경로에 따라 달라짐)
            //H = 목적지에서 얼마나 가까운지(작을수록 좋음, 고정)

            //(y, x) 이미 방문했는지 여부(방문 = closed 상태)
            bool[,] closed = new bool[_board.Size, _board.Size]; //CloseList

            //(y, x) 가는 길을 한번이라도 발견했는지
            //발견 x => MaxValue
            //발견 o => F = G + H
            int[,] open = new int[_board.Size, _board.Size]; //OpenList 
            for(int y = 0; y < _board.Size; y++)            
                for(int x = 0; x < _board.Size; x++)               
                    open[y, x] = Int32.MaxValue;

            Pos[,] parent = new Pos[_board.Size, _board.Size];

            //오픈리스트에 있는 정보들 중에서 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            //시작점 발견(예약 진행)
            //abs를 사용하여 절댓값 만큼 H로 인정
            open[PosY, PosX] = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX));
            pq.Push(new PQNode() { F = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX)),
                                   G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (pq.Count > 0)
            {
                //제일 좋은 후보를 찾는다
                PQNode node = pq.Pop();
                //동일한 죄표를 여러 경로로 찾아서 더 빠른 경로로 인해서 이미 방문된 경우 스킵
                if (closed[node.Y, node.X]) continue;

                //방문한다
                closed[node.Y, node.X] = true;
                //목적지 도착했으면 종료
                if (node.Y == _board.DestY && node.X == _board.DestX) break;

                //상하죄우 등 이동할 수 있는 죄표인지 확인해서 예약한다
                for(int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    //예외 처리
                    //유효범위를 벗어났으면 스킵
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    //벽으로 막혀서 갈 수 없으면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    //이미 방문한 곳이면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    //비용 계산
                    int g = node.G + cost[i];
                    int h = 10 * (Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX));
                    //다른 경로에서 더 빠른 길 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h) continue;

                    //예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }

            }

            CalcPathFromParent(parent);
        }

        void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while(q.Count > 0)
            {
                Pos pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                for(int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];

                    //예외 처리
                    if(nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall) 
                        continue;
                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);
                }
            }

            CalcPathFromParent(parent);
        }

        void CalcPathFromParent(Pos[,] parent)
        {
            int y = _board.DestY;
            int x = _board.DestX;
            //부모와 자신의 좌표가 같은지 확인
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Pos(y, x));
                Pos pos = parent[y, x];
                y = pos.Y;
                x = pos.X;
            }
            _points.Add(new Pos(y, x));
            _points.Reverse();
            //도착지점 부터 역순으로 시작지점까지 비교하면서 가기 때문에 Reverse를 사용해서 뒤집는다
        }
        //우수법
        void RightHand()
        {
            //현재 바라보고 있는 방향을 기준으로 죄표 변화를 나타낸다
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            _points.Add(new Pos(PosY, PosX));

            //모든 경로를 미리 계산
            //목적지에 도착하기 전에는 계속 실행
            while (PosY != _board.DestY || PosX != _board.DestX)
            {
                //1. 현재 바라보는 기준으로 오른쪽으로 갈 수 있는지 확인
                if (_board.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    //오른쪽 방향으로 90도 회전
                    //나머지 연산으로 회전 순환 시키기
                    _dir = (_dir - 1 + 4) % 4;
                    //앞으로 한보 전진
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                //2. 현재 바라보는 방향을 기준으로 전진할 수 있는지 확인
                else if (_board.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    //앞으로 한보 전진
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                else
                {
                    //왼쪽 방향으로 90도 회전
                    _dir = (_dir + 1 + 4) % 4;
                }
            }
        }

        const int MOVE_TICK = 10;
        int _sumTick = 0;
        int _lastIndex = 0;
        //프레임 관리
        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count) return;

            _sumTick += deltaTick;
            if(_sumTick >= MOVE_TICK)
            {                
                _sumTick = 0;

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
                
                #region 플레이어 이동 초기 로직
                /*
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
                */
                #endregion

            }
        }
        
    }
}
