using UnityEngine;
using UnityEngine.Animations;
using System.Linq;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.Constraint.Components;
using System;

public class PetraConstraints : MonoBehaviour
{

    GameObject arma, ctr;
    
    public void FixConstraints()
    {
        // Find armature.
        Transform at = gameObject.transform.Find("Armature");
        if (at == null)
        {
            Debug.LogError($"Did not find 'Armature' under {gameObject}");
            return;
        }
        arma = at.gameObject;

        // Constraint container.
        ctr = gameObj(gameObject, "Constraints", true);

        // Volume
        constr("Arm L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L", "hips/spine/chest/shoulder_L/upper_arm_L/Zelbow_volume_1_L", 1.0f, false);
        constr("Arm L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L", "hips/spine/chest/shoulder_L/upper_arm_L/Zelbow_volume_2_L", 1.0f, false);
        constr("Arm R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R", "hips/spine/chest/shoulder_R/upper_arm_R/Zelbow_volume_1_R", 1.0f, false);
        constr("Arm R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R", "hips/spine/chest/shoulder_R/upper_arm_R/Zelbow_volume_2_R", 1.0f, false);

        constr("Leg L", "hips/upper_leg_L/lower_leg_L", "hips/upper_leg_L/Zknee_volume_1_L", 1.0f, false);
        constr("Leg L", "hips/upper_leg_L/lower_leg_L", "hips/upper_leg_L/Zknee_volume_2_L", 1.0f, false);
        constr("Leg R", "hips/upper_leg_R/lower_leg_R", "hips/upper_leg_R/Zknee_volume_1_R", 1.0f, false);
        constr("Leg R", "hips/upper_leg_R/lower_leg_R", "hips/upper_leg_R/Zknee_volume_2_R", 1.0f, false);

        constr("Torso", "hips/spine", "hips/Zbelly_volume_2", 1.0f, false);
        constr("Torso", "hips/spine", "hips/Zbelly_volume_3", 1.0f, false);
        constr("Torso", "hips/upper_leg_L", "hips/Zparasite_hips/Zass_volume_L", 1.0f, false);
        constr("Torso", "hips/upper_leg_R", "hips/Zparasite_hips/Zass_volume_R", 1.0f, false);

        // Arm Twist: Upper arm twist are nested 3/2/1. #3 doesn't have a constraint; 2 and 1 do. Lower arm are 1/2/3, all have constraints.
        constr("Arm L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L", "hips/spine/chest/shoulder_L/upper_arm_L/Zupper_arm_twist_3_L/Zupper_arm_twist_2_L", 0.49f, true);
        constr("Arm L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L", "hips/spine/chest/shoulder_L/upper_arm_L/Zupper_arm_twist_3_L/Zupper_arm_twist_2_L/Zupper_arm_twist_1_L", 0.502f, true);
        constr("Arm L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L/hand_L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L/Zlower_arm_twist_1_L", 0.333f, true);
        constr("Arm L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L/hand_L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L/Zlower_arm_twist_1_L/Zlower_arm_twist_2_L", 0.333f, true);
        constr("Arm L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L/hand_L", "hips/spine/chest/shoulder_L/upper_arm_L/lower_arm_L/Zlower_arm_twist_1_L/Zlower_arm_twist_2_L/Zlower_arm_twist_3_L", 0.333f, true);

        constr("Arm R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R", "hips/spine/chest/shoulder_R/upper_arm_R/Zupper_arm_twist_3_R/Zupper_arm_twist_2_R", 0.49f, true);
        constr("Arm R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R", "hips/spine/chest/shoulder_R/upper_arm_R/Zupper_arm_twist_3_R/Zupper_arm_twist_2_R/Zupper_arm_twist_1_R", 0.502f, true);
        constr("Arm R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R/hand_R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R/Zlower_arm_twist_1_R", 0.333f, true);
        constr("Arm R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R/hand_R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R/Zlower_arm_twist_1_R/Zlower_arm_twist_2_R", 0.333f, true);
        constr("Arm R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R/hand_R", "hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R/Zlower_arm_twist_1_R/Zlower_arm_twist_2_R/Zlower_arm_twist_3_R", 0.333f, true);
    }

    private GameObject gameObj(GameObject parent, string name, bool delete = false)
    {
        Transform t = parent.transform.Find(name);
        if (t != null)
        {
            if (delete)
            {
                DestroyImmediate(t.gameObject);
            }
            else
            {
                return t.gameObject;
            }
        }

        GameObject o = new GameObject(name);
        o.transform.parent = parent.transform;
        return o;
    }

    private void constr(string group, string source, string target, float weight, bool twist)
    {
        string sourceName = source.Split("/").Last();
        string targetName = target.Split("/").Last();

        GameObject grp = gameObj(ctr, group);
        GameObject holder = gameObj(grp, targetName);

        // Source bone
        Transform srct = arma.transform.Find(source);
        if (srct == null)
        {
            Debug.LogError($"Failed to find source bone {source}");
            return;
        }
        
        // Target bone
        Transform trgt = arma.transform.Find(target);
        if (trgt == null)
        {
            Debug.LogError($"Failed to find target bone {target}");
            return;
        }

        // Strong inspiration from: https://github.com/anatawa12/ProjectWideVRCConstraintsConverter/blob/master/Editor/ConverterWindow.cs
        VRCRotationConstraint rc = holder.AddComponent<VRCRotationConstraint>();
        rc.GlobalWeight = weight;
        rc.TargetTransform = trgt;
        rc.Sources.SetLength(1);
        rc.Sources[0] = new VRCConstraintSource(srct, 1.0f, Vector3.zero, Vector3.zero);
        rc.AffectsRotationX = !twist;
        rc.AffectsRotationZ = !twist;
        rc.ActivateConstraint();    
    }
}
