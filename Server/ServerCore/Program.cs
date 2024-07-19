namespace ServerCore
{
    internal class Program
    {
        // Lock 기초
        // Interlocked은 계산을 구현하는게 어려울 수 있다. 때문에 다른 방식의 Lock의 개념을 적용
        // Monitor.Enter/Exit... 
        // 단점 - 관리하기가 조금 어려울 수 있다. 그리고 데드락이 발생할 수 있다.

        // 그런 단점을 보완하기 위해 lock()라는 키워드가 있다 -> 데드락 방지가 가능하다.

        static int number = 0;
        static object _obj = new object();

        static void Thread_1()
        {
            for(int i = 0; i < 1000000; i++)
            {
                lock(_obj) // Monitor.Enter/Exit와 같은역할
                {
                    number++;
                }

                //// 상호배제 Mutual Exlusive

                //Monitor.Enter(_obj); // 문을 잠구는 행위
                ////{
                //    // 만약 여기서 Exit를 안해주고 return을 해버리면 
                //    // Thread_2는 데드락(DeadLock)이 상태가 된다. 때문에 주의 해야한다.
                ////}

                //number++;

                //Monitor.Exit(_obj); // 잠금을 풀어준다.
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                lock (_obj) // Monitor.Enter/Exit와 같은역할
                {
                    number--;
                }
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }
}