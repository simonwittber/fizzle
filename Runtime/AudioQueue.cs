using System.Runtime.CompilerServices;

namespace Fizzle
{
    public class AudioQueue
    {
        volatile int read, write, count;
        float[] buffer;

        public readonly int bufferSize;

        public void Clear()
        {
            read = write = count = 0;
        }

        public int AvailableToRead
        {
            get
            {
                return count;
            }
        }

        public int AvailableToWrite
        {
            get
            {
                return bufferSize - count;
            }
        }

        public AudioQueue(int bufferSize)
        {
            buffer = new float[bufferSize];
            this.bufferSize = bufferSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInto(float[] otherBuffer)
        {
            var length = otherBuffer.Length;
            length = length <= count ? length : count;
            for (var i = 0; i < length; i++)
            {
                otherBuffer[i] = buffer[read];
                read = (read + 1) % buffer.Length;
            }
            count -= length;
            return length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Dequeue(out float item)
        {
            if (read == write)
            {
                item = 0;
                return false;
            }
            item = buffer[read];
            if ((read + 1) >= buffer.Length)
                read = 0;
            else
                read = read + 1;
            count--;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Enqueue(float item)
        {
            int newidx;
            if ((write + 1) >= buffer.Length)
                newidx = 0;
            else
                newidx = write + 1;
            if (newidx == read)
                return false;
            count++;
            buffer[write] = item;
            write = newidx;
            return true;
        }

    }
}

