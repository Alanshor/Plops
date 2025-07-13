using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Happy_Plops
{
    public class GameData
    {
        public DateTime _lastTimeSaved { get; set; }  = new DateTime();
        public bool hasEdited { get; set; }  = false;
        public List<Plop> Plops { get; set; } = new List<Plop> ();
        public List<PlopGroup> Groups { get; set; } = new List<PlopGroup> ();

        public GameData()
        {
            _lastTimeSaved = DateTime.Now;

        }

        public JsonSerializerSettings Settings()
        {
            return new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
        }

        public Plop NewPlop(string name, int level = 1)
        {
            var plop = new Plop(name, level);
            Plops.Add(plop);
            hasEdited = true;
            return plop;
        }

        public PlopGroup NewGroup(string name=null)
        {
            var group = new PlopGroup(name);
            Groups.Add(group);
            hasEdited = true;
            return group;
        }

        public void Add(Plop plop)
        {
            if (plop == null || Plops.Contains(plop))
                return;

            Plops.Add(plop);
            hasEdited = true;
        }

        public void Add(PlopGroup group)
        {
            if (group == null || Groups.Contains(group))
                return;

            Groups.Add(group);
            hasEdited = true;
        }

        public bool Remove(Plop plop)
        {
            if (plop == null)
                return false;

            bool removed = Plops.Remove(plop);
            hasEdited |= removed;
            return removed;
        }

        public bool Remove(PlopGroup group)
        {
            if (group == null)
                return false;

            bool removed = Groups.Remove(group);
            hasEdited |= removed;
            return removed;
        }

        public int MinutesPassedSinceLastSave()
        {
            DateTime now = DateTime.Now;
            long timeDifferenceTicks = now.Ticks - _lastTimeSaved.Ticks;
            int timeDifferenceMinutes = (int)(timeDifferenceTicks / TimeSpan.TicksPerMinute);

            return timeDifferenceMinutes;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Last Saved: {_lastTimeSaved}");
            sb.AppendLine($"Has Edited: {hasEdited}");
            sb.AppendLine($"Plops ({Plops.Count}):");

            foreach (var plop in Plops)
            {
                sb.AppendLine($"  - {plop}");
            }

            sb.AppendLine($"Groups ({Groups.Count}):");

            foreach (var group in Groups)
            {
                sb.AppendLine($"  - {group}");
            }

            return sb.ToString();
        }
    }
}
