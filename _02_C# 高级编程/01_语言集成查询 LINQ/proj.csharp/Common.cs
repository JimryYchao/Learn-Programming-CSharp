
internal static class Common
{
    public static void NullCheck(params object?[] args)
    {
        for (int i = 0; i < args.Length; i++)
            if (args[i] is null)
                throw new ArgumentNullException($"ELem[{i}]");
    }
}
