﻿namespace DevToys.Api;

/// <summary>
/// Represents the definition of a setting in the application.
/// </summary>
/// <typeparam name="T">The type of value of the setting</typeparam>
[DebuggerDisplay($"Name = {{{nameof(Name)}}}")]
public readonly struct SettingDefinition<T> : IEquatable<SettingDefinition<T>>
{
    /// <summary>
    /// Gets the name of the setting.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the default value of the setting.
    /// </summary>
    public T DefaultValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingDefinition"/> structure.
    /// </summary>
    /// <param name="name">The name of the setting. Should be unique.</param>
    /// <param name="defaultValue">The default value of the setting.</param>
    public SettingDefinition(string name, T defaultValue)
    {
        Guard.IsNotNullOrWhiteSpace(name);

        if (name.Length > 255)
        {
            // Come one! Make it shorter!
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(name), "Setting name is limited to 255 characters.");
        }
        else if (name.Contains("="))
        {
            // For portable apps, settings are stored in a .ini file where the format is "setting_name=value".
            // Therefore, the setting name shouldn't contain "=".
            ThrowHelper.ThrowArgumentException(nameof(name), "Setting name cannot contain '='.");
        }

        Name = name;
        DefaultValue = defaultValue;
    }

    public override bool Equals(object? obj)
    {
        if (obj is SettingDefinition<T> definition)
        {
            return Equals(definition);
        }

        return false;
    }

    public bool Equals(SettingDefinition<T> other)
    {
        return string.Equals(other.Name, Name, StringComparison.Ordinal)
            && other.DefaultValue is not null
            && other.DefaultValue.Equals(DefaultValue);
    }

    public override int GetHashCode()
    {
        return (Name.GetHashCode() ^ 47)
             * (DefaultValue is null ? 13 : DefaultValue.GetHashCode() ^ 73);
    }

    public static bool operator ==(SettingDefinition<T> left, SettingDefinition<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SettingDefinition<T> left, SettingDefinition<T> right)
    {
        return !(left == right);
    }
}