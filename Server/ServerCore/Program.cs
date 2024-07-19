namespace ServerCore
{
    internal class Program
    {
        // 경합 조건
        /* number++는 코드상으로 볼 때 한번에 실행되는 코드지만 어샘블리에서 컴퓨터가 처리할 때는 3번에 나눠서 더하기가 진행이 된다.
         * 이렇게 되면 멀티쓰레드에서 number의 값을 누가 사용하고 있는지 알 수 없을 때 값이 중복되어 더해지거나 빼질 수 있다. 
         때문에 원자성 즉, atomic의 개념이 존재한다. 

        만약 유저간 거래를 한다고 가정해보자. 거래를 통해 돈은 빠져 나가고 아이템이 들어올 텐데
        그 과정서 돈을 주고 나서 서버가 다운이 된다면 돈은 빠져가지만 아이템은 안들어오는 상태가 될 것이다.
        때문에 이를 방지하기 위해 원자성의 개념을 사용한다. (InterLocked 등, CPU에서 처리해주는 명령어가 따로 있긴하다.)

        */

        static int number = 0;

        static void Thread_1()
        {
            for(int i = 0; i < 1000000; i++)
            {
                // All or Nothing / 실행이 되거나 안되거나
                Interlocked.Increment(ref number); // 값을 직접 사용하지 않고 주소의 참조값을 통해 원자성을 보장해준다.

                //number++;
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Interlocked.Decrement(ref number);
                //number--;
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