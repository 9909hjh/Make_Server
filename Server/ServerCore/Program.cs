namespace ServerCore
{
    internal class Program
    {
        static int _num = 0;

        // Mutex는 AutoReset과 같게 동작하지만 더 많은 정보를 갖고 있다.

        // 갖는 정보 : int count, ThreadID ...
        // 예를 들어 몇번이나 잠궜는지 카운팅을 하고 있다. 그래서 잠근 만큼 풀어줘야 최종적으로 풀리게 되어있다.
        // 또한 ID를 갖고 있어 엉뚱한 곳에서 ReleaseMutex를 하는 것을 방지할 수 있다.

        // 그래서 추가 비용이 많이 들어가고 느려서 어지간해서는 오토리셋을 사용한다.
        static Mutex _lock = new Mutex();

        static void Thread_1()
        {
            for(int  i = 0; i < 100000; i++)
            {
                _lock.WaitOne();
                _num++;
                _lock.ReleaseMutex();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.WaitOne();
                _num--;
                _lock.ReleaseMutex();
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