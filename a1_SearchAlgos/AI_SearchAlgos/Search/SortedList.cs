using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    public class hSortedList<K,T> 
        where T : class
        where K : IComparable
    {
        private List<Tuple<K, T>> _list;
        
        public hSortedList()
        {
            //Instantiate with some size to avoid runtime slowdown. 
            this._list = new List<Tuple<K, T>>(1000);
        }

        public void Add(K Key, T Value)
        {
            Tuple<K,T> old;
            if ((old = Contains(Value)) != null)
            {
                if (old.Item1.CompareTo(Key) < 0)
                {
                    _list.Remove(old);
                }
                else
                {
                    return;
                }
            }

            //Either the element is not in the list, or we have removed the old copy with a greater
            //key value.
            if(_list.Count == 0)
            {
                _list.Add(new Tuple<K, T>(Key, Value));
            }
            else if(Key.CompareTo(_list[0].Item1) <= 0)
            {
                _list.Insert(0, new Tuple<K,T>(Key, Value));
            }
            else if (Key.CompareTo(_list[_list.Count - 1].Item1) > 0)
            {
                _list.Add(new Tuple<K,T>(Key, Value));
            }
            else
            {
                int min = 0;
                int max = _list.Count - 1;
                int mid = 0;
                K mid_val;
                int comp;

                while( min <= max)
                {
                    mid = min + ((max - min) / 2);
                    mid_val = _list[mid].Item1;


                    comp = Key.CompareTo(mid_val);
                    if(comp < 0)
                    {
                        max = mid - 1;
                    }
                    else if(comp > 0)
                    {
                        min = mid + 1;
                    }
                    else
                    {
                        //We have a same value. Walk back until we find the end of this section of the list. 
                        //because we handle the very front case above, we can make the assumption that we 
                        //not walk off the end of the list before we find the end of this set of equal keys. 
                        while (Key.CompareTo(_list[mid].Item1) == 0) mid--;
                        _list.Insert(mid+1, new Tuple<K, T>(Key, Value));
                        break;
                    }
               } 
            }
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public T Peek()
        {
            if(_list.Count > 0)
            {
                return _list[0].Item2;
            }
            else
            {
                return null;
            }
            
        }
        public T Pop()
        {
            T result = null;
            if(_list.Count> 0)
            {
                result = _list[0].Item2;
                _list.RemoveAt(0);
            }
                return result;
        }

        public void Clear()
        {
            _list.Clear();
        }

        public Tuple<K,T> Contains(T target)
        {
            if (_list.Exists(x => x.Item2 == target))
            {
                return _list.Find(x => x.Item2 == target);
            }
            else
                return null;
        }


    }
}
