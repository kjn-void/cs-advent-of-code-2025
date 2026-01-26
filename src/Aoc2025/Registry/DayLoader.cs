using System.Reflection;
using Aoc2025.Days;

namespace Aoc2025.Registry;

public static class DayLoader
{
    private static bool _loaded;

    public static void LoadAll()
    {
        if (_loaded)
        {
            return;
        }

        _loaded = true;

        var asm = Assembly.GetExecutingAssembly();

        foreach (var type in asm.GetTypes())
        {
            // Force static constructors for all ISolution implementations
            if (!type.IsAbstract
                && typeof(ISolution).IsAssignableFrom(type))
            {
                // Touch the type to trigger static ctor
                System.Runtime.CompilerServices.RuntimeHelpers
                    .RunClassConstructor(type.TypeHandle);
            }
        }
    }
}