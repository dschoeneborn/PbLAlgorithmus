using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbLAlgorithmus
{
    class Modul
    {
        public readonly string Serial;
        private List<LED> _leds = new List<LED>();
        public LED LedUp
        {
            get { return _leds[0]; }
            set { _leds.Insert(0, value); }
        }
        public LED LedDown
        {
            get { return _leds[1]; }
            set { _leds.Insert(1, value); }
        }

        public Modul(string serial)
        {
            Serial = serial;
            LedUp = new LED(serial, LedPosition.Up);
            LedDown = new LED(serial, LedPosition.Down);
        }

        public Modul(string serial, LED up, LED down)
        {
            Serial = serial;
            LedUp = up;
            LedDown = down;
        }

        public void AddFlashLight(Color color)
        {
            LED freeLed;

            // Gucken ob das Modul bereits die Farbe enthält
            if(this.HasAlreadyColor(color))
            {
                throw new Exception("Modul enthält bereits diese Farbe.");
            }

            freeLed = this.GetNextLedWithFreePhase();

            freeLed.SetColor(color, freeLed.GetNextFreePhase());
        }

        public void RemoveFlashLight(Color color)
        {
            LED led;
            if (!this.HasAlreadyColor(color))
            {
                throw new Exception("Modul enthält nicht diese Farbe.");
            }

            led = GetLedWithColor(color);

            led.RemoveColor(color);
        }

        public Boolean HasAlreadyColor(Color color)
        {
            Boolean has = false;

            foreach (LED moduleLed in _leds)
            {
                if (moduleLed.HasAlreadyColor(color) != LedStatus.Off)
                {
                    has = true;
                }
            }

            return has;
        }

        public LED GetLedWithColor(Color color)
        {
            if (!this.HasAlreadyColor(color))
            {
                throw new Exception("Modul enthält nicht diese Farbe.");
            }

            foreach (LED moduleLed in _leds)
            {
                if (moduleLed.HasAlreadyColor(color) != LedStatus.Off)
                {
                    return moduleLed;
                }
            }

            throw new Exception("Es ist ein unbekannter Fehler aufgetreten.");
        }

        public LED GetNextLedWithFreePhase()
        {
            foreach (LED moduleLed in _leds)
            {
                if (moduleLed.HasFreePhase())
                {
                    return moduleLed;
                }
            }

            throw new Exception("Keine freie Phase in den LEDs dieses Moduls vorhanden.");
        }
    }
}
