using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace br.corp.bonus630.plugin.Repeater
{
    public enum ItemType
    {
        Code,
        Text,
        Enumerator,
        ImagePath
    }
        
    public class ItemTuple<T>
    {
        private int index;
        private T item;
        public int Index { get { return this.index; } }
        public T Item { get { return this.item; } }

        public ItemTuple(int index, T item)
        {
            this.index = index;
            this.item = item;
        }
        public override string ToString()
        {
            return item.ToString();

        }
    }
}
