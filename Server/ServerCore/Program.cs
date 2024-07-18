namespace ServerCore
{
    internal class Program
    {
        static void MainThread()
        {
            Console.WriteLine("Hello Thread");
        }

        // 핵심 기능
        static void Main(string[] args)
        {
            Thread t = new Thread(MainThread);
            t.Start();

            Console.WriteLine("Hello, World!");
        }
    }
}