namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;

        public void Acquire()
        {
            while(true)
            {
                //int original = Interlocked.Exchange(ref _locked, 1); // 통상적으로 이런 방식은
                //사용하지 않지만 기본 원리를 보여주는 코드다.
                //if (original == 0)
                //    break;

                /* CAS : Compare-And-Swap */
                // 일반적으로 사용하는 방식
                // Interlocked.CompareExchange(ref _locked, 1, 0);

                int expected = 0; // 내가 예상한 값
                int desired = 1; // 내가 원하는 값
                // 내가 예상한 값이 0인데 그 값이 맞다면 내가 원하는 값을 넣어준다.
                if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected) 
                    break;
            }

        }

        public void Release()
        {
            _locked = 0;
        }
    }

    internal class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

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