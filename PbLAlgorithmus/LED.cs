using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbLAlgorithmus
{
    class LED
    {
        public readonly string Serial;
        public readonly LedPosition LedPosition;
        public LedStatus Red;
        public LedStatus Green;
        public LedStatus Blue;

        public LED(string serial, LedPosition ledPosition)
        {
            this.Serial = serial;
            this.LedPosition = ledPosition;
        }

        public LED(string serial, LedPosition ledPosition, LedStatus red, LedStatus green, LedStatus blue) : this(serial, ledPosition)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public LedStatus HasAlreadyColor(Color color)
        {
            Byte[] bColor = color.ToByte();
            Byte[] bLedStatus = this.GetLedStatusByteArray();

            Boolean[] p1 = { false, false, false };
            Boolean[] p2 = { false, false, false };

            LedStatus status = LedStatus.Off;

            for(int i = 0; i<bColor.Length; i++)
            {
                if(bColor[i] == 1)
                {
                    // Prüfe Phase 1
                    if(bLedStatus[i] == 2 || bLedStatus[i] == 1)
                    {
                        p1[i] = true;
                    }

                    // Prüfe Phase 2
                    if (bLedStatus[i] == 3 || bLedStatus[i] == 1)
                    {
                        p2[i] = true;
                    }
                }
                else if(bColor[i] == 0)
                {
                    // Prüfe Phase 1
                    if (bLedStatus[i] == 3 || bLedStatus[i] == 0)
                    {
                        p1[i] = true;
                    }

                    // Prüfe Phase 2
                    if (bLedStatus[i] == 2 || bLedStatus[i] == 0)
                    {
                        p2[i] = true;
                    }
                }
            }

            if(p1.All(x => x))
            {
                status = LedStatus.Phase1;
            }

            if (p2.All(x => x))
            {
                status = LedStatus.Phase2;
            }

            if (p1.All(x => x) && p2.All(x => x))
            {
                status = LedStatus.On;
            }

            return status;
        }

        public Boolean HasFreePhase()
        {
            Byte[] bLedStatus = this.GetLedStatusByteArray();
            Boolean p1 = true;
            Boolean p2 = true;

            for (int i = 0; i < bLedStatus.Length; i++)
            {
                // Prüfe Phase 1
                if (bLedStatus[i] == 2 || bLedStatus[i] == 1)
                {
                    p1 = false;
                }

                // Prüfe Phase 2
                if (bLedStatus[i] == 3 || bLedStatus[i] == 1)
                {
                    p2 = false;
                }
            }

            return p1 || p2;
        }

        public Phase GetNextFreePhase()
        {
            Byte[] bLedStatus = this.GetLedStatusByteArray();
            Boolean p1 = false;
            Boolean p2 = false;
            Phase phase = Phase.Phase1;

            for (int i = 0; i < bLedStatus.Length; i++)
            {
                // Prüfe Phase 1
                if (bLedStatus[i] == 2 || bLedStatus[i] == 1)
                {
                    p1 = true;
                }

                // Prüfe Phase 2
                if (bLedStatus[i] == 3 || bLedStatus[i] == 1)
                {
                    p2 = true;
                }
            }

            if(p1 && p2)
            {
                throw new Exception("Beide Phasen sind belegt.");
            }
            else if (!p1 && !p2)
            {
                phase = Phase.Phase1;
            }
            else if(!p1)
            {
                phase = Phase.Phase1;
            }
            else if(!p2)
            {
                phase = Phase.Phase2;
            }


            return phase;
        }

        public Phase GetPhaseWithColor(Color color)
        {
            Byte[] bColor = color.ToByte();
            Byte[] bLedStatus = this.GetLedStatusByteArray();

            Boolean[] p1 = { false, false, false };
            Boolean[] p2 = { false, false, false };

            Phase phase = Phase.Phase1;

            for (int i = 0; i < bColor.Length; i++)
            {
                if (bColor[i] == 1)
                {
                    // Prüfe Phase 1
                    if (bLedStatus[i] == 2 || bLedStatus[i] == 1)
                    {
                        p1[i] = true;
                    }

                    // Prüfe Phase 2
                    if (bLedStatus[i] == 3 || bLedStatus[i] == 1)
                    {
                        p2[i] = true;
                    }
                }
                else if (bColor[i] == 0)
                {
                    // Prüfe Phase 1
                    if (bLedStatus[i] == 3 || bLedStatus[i] == 0)
                    {
                        p1[i] = true;
                    }

                    // Prüfe Phase 2
                    if (bLedStatus[i] == 2 || bLedStatus[i] == 0)
                    {
                        p2[i] = true;
                    }
                }
            }

            if (p1.All(x => x))
            {
                phase = Phase.Phase1;
            }

            if (p2.All(x => x))
            {
                phase = Phase.Phase2;
            }

            if (p1.All(x => x) && p2.All(x => x))
            {
                throw new Exception("Die gesuchte Farbe ist auf dauerleuchten.");
            }

            if(!p1.All(x => x) && !p2.All(x => x))
            {
                throw new Exception("Die gesuchte Farbe wurde in keiner Phase gefunden.");
            }

            return phase;
        }

        public Byte[] GetLedStatusByteArray()
        {
            Byte[] b = { Convert.ToByte(Red), Convert.ToByte(Green), Convert.ToByte(Blue) };
            return b;
        }

        public void SetLedStatus(Byte[] BLedStatus)
        {
            for (int i = 0; i < BLedStatus.Length; i++)
            {
                switch(i)
                {
                    case 0:
                        Red = (LedStatus)BLedStatus[i];
                        break;
                    case 1:
                        Green = (LedStatus)BLedStatus[i];
                        break;
                    case 2:
                        Blue = (LedStatus)BLedStatus[i];
                        break;
                }

            }
        }

        public void SetColor(Color color, Phase p)
        {
            Byte[] bColor = convertColorAndPhaseToLedStatus(color, p);
            Byte[] bLedStatus = this.GetLedStatusByteArray();

            for (int i = 0; i < bColor.Length; i++)
            {
                if(bColor[i] == 0)
                {
                    // tu nix
                }
                else if(bLedStatus[i] == 0)
                {
                    bLedStatus[i] = bColor[i];
                }
                else if(bLedStatus[i] != 0 && bColor[i] != 0)
                {
                    bLedStatus[i] = 1;
                }
            }

            this.SetLedStatus(bLedStatus);
        }

        public void RemoveColor(Color color)
        {
            Phase phase = GetPhaseWithColor(color);
            Byte[] bColor = convertColorAndPhaseToLedStatus(color, phase);
            Byte[] bLedStatus = this.GetLedStatusByteArray();

            for (int i = 0; i < bColor.Length; i++)
            {
                if(bColor[i] != 0)
                {
                    if(bLedStatus[i] != 0 && bLedStatus[i] != 1)
                    {
                        bLedStatus[i] = 0;
                    }
                    else if(bLedStatus[i] == 1)
                    {
                        if(phase == Phase.Phase1)
                        {
                            bLedStatus[i] = Convert.ToByte(Phase.Phase2);
                        }
                        else if(phase == Phase.Phase2)
                        {
                            bLedStatus[i] = Convert.ToByte(Phase.Phase1);
                        }
                    }
                }
            }

            SetLedStatus(bLedStatus);
        }

        private Byte[] convertColorAndPhaseToLedStatus(Color color, Phase p)
        {
            Byte[] bColor = color.ToByte();

            for(int i = 0; i<bColor.Length; i++)
            {
                if(bColor[i] == 1)
                {
                    bColor[i] = Convert.ToByte(p);
                }
            }

            return bColor;
        }
    }

    enum LedPosition { Up, Down };

    enum LedStatus { Off, On, Phase1, Phase2 };

    enum Phase { Phase1 = 2, Phase2 = 3};

}
