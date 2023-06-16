using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList
{
    public class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> Next { get; set; }

        public LinkedListNode(T value)
        {
            Value = value;
            Next = null;
        }
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        private LinkedListNode<T> head;
        private LinkedListNode<T> tail;

        public void Add(T value)
        {
            var newNode = new LinkedListNode<T>(value);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
        }

        public void Remove(T value)
        {
            if (head == null)
                return;

            if (head.Value.Equals(value))
            {
                head = head.Next;
                if (head == null)
                    tail = null;
            }
            else
            {
                var current = head;
                while (current.Next != null)
                {
                    if (current.Next.Value.Equals(value))
                    {
                        if (current.Next == tail)
                            tail = current;

                        current.Next = current.Next.Next;
                        break;
                    }
                    current = current.Next;
                }
            }
        }

        public void Print()
        {
            var current = head;
            while (current != null)
            {
                Console.Write(current.Value + " ");
                current = current.Next;
            }
            Console.WriteLine();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Linked List");

            var linkedList = new LinkedList<int>();
            linkedList.Add(1);
            linkedList.Add(2);
            linkedList.Add(3);

            foreach(var item in linkedList)
            {
                Console.WriteLine(item);
            }

            linkedList.Remove(2);
            linkedList.Print();
        }
    }
}
