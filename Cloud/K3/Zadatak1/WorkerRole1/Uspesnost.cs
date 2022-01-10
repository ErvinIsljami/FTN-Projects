using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using System.IO;

namespace WorkerRole1
{
    public class Uspesnost : IUspesnost
    {
        public void Obavestenje(Porudzbina p, bool uspesnost)
        {
            if (uspesnost)
            {
                var blob = BlobHelper.GetBlobReference($"porudzbina", "blobbb");
                using (MemoryStream memStream = new MemoryStream(5000))
                {
                    using (StreamWriter stream = new StreamWriter(memStream))
                    {
                        stream.Write($"{p.ToString()} {uspesnost}");
                        blob.UploadFromStream(memStream);
                    }
                }
                return;
            }

        }
    }
}
