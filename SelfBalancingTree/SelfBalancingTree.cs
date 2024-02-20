using System;
using System.Drawing;
using System.Numerics;

public class SelfBalancingTree<T> where T : IComparable<T>
{
    public class Node
    {
        public T Value;

        public int Frequency;

        public Node Left;

        public Node Right;

        public Node(T value)
        {
            Value = value;

            Frequency = 1;
        }

        public void Add(T value)
        {
            if (value.CompareTo(Value) == 0)
            {
                Frequency++;
            }
            else if (value.CompareTo(Value) < 0)
            {
                if (Left == null)
                {
                    Left = new Node(value);
                }
                else
                {
                    Left.Add(value);
                }
            }
            else
            {
                if (Right == null)
                {
                    Right = new Node(value);
                }
                else
                {
                    Right.Add(value);
                }
            }
        }
    }

    public Node Root;

    public void Add(T value)
    {
        if (Root == null)
        {
            Root = new Node(value);
        }
        else
        {
            Root.Add(value);
        }
    }

    public bool Insert(T value)
    {
        Node current = Root;

        Node parent = null;

        while (current != null)
        {
            parent = current;

            if (value.CompareTo(current.Value) == 0)
            {
                current.Frequency++;

                return false;
            }
            else if (value.CompareTo(current.Value) < 0)
                current = current.Left;
            else
                current = current.Right;
        }

        if (parent == null)
            Root = new Node(value);
        else if (value.CompareTo(parent.Value) < 0)
            parent.Left = new Node(value);
        else
            parent.Right = new Node(value);

        return true;
    }

    public void InsertAVL(T value)
    {
        Root = InsertAVL(value, Root);
    }

    private Node InsertAVL(T value, Node parent)
    {
        if (parent == null)
            return new Node(value);
        
        if (value.CompareTo(parent.Value) == 0)
        {
            parent.Frequency++;

            return parent;
        }
        
        if (value.CompareTo(parent.Value) < 0)
            parent.Left = InsertAVL(value, parent.Left);
        else if (value.CompareTo(parent.Value) > 0)
            parent.Right = InsertAVL(value, parent.Right);

        parent = BalanceTree_AVL(parent);

        return parent;
    }

    public bool Find(T value)
    {
        if (Root == null) return false;

        return Find(value, Root);
    }

    private bool Find(T value, Node parent)
    {
        if (parent != null)
        {
            if (value.CompareTo(parent.Value) == 0)
                return true;
            else if (value.CompareTo(parent.Value) < 0)
                return Find(value, parent.Left);
            else
                return Find(value, parent.Right);
        }

        return false;
    }

    private bool Find(Node target)
    {
        if (target == null || Root == null) return false;

        return Find(target, Root);
    }

    private bool Find(Node target, Node parent)
    {
        if (parent != null)
        {
            if (target == parent)
                return true;
            else if (target.Value.CompareTo(parent.Value) < 0)
                return Find(target, parent.Left);
            else
                return Find(target, parent.Right);
        }

        return false;
    }

    public void Remove(T value)
    {
        Root = Remove(value, Root);
    }

    private Node Remove(T value, Node parent, bool checkFrequency = true)
    {
        if (parent == null) return parent;

        if (value.CompareTo(parent.Value) == 0)
        {
            if (checkFrequency && parent.Frequency > 1)
            {
                parent.Frequency--;

                return parent;
            }

            if (parent.Left == null)
                return parent.Right;
            else if (parent.Right == null)
                return parent.Left;

            Node minNode = MinNode(parent.Right);

            parent.Value = minNode.Value;
            parent.Frequency = minNode.Frequency;

            parent.Right = Remove(parent.Value, parent.Right, false);
        }
        else if (value.CompareTo(parent.Value) < 0)
            parent.Left = Remove(value, parent.Left, checkFrequency);
        else
            parent.Right = Remove(value, parent.Right, checkFrequency);

        return parent;
    }

    public void RemoveAVL(T value)
    {
        Root = RemoveAVL(value, Root);
    }

    private Node RemoveAVL(T value, Node parent, bool checkFrequency = true)
    {
        if (parent == null) return parent;

        if (value.CompareTo(parent.Value) == 0)
        {
            if (checkFrequency && parent.Frequency > 1)
            {
                parent.Frequency--;

                return parent;
            }

            if (parent.Left == null)
                return parent.Right;
            else if (parent.Right == null)
                return parent.Left;

            Node minNode = MinNode(parent.Right);

            parent.Value = minNode.Value;
            parent.Frequency = minNode.Frequency;

            parent.Right = RemoveAVL(parent.Value, parent.Right, false);
        }
        else if (value.CompareTo(parent.Value) < 0)
            parent.Left = RemoveAVL(value, parent.Left, checkFrequency);
        else
            parent.Right = RemoveAVL(value, parent.Right, checkFrequency);

        parent = BalanceTree_AVL(parent);

        return parent;
    }

    public Node Remove(Node target)
    {
        return Remove(target, Root);
    }

    private Node Remove(Node target, Node parent)
    {
        if (parent == null) return parent;

        if (target == parent)
        {
            if (parent.Frequency == 1)
            {
                if (parent.Left == null)
                    return parent.Right;
                else if (parent.Right == null)
                    return parent.Left;

                parent.Value = MinValue(parent.Right);

                parent.Right = Remove(parent, parent.Right);
            }
            else
            {
                parent.Frequency--;

                return parent;
            }
        }
        else if (target.Value.CompareTo(parent.Value) < 0)
            parent.Left = Remove(target, parent.Left);
        else
            parent.Right = Remove(target, parent.Right);

        return parent;
    }

    private T MinValue(Node node)
    {
        T minvalue = node.Value;

        while (node.Left != null)
        {
            minvalue = node.Left.Value;
            node = node.Left;
        }

        return minvalue;
    }

    private T MaxValue(Node node)
    {
        T maxvalue = node.Value;

        while (node.Right != null)
        {
            maxvalue = node.Right.Value;
            node = node.Right;
        }

        return maxvalue;
    }

    private Node MinNode(Node node)
    {
        while (node.Left != null)
            node = node.Left;

        return node;
    }

    private Node MaxNode(Node node)
    {
        while (node.Right != null)
            node = node.Right;

        return node;
    }

    public int Size()
    {
        return Size(Root);
    }

    private int Size(Node parent)
    {
        if (parent == null) return 0;

        return Size(parent.Left) + Size(parent.Right) + 1;
    }

    public int GetTreeHeight()
    {
        return GetSubTreeHeight(Root);
    }

    private int GetSubTreeHeight(Node parent)
    {
        if (parent == null) return -1;

        return Math.Max(GetSubTreeHeight(parent.Left), GetSubTreeHeight(parent.Right)) + 1;
    }

    public int SumAllSubTreeHeights()
    {
        if (Root == null) return -1;

        return SumAllSubTreeHeights(Root);
    }

    private int SumAllSubTreeHeights(Node parent, int sumHeights = 0)
    {
        if (parent != null)
        {
            sumHeights += GetSubTreeHeight(parent);

            sumHeights = SumAllSubTreeHeights(parent.Left, sumHeights);
            sumHeights = SumAllSubTreeHeights(parent.Right, sumHeights);
        }

        return sumHeights;
    }

    private int SumAllSubTreeHeights2(Node parent)
    {
        int sumHeights = 0;

        SumAllSubTreeHeights2(parent, ref sumHeights);

        return sumHeights;
    }

    private void SumAllSubTreeHeights2(Node parent, ref int sumHeights)
    {
        if (parent != null)
        {
            sumHeights += GetSubTreeHeight(parent);

            SumAllSubTreeHeights2(parent.Left, ref sumHeights);
            SumAllSubTreeHeights2(parent.Right, ref sumHeights);
        }
    }

    public int SumAllSumAllSubTreeHeights()
    {
        if (Root == null) return -1;

        return SumAllSumAllSubTreeHeights(Root);
    }

    private int SumAllSumAllSubTreeHeights(Node parent, int sumSumHeights = 0)
    {
        if (parent != null)
        {
            sumSumHeights += SumAllSubTreeHeights(parent);

            sumSumHeights = SumAllSumAllSubTreeHeights(parent.Left, sumSumHeights);
            sumSumHeights = SumAllSumAllSubTreeHeights(parent.Right, sumSumHeights);
        }

        return sumSumHeights;
    }

    public int SumAllSumAllSubTreeHeights2()
    {
        if (Root == null) return -1;

        int sumSumHeights = 0;

        SumAllSumAllSubTreeHeights2(Root, ref sumSumHeights);

        return sumSumHeights;
    }

    private void SumAllSumAllSubTreeHeights2(Node parent, ref int sumSumHeights)
    {
        if (parent != null)
        {
            sumSumHeights += SumAllSubTreeHeights2(parent);

            SumAllSumAllSubTreeHeights2(parent.Left, ref sumSumHeights);
            SumAllSumAllSubTreeHeights2(parent.Right, ref sumSumHeights);
        }
    }

    public int GetNodeDepth(T val)
    {
        if (!Find(val)) return -1;

        return GetNodeDepth(val, Root);
    }

    private int GetNodeDepth(T val, Node parent, int depth = 0)
    {
        if (parent != null && val.CompareTo(parent.Value) != 0)
        {
            depth++;

            if (val.CompareTo(parent.Value) < 0)
                depth = GetNodeDepth(val, parent.Left, depth);
            else
                depth = GetNodeDepth(val, parent.Right, depth);
        }

        return depth;
    }

    private int GetNodeDepth(Node target)
    {
        if (!Find(target)) return -1;

        return GetNodeDepth(target, Root);
    }

    private int GetNodeDepth(Node target, Node parent, int depth = 0)
    {
        if (parent != null && target != parent)
        {
            depth++;

            if (target.Value.CompareTo(parent.Value) < 0)
                depth = GetNodeDepth(target, parent.Left, depth);
            else
                depth = GetNodeDepth(target, parent.Right, depth);
        }

        return depth;
    }

    private int GetNodeDepth2(Node target)
    {
        if (!Find(target)) return -1;

        return GetNodeDepth2(target, Root);
    }

    private int GetNodeDepth2(Node target, Node parent)
    {
        int depth = 0;

        GetNodeDepth2(target, parent, ref depth);

        return depth;
    }

    private void GetNodeDepth2(Node target, Node parent, ref int depth)
    {
        if (target != parent)
        {
            depth++;

            if (target.Value.CompareTo(parent.Value) < 0)
                GetNodeDepth2(target, parent.Left, ref depth);
            else if (target.Value.CompareTo(parent.Value) >= 0)
                GetNodeDepth2(target, parent.Right, ref depth);
        }
    }

    public int SumAllNodeDepths()
    {
        if (Root == null) return -1;

        return SumAllNodeDepths(Root);
    }

    private int SumAllNodeDepths(Node parent)
    {
        return SumAllNodeDepths(parent, parent);
    }

    private int SumAllNodeDepths(Node target, Node parent, int sumDepths = 0)
    {
        if (parent != null)
        {
            sumDepths += GetNodeDepth(parent, target);

            sumDepths = SumAllNodeDepths(target, parent.Left, sumDepths);
            sumDepths = SumAllNodeDepths(target, parent.Right, sumDepths);
        }

        return sumDepths;
    }

    private int SumAllNodeDepths2(Node parent)
    {
        return SumAllNodeDepths2(parent, parent);
    }

    private int SumAllNodeDepths2(Node target, Node parent)
    {
        int sumDepths = 0;

        SumAllNodeDepths2(target, parent, ref sumDepths);

        return sumDepths;
    }

    private void SumAllNodeDepths2(Node target, Node parent, ref int sumDepths)
    {
        if (parent != null)
        {
            sumDepths += GetNodeDepth2(parent, target);

            SumAllNodeDepths2(target, parent.Left, ref sumDepths);
            SumAllNodeDepths2(target, parent.Right, ref sumDepths);
        }
    }

    public int SumAllSumAllNodeDepths()
    {
        if (Root == null) return -1;

        return SumAllSumAllNodeDepths(Root);
    }

    private int SumAllSumAllNodeDepths(Node parent, int sumSumDepths = 0)
    {
        if (parent != null)
        {
            sumSumDepths += SumAllNodeDepths(parent);

            sumSumDepths = SumAllSumAllNodeDepths(parent.Left, sumSumDepths);
            sumSumDepths = SumAllSumAllNodeDepths(parent.Right, sumSumDepths);
        }

        return sumSumDepths;
    }

    public int SumAllSumAllNodeDepths2()
    {
        if (Root == null) return -1;

        int sumSumDepths = 0;

        SumAllSumAllNodeDepths2(Root, ref sumSumDepths);

        return sumSumDepths;
    }

    private void SumAllSumAllNodeDepths2(Node parent, ref int sumSumDepths)
    {
        if (parent != null)
        {
            sumSumDepths += SumAllNodeDepths2(parent);

            SumAllSumAllNodeDepths2(parent.Left, ref sumSumDepths);
            SumAllSumAllNodeDepths2(parent.Right, ref sumSumDepths);
        }
    }

    public void NonDecreasingOrderTraversal_DFS_Recursion()
    {
        NonDecreasingOrderTraversal_DFS_Recursion(Root);
    }

    private void NonDecreasingOrderTraversal_DFS_Recursion(Node parent)
    {
        if (parent != null)
        {
            NonDecreasingOrderTraversal_DFS_Recursion(parent.Left);

            PrintNode(parent);

            NonDecreasingOrderTraversal_DFS_Recursion(parent.Right);
        }
    }

    public void NonDecreasingOrderTraversal_DFS_Iteration_Stack()
    {
        if (Root == null) return;

        Stack<Node> stack = new Stack<Node>();

        stack.Push(Root);

        Node parent;

        do
        {
            parent = stack.Peek();

            do
            {
                parent = parent.Left;

                if (parent != null)
                    stack.Push(parent);

            } while (parent != null);

            do
            {
                parent = stack.Pop();

                PrintNode(parent);

                parent = parent.Right;

                if (parent != null)
                {
                    stack.Push(parent);

                    break;
                }

            } while (stack.Count > 0);

        } while (stack.Count > 0);
    }

    public void PreOrderTraversal_DFS_Recursion()
    {
        PreOrderTraversal_DFS_Recursion(Root);
    }

    private void PreOrderTraversal_DFS_Recursion(Node parent)
    {
        if (parent != null)
        {
            PrintNode(parent);

            PreOrderTraversal_DFS_Recursion(parent.Left);

            PreOrderTraversal_DFS_Recursion(parent.Right);
        }
    }

    public void PreOrderTraversal_DFS_Iteration_Stack()
    {
        if (Root == null) return;

        Stack<Node> stack = new Stack<Node>();

        stack.Push(Root);

        Node parent;

        do
        {
            parent = stack.Peek();

            do
            {
                PrintNode(parent);

                parent = parent.Left;

                if (parent != null)
                    stack.Push(parent);

            } while (parent != null);

            do
            {
                parent = stack.Pop();

                parent = parent.Right;

                if (parent != null)
                {
                    stack.Push(parent);

                    break;
                }

            } while (stack.Count > 0);

        } while (stack.Count > 0);
    }

    public void PostOrderTraversal_DFS_Recursion()
    {
        PostOrderTraversal_DFS_Recursion(Root);
    }

    private void PostOrderTraversal_DFS_Recursion(Node parent)
    {
        if (parent != null)
        {
            PostOrderTraversal_DFS_Recursion(parent.Left);

            PostOrderTraversal_DFS_Recursion(parent.Right);

            PrintNode(parent);
        }
    }

    public void PostOrderTraversal_DFS_Iteration_Stack()
    {
        if (Root == null) return;

        Stack<Node> stack = new Stack<Node>();
        Stack<Node> popped = new Stack<Node>();

        stack.Push(Root);

        Node parent;

        do
        {
            parent = stack.Peek();

            do
            {
                parent = parent.Left;

                if (parent != null)
                    stack.Push(parent);

            } while (parent != null);

            do
            {
                parent = stack.Peek();

                if (parent.Right != null && !popped.Contains(parent.Right))
                {
                    stack.Push(parent.Right);

                    break;
                }

                stack.Pop();

                popped.Push(parent);

                PrintNode(parent);

            } while (stack.Count > 0);

        } while (stack.Count > 0);
    }

    public void NonIncreasingOrderTraversal_DFS_Recursion()
    {
        NonIncreasingOrderTraversal_DFS_Recursion(Root);
    }

    private void NonIncreasingOrderTraversal_DFS_Recursion(Node parent)
    {
        if (parent != null)
        {
            NonIncreasingOrderTraversal_DFS_Recursion(parent.Right);

            PrintNode(parent);

            NonIncreasingOrderTraversal_DFS_Recursion(parent.Left);
        }
    }

    public void NonIncreasingOrderTraversal_DFS_Iteration_Stack()
    {
        if (Root == null) return;

        Stack<Node> stack = new Stack<Node>();

        stack.Push(Root);

        Node parent;

        do
        {
            parent = stack.Peek();

            do
            {
                parent = parent.Right;

                if (parent != null)
                    stack.Push(parent);

            } while (parent != null);

            do
            {
                parent = stack.Pop();

                PrintNode(parent);

                parent = parent.Left;

                if (parent != null)
                {
                    stack.Push(parent);

                    break;
                }

            } while (stack.Count > 0);

        } while (stack.Count > 0);
    }

    public void LevelOrderTraversal_BFS_Recursion()
    {
        LevelOrderTraversal_BFS_Recursion(Root);
    }

    private void LevelOrderTraversal_BFS_Recursion(Node parent, int target_level = 0, int current_level = 0, int stack_trace = 0)
    {
        stack_trace++;

        if (parent != null)
        {
            if (current_level == target_level)
                PrintNode(parent);
            else if (current_level++ < target_level)
            {
                LevelOrderTraversal_BFS_Recursion(parent.Left, target_level, current_level, stack_trace);
                LevelOrderTraversal_BFS_Recursion(parent.Right, target_level, current_level, stack_trace);
            }

            if (stack_trace == 1 && target_level++ < GetTreeHeight())
            {
                Console.WriteLine();

                LevelOrderTraversal_BFS_Recursion(parent, target_level);
            }
        }
    }

    public void LevelOrderTraversal_BFS_Recursion2()
    {
        for (int i = 0; i <= GetTreeHeight(); i++)
        {
            LevelOrderTraversal_BFS_Recursion2(Root, 0, i);

            Console.WriteLine();
        }
    }

    private void LevelOrderTraversal_BFS_Recursion2(Node parent, int distance, int depth)
    {
        if (parent != null)
        {
            if (distance == depth)
                PrintNode(parent);
            else if (distance++ < depth)
            {
                LevelOrderTraversal_BFS_Recursion2(parent.Left, distance, depth);
                LevelOrderTraversal_BFS_Recursion2(parent.Right, distance, depth);
            }
        }
    }

    public void LevelOrderTraversal_BFS_Iteration_Queue()
    {
        if (Root == null) return;

        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(Root);

        Node parent;

        int currentLevel = -1;
        int parentLevel;

        do
        {
            parent = queue.Dequeue();

            parentLevel = GetNodeDepth(parent);

            if (currentLevel != parentLevel)
            {
                currentLevel = parentLevel;

                if (currentLevel != 0)
                    Console.WriteLine();
            }

            PrintNode(parent);

            if (parent.Left != null)
                queue.Enqueue(parent.Left);

            if (parent.Right != null)
                queue.Enqueue(parent.Right);

        } while (queue.Count > 0);
    }

    public void InvertedLevelOrderTraversal_BFS_Recursion()
    {
        InvertedLevelOrderTraversal_BFS_Recursion(Root);
    }

    private void InvertedLevelOrderTraversal_BFS_Recursion(Node parent, int target_level = 0, int current_level = 0, int stack_trace = 0)
    {
        stack_trace++;

        if (parent != null)
        {
            if (current_level == target_level)
                PrintNode(parent);
            else if (current_level++ < target_level)
            {
                InvertedLevelOrderTraversal_BFS_Recursion(parent.Right, target_level, current_level, stack_trace);
                InvertedLevelOrderTraversal_BFS_Recursion(parent.Left, target_level, current_level, stack_trace);
            }

            if (stack_trace == 1 && target_level++ < GetTreeHeight())
            {
                Console.WriteLine();

                InvertedLevelOrderTraversal_BFS_Recursion(parent, target_level);
            }
        }
    }

    public void InvertedLevelOrderTraversal_BFS_Iteration_Queue()
    {
        if (Root == null) return;

        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(Root);

        Node parent;

        int currentLevel = -1;
        int parentLevel;

        do
        {
            parent = queue.Dequeue();

            parentLevel = GetNodeDepth(parent);

            if (currentLevel != parentLevel)
            {
                currentLevel = parentLevel;

                if (currentLevel != 0)
                    Console.WriteLine();
            }

            PrintNode(parent);

            if (parent.Right != null)
                queue.Enqueue(parent.Right);

            if (parent.Left != null)
                queue.Enqueue(parent.Left);

        } while (queue.Count > 0);
    }

    public void ReverseLevelOrderTraversal_BFS_Recursion()
    {
        ReverseLevelOrderTraversal_BFS_Recursion(Root, GetTreeHeight());
    }

    private void ReverseLevelOrderTraversal_BFS_Recursion(Node parent, int target_level, int current_level = 0, int stack_trace = 0)
    {
        stack_trace++;

        if (parent != null)
        {
            if (current_level == target_level)
                PrintNode(parent);
            else if (current_level++ < target_level)
            {
                ReverseLevelOrderTraversal_BFS_Recursion(parent.Left, target_level, current_level, stack_trace);
                ReverseLevelOrderTraversal_BFS_Recursion(parent.Right, target_level, current_level, stack_trace);
            }

            if (stack_trace == 1 && target_level-- > 0)
            {
                Console.WriteLine();

                ReverseLevelOrderTraversal_BFS_Recursion(parent, target_level);
            }
        }
    }

    public void ReverseLevelOrderTraversal_BFS_Iteration_QueueStack()
    {
        if (Root == null) return;

        Queue<Node> queue = new Queue<Node>();
        Stack<Node> stack = new Stack<Node>();

        queue.Enqueue(Root);

        Node parent;

        int treeHeight = GetTreeHeight();
        int currentLevel = treeHeight;
        int parentLevel;

        do
        {
            parent = queue.Dequeue();

            stack.Push(parent);

            if (parent.Right != null)
                queue.Enqueue(parent.Right);

            if (parent.Left != null)
                queue.Enqueue(parent.Left);

        } while (queue.Count > 0);

        while (stack.Count > 0)
        {
            parent = stack.Pop();

            parentLevel = GetNodeDepth(parent);

            if (currentLevel != parentLevel)
            {
                currentLevel = parentLevel;

                if (currentLevel != treeHeight)
                    Console.WriteLine();
            }

            PrintNode(parent);
        }
    }

    public void ReverseInvertedLevelOrderTraversal_BFS_Recursion()
    {
        ReverseInvertedLevelOrderTraversal_BFS_Recursion(Root, GetTreeHeight());
    }

    private void ReverseInvertedLevelOrderTraversal_BFS_Recursion(Node parent, int target_level, int current_level = 0, int stack_trace = 0)
    {
        stack_trace++;

        if (parent != null)
        {
            if (current_level == target_level)
                PrintNode(parent);
            else if (current_level++ < target_level)
            {
                ReverseInvertedLevelOrderTraversal_BFS_Recursion(parent.Right, target_level, current_level, stack_trace);
                ReverseInvertedLevelOrderTraversal_BFS_Recursion(parent.Left, target_level, current_level, stack_trace);
            }

            if (stack_trace == 1 && target_level-- > 0)
            {
                Console.WriteLine();

                ReverseInvertedLevelOrderTraversal_BFS_Recursion(parent, target_level);
            }
        }
    }

    public void ReverseInvertedLevelOrderTraversal_BFS_Iteration_QueueStack()
    {
        if (Root == null) return;

        Queue<Node> queue = new Queue<Node>();
        Stack<Node> stack = new Stack<Node>();

        queue.Enqueue(Root);

        Node parent;

        int treeHeight = GetTreeHeight();
        int currentLevel = treeHeight;
        int parentLevel;

        do
        {
            parent = queue.Dequeue();

            stack.Push(parent);

            if (parent.Left != null)
                queue.Enqueue(parent.Left);

            if (parent.Right != null)
                queue.Enqueue(parent.Right);

        } while (queue.Count > 0);

        while (stack.Count > 0)
        {
            parent = stack.Pop();

            parentLevel = GetNodeDepth(parent);

            if (currentLevel != parentLevel)
            {
                currentLevel = parentLevel;

                if (currentLevel != treeHeight)
                    Console.WriteLine();
            }

            PrintNode(parent);
        }
    }

    public Node CopyTree_DFS_Recursion()
    {
        return CopyTree_DFS_Recursion(Root);
    }

    private Node CopyTree_DFS_Recursion(Node parent, Node copy = null)
    {
        if (parent == null) return parent;

        copy = new Node(parent.Value);
        copy.Frequency = parent.Frequency;

        copy.Left = CopyTree_DFS_Recursion(parent.Left, copy.Left);
        copy.Right = CopyTree_DFS_Recursion(parent.Right, copy.Right);

        return copy;
    }

    public void DeleteTree_DFS_Recursion()
    {
        Root = DeleteTree_DFS_Recursion(Root);
    }

    private Node DeleteTree_DFS_Recursion(Node parent)
    {
        if (parent == null) return parent;

        parent.Left = DeleteTree_DFS_Recursion(parent.Left);
        parent.Right = DeleteTree_DFS_Recursion(parent.Right);

        parent = null;

        return parent;
    }

    public void BalanceTree_BST_Recursion()
    {
        Root = BalanceTree_BST_Recursion(Root);
    }

    private Node BalanceTree_BST_Recursion(Node parent)
    {
        if (parent == null) return null;

        parent.Left = BalanceTree_BST_Recursion(parent.Left);
        parent.Right = BalanceTree_BST_Recursion(parent.Right);

        int leftHeight = GetSubTreeHeight(parent.Left);
        int rightHeight = GetSubTreeHeight(parent.Right);

        if (leftHeight > (rightHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Left.Left);
            rightHeight = GetSubTreeHeight(parent.Left.Right);

            if (leftHeight >= rightHeight)
            {
                parent = RotateSubTreeRight(parent);
            }
            else
            {
                parent.Left = RotateSubTreeLeft(parent.Left);

                parent = RotateSubTreeRight(parent);
            }

            parent = BalanceTree_BST_Recursion(parent);
        }
        else if (rightHeight > (leftHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Right.Left);
            rightHeight = GetSubTreeHeight(parent.Right.Right);

            if (rightHeight >= leftHeight)
            {
                parent = RotateSubTreeLeft(parent);
            }
            else
            {
                parent.Right = RotateSubTreeRight(parent.Right);

                parent = RotateSubTreeLeft(parent);
            }

            parent = BalanceTree_BST_Recursion(parent);
        }

        return parent;
    }

    public void BalanceTree_BST_Recursion2()
    {
        Root = BalanceTree_BST_Recursion2(Root);
    }

    public Node BalanceTree_BST_Recursion2(Node parent)
    {
        if (parent == null) return parent;

        parent.Left = BalanceTree_BST_Recursion2(parent.Left);
        parent.Right = BalanceTree_BST_Recursion2(parent.Right);

        int leftHeight = GetSubTreeHeight(parent.Left);
        int rightHeight = GetSubTreeHeight(parent.Right);

        if (leftHeight > (rightHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Left.Left);
            rightHeight = GetSubTreeHeight(parent.Left.Right);

            if (leftHeight >= rightHeight)
            {
                parent = RotateSubTreeRight(parent);
            }
            else
            {
                parent.Left = RotateSubTreeLeft(parent.Left);

                parent = RotateSubTreeRight(parent);
            }
        }
        else if (rightHeight > (leftHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Right.Left);
            rightHeight = GetSubTreeHeight(parent.Right.Right);

            if (rightHeight >= leftHeight)
            {
                parent = RotateSubTreeLeft(parent);
            }
            else
            {
                parent.Right = RotateSubTreeRight(parent.Right);

                parent = RotateSubTreeLeft(parent);
            }
        }

        return parent;
    }

    public void BalanceTree_BST_Recursion3()
    {
        Root = BalanceTree_BST_Recursion3(Root);
    }

    public Node BalanceTree_BST_Recursion3(Node parent)
    {
        if (parent == null) return parent;

        int leftHeight = GetSubTreeHeight(parent.Left);
        int rightHeight = GetSubTreeHeight(parent.Right);

        if (leftHeight > (rightHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Left.Left);
            rightHeight = GetSubTreeHeight(parent.Left.Right);

            if (leftHeight >= rightHeight)
            {
                parent = RotateSubTreeRight(parent);
            }
            else
            {
                parent.Left = RotateSubTreeLeft(parent.Left);

                parent = RotateSubTreeRight(parent);
            }

            parent = BalanceTree_BST_Recursion3(parent);
        }
        else if (rightHeight > (leftHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Right.Left);
            rightHeight = GetSubTreeHeight(parent.Right.Right);

            if (rightHeight >= leftHeight)
            {
                parent = RotateSubTreeLeft(parent);
            }
            else
            {
                parent.Right = RotateSubTreeRight(parent.Right);

                parent = RotateSubTreeLeft(parent);
            }

            parent = BalanceTree_BST_Recursion3(parent);
        }

        return parent;
    }

    public Node BalanceTree_AVL(Node parent)
    {
        if (parent == null) return parent;

        int leftHeight = GetSubTreeHeight(parent.Left);
        int rightHeight = GetSubTreeHeight(parent.Right);

        if (leftHeight > (rightHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Left.Left);
            rightHeight = GetSubTreeHeight(parent.Left.Right);

            if (leftHeight >= rightHeight)
            {
                parent = RotateSubTreeRight(parent);
            }
            else
            {
                parent.Left = RotateSubTreeLeft(parent.Left);

                parent = RotateSubTreeRight(parent);
            }
        }
        else if (rightHeight > (leftHeight + 1))
        {
            leftHeight = GetSubTreeHeight(parent.Right.Left);
            rightHeight = GetSubTreeHeight(parent.Right.Right);

            if (rightHeight >= leftHeight)
            {
                parent = RotateSubTreeLeft(parent);
            }
            else
            {
                parent.Right = RotateSubTreeRight(parent.Right);

                parent = RotateSubTreeLeft(parent);
            }
        }

        return parent;
    }

    private Node RotateSubTreeRight(Node parent)
    {
        if (parent == null) return parent;

        if (parent.Left != null)
        {
            Node temp1 = parent;
            Node temp2 = parent.Left.Right;
            parent = parent.Left;
            parent.Right = temp1;
            parent.Right.Left = temp2;
        }

        return parent;
    }

    private Node RotateSubTreeLeft(Node parent)
    {
        if (parent == null) return parent;

        if (parent.Right != null)
        {
            Node temp1 = parent.Right;
            Node temp2= parent.Right.Left;
            parent.Right.Left = parent;
            parent.Right = temp2;
            parent = temp1;
        }

        return parent;
    }

    public List<T> ToSortedList()
    {
        return ToSortedList(Root);
    }

    private List<T> ToSortedList(Node parent, List<T> list = null)
    {
        if (parent == null) return list;
        
        list = ToSortedList(parent.Left, list);

        if (list == null)
            list = new List<T>();

        for (int i = 1; i <= parent.Frequency; i++)
            list.Add(parent.Value);

        list = ToSortedList(parent.Right, list);

        return list;
    }

    public (Node, Node) ToSinglyLinkedList()
    {
        Node head = null;

        Node tail = ToSinglyLinkedList(Root, ref head);

        return (head, tail);
    }

    private Node ToSinglyLinkedList(Node parent, ref Node head, Node tail = null)
    {
        if (parent == null) return tail;

        tail = ToSinglyLinkedList(parent.Left, ref head, tail);

        if (tail == null)
        {
            tail = new Node(parent.Value);
            tail.Frequency = parent.Frequency;
            head = tail;
        }
        else
        {
            tail.Right = new Node(parent.Value);
            tail.Right.Frequency = parent.Frequency;
            tail = tail.Right;
        }
        
        tail = ToSinglyLinkedList(parent.Right, ref head, tail);

        return tail;
    }

    public (Node, Node) ToDoublyLinkedList()
    {
        Node head = null;

        Node tail = ToDoublyLinkedList(Root, ref head);

        return (head, tail);
    }

    private Node ToDoublyLinkedList(Node parent, ref Node head, Node tail = null)
    {
        if (parent == null) return tail;

        tail = ToDoublyLinkedList(parent.Left, ref head, tail);

        if (tail == null)
        {
            tail = new Node(parent.Value);
            tail.Frequency = parent.Frequency;
            head = tail;
        }
        else
        {
            Node previous = tail;
            tail.Right = new Node(parent.Value);
            tail.Right.Frequency = parent.Frequency;
            tail = tail.Right;
            tail.Left = previous;
        }

        tail = ToDoublyLinkedList(parent.Right, ref head, tail);

        return tail;
    }

    public (Node, Node) ToCircularSinglyLinkedList()
    {
        (Node, Node) nodes = ToSinglyLinkedList();

        nodes.Item2.Right = nodes.Item1;

        return (nodes.Item1, nodes.Item2);
    }

    public (Node, Node) ToCircularDoublyLinkedList()
    {
        (Node, Node) nodes = ToDoublyLinkedList();

        nodes.Item2.Right = nodes.Item1;
        nodes.Item1.Left = nodes.Item2;

        return (nodes.Item1, nodes.Item2);
    }

    public SelfBalancingTree<T> ToBalancedBSTree(List<T> list)
    {
        return ToBalancedBSTree(list, 0, list.Count - 1);
    }

    private SelfBalancingTree<T> ToBalancedBSTree(List<T> list, int lIndex, int rIndex, SelfBalancingTree<T> bst = null)
    {
        if (lIndex > rIndex) return bst;

        int midpoint = ((lIndex + rIndex) / 2) + ((lIndex + rIndex) % 2);

        if (bst == null)
            bst = new SelfBalancingTree<T>();

        bst.Add(list[midpoint]);

        bst = ToBalancedBSTree(list, lIndex, midpoint - 1, bst);
        bst = ToBalancedBSTree(list, midpoint + 1, rIndex, bst);

        return bst;
    }

    public Node ToBalancedBSTree2(List<T> list)
    {
        return ToBalancedBSTree2(list, 0, list.Count - 1);
    }

    private Node ToBalancedBSTree2(List<T> list, int lIndex, int rIndex, Node root = null)
    {
        if (lIndex > rIndex) return root;

        int midpoint = (lIndex + rIndex) / 2;

        root = new Node(list[midpoint]);

        root.Left = ToBalancedBSTree2(list, lIndex, midpoint - 1, root.Left);
        root.Right = ToBalancedBSTree2(list, midpoint + 1, rIndex, root.Right);

        return root;
    }

    public Node InvertTree_DFS_Recursion()
    {
        return InvertTree_DFS_Recursion(Root);
    }

    private Node InvertTree_DFS_Recursion(Node parent)
    {
        if (parent == null) return parent;

        Node left = InvertTree_DFS_Recursion(parent.Left);
        Node right = InvertTree_DFS_Recursion(parent.Right);

        parent.Left = right;
        parent.Right = left;

        return parent;
    }

    private Node InvertTree_BFS_Iteration_Queue()
    {
        if (Root == null) return Root;

        Node parent = Root;

        Node temp = parent.Left;
        parent.Left = parent.Right;
        parent.Right = temp;

        return parent;
    }

    public void PrintNode(Node node, string delimeter = " ")
    {
        if (node == null) return;

        Console.Write(node.Value.ToString());
        if (node.Frequency > 1) Console.Write("(" + node.Frequency + ")");
        Console.Write(delimeter);
    }

    public void PrintTree()
    {
        PrintTree(Root);
    }

    private void PrintTree(Node parent, int space = 0, int count = 5)
    {
        if (parent == null) return;

        space += count;

        PrintTree(parent.Right, space);

        Console.WriteLine();

        for (int i = count; i < space; i++)
            Console.Write(" ");

        PrintNode(parent, "\n");

        PrintTree(parent.Left, space);
    }
}

public class SelfBalancingTree
{
    public static void Main(String[] args)
    {
        SelfBalancingTree<int> tree = new SelfBalancingTree<int>();
        SelfBalancingTree<int> tree1 = new SelfBalancingTree<int>();
        SelfBalancingTree<int> tree2 = new SelfBalancingTree<int>();

        Queue<int> queue = new Queue<int>();

        while (true)
        {
            Console.WriteLine("Creating new Unbalanced BSTree and AVL Tree and inserting the same integer values into each tree...");

            Console.WriteLine();

            for (int i = 1; i < 96; i++)
            {
                int n = new Random().Next(1, 512);

                if (!tree1.Find(n))
                {
                    tree1.Add(n);
                    Console.Write(n + " ");

                    queue.Enqueue(n);
                }
                else
                    i--;
            }

            Console.WriteLine();

            Console.WriteLine();

            foreach (int n in queue)
            {
                tree2.InsertAVL(n);
                //Console.Write(n + " ");
            }

            //Console.WriteLine();

            //Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Unbalanced BSTree");

            Console.WriteLine();

            Console.Write("Tree Size:  ");
            int size = tree1.Size();
            Console.WriteLine(size);

            Console.WriteLine();

            Console.Write("Tree Height:  ");
            int height = tree1.GetTreeHeight();
            Console.WriteLine(height);

            Console.Write("Sum of all sub-tree heights:  ");
            int sumHeights = tree1.SumAllSubTreeHeights();
            Console.WriteLine(sumHeights);

            Console.Write("Sum of all sums of all sub-tree heights:  ");
            int sumSumHeights = tree1.SumAllSumAllSubTreeHeights();
            Console.WriteLine(sumSumHeights);

            Console.WriteLine();

            Console.Write("Sum of all node depths:  ");
            int sumDepths = tree1.SumAllNodeDepths();
            Console.WriteLine(sumDepths);

            Console.Write("Sum of all sums of all node depths:  ");
            int sumSumDepths = tree1.SumAllSumAllNodeDepths();
            Console.WriteLine(sumSumDepths);

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Recursion):");
            tree1.LevelOrderTraversal_BFS_Recursion();
            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Iteration using a Queue):");
            tree1.LevelOrderTraversal_BFS_Iteration_Queue();
            Console.WriteLine();

            tree1.PrintTree();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            //Console.WriteLine("Copying BST Tree... (Recursion)");
            SelfBalancingTree<int> tree1Copy = new SelfBalancingTree<int>();
            tree1Copy.Root = tree1.CopyTree_DFS_Recursion();
            //Console.WriteLine();

            //Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine("Balancing BSTree... (Recursion)");
            tree1.BalanceTree_BST_Recursion();
            Console.WriteLine();

            Console.WriteLine("Balanced BSTree");

            Console.WriteLine();

            Console.Write("Tree Size:  ");
            size = tree1.Size();
            Console.WriteLine(size);

            Console.WriteLine();

            Console.Write("Tree Height:  ");
            height = tree1.GetTreeHeight();
            Console.WriteLine(height);

            Console.Write("Sum of all sub-tree heights:  ");
            sumHeights = tree1.SumAllSubTreeHeights();
            Console.WriteLine(sumHeights);

            Console.Write("Sum of all sums of all sub-tree heights:  ");
            sumSumHeights = tree1.SumAllSumAllSubTreeHeights();
            Console.WriteLine(sumSumHeights);

            Console.WriteLine();

            Console.Write("Sum of all node depths:  ");
            sumDepths = tree1.SumAllNodeDepths();
            Console.WriteLine(sumDepths);

            Console.Write("Sum of all sums of all node depths:  ");
            sumSumDepths = tree1.SumAllSumAllNodeDepths();
            Console.WriteLine(sumSumDepths);

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Recursion):");
            tree1.LevelOrderTraversal_BFS_Recursion();
            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Iteration using a Queue):");
            tree1.LevelOrderTraversal_BFS_Iteration_Queue();
            Console.WriteLine();

            tree1.PrintTree();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("AVL Tree");

            Console.WriteLine();

            Console.Write("Tree Size:  ");
            size = tree2.Size();
            Console.WriteLine(size);

            Console.WriteLine();

            Console.Write("Tree Height:  ");
            height = tree2.GetTreeHeight();
            Console.WriteLine(height);

            Console.Write("Sum of all sub-tree heights:  ");
            sumHeights = tree2.SumAllSubTreeHeights();
            Console.WriteLine(sumHeights);

            Console.Write("Sum of all sums of all sub-tree heights:  ");
            sumSumHeights = tree2.SumAllSumAllSubTreeHeights();
            Console.WriteLine(sumSumHeights);

            Console.WriteLine();

            Console.Write("Sum of all node depths:  ");
            sumDepths = tree2.SumAllNodeDepths();
            Console.WriteLine(sumDepths);

            Console.Write("Sum of all sums of all node depths:  ");
            sumSumDepths = tree2.SumAllSumAllNodeDepths();
            Console.WriteLine(sumSumDepths);

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Recursion):");
            tree2.LevelOrderTraversal_BFS_Recursion();
            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Iteration using a Queue):");
            tree2.LevelOrderTraversal_BFS_Iteration_Queue();
            Console.WriteLine();

            tree2.PrintTree();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            try
            {
                Console.Write("Enter integer value to remove from trees or press Enter to continue:  ");
                int num = Convert.ToInt32(Console.ReadLine());

                while (num != 0)
                {
                    Console.WriteLine();

                    Console.WriteLine("Removing Node with Value " + num.ToString() + " from Balanced BSTree... (Recursion)");
                    tree1.Remove(Convert.ToInt32(num));
                    Console.WriteLine();

                    Console.WriteLine("Removing Node with Value " + num.ToString() + " from AVL Tree... (Recursion)");
                    tree2.RemoveAVL(Convert.ToInt32(num));
                    Console.WriteLine();

                    Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Recursion):");
                    tree2.LevelOrderTraversal_BFS_Recursion();
                    Console.WriteLine();

                    tree2.PrintTree();

                    Console.WriteLine();

                    Console.WriteLine("---------------------------------------------------------------------------------------------");

                    Console.WriteLine();

                    Console.Write("Enter number to remove or press Enter to continue:  ");
                    num = Convert.ToInt32(Console.ReadLine());
                }
            }
            catch
            {

            }

            //Console.WriteLine("Copying 2nd Tree... (Recursion)");
            //SelfBalancingTree<int> tree2Copy = new SelfBalancingTree<int>();
            //tree2Copy.Root = tree2.CopyTree_DFS_Recursion();
            //Console.WriteLine();

            //Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Trees Size:  ");
            size = tree1.Size();
            Console.WriteLine(size);

            Console.WriteLine();

            Console.WriteLine("-------");

            Console.WriteLine();

            Console.WriteLine("Unbalanced BSTree:");

            Console.WriteLine();

            Console.Write("Tree Height:  ");
            height = tree1Copy.GetTreeHeight();
            Console.WriteLine(height);

            Console.Write("Sum of all sub-tree heights:  ");
            sumHeights = tree1Copy.SumAllSubTreeHeights();
            Console.WriteLine(sumHeights);

            Console.Write("Sum of all sums of all sub-tree heights:  ");
            sumSumHeights = tree1Copy.SumAllSumAllSubTreeHeights();
            Console.WriteLine(sumSumHeights);

            Console.WriteLine();

            Console.Write("Sum of all node depths:  ");
            sumDepths = tree1Copy.SumAllNodeDepths();
            Console.WriteLine(sumDepths);

            Console.Write("Sum of all sums of all node depths:  ");
            sumSumDepths = tree1Copy.SumAllSumAllNodeDepths();
            Console.WriteLine(sumSumDepths);

            Console.WriteLine();

            Console.WriteLine("-------");

            Console.WriteLine();

            Console.WriteLine("Balanced BSTree:  (top-down balance using recursion...this happens once after the tree has been fully generated)");

            Console.WriteLine();

            Console.Write("Tree Height:  ");
            height = tree1.GetTreeHeight();
            Console.WriteLine(height);

            Console.Write("Sum of all sub-tree heights:  ");
            sumHeights = tree1.SumAllSubTreeHeights();
            Console.WriteLine(sumHeights);

            Console.Write("Sum of all sums of all sub-tree heights:  ");
            sumSumHeights = tree1.SumAllSumAllSubTreeHeights();
            Console.WriteLine(sumSumHeights);

            Console.WriteLine();

            Console.Write("Sum of all node depths:  ");
            sumDepths = tree1.SumAllNodeDepths();
            Console.WriteLine(sumDepths);

            Console.Write("Sum of all sums of all node depths:  ");
            sumSumDepths = tree1.SumAllSumAllNodeDepths();
            Console.WriteLine(sumSumDepths);

            Console.WriteLine();

            Console.WriteLine("-------");

            Console.WriteLine();

            Console.WriteLine("AVL Tree:  (bottom-up balance using recursion...this happens each time a new Node is inserted)");

            Console.WriteLine();

            Console.Write("Tree Height:  ");
            height = tree2.GetTreeHeight();
            Console.WriteLine(height);

            Console.Write("Sum of all sub-tree heights:  ");
            sumHeights = tree2.SumAllSubTreeHeights();
            Console.WriteLine(sumHeights);

            Console.Write("Sum of all sums of all sub-tree heights:  ");
            sumSumHeights = tree2.SumAllSumAllSubTreeHeights();
            Console.WriteLine(sumSumHeights);

            Console.WriteLine();

            Console.Write("Sum of all node depths:  ");
            sumDepths = tree2.SumAllNodeDepths();
            Console.WriteLine(sumDepths);

            Console.Write("Sum of all sums of all node depths:  ");
            sumSumDepths = tree2.SumAllSumAllNodeDepths();
            Console.WriteLine(sumSumDepths);

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Converting Unbalanced BSTree to a Sorted List (Dynamic Array)... (Recursion)");
            List<int> list = tree1Copy.ToSortedList();
            Console.WriteLine();

            for (int i = 0; i < list.Count; i++)
                Console.Write(list[i] + " ");

            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Converting Sorted List to a Optimally Balanced BSTree... (Recursion)");
            SelfBalancingTree<int> tree3 = tree1Copy.ToBalancedBSTree(list);
            Console.WriteLine();

            Console.Write("Tree Size:  ");
            size = tree3.Size();
            Console.WriteLine(size);

            Console.WriteLine();

            Console.Write("Tree Height:  ");
            height = tree3.GetTreeHeight();
            Console.WriteLine(height);

            Console.Write("Sum of all sub-tree heights:  ");
            sumHeights = tree3.SumAllSubTreeHeights();
            Console.WriteLine(sumHeights);

            Console.Write("Sum of all sums of all sub-tree heights:  ");
            sumSumHeights = tree3.SumAllSumAllSubTreeHeights();
            Console.WriteLine(sumSumHeights);

            Console.WriteLine();

            Console.Write("Sum of all node depths:  ");
            sumDepths = tree3.SumAllNodeDepths();
            Console.WriteLine(sumDepths);

            Console.Write("Sum of all sums of all node depths:  ");
            sumSumDepths = tree3.SumAllSumAllNodeDepths();
            Console.WriteLine(sumSumDepths);

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Recursion):");
            tree3.LevelOrderTraversal_BFS_Recursion();
            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("Level-Order (Depth-Order) Traversal (BFS Iteration using a Queue):");
            tree3.LevelOrderTraversal_BFS_Iteration_Queue();
            Console.WriteLine();

            tree3.PrintTree();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Creating a Sorted Singly-Linked List from the Unbalaced BSTree... (Recursion)");
            (SelfBalancingTree<int>.Node, SelfBalancingTree<int>.Node) llNodes = tree1Copy.ToSinglyLinkedList();
            Console.WriteLine();

            SelfBalancingTree<int>.Node head1 = llNodes.Item1;
            SelfBalancingTree<int>.Node tail1 = llNodes.Item2;

            SelfBalancingTree<int>.Node head1Copy = head1;

            while (head1 != tail1)
            {
                Console.Write(head1.Value.ToString() + " ");

                head1 = head1.Right;
            }

            Console.Write(head1.Value.ToString() + " ");

            head1 = head1Copy;

            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Creating a Sorted Doubly-Linked List (\"Flattened Tree\") from the Unbalaced BSTree... (Recursion)");
            (SelfBalancingTree<int>.Node, SelfBalancingTree<int>.Node) dllNodes = tree1Copy.ToDoublyLinkedList();
            Console.WriteLine();

            SelfBalancingTree<int>.Node head2 = dllNodes.Item1;
            SelfBalancingTree<int>.Node tail2 = dllNodes.Item2;

            SelfBalancingTree<int>.Node head2Copy = head2;
            SelfBalancingTree<int>.Node tail2Copy = tail2;

            while (head2 != tail2)
            {
                Console.Write(head2.Value.ToString() + " ");

                head2 = head2.Right;
            }

            Console.Write(head2.Value.ToString() + " ");

            head2 = head2Copy;

            Console.WriteLine();

            Console.WriteLine();

            while (tail2 != head2)
            {
                Console.Write(tail2.Value.ToString() + " ");

                tail2 = tail2.Left;
            }

            Console.Write(tail2.Value.ToString() + " ");

            tail2 = tail2Copy;

            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Creating a Sorted Circular Singly-Linked List from the Unbalaced BSTree... (Recursion)");
            (SelfBalancingTree<int>.Node, SelfBalancingTree<int>.Node) cllNodes = tree1Copy.ToCircularSinglyLinkedList();
            Console.WriteLine();

            SelfBalancingTree<int>.Node head3 = cllNodes.Item1;
            SelfBalancingTree<int>.Node tail3 = cllNodes.Item2;

            while (head3 != tail3)
            {
                Console.Write(head3.Value.ToString() + " ");

                head3 = head3.Right;
            }

            Console.Write(head3.Value.ToString() + " ");

            head3 = head3.Right;

            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Creating a Sorted Circular Doubly-Linked List from the Unbalaced BSTree... (Recursion)");
            (SelfBalancingTree<int>.Node, SelfBalancingTree<int>.Node) cdllNodes = tree1Copy.ToCircularDoublyLinkedList();
            Console.WriteLine();

            SelfBalancingTree<int>.Node head4 = cdllNodes.Item1;
            SelfBalancingTree<int>.Node tail4 = cdllNodes.Item2;

            SelfBalancingTree<int>.Node head4Copy = head4;
            SelfBalancingTree<int>.Node tail4Copy = tail4;

            while (head4 != tail4)
            {
                Console.Write(head4.Value.ToString() + " ");

                head4 = head4.Right;
            }

            Console.Write(head4.Value.ToString() + " ");

            head4 = head4.Right;

            Console.WriteLine();

            Console.WriteLine();

            while (tail4 != head4)
            {
                Console.Write(tail4.Value.ToString() + " ");

                tail4 = tail4.Left;
            }

            Console.Write(tail4.Value.ToString() + " ");

            tail4 = tail4.Left;

            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.Write("Press Enter to create new randomly generated trees...");
            Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("-------------");

            Console.WriteLine("| NEW TREES |");

            Console.WriteLine("-------------");

            Console.WriteLine();

            tree1.DeleteTree_DFS_Recursion();

            tree2.DeleteTree_DFS_Recursion();

            tree2.DeleteTree_DFS_Recursion();

            tree1Copy.DeleteTree_DFS_Recursion();

            queue.Clear();

            //tree1Copy.DeleteTree_DFS_Recursion();

            //tree2Copy.DeleteTree_DFS_Recursion();
        }

        Console.ReadLine();
    }
}