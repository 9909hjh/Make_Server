namespace ServerCore
{
    /* AutoResetEvent */
    class Lock
    {
        AutoResetEvent _available = new AutoResetEvent(true); // _available가 true면 누구나 들어올 수 있는 상태, false면 반대.
        
        public void Acquire()
        {
            _available.WaitOne(); // 입장 시도.
            //_available.Reset(); // bool = false <- 이건 WaitOne에 세트로 들어가 있다.
        }

        public void Release()
        {
            _available?.Set();
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