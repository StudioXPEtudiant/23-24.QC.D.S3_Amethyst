using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputData
{
    public bool Accelerate;
    public bool Brake;
    public float TurnInput;
}

public interface IInput
{
    InputData GenerateInput();
}

public abstract class BaseInput : MonoBehaviour, IInput
{
    /// <summary>
    /// </summary>
    public abstract InputData GenerateInput();
}