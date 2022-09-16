using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Chandler Hummingbird
 * Date Created: Sep 23, 2020
 * Date Modified Sep 23, 2020
 * Description: This class is attached to every level prefab and
 * carries per-level stats such as name and par time.
 */

public class Level : MonoBehaviour
{
    [Tooltip("The name of the level.")]
    public string levelName;
    [Space]
    [Header("Completion Times")]
    [Tooltip("The time within which the player must beat the level to earn three stars.")]
    public float goldCompletionTime;
    [Tooltip("The time within which the player must beat the level to earn two stars. Spending more time than this to beat the level will net the player only one star.")]
    public float silverCompletionTime;
    [Space]
    [Tooltip("A force is passively applied to the charge in this level. X value is angle in degrees counterclockwise from +x, Y value is magnitude.")]
    public Vector2 uniformField;
}
