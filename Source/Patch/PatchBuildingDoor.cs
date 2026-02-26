using HarmonyLib;
using RimWorld;
using Verse;

namespace Autocleaner.Patch
{
    /// <summary>
    /// Allows autocleaners to open any door, bypassing restrictions added by mods
    /// like Locks. Autocleaners are small cleaning robots; locked doors shouldn't
    /// impede them — they already respect home areas and allowed-area assignments.
    /// </summary>
    [HarmonyPatch(typeof(Building_Door), "PawnCanOpen")]
    class PatchBuildingDoorPawnCanOpen
    {
        static bool Prefix(ref bool __result, Pawn p)
        {
            if (p is PawnAutocleaner)
            {
                __result = true;
                return false;
            }

            return true;
        }
    }
}
