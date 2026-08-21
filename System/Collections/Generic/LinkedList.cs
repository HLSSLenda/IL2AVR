namespace System.Collections.Generic;

public sealed class LinkedList<T>
{
    public unsafe struct Node<T>
    {
        private Node<T>* _previous;
        private Node<T>* _next;
        public T Value;

        public ref Node<T> Preious
        {
            get => ref *_previous;
            set => *_previous = value;
        }

        public ref Node<T> Next
        {
            get => ref *_next;
            set => *_next = value;
        }
    }
}
