using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public Sentence[] sentences;
}
[Serializable]
public class Sentence
{
    public string name;
    [TextArea(3, 10)]
    public string sentence;
}