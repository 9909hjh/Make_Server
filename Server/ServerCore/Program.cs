namespace ServerCore
{
    // 가시성 문제

    internal class Program
    {
        int _anwser;
        bool _complete;

        /*read/write를 할 때 각각 Thread.MemoryBarrier()의 위치가 달라진다.*/

        void A() // write를 할 때의 코드 // 선언 후 베리어
        {
            _anwser = 123;
            Thread.MemoryBarrier(); // Barrier1
            _complete = true;
            Thread.MemoryBarrier(); // Barrier2 
        }

        void B() // read를 할때의 코드  // 사용하기 전 베리어
        {
            Thread.MemoryBarrier(); // Barrier3
            if (_complete )
            {
                Thread.MemoryBarrier(); // Barrier4
                Console.WriteLine(_anwser);
            }
        }

        static void Main(string[] args)
        {
            
        }
    }
}