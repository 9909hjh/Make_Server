namespace ServerCore
{
    internal class Program
    {
        volatile static bool _stop = false; //C# = volatile 휘발성 데이터 선언 : 코드상 최적화를 하지 말아달라 라는 뜻
        // 하지만 사용하지 말자 그냥 컴파일 릴리즈를 테스트하고 동작하는 것을 보기 위한 선언이다

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작");

            while (_stop == false)
            {
                //누군가가 stop 신호를 해주기를 기다린다.
            }


            Console.WriteLine("쓰레드 종료");
        }


        // 핵심 기능
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");

            t.Wait();

            Console.WriteLine("종료 성공");
        }
    }
}