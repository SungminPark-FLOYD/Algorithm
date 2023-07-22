﻿using System;


namespace Algorithm
{
    static class Program
    {
        
        static void Main(string[] args)
       {
            Board board = new Board();
            Player player = new Player();
            board.Initialize(25, player);
            player.Initialize(1, 1, board.Size - 2, board.Size - 2, board);
            
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
                int deltaTick = lastTick = currentTick;
                #endregion
                //입력

                //로직
                player.Update(deltaTick);
                //렌더링
                Console.SetCursorPosition(0, 0);
                board.Render();
                }
            }
       }
    }

