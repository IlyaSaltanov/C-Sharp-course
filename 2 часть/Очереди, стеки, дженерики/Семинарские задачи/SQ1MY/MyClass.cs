

namespace MyNamespace
{
    public class MyClass
    {
        public Stack<int> first = new Stack<int>();
        public Stack<int> second = new Stack<int>();

        public void MyPush(int x)
        {
            if (first.Count == 0)
            {
                first.Push(x);
                second.Push(x);
            }
            else
            {
                if (x < first.Peek())
                {
                    first.Push(x);
                }
                else
                {
                    first.Push(x);
                    second.Push(x);
                }
            }
        }

        public int GetMax()
        {
            return second.Peek();
        }

        public int MyPop()
        {
            if (first.Count == 0)
            {
                throw new Exception("Стек пуст");
            }
            int x = first.Pop();
            second.Pop();
            return x;
        }
    }
}