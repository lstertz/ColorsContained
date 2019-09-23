using Components.Activity;
using Components.Supports;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using UInput = UnityEngine.Input;
using DestroyComponent = Components.Attributes.Destroy;
using Components.Attributes;
using System;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the Input System, which interprets user input to initiate 
    /// drawing or mark a Block for destruction.
	/// </summary>
    [AlwaysUpdateSystem]
    [UpdateBefore(typeof(RenderTiles))]
    public class Input : JobComponentSystem
    {
        private static readonly Vector3 positionShift = new Vector3(-0.5f, 0.0f, -0.5f);

        public static HashSet<float3> DrawLocations =
            new HashSet<float3>();
        public static Plane InteractionPlane { get; set; }

        private static List<int> drawRestrictions = null;
        private static bool hasDragged;
        private static int3 positionDown;
        private static int restrictionIndex = 0;

        public static void SetDrawRestrictions(int[] restrictions)
        {
            if (restrictions == null)
            {
                drawRestrictions = null;
                return;
            }

            drawRestrictions = new List<int>(restrictions);
            drawRestrictions.Sort();
        }
        
        
        private struct MarkForDestructionJob : IJobProcessComponentDataWithEntity<
            TileProperties, Position>
        {
            [ReadOnly]
            public ComponentDataFromEntity<Indestructible> Indestructibles;

            [ReadOnly]
            public float3 InteractionPosition;

            [WriteOnly]
            public EntityCommandBuffer.Concurrent Buffer;

            public void Execute(Entity entity, int index,
                [ReadOnly] ref TileProperties properties, 
                [ReadOnly] ref Position position)
            {
                if (properties.Tileset == Entity.Null)
                    return;

                if (Indestructibles.Exists(properties.Tileset))
                    return;

                if (InteractionPosition.x == position.Value.x &&
                    InteractionPosition.y == position.Value.y &&
                    InteractionPosition.z == position.Value.z)
                {
                    Buffer.AddComponent(index, properties.Tileset,
                        new DestroyComponent());
                }
            }
        }

        private EntityCommandBufferSystem markDestructionBarrier;

        protected override void OnCreateManager()
        {
            markDestructionBarrier = 
                World.GetOrCreateManager<EndSimulationEntityCommandBufferSystem>();

            // TODO :: Adjust Z later.
            InteractionPlane = new Plane(Vector3.up, Vector3.zero);
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            JobHandle jobHandle = inputDependencies;

            if (UInput.GetButtonDown("Fire1"))
            {
                hasDragged = false;
                positionDown = GetGridPosition();
                // TODO :: Use a job to highlight any Block that were pressed upon.
            }
            else if (UInput.GetButton("Fire1"))
            {
                int3 newPosition = GetGridPosition();

                if (!newPosition.Equals(positionDown))
                    HandleDrag(newPosition);
            }
            else if (UInput.GetButtonUp("Fire1"))
            {
                int3 positionUp = GetGridPosition();

                if (!hasDragged)
                    jobHandle = HandleTap(inputDependencies);
                else
                    HandleDragEnd();
            }

            return jobHandle;
        }


        private int3 GetGridPosition()
        {
            float enter;

            Ray ray = Camera.main.ScreenPointToRay(
                new Vector3(UInput.mousePosition.x, UInput.mousePosition.y, 0));
            InteractionPlane.Raycast(ray, out enter);

            Vector3 planePoint = ray.GetPoint(enter) + positionShift;
            return new int3(
                Mathf.RoundToInt(planePoint.x),
                Mathf.RoundToInt(planePoint.y),
                Mathf.RoundToInt(planePoint.z));
        }

        private void HandleDrag(int3 draggedPosition)
        {
            if (drawRestrictions == null)
                return;

            if (Math.Abs(positionDown.x) > Resources.BackgroundHalfSize ||
                Math.Abs(positionDown.z) > Resources.BackgroundHalfSize)
                return;

            if (Math.Abs(draggedPosition.x) > Resources.BackgroundHalfSize ||
                Math.Abs(draggedPosition.z) > Resources.BackgroundHalfSize)
                return;

            if (!hasDragged)
            {
                DrawLocations.Add(positionDown);
                hasDragged = true;

                DrawRestrictionUI.SetVisibility(true);
                DrawRestrictionUI.SetTotalRequired(drawRestrictions[restrictionIndex]);
            }
            
            DrawLocations.Add(draggedPosition);

            if (DrawLocations.Count > drawRestrictions[restrictionIndex])
            {
                if (restrictionIndex != drawRestrictions.Count - 1)
                {
                    restrictionIndex++;
                    DrawRestrictionUI.SetTotalRequired(drawRestrictions[restrictionIndex]);
                }
            }
            DrawRestrictionUI.SetRequirementMet(DrawLocations.Count);

            Draw draw = World.GetOrCreateManager<Draw>();
            draw.Enabled = true;
            {
                draw.FinalizeTileset = false;
                draw.Update();
            }
            draw.Enabled = false;
        }

        private void HandleDragEnd()
        {
            if (drawRestrictions.Contains(DrawLocations.Count))
            {
                Draw draw = World.GetOrCreateManager<Draw>();
                draw.Enabled = true;
                {
                    draw.FinalizeTileset = true;
                    {
                        draw.Update();
                    }
                    draw.FinalizeTileset = false;
                }
                draw.Enabled = false;

                DrawLocations.Clear();
            }
            else
            {
                DrawLocations.Clear();

                Draw draw = World.GetOrCreateManager<Draw>();
                draw.Enabled = true;
                {
                    draw.FinalizeTileset = false;
                    {
                        draw.Update();
                    }
                    draw.FinalizeTileset = false;
                }
                draw.Enabled = false;
            }

            restrictionIndex = 0;
            DrawRestrictionUI.SetVisibility(false);
        }

        private JobHandle HandleTap(JobHandle inputDependencies)
        {
            EntityCommandBuffer.Concurrent markDestructionBuffer =
                markDestructionBarrier.CreateCommandBuffer().ToConcurrent();

            ComponentDataFromEntity<Indestructible> indestructibles =
                GetComponentDataFromEntity<Indestructible>(true);
            JobHandle job = new MarkForDestructionJob()
            {
                Buffer = markDestructionBuffer,
                Indestructibles = indestructibles,
                InteractionPosition = positionDown
            }.Schedule(this, inputDependencies);

            markDestructionBarrier.AddJobHandleForProducer(job);
            return job;
        }
    }
}
