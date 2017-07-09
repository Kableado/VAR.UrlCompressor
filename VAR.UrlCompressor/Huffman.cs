using System;
using System.Collections.Generic;
using System.Linq;

namespace VAR.UrlCompressor
{
    class HuffmanTree
    {
        private List<HuffmanTreeNode> nodes = new List<HuffmanTreeNode>();
        public HuffmanTreeNode Root { get; set; }
        private Dictionary<char, int> _frequencies = new Dictionary<char, int>();

        private const char EOD = (char)0xFFFF;

        public HuffmanTree(Dictionary<char, int> frequencies)
        {
            _frequencies = frequencies;
            _frequencies.Add(EOD, 1);
            BuildTree();
        }

        public HuffmanTree(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!_frequencies.ContainsKey(source[i]))
                {
                    _frequencies.Add(source[i], 0);
                }

                _frequencies[source[i]]++;
            }
            _frequencies.Add(EOD, 1);

            BuildTree();
        }

        private void BuildTree()
        {
            foreach (KeyValuePair<char, int> symbol in _frequencies)
            {
                nodes.Add(new HuffmanTreeNode() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                List<HuffmanTreeNode> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    HuffmanTreeNode parent = new HuffmanTreeNode()
                    {
                        Symbol = '*',
                        Frequency = orderedNodes[0].Frequency + orderedNodes[1].Frequency,
                        Left = orderedNodes[0],
                        Right = orderedNodes[1]
                    };

                    nodes.Remove(orderedNodes[0]);
                    nodes.Remove(orderedNodes[1]);
                    nodes.Add(parent);
                }

                Root = nodes.FirstOrDefault();
            }
        }

        public byte[] Encode(byte[] data)
        {
            byte[] scratch = new byte[data.Length * 2];
            int bitPosition = 0;
            var encodedSymbol = new List<bool>();

            for (int i = 0; i < data.Length; i++)
            {
                encodedSymbol.Clear();
                encodedSymbol = Root.Traverse((char)data[i], encodedSymbol);
                foreach(bool v in encodedSymbol)
                {
                    scratch.WriteBit(bitPosition, 0, v);
                    bitPosition++;
                }
            }
            encodedSymbol.Clear();
            encodedSymbol = Root.Traverse(EOD, encodedSymbol);
            foreach (bool v in encodedSymbol)
            {
                scratch.WriteBit(bitPosition, 0, v);
                bitPosition++;
            }
            int byteLenght = (int)Math.Ceiling((double)bitPosition / 8);
            byte[] compressedData = new byte[byteLenght];
            Array.Copy(scratch, compressedData, byteLenght);

            return compressedData;
        }

        public byte[] Decode(byte[] data)
        {
            HuffmanTreeNode current = Root;
            byte[] scratch = new byte[data.Length];
            int bitPosition = 0;
            int bytePosition = 0;

            int lenght = data.Length * 8;
            while (bitPosition < lenght)
            {
                bool bit = data.ReadBit(bitPosition, 0);
                bitPosition++;
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (current.IsLeaf())
                {
                    if (current.Symbol == EOD) { break; }
                    scratch = scratch.WriteByte(bytePosition, (byte)current.Symbol);
                    bytePosition++;
                    current = Root;
                }
            }

            byte[] decompressedData = new byte[bytePosition];
            Array.Copy(scratch, decompressedData, bytePosition);

            return decompressedData;
        }
    }

    class HuffmanTreeNode
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public HuffmanTreeNode Right { get; set; }
        public HuffmanTreeNode Left { get; set; }

        public bool IsLeaf()
        {
            return (Right == null && Left == null);
        }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            // Leaf
            if (IsLeaf())
            {
                if (symbol == Symbol)
                {
                    return data;
                }
                return null;
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Traverse(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Traverse(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
