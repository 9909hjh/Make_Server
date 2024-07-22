namespace ServerCore
{
    /* TLS : Thread Local Storage */
    /*
    - 전역(정적static) : 누구나 접근 가능
    - 스택 : 현재 쓰레드만 접근 가능 BUT 함수가 끝나면 폭파되는 공간
    - TLS : 스택처럼 현재 쓰레드만 접근 가능 BUT 함수가 호출 완료되도 여전히 유효한 공간

    이렇게 구분할 수가 있다.
    즉 쓰레드끼리 경합이 일어나지 않고 안전하면서도,
    반영구적으로 안전히 사용할 수 있는 공간이라는 것.*/

    internal class Program
    {
        //thread마다 얘를 접근하면은 자신만의 공간에다가 얘가 저장이 되기 때문에
        //특정 thread에서 thread name을 고친다고 해도 다른 애들한테는 영향을 주지 않게 된다.
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => { return $"my name is {Thread.CurrentThread.ManagedThreadId}"; });
        
        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated;
            if(repeat)
                Console.WriteLine(ThreadName.Value + "(repeat)");
            else
                Console.WriteLine(ThreadName.Value);
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);

            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            ThreadName.Dispose(); // 필요가 없어졌다면 없애버릴 수도 있다.
        }
    }
}