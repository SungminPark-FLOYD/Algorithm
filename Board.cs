using System;
using System.Collections.Generic;

namespace Algorithm
{
    #region 동적배열 구현 연슴
    /*
        class MyList<T>
        {
            const int DEFAULTSize = 1;
            T[] _data = new T[DEFAULTSize];

            public int Count;       //실제로 사용중인 데이터 개수
            public int Capacity { get { return _data.Length; } }   //예약된 데이터 개수
            //O(1) 예외 케이스
            public void Add(T item)
            {
                //1. 공간이 충분히 남아있는지 확인한다
                if(Count >= Capacity)
                {
                    //공간을 다시 늘려서 확보한다
                    T[] newArray = new T[Count * 2];
                    for (int i = 0; i < Count; i++)
                    {
                        newArray[i] = _data[i];
                    }
                    _data = newArray;              
                }
                //2. 공간에다가 데이터를 넣어준다
                _data[Count] = item;
                Count++;
            }
            //O(1)
            public T this[int index]
            {
                get { return _data[index]; }
                set { _data[index] = value; }
            }
            //O(N)
            public void RemoveAt(int index)
            {
                for(int i=index; i < Count -1; i++)
                {
                    _data[i] = _data[i - 1];
                }
                _data[Count - 1] = default(T);
                Count--;
            }
        }
    class 내부 {

        public List<int> _data2 = new List<int>();  //동적 배열 *c++ vector
        public void Initialize()
        {
            _data2.Add(101);
            _data2.Add(102);
            _data2.Add(103);
            _data2.Add(104);
            _data2.Add(105);
        
            int temp = _data2[2];
            //인덱스 삭제
            _data2.RemoveAt(2);
        }
    }
    */
    #endregion

    #region 연결리스트 구현 연습
    /*
    //연결리스트 주소로 바로 접근이 불가능하다
    class Room<T>
    {
        public T Data;
        public Room<T> Next;
        public Room<T> Prev;
    }

    class RoomList<T>
    {
        public Room<T> Head = null;        //첫번째
        public Room<T> Tail = null;        //마지막
        public int Count = 0;

        //O(1)
        public Room<T> AddLast(T data)
        {
            Room<T> newRoom = new Room<T>();
            newRoom.Data = data;

            //만약 아직 방이 이예 없었다면, 새로 추가한 방이 곧 Head이다
            if (Head == null) Head = newRoom;

            //기존의 마지막 방과 새로 추가되는 방을 연결해준다
            if (Tail != null)
            {
                Tail.Next = newRoom;
                newRoom.Prev = Tail;

            }
            //새로 추가되는 방을 마지막 방으로 인정한다
            Tail = newRoom;
            Count++;
            return newRoom;

        }
        //O(1)
        public void Remove(Room<T> room)
        {
            //기존의 첫번째 방의 다음방을 첫번째 방으로 인정한다
            if (Head == room)
            {
                Head = Head.Next;
            }
            //기존의 마지막 방의 이전 방을 마지막 방으로 인정한다
            if (Tail == room) Tail = Tail.Prev;

            if (room.Prev != null) room.Prev.Next = room.Next;

            if (room.Next != null) room.Next.Prev = room.Prev;

            Count--;
        }
    }
    class 내부 {
    public LinkedList<int> _data3 = new LinkedList<int>(); // 연결 리스트 *c++ list

    public void Initialize()
        {
            _data3.AddLast(101);
            _data3.AddLast(102);
            LinkedListNode<int> node = _data3.AddLast(103); //임시저장
            _data3.AddLast(104);
            _data3.AddLast(105);

            _data3.Remove(node);
        }
    
    }

    */
    #endregion



    class Board
    {
        const char CIRCLE = '\u25cf';
        public TileType[,] Tile { get; private set; } //배열
        public int Size { get; private set; }

        Player _player;

        public enum TileType
        {
            Empty,
            Wall,
        }
        public void Initialize(int size, Player player)
        {
            //짝수면 리턴
            if (size % 2 == 0) return;

            _player = player;

            Tile = new TileType[size, size];
            Size = size;

            //Mazes for Programmers
            //GenerateByBinaryTree();
            GenerateBySideWinder();

        }

        //SideWinder 미로 생성알고리즘 
        void GenerateBySideWinder()
        {
            //일단 길을 다 막아본다
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }
            //랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            Random ran = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    //강제로 벽까지 이동 제한
                    if (y == Size - 2 && x == Size - 2)
                        continue;
                    //벽에 막히면 돌아가도록 한다
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    //우측으로 갈 경우
                    if (ran.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    //아래로 갈 경우
                    else
                    {
                        int randomIndex = ran.Next(0, count);
                        Tile[y + 1, x - randomIndex*2] = TileType.Empty;
                        count = 1;
                    }

                }
            }
        }
        //Binary Tree 미로 생성알고리즘
        void GenerateByBinaryTree()
        {
            
            //벽이 다 막혀 있다는 가정이 깔려있음

            //일단 길을 다 막아본다
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }
            //랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            Random ran = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;
                    if (y == Size - 2 && x == Size - 2)
                        continue;
                    //벽에 막히면 돌아가도록 한다
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    if (ran.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        Tile[y + 1, x] = TileType.Empty;
                    }

                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    //플레이어 좌표를 갖고 와서 그 좌표랑 현재 y,x 가 일치하면 플레이어 전용 색상으로 표시
                    if(y == _player.PosY && x == _player.PosX)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);
        
                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prevColor;
        }
        //색 추출
        ConsoleColor GetTileColor(TileType type)
        {
            switch(type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;
            }
        }
    }
}
