namespace StardewUI.SourceGenerators;

internal static class AttributeCodeGenerator
{
    public const string GenerateDescriptorSource =
        @"namespace StardewUI
        {
            /// <summary>
            /// Marks a class for descriptor precompilation.
            /// </summary>
            [System.AttributeUsage(System.AttributeTargets.Class)]
            internal class GenerateDescriptorAttribute : System.Attribute
            {
            }
        }";
}
