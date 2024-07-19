namespace ServerCore
{
    // 하드웨어 최적화를 방지하기 위한 메모리 베리어

    //메모리 베리어
    // A) 코드 재배치 억제
    // B) 가시성

    // 메모리 베리어 종류 
    /*1) Full Memory Barrier (ASW WFENCE, C# Thread.MemoryBarrier) : Store/Load 둘다 막는다.
      2) Store Memory Barrier (ASW SFENCE) : Store만 막는다.
      3) Load Memory Barrier (ASW LFENCE) : Load 막는다.*/

    internal class Program
    {
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1; // Store y

            Thread.MemoryBarrier();

            // Load x
            r1 = x; // 하드웨어에서 마음대로 먼저 실행이 되지 않게 함. MemoryBarrier()함수를 통해서
        }
        static void Thread_2()
        {
            x = 1;

            Thread.MemoryBarrier();


            r2 = y;
        }

        static void Main(string[] args)
        {
            int count = 0;
            while(true)
            {
                count++;
                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if(r1 == 0 && r2 == 0) // 코드대로 라면 걸리지 말아야 함.
                {
                    break;
                }

            }

            Console.WriteLine(count + "번안에 빠져나옴"); // 하지만 빠져나오게 된다. 그 이유는 하드웨어에서 최적화를 진행하기 떄문
        }
    }
}