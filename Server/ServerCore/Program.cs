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

                /* 랜덤메타 */
                // 쉬다 온다
                //Thread.Sleep(1); // 무조건 휴식 -> 무조거너 1ms정도 쉬고 싶다.

                //Thread.Sleep(0); // 조건부 양보 -> 나보다 우선순위가 낮은 애들한테는 양보 불가
                                 // -> 우선순위가 나보다 같거나 높은 쓰레드가 없으면 다시 본인한테 돌아옴.
                                 // 우선순위란 설정할 때 중요한 쓰레드에게 우선순위를 정해줄 수 있다.
                                 // 대신 다른 쓰레드들은 기아현상이 일어날 수 있다.

                Thread.Yield(); // 관대한 양보 -> 관대하게 양보할테니, 지금 실행이 가능한 쓰레드가 있으면 실행하세요
                                // 실행 가능한 애가 없으면 남은 시간 소진
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