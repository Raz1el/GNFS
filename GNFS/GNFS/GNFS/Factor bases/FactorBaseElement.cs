using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.GNFS.Factor_bases
{
    public class FactorbaseElement
    {
        public ulong Root { get; }
        public ulong Prime { get; }


        public FactorbaseElement(ulong root,ulong prime)
        {
            Root = root;
            Prime = prime;
        }


        public override bool Equals(object obj)
        {
            var element = obj as FactorbaseElement;
            if (element == null)
                return false;
            return element.Root == Root && element.Prime == Prime;
        }

        public override int GetHashCode()
        {
            return (Root.ToString() + Prime).GetHashCode();
        }
    }
}
