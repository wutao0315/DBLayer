﻿namespace DBLayer.Core.Interface;

public interface IConnectionString
{
    IDictionary<string, string> Properties { get; }
    string ConnectionToken { get; }
    string ConnectionValue { get; }
}
