namespace ServerCore
{
    internal class Program
    {
        /*만약 우리가 생각하는 수학적 이론이 맞다면 (y,x)와(x,y)가 동작하는 시간은 같아야 할 것이다.
         하지만 실행해보면 시간이 거의 두 배의 차이가 난다. ((y,x)의 동작 시간이 더 빠르게 완료된다.)*/

        /*왜 그런가? -> 캐시의 동작 방식의 차이 때문이다. 캐시는 공간적 지역성이라고 방금 주문한 사람 근처에 있는 사람이
         * 추가주문을 할 확률이 높다는 것의 개념이 있다.
         * 어떤 변수에 접근을 하면 그 인적한 주소는 접근될 확률이 높을 것이다 
         때문에 [][][][] [][][][] [][][][] [][][][] 이런 식의 배열이라면 인접한 변수가 더 빨리 실핼 되기에 시간의 차이가 발생한 것이다 */
        static void Main(string[] args)
        {
            int[,] arr = new int[10000, 10000];

            {
                long now = DateTime.Now.Ticks;
                for(int y = 0; y < 10000; y++)
                {
                    for (int x = 0; x < 10000; x++)
                        arr[y, x] = 1;
                }
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y,x) 순서 걸린 시간 : {end - now}");
            }

            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                {
                    for (int x = 0; x < 10000; x++)
                        arr[x, y] = 1;
                }
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x,y) 순서 걸린 시간 : {end - now}");
            }
        }
    }
}