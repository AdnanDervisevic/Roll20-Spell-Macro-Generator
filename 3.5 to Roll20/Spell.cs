namespace _3._5_to_Roll20
{
    public class Spell
    {
        public string Name { get; set; }

        public string Level { get; set; }

        public string Descriptor { get; set; }

        public string Components { get; set; }

        public string Range { get; set; }

        public string Target { get; set; }

        public string Duration { get; set; }

        public string SavingThrow { get; set; }

        public string SpellResistance { get; set; }

        public string Description { get; set; }

        public string Fulltext { get; set; }

        public string XpCost { get; set; }

        public string MaterialComponents { get; set; }

        public string School { get; set; }

        public string Effect { get; set; }

        public string CastingTime { get; set; }

        public string DisplayString => $"{Name} ({Level})";

        public ClassInformation ClassInformation { get; set; }
    }
}
