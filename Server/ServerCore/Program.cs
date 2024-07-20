namespace ServerCore
{
    /* ManualResetEvent */
    // 방 문같은 개념
    class Lock
    {
        ManualResetEvent _available = new ManualResetEvent(true); // _available가 true면 누구나 들어올 수 있는 상태, false면 반대.
        
        public void Acquire()
        {
            _available.WaitOne(); // 입장 시도.
            _available.Reset(); // 문을 닫는다. // AutoReset과는 다르게 동작해서 Reset을 추가한다.
                                // 나눠져서 값이 다르게 나온다.
        }

        public void Release()
        {
            _available?.Set(); // 문을 열어준다.
        }
    }

    internal class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();

        static void Thread_1()
        {
            for(int  i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
}