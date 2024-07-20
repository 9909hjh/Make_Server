namespace ServerCore
{
    // 데드락 상황 예제
    class SessionManager
    {
        static object _lock = new object();

        public static void TestSession()
        {
            lock (_lock)
            {

            }
        }

        public static void Test()
        {
            lock (_lock)
            {
                UserManager.TestUser();
            }
        }
    }

    class UserManager
    {
        static object _lock = new object();

        public static void Test()
        {
            lock (_lock)
            {
                SessionManager.TestSession();
            }
        }

        public static void TestUser()
        {
            lock (_lock)
            {

            }
        }
    }

    internal class Program
    {
        static int number = 0;
        static object _obj = new object();

        static void Thread_1()
        {
            for(int i = 0; i < 100000; i++)
            {
                SessionManager.Test();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                UserManager.Test();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();

            Thread.Sleep(100);  // 사실 데드락이 걸리면 오류를 수정하는 쪽이 좋지만
                                // 정 안된다면 시간을 두고 시작하도록 하는 방법이 있다 추천하진 않음.

            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }
}