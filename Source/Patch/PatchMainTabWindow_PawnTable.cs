using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Autocleaner.Patch
{
    /// <summary>
    /// patches the pawn list in the Schedule UI to include autocleaners
    /// </summary>
    [HarmonyPatch(typeof(MainTabWindow_PawnTable), "Pawns", MethodType.Getter)]
    class PatchMainTabWindow_PawnTablePawns
    {
        static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> pawns, MainTabWindow_PawnTable __instance)
        {
            // Materialize to a list first — if another mod's postfix returns a single-use
            // generator (e.g. Milk), calling .Contains() on the already-drained iterator
            // would always return false and re-add autocleaners as duplicates every frame.
            List<Pawn> pawnList = pawns.ToList();

            foreach (Pawn pawn in pawnList)
            {
                yield return pawn;
            }

            if (Autocleaner.settings.disableSchedule)
            {
                yield break;
            }

            if (!(__instance is MainTabWindow_Schedule))
            {
                yield break;
            }

            foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawns.Where(x => x.Faction == Faction.OfPlayer && x.kindDef == Globals.AutocleanerPawnKind && !pawnList.Contains(x)))
            {
                yield return pawn;
            }
        }
    }
}
