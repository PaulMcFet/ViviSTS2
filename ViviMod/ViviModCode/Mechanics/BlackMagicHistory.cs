
using System;
using System.Collections.Generic;

using ViviMod.ViviModCode.Cards;

namespace ViviMod.ViviModCode.Mechanics;

public class BlackMagicHistory
{
    private HashSet<string> _uniqueSpells = new();

    public int UniqueSpellCount => _uniqueSpells.Count;

    public void Record(BlackMagicCard spell)
    {
        // Use the card's class name, not its display name.
        // This means upgraded versions and Trance copies
        // count as the same spell.
        string spellId = spell.GetType().FullName
            ?? spell.GetType().Name;

        _uniqueSpells.Add(spellId);
    }

    public void Reset()
    {
        _uniqueSpells.Clear();
    }

    public BlackMagicHistory Clone()
    {
        var clone = new BlackMagicHistory();

        clone._uniqueSpells =
            new HashSet<string>(_uniqueSpells);

        return clone;
    }
}
