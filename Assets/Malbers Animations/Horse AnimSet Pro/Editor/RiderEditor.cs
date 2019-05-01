using UnityEngine;
using System.Collections;
using UnityEditor;
using MalbersAnimations.Events;
using UnityEditor.Animations;

namespace MalbersAnimations.HAP
{
    public class RiderEditor : Editor
    {
        protected  Rider M;
       
        bool EventHelp = false;
        bool CallHelp = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SetOnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
        protected virtual void SetEnable()
        {
            M = (Rider)target;
           
        }

        public virtual void SetOnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Controls the behaviour when is riding the animal ", MessageType.None);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
#if REWIRED
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PlayerID"), new GUIContent("Player ID", "Rewired Player ID"));
                EditorGUILayout.EndVertical();
#endif


                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUI.indentLevel--;

                    EditorGUILayout.BeginHorizontal();
                    {
                        M.StartMounted = EditorGUILayout.ToggleLeft(new GUIContent(" Start Mounted", "Set an animal to star mounted on it "), M.StartMounted, GUILayout.MaxWidth(110));
                        M.AnimalStored = (Mountable)EditorGUILayout.ObjectField(M.AnimalStored, typeof(Mountable), true);
                    }
                    EditorGUILayout.EndHorizontal();
                    if (M.StartMounted && M.AnimalStored == null)
                    {
                        EditorGUILayout.HelpBox("Select a Animal with 'IMount' interface from the scene if you want to start mounted on it", MessageType.Warning);
                    }
                    var Parent = serializedObject.FindProperty("Parent");
                    Parent.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Parent to Mount Point", "Parent the Rider to the Mount Point on the Mountable Animal"), Parent.boolValue);

                    EditorGUI.indentLevel++;
                }

                EditorGUILayout.EndVertical();




                

                EditorGUI.indentLevel--;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.BeginHorizontal();
                    M.CreateColliderMounted = EditorGUILayout.ToggleLeft("", M.CreateColliderMounted, GUILayout.MaxWidth(10));
                    EditorGUILayout.LabelField(new GUIContent("Create Capsule Collider while Mounted", "This collider is for hit the Rider while mounted"));
                    EditorGUILayout.EndHorizontal();

                    if (M.CreateColliderMounted)
                    {

                        EditorGUI.indentLevel++;
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Center Y", GUILayout.MinWidth(40));
                        EditorGUILayout.LabelField("Height", GUILayout.MinWidth(40));
                        EditorGUILayout.LabelField("Radius", GUILayout.MinWidth(40));
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        M.Col_Center = EditorGUILayout.FloatField(M.Col_Center);
                        M.Col_height = EditorGUILayout.FloatField(M.Col_height);
                        M.Col_radius = EditorGUILayout.FloatField(M.Col_radius);
                        EditorGUILayout.EndHorizontal();
                        M.Col_Trigger = EditorGUILayout.Toggle("Is Trigger" ,M.Col_Trigger);

                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                M.DisableComponents = EditorGUILayout.ToggleLeft(new GUIContent("Disable Components", "If some of the scripts are breaking the Rider Script: disable them"), M.DisableComponents);
                EditorGUI.indentLevel++;
                if (M.DisableComponents)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("DisableList"), new GUIContent("Disable List", "Monobehaviours that will be disabled while mounted"), true);
                    if (M.DisableList.Length == 0)
                    {
                        EditorGUILayout.HelpBox("If 'Disable List' is empty , it will disable all Monovehaviours while riding", MessageType.Info);
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {

                    EditorGUI.indentLevel++;
                    M.Editor_Inputs = EditorGUILayout.Foldout(M.Editor_Inputs, "Inputs");
                    EditorGUI.indentLevel--;

                    if (M.Editor_Inputs)
                    {
                        var mountInput = serializedObject.FindProperty("MountInput");
                        EditorGUILayout.PropertyField(mountInput, new GUIContent("Mount", "Key or Input to Mount"), true);
                        MalbersInputEditor.DrawInputEvents(mountInput, mountInput.FindPropertyRelative("ShowEvents").boolValue);

                        var DismountInput = serializedObject.FindProperty("DismountInput");
                        EditorGUILayout.PropertyField(DismountInput, new GUIContent("Dismount", "Key or Input to Dismount"), true);
                        MalbersInputEditor.DrawInputEvents(DismountInput, DismountInput.FindPropertyRelative("ShowEvents").boolValue);

                        var CallAnimalInput = serializedObject.FindProperty("CallAnimalInput");
                        EditorGUILayout.PropertyField(CallAnimalInput, new GUIContent("Call Animal", "Key or Input to Call the Animal"), true);
                        MalbersInputEditor.DrawInputEvents(CallAnimalInput, CallAnimalInput.FindPropertyRelative("ShowEvents").boolValue);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                M.Editor_RiderCallAnimal = EditorGUILayout.Foldout(M.Editor_RiderCallAnimal, "Call Animal Audio");
                CallHelp = GUILayout.Toggle(CallHelp, "?", EditorStyles.miniButton, GUILayout.Width(18));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                if (M.Editor_RiderCallAnimal)
                {
                    if (CallHelp)
                    {
                        EditorGUILayout.HelpBox("To call an animal, the animal needs to have MointAI(Script) and a NavMeshAgent.\nRemember to bake a NavMesh... See Poly Art Horse as an example", MessageType.None);
                    }

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("CallAnimalA"), new GUIContent("Call Animal", "Sound to call the Stored Animal"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("StopAnimalA"), new GUIContent("Stop Animal", "Sound to stop calling the Stored Animal"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("RiderAudio"), new GUIContent("Audio Source", "The reference for the audio source"));
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LinkUpdate"), new GUIContent("Link Update", "Type of Update to link manually the rider to the Animal without Parenting it to the Mount Point "));
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                M.Editor_Events = EditorGUILayout.Foldout(M.Editor_Events, "Events");
                EventHelp = GUILayout.Toggle(EventHelp, "?", EditorStyles.miniButton, GUILayout.Width(18));
                EditorGUI.indentLevel--;
                EditorGUILayout.EndHorizontal();
              
                if (M.Editor_Events)
                {
                    if (EventHelp)
                    {
                        EditorGUILayout.HelpBox(CustomEventsHelp()+"\n\nOn Find Mount: Invoked when the rider founds something to mount.", MessageType.None);
                    }

                    DrawCustomEvents();

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("OnFindMount"), new GUIContent("On Find Mount"));

                    if (M.StartMounted)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("OnAlreadyMounted"), new GUIContent("On Already Mounted"));
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;

            }
            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(target);
          
        }

        protected virtual string CustomEventsHelp()
        {
            return "";
        } 

        protected virtual void DrawCustomEvents()
        { }
    }
}
