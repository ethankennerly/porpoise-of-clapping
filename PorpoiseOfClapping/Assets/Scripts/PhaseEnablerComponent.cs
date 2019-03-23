using System;
using Unity.Entities;

[Serializable]
public struct PhaseEnabler : IComponentData
{
    public int enabledPhase;
}

public class PhaseEnablerComponent : ComponentDataProxy<PhaseEnabler> { }
