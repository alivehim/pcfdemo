using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityTools.Engine
{
    public class ObjectPool<T>
    {
        private const int MAX_SIZE = 25;
        private readonly ConcurrentQueue<T> objectPool;
        private int counter = 0;

        private static SemaphoreSlim semaphore;

        //public ObjectPool(int size)
        //{
        //    if (size <= 0)
        //    {
        //        throw new Exception
        //       ("The size of the object pool must be greater than zero");
        //    }
        //    objectPool = new Queue<T>();

           
        //}

        public ObjectPool(IEnumerable<T> res)
        {
            objectPool = new ConcurrentQueue<T>(res);

            semaphore = new SemaphoreSlim(res.Count(), res.Count());
        }


        public T Get()
        {
            semaphore.Wait();

            if (objectPool.Count > 0)
            {
                if(objectPool.TryDequeue(out T result))
                {

                    return result;
                }

            }

            return default(T);
        }

        public void Put(T item)
        {
            //if (counter < MAX_SIZE)
            //{
            //    counter++;
               
            //}
            //else
            //{
            //    counter--;
            //}

            objectPool.Enqueue(item);

            semaphore.Release();
        }
    }
}
