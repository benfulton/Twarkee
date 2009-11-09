using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace Twarkee
{
    public class IconCache
    {
        Dictionary<string, Image> images;
        Queue<string> sequence;

        string lastRequest;

        public IconCache()
        {
            images = new Dictionary<string, Image>();
            sequence = new Queue<string>();
        }

        public void LoadInto(PictureBox picUser, string index, string url)
        {
            if (images.ContainsKey(index))
            {
                picUser.Image = images[index];
                sequence.Enqueue(index);
            }
            else
            {
                lastRequest = index;
                picUser.LoadAsync(url);
            }
        }

        public void FillLastRequest(Image image)
        {
            images[lastRequest] = image;
        }

        internal void Expire()
        {
            if (sequence.Count > 0)
                images.Remove(sequence.Dequeue());
        }
    }
}
