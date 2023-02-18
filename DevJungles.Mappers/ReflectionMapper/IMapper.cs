﻿namespace DevJungles.Mappers.ReflectionMapper;

interface IMapper
{
    TDest Map<TSource, TDest>(TSource source) where TDest : new();
    TDest Map<TDest>(object source) where TDest : new();
}
