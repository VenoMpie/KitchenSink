using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom
{
    public class DoomPCSpeakerPlayer
    {
        private Double GetFrequency(byte sample)
        {
            switch (sample)
            {
                case 0:
                    return 0;
                case 1:
                    return 175.00;
                case 2:
                    return 180.02;
                case 3:
                    return 185.01;
                case 4:
                    return 190.02;
                case 5:
                    return 196.02;
                case 6:
                    return 202.02;
                case 7:
                    return 208.01;
                case 8:
                    return 214.02;
                case 9:
                    return 220.02;
                case 10:
                    return 226.02;
                case 11:
                    return 233.04;
                case 12:
                    return 240.02;
                case 13:
                    return 247.03;
                case 14:
                    return 254.03;
                case 15:
                    return 262.00;
                case 16:
                    return 269.03;
                case 17:
                    return 277.03;
                case 18:
                    return 285.04;
                case 19:
                    return 294.03;
                case 20:
                    return 302.07;
                case 21:
                    return 311.04;
                case 22:
                    return 320.05;
                case 23:
                    return 330.06;
                case 24:
                    return 339.06;
                case 25:
                    return 349.08;
                case 26:
                    return 359.06;
                case 27:
                    return 370.09;
                case 28:
                    return 381.08;
                case 29:
                    return 392.10;
                case 30:
                    return 403.10;
                case 31:
                    return 415.01;
                case 32:
                    return 427.05;
                case 33:
                    return 440.12;
                case 34:
                    return 453.16;
                case 35:
                    return 466.08;
                case 36:
                    return 480.15;
                case 37:
                    return 494.07;
                case 38:
                    return 508.16;
                case 39:
                    return 523.09;
                case 40:
                    return 539.16;
                case 41:
                    return 554.19;
                case 42:
                    return 571.17;
                case 43:
                    return 587.19;
                case 44:
                    return 604.14;
                case 45:
                    return 622.09;
                case 46:
                    return 640.11;
                case 47:
                    return 659.21;
                case 48:
                    return 679.10;
                case 49:
                    return 698.17;
                case 50:
                    return 719.21;
                case 51:
                    return 740.18;
                case 52:
                    return 762.41;
                case 53:
                    return 784.47;
                case 54:
                    return 807.29;
                case 55:
                    return 831.48;
                case 56:
                    return 855.32;
                case 57:
                    return 880.57;
                case 58:
                    return 906.67;
                case 59:
                    return 932.17;
                case 60:
                    return 960.69;
                case 61:
                    return 988.55;
                case 62:
                    return 1017.20;
                case 63:
                    return 1046.64;
                case 64:
                    return 1077.85;
                case 65:
                    return 1109.93;
                case 66:
                    return 1141.79;
                case 67:
                    return 1175.54;
                case 68:
                    return 1210.12;
                case 69:
                    return 1244.19;
                case 70:
                    return 1281.61;
                case 71:
                    return 1318.43;
                case 72:
                    return 1357.42;
                case 73:
                    return 1397.16;
                case 74:
                    return 1439.30;
                case 75:
                    return 1480.37;
                case 76:
                    return 1523.85;
                case 77:
                    return 1569.97;
                case 78:
                    return 1614.58;
                case 79:
                    return 1661.81;
                case 80:
                    return 1711.87;
                case 81:
                    return 1762.45;
                case 82:
                    return 1813.34;
                case 83:
                    return 1864.34;
                case 84:
                    return 1921.38;
                case 85:
                    return 1975.46;
                case 86:
                    return 2036.14;
                case 87:
                    return 2093.29;
                case 88:
                    return 2157.64;
                case 89:
                    return 2217.80;
                case 90:
                    return 2285.78;
                case 91:
                    return 2353.41;
                case 92:
                    return 2420.24;
                case 93:
                    return 2490.98;
                case 94:
                    return 2565.97;
                case 95:
                    return 2639.77;
                default:
                    return 0;
            }
        }
        public void PlaySound(byte[] soundLump)
        {
            //Console.Beep is not like DOS so this is just a waste of 15 minutes worth of coding
            //Doesn't work, can only int :( ... but it's 140 samples per second and we give milliseconds to the Console.Beep
            //var duration = (1000 / 140);
            foreach (var sample in soundLump)
            {
                if (sample != 0)
                {
                    int freq = (int)Math.Round(GetFrequency(sample), 0, MidpointRounding.AwayFromZero);
                    Console.Beep(freq, 7);
                }
            }
        }
    }
}
