using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class Reject
    {
        protected Lump TLump { get; set; }
        public bool[,] RejectMap { get; set; }

        public Reject(Lump lump) { TLump = lump; }

        public void Populate(int sectorCount)
        {
            //The formula is (number of SECTORS ^ 2) / 8 -> ROUNDED UP.
            //int rejectSize = (sectorCount * sectorCount) / 8;

            //We have to add one more line as this keeps overflowing when set to sectorCount for Y. Don't know if this is right
            RejectMap = new bool[sectorCount + 1, sectorCount];

            BitArray bitArr = new BitArray(TLump.Data);
            int currentY = 0;
            int currentX = 0;

            for (var i = 0; i <= bitArr.Count - 1; i++)
            {
                RejectMap[currentY, currentX] = bitArr[i];
                currentX += 1;

                if ((i + 1) % sectorCount == 0)
                {
                    currentY += 1;
                    currentX = 0;
                }
            }
        }
    }
}
