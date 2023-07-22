using System;


namespace Algorithm
{
    static class Program
    {
        
        static void Main(string[] args)
       {
            Board board = new Board();
            board.Initialize(25);
            
            Console.CursorVisible = false;

            const int WAIT_TICK = 1000 / 30;
            
            
            int lastTick = 0;
            while (true)
            {
                #region 프레임 관리
                //경과 시간 확인
                int currentTick = System.Environment.TickCount;
                
                //만약 경과한 시간이 1/30 초보다 작다면
                if (currentTick - lastTick < WAIT_TICK) continue;
                lastTick = currentTick;
                #endregion

                Console.SetCursorPosition(0, 0);

                board.Render();
                }
            }
       }
    }

